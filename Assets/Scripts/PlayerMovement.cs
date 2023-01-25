using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool goesLeft, goesRight, jumps, slides;
    public float speed = 7.0f;
    private Rigidbody myRigidbody;
    private static readonly int Jump = Animator.StringToHash("jump");
    private static readonly int Roll = Animator.StringToHash("roll");
    private static readonly int Dead = Animator.StringToHash("dead");
    private bool isGrounded = true;
    public Vector3[] lanes;
    public int currentLane = 1;
    private Vector3 forwardVector;


    private Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody>();
        forwardVector = Vector3.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        goesLeft = Input.GetKeyDown(KeyCode.A);
        goesRight = Input.GetKeyDown(KeyCode.D);
        jumps = Input.GetKeyDown(KeyCode.W);
        slides = Input.GetKeyDown(KeyCode.S);

        if (jumps && isGrounded)
        {
            myRigidbody.AddRelativeForce(Vector3.up * 5, ForceMode.Impulse);
            myAnimator.SetTrigger(Jump);
        }
        else if (slides)
        {
            myRigidbody.AddRelativeForce(-(Vector3.up * 5), ForceMode.Impulse);
            myAnimator.SetTrigger(Roll);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            myAnimator.SetTrigger(Dead);
        }
        else if (goesLeft)
        {
            if (currentLane < lanes.Length - 1)
            {
                currentLane++;
            }
        }
        else if (goesRight)
        {
            if (currentLane > 0)
            {
                currentLane--;
            }
        }

        // Avancer le personnage
        transform.Translate(forwardVector * Time.deltaTime);
        
        // Déplacer le personnage vers les cotés si on change de lane
        var persoPos = transform.position;
        lanes[currentLane] = new Vector3(persoPos.x, persoPos.y, lanes[currentLane].z);
        transform.position = new Vector3(persoPos.x, persoPos.y,
            Vector3.Lerp(
                persoPos,
                lanes[currentLane],
                speed * Time.deltaTime).z
        );
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Ground")) return;
        isGrounded = true;
    }

    private void OnCollisionExit(Collision other)
    {
        if (!other.gameObject.CompareTag("Ground")) return;
        isGrounded = false;
    }
}