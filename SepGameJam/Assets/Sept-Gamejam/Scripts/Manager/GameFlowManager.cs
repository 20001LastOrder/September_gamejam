using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameFlowManager : ManagerBase<GameFlowManager> {
	[Serializable]
	class NameScorePair {
		public string playerName;
		public int playerScore;
	}

    public AudioSource bgMusic;

	[SerializeField]
	private GameObject _gameUI;

    [SerializeField]
    private GameObject _dialoguePrefab;

    [SerializeField]
	SerializedRankingBoard _serializeRankingBoard;

	[SerializeField]
	private GameObject _player;

    [SerializeField]
    private TreasureScript cube;

    private Dictionary<NPCType, NPCscript> npcs;

	[SerializeField]
	private Camera _mainCamera;

	private int _score;
	private string _defaultName = "player1";

	private GameUI _playGameUI;

    private bool hasKey = false;
    private bool hasCube = false;

    private int currentGold = 0;
    private int currentRuby = 0;

    private int randomID;

    protected override void Awake () {
		base.Awake();

		_playGameUI = _gameUI.GetComponentsInChildren<GameUI>(true)[0];
		//DontDestroyOnLoad(menu);

        if (_dialoguePrefab != null)
        {
            var dialogueParser = Instantiate(_dialoguePrefab);
            DontDestroyOnLoad(dialogueParser);
        }

        randomID = Random.Range(0, 100);
        npcs = new Dictionary<NPCType, NPCscript>();
    }

    private void Start()
    {
        bgMusic.Play();
    }

    public void AddNewPlayer(string name, int score) {
		_serializeRankingBoard.AddPair(name, score);
	}
	
	public void AddScore(int points) {
		//update socre in ui
		_playGameUI.UpdateScoreText(_score, points);
		_score += points;
	}

    public void RegisterNpc(NPCType title, NPCscript npc)
    {
        npcs.Add(title, npc);
    }

    public void ObtainKey()
    {
        hasKey = true;
        _playGameUI.UpdateKey(); 
        GameObject.Find("LockedDoor").GetComponent<DoorScript>().Unlock();
    }

    public void ObtainCube()
    {
        hasCube = true;
        _playGameUI.UpdateCube(true);
        NPCEventManager.instance.ObtainCubeTrigger();
    }

    public void LoseCube()
    {
        cube.SetOwner(null);
        hasCube = false;
        _playGameUI.UpdateCube(false);
    }
    
    public void DemonObtainCube()
    {
        cube.SetOwner(npcs[NPCType.Demon].gameObject);
        StartCoroutine(npcs[NPCType.Demon].MoveTo(new Vector3(0, 50, 0)));
        hasCube = false;
        _playGameUI.UpdateCube(false);
    }

    public void UnlockFinalDoor()
    {
        GameObject obj = GameObject.Find("FinalDoor");
        obj.GetComponent<DoorScript>().Unlock();
    }

	public int GetScore() {
		return _score;
	}

	public void StartGame() {
		_player.SetActive(true);
        //switch to main camera
        _mainCamera.gameObject.SetActive(true);
        DialogueManager.instance.StartIntro();
	}

	public void Pause() {
		_mainCamera.gameObject.SetActive(false);
		_player.SetActive(false);
	}

	public void Resume() {
		_mainCamera.gameObject.SetActive(true);
		_player.SetActive(true);
	}

	public void DecreasePlayerLifeUI(float amount) {
		_playGameUI.DecreaseLife(amount);
	}

	public void GameOver() {
		_playGameUI.GameOver();
		_player.SetActive(false);
        cube.gameObject.SetActive(false);
	}

	public void IncreasePlayerLifeUI(float amount) {
		_playGameUI.AddLife(amount);
	}

    public void UpdateRuby(int i)
    {
        switch (i)
        {
            case 0:
                currentGold++;
                _playGameUI.UpdateGoldCount(currentGold);
                break;
            case 1:
                currentRuby++;
                _playGameUI.UpdateRubyCount(currentRuby);
                break;
        }
    }

    public int getGold()
    {
        return currentGold;
    }

    public int getRuby()
    {
        return currentRuby;
    }

    public bool getKey()
    {
        return hasKey;
    }

    // game wins
    public void Win()
    {
        _playGameUI.Win();
        _player.SetActive(false);
    }

    public void Lose()
    {
        _player.GetComponent<PlayerHealth>().LoseHeath(3);
    }

    public void GoToLink()
    {
        Debug.Log("go to link");
        Application.OpenURL("https://casual-development-dogdays.c9users.io/" + randomID + "/game");
        
        _playGameUI.ShowPanel();

        
    }

    public int GetRandomId()
    {
        return randomID;
    }

    public void UICreateSpellKeys(List<string> spell)
    {
        _playGameUI.CreateSpellKeys(spell);
    }

    public void UIResetSpellKeys()
    {
        _playGameUI.ResetSpellKeys();
    }

    public void UIRemoveSpellKey()
    {
        _playGameUI.RemoveSpellKey();
    }

    public void EnterMainScene()
    {
        _playGameUI.UIEnterMainScene();
    }
}
