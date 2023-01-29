using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyHandler : MonoBehaviour
{
    [SerializeField] public GameObject difficultyPanel;
    public static int Difficulty = 1;
    public static bool IsInfinit;

    public void SetEasyMode()
    {
        Difficulty = 1;
        IsInfinit = false;
        StartGame();
    }

    public void SetMediumMode()
    {
        Difficulty = 2;
        IsInfinit = false;
        StartGame();
    }

    public void SetHardMode()
    {
        Difficulty = 3;
        IsInfinit = false;
        StartGame();
    }
    
    public void SetHardCoreMode()
    {
        Difficulty = 3;
        StartGame();
    }
    
    public void SetInfinitMode()
    {
        IsInfinit = true;
        StartGame();
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