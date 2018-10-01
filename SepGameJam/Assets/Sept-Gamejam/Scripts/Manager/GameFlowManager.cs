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

	[SerializeField]
	private GameObject _player;

	[SerializeField]
	private Camera _mainCamera;

	private int _score;
	private string _defaultName = "player1";

	private GameUI _playGameUI;

	protected override void Awake () {
		base.Awake();
		if(_gameUIPrefab != null) {
			var menu = Instantiate(_gameUIPrefab);
			_playGameUI = menu.GetComponentsInChildren<GameUI>(true)[0];
			//DontDestroyOnLoad(menu);
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

	public void StartGame() {
		_player.SetActive(true);
		//switch to main camera
		_mainCamera.gameObject.SetActive(true);
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
	}

	public void IncreasePlayerLifeUI(float amount) {
		_playGameUI.AddLife(amount);
	}
}
