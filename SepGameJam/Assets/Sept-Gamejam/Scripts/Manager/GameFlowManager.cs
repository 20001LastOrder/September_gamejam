using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : ManagerBase<GameFlowManager> {
	[Serializable]
	class NameScorePair {
		public string playerName;
		public int playerScore;
	}

	[SerializeField]
	private GameObject _gameUIPrefab;

    [SerializeField]
    private GameObject _dialoguePrefab;

    [SerializeField]
	SerializedRankingBoard _serializeRankingBoard;

	private int _score;
	private string _defaultName = "player1";

	private GameUI _playGameUI;

	protected override void Awake () {
		base.Awake();
		if(_gameUIPrefab != null) {
			var menu = Instantiate(_gameUIPrefab);
			_playGameUI = menu.GetComponent<GameUI>();
			DontDestroyOnLoad(menu);
		}

        if (_dialoguePrefab != null)
        {
            var dialogueParser = Instantiate(_dialoguePrefab);
            DontDestroyOnLoad(dialogueParser);
        }
    }

	public void AddNewPlayer(string name, int score) {
		_serializeRankingBoard.AddPair(name, score);
	}
	
	public void AddScore(int points) {
		//update socre in ui
		_playGameUI.UpdateScoreText(_score, points);
		_score += points;
	}

	public int GetScore() {
		return _score;
	}

	public void ChangeScene(string sceneName) {
		SceneManager.LoadScene(sceneName);
	}

	public void DecreasePlayerLifeUI(float amount) {
		_playGameUI.DecreaseLife(amount);
	}

	public void IncreasePlayerLifeUI(float amount) {
		_playGameUI.AddLife(amount);
	}
}
