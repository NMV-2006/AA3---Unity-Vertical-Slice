using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // <<< Necesario para la UI

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GroundDetector))]
public class CharacterMover : MonoBehaviour
{
    public Camera cam;
    public float movementAcceleration;
    public float movementDeceleration;
    Vector3 currentMov;
    public float speedMovement;

    [Range(0, 1)]
    public float speedMovementSideways;
    [Range(0, 1)]
    public float speedMovementBackwards;

    public bool lookAtCamera;
    public float speedTurn;

    public float jumpForce;

    // Control en el aire
    public float airControl = 0.5f;

    Rigidbody rb;
    GroundDetector gd;

    public float airSpeedFollowup = 1f;
    float airSpeedFollowupCurrent;

    public Vector3 velocity { get; private set; }
    public float velocityAngular { get; private set; }
    public Vector3 velocityAxis { get; private set; }

    Quaternion velocityRotation;
    Vector3 lastPos;
    Quaternion lastRot;

    InputSystem_Actions input;

    // ================================
    //      ENERGÍA AÉREA
    // ================================
    public float maxAirEnergy = 5f;     // 3 segundos máximos
    public float airEnergy;             // energía actual
    public float slowFallCost = 1f;     // costo por segundo de caída lenta
    public float airJumpCost = 0.4f;      // costo de salto en el aire
    public float airMoveCost = 0.4f;    // costo por mover en el aire

    bool canUseAirAbilities => airEnergy > 0.05f;

    // UI RADIAL
    public Image airEnergyUI;   // <<< Imagen radial del HUD

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gd = GetComponent<GroundDetector>();
        gd.groundedUp.AddListener(DroppedOff);

        input = new InputSystem_Actions();
        input.Enable();

        airEnergy = maxAirEnergy; // energía llena al comenzar
    }

    private void Update()
    {
        UpdateUI(); // <<< actualizar UI

        if (input.Player.Jump.WasPressedThisFrame())
            TryJump();
    }

    void TryJump()
    {
        if (gd.grounded)
        {
            rb.linearVelocity = transform.up * jumpForce;
            return;
        }

        // salto EN EL AIRE
        if (canUseAirAbilities && airEnergy >= airJumpCost)
        {
            rb.linearVelocity = transform.up * jumpForce;
            airEnergy -= airJumpCost;
        }
    }

    void FixedUpdate()
    {
        Velocity();
        Movement();

        lastPos = transform.position;
        lastRot = transform.rotation;
    }

    void Velocity()
    {
        velocity = transform.InverseTransformDirection((transform.position - lastPos) / Time.fixedDeltaTime);
        velocityRotation = Quaternion.Inverse(lastRot) * transform.rotation;

        float _velocityAngular;
        Vector3 _velocityAxis;

        velocityRotation.ToAngleAxis(out _velocityAngular, out _velocityAxis);

        velocityAngular = _velocityAngular / Time.fixedDeltaTime;
        velocityAxis = _velocityAxis;

        if (Vector3.Dot(velocityAxis, transform.up) < 0)
            velocityAngular *= -1;

        airSpeedFollowupCurrent = Mathf.Clamp(airSpeedFollowupCurrent + Time.fixedDeltaTime, 0, airSpeedFollowup);
    }

    void Movement()
    {
        if (gd.grounded)
        {
            RecoverEnergy(); // <<< recuperar energía
            GroundMovement();
        }
        else
        {
            AirMovement();
        }
    }

    // =====================================================
    //              RECUPERAR ENERGÍA AL TOCAR SUELO
    // =====================================================
    void RecoverEnergy()
    {
        if (airEnergy < maxAirEnergy)
            airEnergy = maxAirEnergy;
    }

    void GroundMovement()
    {
        Vector3 mov = new Vector3(
            input.Player.Move.ReadValue<Vector2>().x * speedMovementSideways,
            0,
            input.Player.Move.ReadValue<Vector2>().y
        );

        if (mov.z < 0)
            mov.z *= speedMovementBackwards;

        float magnitude = Mathf.Clamp01(mov.magnitude);

        if (magnitude > 0)
        {
            Vector3 movForward = cam.transform.forward * mov.z;
            Vector3 movRight = cam.transform.right * mov.x;

            mov = movForward + movRight;
            mov.y = 0;
            mov = mov.normalized * magnitude;
        }

        if (magnitude > currentMov.magnitude)
            currentMov = Vector3.Lerp(currentMov, mov, movementAcceleration * Time.fixedDeltaTime);
        else
            currentMov = Vector3.Lerp(currentMov, mov, movementDeceleration * Time.fixedDeltaTime);

        Quaternion rot;

        if (lookAtCamera)
        {
            Vector3 camForward = cam.transform.forward;
            camForward.y = 0;
            camForward.Normalize();

            rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(camForward), speedTurn * Time.fixedDeltaTime);
        }
        else
        {
            rot = currentMov.magnitude > 0.01f ?
                Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(currentMov), speedTurn * Time.fixedDeltaTime)
                : transform.rotation;
        }

        rb.Move(rb.position + currentMov * speedMovement * Time.fixedDeltaTime, rot);
    }

    // =====================================================
    //                  MOVIMIENTO EN EL AIRE
    // =====================================================
    void AirMovement()
    {
        // -------------------------------------------
        // CAÍDA LENTA si pulsa Shift y tiene energía
        // -------------------------------------------
        if (input.Player.Sprint.IsPressed() && canUseAirAbilities)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * 0.3f, rb.linearVelocity.z);
            airEnergy -= slowFallCost * Time.fixedDeltaTime;
        }

        // -------------------------------------------
        // MOVIMIENTO AÉREO (gasta energía)
        // -------------------------------------------
        if (canUseAirAbilities)
        {
            Vector3 inputVector = new Vector3(
                input.Player.Move.ReadValue<Vector2>().x,
                0,
                input.Player.Move.ReadValue<Vector2>().y
            );

            float magnitude = Mathf.Clamp01(inputVector.magnitude);

            if (magnitude > 0)
            {
                airEnergy -= airMoveCost * Time.fixedDeltaTime;

                Vector3 movForward = cam.transform.forward * inputVector.z;
                Vector3 movRight = cam.transform.right * inputVector.x;

                Vector3 airMov = (movForward + movRight);
                airMov.y = 0;
                airMov = airMov.normalized * magnitude;

                rb.linearVelocity += airMov * (speedMovement * airControl * Time.fixedDeltaTime);
            }
        }

        // si la energía llega a cero → caída normal
        if (airEnergy < 0)
            airEnergy = 0;
    }

    void DroppedOff()
    {
        if (airSpeedFollowupCurrent > 0)
        {
            rb.linearVelocity += transform.TransformDirection(velocity * airSpeedFollowupCurrent - rb.linearVelocity);
        }

        airSpeedFollowupCurrent = 0;
    }

    // =====================================================
    //                  ACTUALIZACIÓN UI
    // =====================================================
    void UpdateUI()
    {
        if (airEnergyUI != null)
            airEnergyUI.fillAmount = airEnergy / maxAirEnergy;
    }
}
