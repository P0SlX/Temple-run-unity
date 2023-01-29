using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public Image gameOverCanvas;
    public TMP_InputField pseudoInput;

    private float _timer;

    // Start is called before the first frame update
    void Start()
    {
        healthText.text = "2";
        scoreText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.DodgeRemaining)
        {
            healthText.text = "2";
        }
        else
        {
            if (GameManager.IsGameOver)
            {
                healthText.text = "0";
                gameOverCanvas.gameObject.SetActive(true);
            }
            else healthText.text = "1";
        }
        
        scoreText.text = GameManager.Score.ToString();
        Timer();
    }

    public void Retry()
    {
        SaveScore();
        SceneManager.LoadScene("Level");
    }

    public void Quit()
    {
        SaveScore();
        SceneManager.LoadScene("Menu");
    }

    private void SaveScore()
    {
        var pseudo = pseudoInput.text;
        
        if (pseudo == "") pseudo = "Anonyme";
        
        var score = GameManager.Score.ToString();
        PlayerPrefs.SetString(pseudo, score);
    }

    private void Timer()
    {
        if (GameManager.IsGameOver || GameManager.IsFinished) return;
        _timer += Time.deltaTime;
        var minutes = Mathf.FloorToInt(_timer / 60F);
        var seconds = Mathf.FloorToInt(_timer % 60F);
        timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}