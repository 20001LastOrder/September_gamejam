using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : ManagerBase<GameFlowManager> {
	[Serializable]
	class NameScorePair {
		public string playerName;
		public int playerScore;
	}

	[SerializeField]
	private GameObject _gameUIPrefab;

	[SerializeField]
	SerializedRankingBoard _serializeRankingBoard;

	private int _score;
	private string _defaultName = "player1";

	private GameUI _playGameUI;

	void Start () {
		if(_gameUIPrefab != null) {
			var menu = Instantiate(_gameUIPrefab);
			_playGameUI = menu.GetComponent<GameUI>();
			DontDestroyOnLoad(menu);
		}
	}

	public void AddNewPlayer(string name, int score) {
		_serializeRankingBoard.AddPair(name, score);
	}
	
	public void AddScore(int points) {
		_score += points;
		//update socre in ui
		_playGameUI.UpdateScoreText(points);
	}

	public int GetScore() {
		return _score;
	}
}
