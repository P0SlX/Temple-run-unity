using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int Score = 0;
    public static bool IsGameOver = false;
    public static bool IsFinished = false;
    public static bool DodgeRemaining = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public static void AddScore(int score, GameObject collectible)
    {
        Score += score;
        collectible.SetActive(false);
    }
}
