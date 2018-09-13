using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedRankingBoard : ScriptableObject {
	[Serializable]
	public class NameScorePair {
		public string playerName;
		public int playerScore;
	}

	[SerializeField]
	private List<NameScorePair> _playerRanks;

	//return the whole play rank list for displaying
	public List<NameScorePair> PlayerRanks {
		get {
			return _playerRanks;
		}
	}

	public void AddPair(string name, int score) {
		int addPosition = 0;
		for(var i = 0; i < _playerRanks.Count; i++) {
			if (score < _playerRanks[i].playerScore) continue;
			addPosition = i;
			break;
		}
		var newPair = new NameScorePair();
		newPair.playerName = name;
		newPair.playerScore = score;
		_playerRanks.Insert(addPosition, newPair);
	}
}
