using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // PARA Keyboard.current

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

    // ▼▼ NUEVO: multiplicador de caída lenta ▼▼
    [Range(0f, 1f)]
    public float slowFallMultiplier = 0.3f;

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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gd = GetComponent<GroundDetector>();
        gd.groundedUp.AddListener(DroppedOff);

        input = new InputSystem_Actions();
        input.Enable();
    }

    private void Update()
    {
        if (input.Player.Jump.WasPressedThisFrame())
            TryJump();
    }

    void TryJump()
    {
        rb.linearVelocity = transform.up * jumpForce;
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
            GroundMovement();
        }
        else
        {
            AirMovement();
        }
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

    void AirMovement()
    {
        // ▼▼ CAÍDA LENTA: detecta Shift directamente (nuevo Input System o Input.Legacy) ▼▼
        if (rb.linearVelocity.y < 0 && IsHoldingSlowFall())
        {
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x,
                rb.linearVelocity.y * slowFallMultiplier,
                rb.linearVelocity.z
            );
        }

        // --- movimiento horizontal en el aire ---
        Vector3 inputVector = new Vector3(
            input.Player.Move.ReadValue<Vector2>().x,
            0,
            input.Player.Move.ReadValue<Vector2>().y
        );

        float magnitude = Mathf.Clamp01(inputVector.magnitude);

        if (magnitude > 0)
        {
            Vector3 movForward = cam.transform.forward * inputVector.z;
            Vector3 movRight = cam.transform.right * inputVector.x;

            Vector3 airMov = (movForward + movRight);
            airMov.y = 0;
            airMov = airMov.normalized * magnitude;

            rb.linearVelocity += airMov * (speedMovement * airControl * Time.fixedDeltaTime);
        }
    }

    // Comprueba si el jugador está manteniendo Shift (compatibilidad con Input System y con Input legacy)
    bool IsHoldingSlowFall()
    {
        // NUEVO Input System (recomendado)
        if (Keyboard.current != null)
        {
            if (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed)
                return true;
        }

        // Fallback al Input clásico
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            return true;

        return false;
    }

    void DroppedOff()
    {
        if (airSpeedFollowupCurrent > 0)
        {
            rb.linearVelocity += transform.TransformDirection(velocity * airSpeedFollowupCurrent - rb.linearVelocity);
        }

        airSpeedFollowupCurrent = 0;
    }
}
