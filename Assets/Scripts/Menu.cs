using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] public GameObject settingsPanel;
    [SerializeField] public GameObject difficultyPanel;
    public GameObject scorePanel;
    public TextMeshProUGUI scoreText;
    public AudioSource audioSource;
    public Slider volumeSlider;

    private SortedDictionary<int, string> _scores;
    private List<TextMeshProUGUI> _scoreTexts;

    private void Start()
    {
        // Create a new list of TextMeshProUGUI
        _scoreTexts = new List<TextMeshProUGUI>();
        
        // Display the scores
        DisplayScores();
        
        // Set the volume slider to the volume of the audio source
        volumeSlider.onValueChanged.AddListener (delegate {SetVolume();});
        
        DontDestroyOnLoad(audioSource);
    }

    private void DisplayScores()
    {
        // Get the scores from PlayerPrefs
        var scores = PlayerPrefs.GetString("scores");
        if (scores == "") return;

        // Fetch all scores from PlayerPrefs (sort them by score)
        _scores = new SortedDictionary<int, string>();

        // Add the scores to the dictionary
        foreach (var score in scores.Split(";"))
        {
            try
            {
                var split = score.Split(":");
                _scores.Add(int.Parse(split[1]), split[0]);
            }
            catch
            {
                // La personne a déjà été ajoutée
            }
        }

        // Display the scores
        foreach (var score in _scores)
        {
            try
            {
                var textMeshProUGUI = Instantiate(scoreText, scorePanel.transform);
                textMeshProUGUI.text = score.Value + " : " + score.Key;
                _scoreTexts.Add(textMeshProUGUI);
            } catch
            {
                // La personne a déjà été ajoutée
            }
        }
    }

    public void StartGame()
    {
        difficultyPanel.SetActive(true);
    }

    public void Return()
    {
        difficultyPanel.SetActive(false);
    }

    public void Settings()
    {
        settingsPanel.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void HideSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void ResetScores()
    {
        // Reset the scores
        PlayerPrefs.SetString("scores", "");
        PlayerPrefs.Save();
        
        // Destroy the scores from the UI
        foreach (var score in _scoreTexts)
        {
            try
            {
                Destroy(score.gameObject);
            } catch
            {
                // La personne a déjà été supprimée
            }
        }
    }
    
    private void SetVolume()
    {
        // Set the volume of the game
        audioSource.volume = volumeSlider.value;
    }
}