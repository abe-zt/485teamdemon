using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class WowStylePlayerController : MonoBehaviour {

    //Transform player;
    //public WowStylePlayerController player;

    const float locomotionAnimationSmoothTime = .1f;

    NavMeshAgent agent;

    public float speedPercent;

    Animator animator;
    Camera cam;
    //PlayerMotor motor;

    public float turnSpeed = 20.0f;
    public float moveSpeed = 20.0f;
    public float mouseTurnMultiplier = 1;

    private float x = 0.0f;
    private float z = 0.0f;


    // Use this for initialization
    void Start () {
        //cam = Camera.main;
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        //agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }
	
	// Update is called once per frame
    void Update()
    {
         speedPercent = 0;

        // x is used for the x axis.  set it to zero so it doesn't automatically rotate
        x = 0;

        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            speedPercent = 1f;
            z = Time.deltaTime * moveSpeed;

            // Get the difference in horizontal mouse movement
            x = Input.GetAxis("Mouse X") * turnSpeed * mouseTurnMultiplier;

            // Move the character forwards or backwards
            transform.Translate(0, 0, z);
            transform.Rotate(0, x, 0);

        }
        else
        {
            // check to see if the W or S key is being pressed.  
            if (Input.GetAxis("Vertical") != 0)
            {
                speedPercent = 1f;
                z = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
                // Move the character forwards or backwards
                transform.Translate(0, 0, z);
                //transform.Rotate(0, x, 0);
            }
        }

        if (Input.GetMouseButton(1) && !Input.GetMouseButton(0))
        {
            speedPercent = .3f;
            // Get the difference in horizontal mouse movement
            x = Input.GetAxis("Mouse X") * turnSpeed * mouseTurnMultiplier;
            transform.Rotate(0, x, 0);
        }

        // Check to see if the A or S key are being pressed
        else if (Input.GetAxis("Horizontal") != 0)
        {
            speedPercent = .3f;
            // Get the A or S key (-1 or 1)
            x = Input.GetAxis("Horizontal");
            transform.Rotate(0, x, 0);
        }



        // Check to see if the right mouse button is pressed


        // rotate the character based on the x value
        //transform.Rotate(0, x, 0);

        animator.SetFloat("speedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);

    }
}

