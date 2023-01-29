using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var dropdown = transform.GetComponent<Dropdown>();
        
        dropdown.options.Clear();

        List<string> options = new List<string>()
        {
            "WASD",
            "ZQSD",
            "Flèches directionnelles"
        };
        
        foreach (var option in options)
        {
            dropdown.options.Add(new Dropdown.OptionData() {text = option});
        }
        
        DropdownValueChanged(dropdown);
        
        dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(dropdown); });
    }

    
    void DropdownValueChanged(Dropdown dropdown)
    {
        var option = dropdown.options[dropdown.value].text;

        // Write keybindings in PlayerPrefs
        switch (option)
        {
            case "WASD":
                PlayerPrefs.SetString("jump", "w");
                PlayerPrefs.SetString("roll", "s");
                PlayerPrefs.SetString("left", "a");
                PlayerPrefs.SetString("right", "d");
                break;
            case "ZQSD":
                PlayerPrefs.SetString("jump", "z");
                PlayerPrefs.SetString("roll", "s");
                PlayerPrefs.SetString("left", "q");
                PlayerPrefs.SetString("right", "d");
                break;
            case "Flèches directionnelles":
                PlayerPrefs.SetString("jump", "up");
                PlayerPrefs.SetString("roll", "down");
                PlayerPrefs.SetString("left", "left");
                PlayerPrefs.SetString("right", "right");
                break;
        }
    }
}
