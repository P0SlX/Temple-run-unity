using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public Image gameOverCanvas;
    public TMP_InputField pseudoInputGameOver;
    public TMP_InputField pseudoInputFinished;
    public Image finishedCanvas;

    public static bool IsGameOver;
    public static bool IsFinished;
    public static bool DodgeRemaining = true;
    public static int Score;

    private float _timer;

    private void Start()
    {
        // Reset the game
        healthText.text = "2";
        scoreText.text = "0";
    }

    private void Update()
    {
        UpdateHUD();
        Timer();
    }

    public void Retry()
    {
        // Reset the game and reload the level
        SaveScore();
        gameOverCanvas.gameObject.SetActive(false);
        IsGameOver = false;
        IsFinished = false;
        DodgeRemaining = true;
        Score = 0;
        SceneManager.LoadScene("Level");
    }

    public void Quit()
    {
        // Reset the game and load the menu
        SaveScore();
        gameOverCanvas.gameObject.SetActive(false);
        IsGameOver = false;
        IsFinished = false;
        DodgeRemaining = true;
        Score = 0;
        SceneManager.LoadScene("Menu");
    }

    private void SaveScore()
    {
        // Save the score in the PlayerPrefs
        // The score is saved in the format "pseudo:score"
        // If the player didn't enter a pseudo, "Anonyme" is used
        var pseudoGameOver = pseudoInputGameOver.text;
        var pseudoFinished = pseudoInputFinished.text;

        string pseudo;
        if (pseudoGameOver == "") 
            pseudo = pseudoFinished == "" ? "Anonyme" : pseudoFinished;
        else
            pseudo = pseudoGameOver;

        var score = Score.ToString();
        var storedScore = PlayerPrefs.GetString("scores");

        if (storedScore != "")
        {
            storedScore += ";";
        }

        storedScore += pseudo + ":" + score;
        PlayerPrefs.SetString("scores", storedScore);
        PlayerPrefs.Save();
        
        // Reset the input fields
        pseudoInputGameOver.text = "";
        pseudoInputFinished.text = "";
    }

    private void Timer()
    {
        // Update the timer text in the HUD
        if (IsGameOver || IsFinished) return;
        _timer += Time.deltaTime;

        var minutes = Mathf.FloorToInt(_timer / 60F);
        var seconds = Mathf.FloorToInt(_timer % 60F);

        timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    private void UpdateHUD()
    {
        // Update life and score text in the HUD
        if (DodgeRemaining)
        {
            healthText.text = "2";
        }
        else
        {
            if (IsGameOver)
            {
                healthText.text = "0";
                gameOverCanvas.gameObject.SetActive(true);
            }
            else healthText.text = "1";
        }

        if (IsFinished)
        {
            finishedCanvas.gameObject.SetActive(true);
        }

        scoreText.text = Score.ToString();
    }

    public static void AddScore(int score, GameObject collectible)
    {
        // Add score to the total score
        // Then disable the collectible
        Score += score;
        collectible.SetActive(false);
    }
}