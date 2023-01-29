using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour
{
    private void Start()
    {
        // Create the dropdown menu
        var dropdown = transform.GetComponent<Dropdown>();
        
        // Clear before adding options
        dropdown.options.Clear();

        // List all options
        var options = new List<string>()
        {
            "WASD",
            "ZQSD",
            "Flèches directionnelles"
        };
        
        // Add options to the dropdown
        foreach (var option in options)
        {
            dropdown.options.Add(new Dropdown.OptionData() {text = option});
        }
        
        // Set the default value
        DropdownValueChanged(dropdown);
        
        // Add listener for when the value of the Dropdown changes, to take action
        dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(dropdown); });
    }

    private void DropdownValueChanged(Dropdown dropdown)
    {
        // Get the selected option
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
        
        // Save the changes
        PlayerPrefs.Save();
    }
}
