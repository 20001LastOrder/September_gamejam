using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChoiceButton : MonoBehaviour
{

    public string option;
    public DialogueManager box;


    public void SetText(string newText)
    {
        this.GetComponentInChildren<Text>().text = newText;
    }

    public void SetOption(string newOption)
    {
        this.option = newOption;
    }

    public void ParseOption()
    {
        string command = option.Split(',')[0];
        string commandModifier = option.Split(',')[1];
        if (command == "line")
        {
            box.ClearText();
            box.lineNum = int.Parse(commandModifier);
            box.ShowDialogue();
            box.lineNum++;
        }
        else if (command == "scene")
        {
            SceneManager.LoadScene("Scene" + commandModifier);
        }
    }
}