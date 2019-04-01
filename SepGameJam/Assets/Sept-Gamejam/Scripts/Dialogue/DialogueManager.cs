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
    [SerializeField]
    private bool isTyping;
    public bool isActive;
    private NPCscript npc;
    public bool nextLine;

    private string trigger;
    private bool hasTrigger;
    private bool intro = false;

    public Text dialogueBox;
    public Text nameBox;
    public GameObject choiceBox;
    public AudioSource aud;
    public AudioClip typeSound;

    private Coroutine cur_Coroutine;
    private float last;

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
        this.gameObject.GetComponent<Image>().enabled = false;
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

    public void StartIntro()
    {
        intro = true;
        nextLine = true;
        Show(0);
    }

    private void Update()
    {
        if (intro)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isTyping && Time.time - last > 0.5f)
                {
                    nextLine = true;
                }
                else if (isTyping)
                {
                    StopCoroutine(cur_Coroutine);
                    dialogueBox.text = dialogue;
                    Debug.Log(dialogue);
                    isTyping = false;
                    nextLine = false;
                    last = Time.time;
                }
            }
        }

        if (!isTyping && hasTrigger)
        {
            switch (trigger)
            {
                case "EnterMainScene":
                    GameFlowManager.Instance.EnterMainScene();
                    break;
                default:
                    break;
            }
            
            hasTrigger = false;
            trigger = "";
        }
    }


    public void Show(int numLine)
    {
        Debug.Log(numLine);
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
            //yield return new WaitUntil(() => nextLine == true);
            if (nextLine)
            {
                gameObject.GetComponent<Image>().enabled = true;

                if (playerTalking == false)
                {
                    ShowDialogue();

                    lineNum++;
                }
                Debug.Log("next line = " + nextLine);

                UpdateUI();

                nextLine = false;
            }
            yield return new WaitForEndOfFrame();
        }

        npc.EndState();
    }

    public void ShowDialogue()
    {
        ParseLine();
    }

    void ParseLine()
    {

        if (parser.GetName(lineNum) == "Player")
        {
            playerTalking = true;
            options = parser.GetOptions(lineNum);
            CreateButtons();
        }
        else if (parser.GetName(lineNum) == "End")
        {
            switch (parser.GetContent(lineNum))
            {
                case "end1":
                    NPCEventManager.instance.FinishMeetUp();
                    SetActive(false);
                    break;
                case "endDoom":
                    // the evil wins the cube
                    GameFlowManager.Instance.LoseCube();
                    GameFlowManager.Instance.UnlockFinalDoor();
                    SetActive(false);
                    break;
                case "endBad":
                    // go to the website
                    Application.OpenURL("https://casual-development-dogdays.c9users.io/dead");
                    GameFlowManager.Instance.Lose();
                    SetActive(false);
                    break;
                default:
                    SetActive(false);
                    break;
            }
            
        }
        else if (parser.GetName(lineNum) == "Narrative")
        {
            playerTalking = false;
            ClearButtons();
            characterName = " ";
            dialogue = parser.GetContent(lineNum);
           
            //dialogue = dialogue.Replace ('$', '\n');
            cur_Coroutine = StartCoroutine(TypeWriterEffect());

            trigger = parser.GetTrigger(lineNum);
            if (trigger != "")
            {
                hasTrigger = true;
            }
        }
        else
        {
            playerTalking = false;
            ClearButtons();
            characterName = parser.GetName(lineNum);
            dialogue = parser.GetContent(lineNum);

            //dialogue = dialogue.Replace ('$', '\n');
            cur_Coroutine = StartCoroutine(TypeWriterEffect());

            trigger = parser.GetTrigger(lineNum);
            if (trigger != "")
            {
                hasTrigger = true;
            }
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
            b.transform.localPosition = new Vector3(-170 + (i * 320), 50);
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
            Debug.Log("typing typing");
            //aud.clip = typeSound;
            //aud.Play();
            yield return new WaitForSeconds(delayTime);
        }
        isTyping = false;
    }

    public void SetActive(bool b)
    {
        if (b == false)
        {
            this.gameObject.GetComponent<Image>().enabled = false;
            //Debug.Log("reset buttons and text");
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
