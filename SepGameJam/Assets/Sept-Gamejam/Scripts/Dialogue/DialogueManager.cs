using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance = null;
    DialogueParser parser;

    //animator of the character talking
    public Animator anim;
    public string dialogue, characterName, day;
    public int lineNum;
    string[] options;
    public float delayTime = 0.3f;
    List<Button> buttons = new List<Button>();
    //not sure if I need it
    public bool playerTalking;
    public bool hasChoice = false;
    private bool isTyping;
    public bool isActive;
    private NPCscript npc;
    public bool nextLine;

    public Text dialogueBox;
    public Text nameBox;
    public GameObject choiceBox;
    public AudioSource aud;
    public AudioClip typeSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        isActive = false;
        isTyping = false;
        dialogue = "";
        characterName = "";
        day = "";
        playerTalking = false;
        parser = GameObject.FindObjectOfType<DialogueParser>();
        npc = GameObject.Find("NPC").GetComponent<NPCscript>();
        lineNum = 0;
       // menu = GetComponent<MenuManager>();
        anim.GetComponent<Animator>();
        //aud.GetComponent<AudioSource>();
    }

    public void Show(int numLine)
    {
        lineNum = numLine;

        StartCoroutine(Dialogue());
    }

    IEnumerator Dialogue()
    {
        // activate the update function
        SetActive(true);

        while (isActive)
        {
            Debug.Log("next line before = " + nextLine);
            yield return new WaitUntil(() => nextLine == true);

            if (playerTalking == false)
            {
                nextLine = true;

                ShowDialogue();

                lineNum++;
            }
            Debug.Log("next line = " + nextLine);

            UpdateUI();

            nextLine = false;
                     
        }

        npc.EndState();
    }

    public void ShowDialogue()
    {
        ParseLine();
    }

    void ParseLine()
    {

        if (parser.GetName(lineNum) != "Player")
        {
            playerTalking = false;
            ClearButtons();
            characterName = parser.GetName(lineNum);
            dialogue = parser.GetContent(lineNum);
            //dialogue = dialogue.Replace ('$', '\n');
            StartCoroutine(TypeWriterEffect());
        }
        else if (parser.GetName(lineNum) == "End")
        {
            SetActive(false);
        }
        else
        {
            playerTalking = true;
            options = parser.GetOptions(lineNum);
            CreateButtons();
        }
    }

    void CreateButtons()
    {
        for (int i = 0; i < options.Length; i++)
        {
            hasChoice = true;
            GameObject button = (GameObject)Instantiate(choiceBox);
            Button b = button.GetComponent<Button>();
            ChoiceButton cb = button.GetComponent<ChoiceButton>();
            cb.SetText(options[i].Split(':')[0]);
            cb.option = options[i].Split(':')[1];
            cb.box = this;
            b.transform.SetParent(this.transform);
            b.transform.localPosition = new Vector3(-200 + (i * 400), -100);
            b.transform.localScale = new Vector3(1, 1, 1);
            buttons.Add(b);
        }
    }

    void UpdateUI()
    {
        if (!playerTalking)
        {
            Debug.Log("player is not talking, clear buttons");
            hasChoice = false;
            ClearButtons();
        }

        //dialogueBox.text = dialogue;
        nameBox.text = characterName;
    }

    void ClearButtons()
    {
        foreach (Button b in buttons)
        {
            print(buttons.Count);
            Destroy(b.gameObject);
        }

        buttons.Clear();
    }

    public void ClearText()
    {
        dialogue = "";
        characterName = "";
    }

    private IEnumerator TypeWriterEffect()
    {
        isTyping = true;
        for (int i = 0; i < dialogue.Length + 1; i++)
        {
            dialogueBox.text = dialogue.Substring(0, i);
            //aud.clip = typeSound;
            //aud.Play();
            yield return new WaitForSeconds(delayTime);
        }
        isTyping = false;
        StopCoroutine(TypeWriterEffect());
    }

    public void SetActive(bool b)
    {
        if (b == false)
        {
            Debug.Log("reset buttons and text");
            playerTalking = false;
            hasChoice = false;
            nextLine = false;
            ClearButtons();
            ClearText();

            dialogueBox.text = dialogue;
            nameBox.text = characterName;
        }

        isActive = b;
    }
}
