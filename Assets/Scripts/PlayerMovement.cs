using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public MovementState State;
    public enum MovementState
    {
        freeze,
        unlimited,
        walking,
        sprinting,
        air,
        climbing,
        crouching,
        sliding,
    }

    public bool sliding;

    public bool freeze;
    public bool unlimited;
    public bool climbing;
    public bool restricted;



    [Header("References")]
    public Climbing climbingScript;

    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;

    public float slideSPeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    public float jumpFroce = 10.0f;
    public float climbspeed;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;


    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;


    [Header("Move Direction")]
    public Transform orientation;
    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startYScale = transform.localScale.y;

    }

    private void Update()
    {

        //ground check

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();
        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;



    }
    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void StateHandler()
    {

        if (sliding)
        {
            State = MovementState.sliding;

            if (OnSlope() && rb.velocity.y < 0.1f) desiredMoveSpeed = slideSPeed;

            else desiredMoveSpeed = sprintSpeed;

        }

        else if (Input.GetKey(crouchKey))
        {
            State = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }
        else if (grounded && Input.GetKey(sprintKey))
        {
            State = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }

        else if (grounded)
        {
            State = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        else
        {
            State = MovementState.air;
        }

        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }
        lastDesiredMoveSpeed = desiredMoveSpeed;
    }
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            Vector3 jumpDirection = Vector3.up * jumpFroce;
            rb.AddForce(jumpDirection, ForceMode.VelocityChange);
        }



        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }


    }
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
            time += Time.deltaTime;
            yield return null;
        }
        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);
            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);

        }
        if (climbingScript.extitingWall) return;
        if (restricted) return;
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        //on ground
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {

        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        //liminting speed on gorun or in air

        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
}

