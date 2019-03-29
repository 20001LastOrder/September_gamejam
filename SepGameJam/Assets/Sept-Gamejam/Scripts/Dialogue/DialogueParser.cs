using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

public class DialogueParser : MonoBehaviour
{

    struct DialogueLine
    {
        public string name;
        public string content;
        public string extra;
        public string[] options;

        public DialogueLine(string Name, string Content, string Extra)
        {
            name = Name;
            content = Content;
            extra = Extra;
            options = new string[0];
        }
    }

    List<DialogueLine> lines;

    void Start()
    {
        string file = "Assets/Sept-Gamejam/Data/Dialogue";
        string sceneNum = SceneManager.GetActiveScene().name;
        sceneNum = Regex.Replace(sceneNum, "[^0-9]", "");
        file += sceneNum;
        file += ".txt";

        lines = new List<DialogueLine>();
        
        LoadDialogue(file);
    }

    void LoadDialogue(string filename)
    {
        string line;
        StreamReader r = new StreamReader(filename);

        using (r)
        {
            do
            {
                line = r.ReadLine();
                if (line != null)
                {
                    string[] lineData = line.Split(';');
                    if (lineData[0] == "Player")
                    {
                        DialogueLine lineEntry = new DialogueLine(lineData[0], "", "");
                        lineEntry.options = new string[lineData.Length - 1];

                        for (int i = 1; i < lineData.Length; i++)
                        {
                            lineEntry.options[i - 1] = lineData[i];
                        }
                        lines.Add(lineEntry);
                    }
                    else if (lineData[0] == "")
                    {
                        DialogueLine lineEntry = new DialogueLine("", "", "");
                        lines.Add(lineEntry);
                    }
                    else
                    {
                        DialogueLine lineEntry = new DialogueLine(lineData[0], lineData[1], lineData[2]);
                        lines.Add(lineEntry);
                    }
                }
            } while (line != null);
            r.Close();
        }
    }

    public string GetName(int lineNumber)
    {
        if (lineNumber < lines.Count)
            return lines[lineNumber].name;

        return "";
    }

    public string GetContent(int lineNumber)
    {
        if (lineNumber < lines.Count)
            return lines[lineNumber].content;

        return "";
    }

    public string GetTrigger(int lineNumber)
    {
        if (lineNumber < lines.Count)
            return lines[lineNumber].extra;

        return "";
    }

    public string[] GetOptions(int lineNumber)
    {
        if (lineNumber < lines.Count)
            return lines[lineNumber].options;

        return new string[0];
    }
}