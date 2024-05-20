using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playObj;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Slidng")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;

  

    private void Start()
    {
        rb= GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        startYScale = playObj.localScale.y;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

       if(Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0))
        {
            StartSlide();
        }

       if(Input.GetKeyUp(slideKey) && pm.sliding)
        {
            StopSLide();
        }
    }

    private void FixedUpdate()
    {
        if(pm.sliding) SlidingMovement();
    }

    private void StartSlide()
    {
        pm.sliding = true;

        playObj.localScale = new Vector3(playObj.localScale.x, slideYScale, playObj.localScale.z);
        rb.AddForce(Vector3.down *5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(!pm.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }

        else
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }


        if(slideTimer <= 0)
        {
            StopSLide() ;
        }
    }

    private void StopSLide()
    {
        playObj.localScale = new Vector3(playObj.localScale.x, startYScale, playObj.localScale.z);
        pm.sliding = false;
    }
}
