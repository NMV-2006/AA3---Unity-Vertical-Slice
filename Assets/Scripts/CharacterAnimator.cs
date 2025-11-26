
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    public GroundDetector gd;
    public CharacterMover cm;
    public Camera cam;
    Animator anim;
    public float rotationScale;

    [Range(0f, 1f)]

    [Header("IK")]
    public Transform rightHand;
    public Transform leftHand;
    public Transform lookAt;
    Vector3 lookAtPos;
    public float lookAtMaxAngle = 0.5f;
    public float lookAtSpeed = 10;
    public float lookAtDistance = 10;
    private void Start()
    {
        anim = GetComponent<Animator>();
        if(cam == null)
        {
            cam = Camera.main;
        }

    }
    void Update()
    {
        anim.SetFloat("Sideways", cm.velocity.x);
        anim.SetFloat("Upwards", cm.velocity.y);
        anim.SetFloat("Forward", cm.velocity.z);
        anim.SetFloat("Rotation", cm.velocityAngular * rotationScale);
        anim.SetBool("Grounded", gd.grounded);

        FixLookat();
    }

    private void FixLookat()
    {
        if (lookAt == null) return;
        lookAtPos = Vector3.Lerp(lookAtPos, lookAt.position, lookAtSpeed * Time.deltaTime);

        Vector3 forwardLookAt = (lookAtPos - lookAt.position).normalized;
        float dot = Vector3.Dot(forwardLookAt, transform.forward);
        if (dot < lookAtMaxAngle)
        {
            Vector3 axis = Vector3.Cross(forwardLookAt, transform.forward);
            float angle = Vector3.SignedAngle(forwardLookAt, transform.forward, axis);
            float distance = Vector3.Distance(lookAtPos, lookAt.transform.position);
            float maxAngle = Mathf.Acos(lookAtMaxAngle) * Mathf.Rad2Deg;
            forwardLookAt = Quaternion.AngleAxis(angle > 0 ? -maxAngle : maxAngle, axis) * transform.forward;
            lookAtPos = lookAt.transform.position + forwardLookAt * distance;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (rightHand)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
            anim.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
            anim.SetIKRotation(AvatarIKGoal.RightHand, rightHand.rotation);
        }
        else
        {
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
        }
        if (leftHand)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
            anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation);
        }
        else
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
        }
        if (lookAt)
        {
            anim.SetLookAtPosition(lookAtPos);
            anim.SetLookAtWeight(1, 1, 1, 1);
        }
        else
        {
            anim.SetLookAtWeight(0);
        }
    }
}
