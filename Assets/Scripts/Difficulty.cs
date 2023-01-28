using UnityEngine;
using UnityEngine.SceneManagement;

public class Difficulty : MonoBehaviour
{
    [SerializeField] public GameObject difficultyPanel;
    public static int difficulty = 1;
    public static bool IsInfinit;

    public void SetEasyMode()
    {
        difficulty = 1;
        IsInfinit = false;
        StartGame();
    }

    public void SetMediumMode()
    {
        difficulty = 2;
        IsInfinit = false;
        StartGame();
    }

    public void SetHardMode()
    {
        difficulty = 3;
        IsInfinit = false;
        StartGame();
    }
    
    public void SetHardCoreMode()
    {
        difficulty = 3;
        StartGame();
    }
    
    public void SetInfinitMode()
    {
        difficultyPanel.SetActive(false);
    }

    public void StartGame()
    {
        HideDifficulty();
        SceneManager.LoadScene("Level");
    }

    public void HideDifficulty()
    {
        difficultyPanel.SetActive(false);
    }
}