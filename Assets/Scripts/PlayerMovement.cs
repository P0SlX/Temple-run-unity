using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static readonly int Jump = Animator.StringToHash("jump");
    private static readonly int Roll = Animator.StringToHash("roll");
    private static readonly int Dead = Animator.StringToHash("dead");
    public float speed = 7.0f;
    public Vector3[] lanes;
    public int currentLane = 1;
    private bool _goesLeft, _goesRight, _jumps, _slides;
    private bool _isGrounded = true;


    private Animator _myAnimator;
    private Rigidbody _myRigidbody;

    // Start is called before the first frame update
    private void Start()
    {
        _myAnimator = GetComponent<Animator>();
        _myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        _goesLeft = Input.GetKeyDown(KeyCode.A);
        _goesRight = Input.GetKeyDown(KeyCode.D);
        _jumps = Input.GetKeyDown(KeyCode.W);
        _slides = Input.GetKeyDown(KeyCode.S);

        if (_jumps && _isGrounded)
        {
            _myRigidbody.AddRelativeForce(Vector3.up * 6, ForceMode.Impulse);
            _myAnimator.SetTrigger(Jump);
            _isGrounded = false;
        }
        else if (_slides)
        {
            _myRigidbody.AddRelativeForce(Vector3.up * -6, ForceMode.Impulse);
            _myAnimator.SetTrigger(Roll);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            _myAnimator.SetTrigger(Dead);
        }
        else if (_goesLeft)
        {
            if (currentLane < lanes.Length - 1) currentLane++;
        }
        else if (_goesRight)
        {
            if (currentLane > 0) currentLane--;
        }
        
        // Déplacer le personnage vers les cotés si on change de lane et vers l'avant constamment
        var persoPos = transform.position;
        var nextPersoPosX = persoPos.x + speed * Time.deltaTime;
        lanes[currentLane] = new Vector3(nextPersoPosX, persoPos.y, lanes[currentLane].z);
        transform.position = new Vector3(nextPersoPosX, persoPos.y,
            Vector3.Lerp(
                persoPos,
                lanes[currentLane],
                speed * Time.deltaTime).z
        );
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Ground")) return;
        _isGrounded = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Coin":
                GameManager.Score += 50;
                break;
            case "Emerald":
                GameManager.Score += 100;
                break;
            case "Ruby":
                GameManager.Score += 200;
                break;
            case "Diamond":
                GameManager.Score += 500;
                break;
            case "DiamondBlack":
                GameManager.Score -= 500;
                break;
        }
    }
}