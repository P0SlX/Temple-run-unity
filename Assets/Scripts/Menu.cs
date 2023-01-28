using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] public GameObject settingsPanel;
    [SerializeField] public GameObject difficultyPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
