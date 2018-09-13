using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
	[SerializeField]
	private Text _playerScoreText;

	private const string _scoreText = "Socre: ";

	public void UpdateScoreText(int score) {
		_playerScoreText.text = _scoreText + score;
	}
}
