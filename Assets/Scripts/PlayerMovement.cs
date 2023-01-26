using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static readonly int Jump = Animator.StringToHash("jump");
    private static readonly int Roll = Animator.StringToHash("roll");
    private static readonly int Dead = Animator.StringToHash("dead");
    public float speed = 7.0f;
    public Vector3[] lanes;
    public int currentLane = 1;
    private Vector3 forwardVector;
    private bool goesLeft, goesRight, jumps, slides;
    private bool isGrounded = true;


    private Animator myAnimator;
    private Rigidbody myRigidbody;

    // Start is called before the first frame update
    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody>();
        forwardVector = Vector3.forward * speed;
    }

    // Update is called once per frame
    private void Update()
    {
        goesLeft = Input.GetKeyDown(KeyCode.A);
        goesRight = Input.GetKeyDown(KeyCode.D);
        jumps = Input.GetKeyDown(KeyCode.W);
        slides = Input.GetKeyDown(KeyCode.S);

        if (jumps && isGrounded)
        {
            myRigidbody.AddRelativeForce(Vector3.up * 6, ForceMode.Impulse);
            myAnimator.SetTrigger(Jump);
            isGrounded = false;
        }
        else if (slides)
        {
            myRigidbody.AddRelativeForce(Vector3.up * -6, ForceMode.Impulse);
            myAnimator.SetTrigger(Roll);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            myAnimator.SetTrigger(Dead);
        }
        else if (goesLeft)
        {
            if (currentLane < lanes.Length - 1) currentLane++;
        }
        else if (goesRight)
        {
            if (currentLane > 0) currentLane--;
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
}