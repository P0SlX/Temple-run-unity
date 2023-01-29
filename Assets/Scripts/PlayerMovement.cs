using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Animations
    private static readonly int Jump = Animator.StringToHash("jump");
    private static readonly int Roll = Animator.StringToHash("roll");
    private static readonly int Dead = Animator.StringToHash("dead");
    private static readonly int Finish = Animator.StringToHash("finish");

    public float speed = 7.0f;

    // Possible lanes (left, middle, right)
    public Vector3[] lanes;
    public int currentLane = 1;

    private bool _goesLeft, _goesRight, _jumps, _roll;
    private bool _isGrounded = true;

    private Animator _myAnimator;
    private Rigidbody _myRigidbody;
    private CapsuleCollider _myCollider;

    private void Start()
    {
        _myAnimator = GetComponent<Animator>();
        _myRigidbody = GetComponent<Rigidbody>();
        _myCollider = GetComponent<CapsuleCollider>();

        // If Game is in infinite mode, increase speed with time
        if (DifficultyHandler.IsInfinit)
        {
            InvokeRepeating(nameof(IncreaseSpeed), 0, 1);
        }
        // Else, increase speed according with difficulty
        else
        {
            speed *= DifficultyHandler.Difficulty;
        }
    }

    private void Update()
    {
        if (HUD.IsGameOver || HUD.IsFinished) return;

        // Read input from keyboard with PlayerPrefs
        _goesLeft = Input.GetKeyDown(PlayerPrefs.GetString("left"));
        _goesRight = Input.GetKeyDown(PlayerPrefs.GetString("right"));
        _jumps = Input.GetKeyDown(PlayerPrefs.GetString("jump"));
        _roll = Input.GetKeyDown(PlayerPrefs.GetString("roll"));

        if (_jumps && _isGrounded)
        {
            // Jump if player is on the ground
            // Trigger animation and add force to rigidbody
            _myRigidbody.AddRelativeForce(Vector3.up * 6, ForceMode.Impulse);
            _myAnimator.SetTrigger(Jump);
            _isGrounded = false;
        }
        else if (_roll)
        {
            // Add force to rigidbody to propel player towards the ground
            _myRigidbody.AddRelativeForce(Vector3.up * -6, ForceMode.Impulse);
            _myAnimator.SetTrigger(Roll);

            // Make collider smaller when rolling
            _myCollider.height = 0.5f;
            _myCollider.center = new Vector3(0, 0.27f, 0);
        }

        // Switching lanes
        else if (_goesLeft)
        {
            if (currentLane < lanes.Length - 1) currentLane++;
        }
        else if (_goesRight)
        {
            if (currentLane > 0) currentLane--;
        }


        var persoPos = transform.position;

        // Compute next position
        var nextPersoPosX = persoPos.x + speed * Time.deltaTime;

        // Update position of player according to current lane
        lanes[currentLane] = new Vector3(nextPersoPosX, persoPos.y, lanes[currentLane].z);

        // Lerp to next position to make movement smooth
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
                HUD.AddScore(50, other.gameObject);
                break;
            case "Emerald":
                HUD.AddScore(100, other.gameObject);
                break;
            case "Ruby":
                HUD.AddScore(200, other.gameObject);
                break;
            case "Diamond":
                HUD.AddScore(500, other.gameObject);
                break;
            case "DiamondBlack":
                HUD.AddScore(-500, other.gameObject);
                break;
            case "FinishLine":
                HUD.IsFinished = true;
                _myAnimator.SetTrigger(Finish);
                break;
            case "Obstacle":
                // If player has a dodge remaining, use it
                if (HUD.DodgeRemaining) HUD.DodgeRemaining = false;
                else
                {
                    _myAnimator.SetTrigger(Dead);
                    HUD.IsGameOver = true;
                }
                break;
        }
    }

    private void ResetCollider()
    {
        // Reset collider size when not rolling
        _myCollider.height = 1.897241f;
        _myCollider.center = new Vector3(6.792558e-17f, 0.9486203f, -0.02647015f);
    }

    private void IncreaseSpeed()
    {
        // Increase speed by 0.05 every second if game is in infinite mode
        speed += 0.05f;
    }
}