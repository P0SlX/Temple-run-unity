using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static readonly int Jump = Animator.StringToHash("jump");
    private static readonly int Roll = Animator.StringToHash("roll");
    private static readonly int Dead = Animator.StringToHash("dead");
    public float speed = 7.0f;
    public Vector3[] lanes;
    public int currentLane = 1;
    
    private bool _goesLeft, _goesRight, _jumps, _roll;
    private bool _isGrounded = true;
    private Animator _myAnimator;
    private Rigidbody _myRigidbody;
    private CapsuleCollider _myCollider;

    // Start is called before the first frame update
    private void Start()
    {
        _myAnimator = GetComponent<Animator>();
        _myRigidbody = GetComponent<Rigidbody>();
        _myCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    private void Update()
    {
        _goesLeft = Input.GetKeyDown(KeyCode.A);
        _goesRight = Input.GetKeyDown(KeyCode.D);
        _jumps = Input.GetKeyDown(KeyCode.W);
        _roll = Input.GetKeyDown(KeyCode.S);

        if (_jumps && _isGrounded)
        {
            _myRigidbody.AddRelativeForce(Vector3.up * 6, ForceMode.Impulse);
            _myAnimator.SetTrigger(Jump);
            _isGrounded = false;
        }
        else if (_roll)
        {
            _myRigidbody.AddRelativeForce(Vector3.up * -6, ForceMode.Impulse);
            _myAnimator.SetTrigger(Roll);
            // Make collider smaller when rolling
            _myCollider.height = 0.5f;
            _myCollider.center = new Vector3(0, 0.27f, 0);
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
                GameManager.AddScore(50, other.gameObject);
                break;
            case "Emerald":
                GameManager.AddScore(100, other.gameObject);
                break;
            case "Ruby":
                GameManager.AddScore(200, other.gameObject);
                break;
            case "Diamond":
                GameManager.AddScore(500, other.gameObject);
                break;
            case "DiamondBlack":
                GameManager.AddScore(-500, other.gameObject);
                break;
        }
    }

    private void ResetCollider()
    {
        _myCollider.height = 1.897241f;
        _myCollider.center = new Vector3(6.792558e-17f, 0.9486203f, -0.02647015f);
    }
}