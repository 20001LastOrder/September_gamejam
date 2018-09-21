using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
	[SerializeField]
	private Text _playerScoreText;

	[SerializeField]
	private GameObject _playerLifeParent;
	[SerializeField]
	private GameObject _playerLifePrefab;
	private Stack<Image> _playerLifes;
	[SerializeField]
	private float _playerLifeDistance = 15;
	[SerializeField]
	private float _lifeFillMax = 1;

	[SerializeField]
	private float _scoreRollingUpTime = 2.0f;

	private const string _defaultScoreText = "Socre: ";

	private void Awake() {
		_playerLifes = new Stack<Image>();
	}

	public void UpdateScoreText(int originalScore, int increment) {
		StartCoroutine(ScrollUpPoint(_playerScoreText, originalScore, increment, _scoreRollingUpTime, _defaultScoreText));
		AddLife(10);
	}

	public void DecreaseLife(float life) {
		if(life > _playerLifes.Count) {
			RemoveAllLife();
			return;
		}
		RemoveLifeIcon(life);
	}

	private void RemoveAllLife() {
		while (_playerLifes.Count > 0) {
			var lifeObj = _playerLifes.Pop();
			Destroy(lifeObj.gameObject);
		}
	}

	private void RemoveLifeIcon(float life) {
		while(life > 0) {
			var heart = _playerLifes.Pop();
			var fillAmount = heart.fillAmount;
			if(life >= fillAmount) {
				Destroy(heart);
			} else {
				heart.fillAmount = fillAmount - life;
			}
			life -= fillAmount;
		}
	}

	public void AddLife(float life) {
		while(life > 0) {
			float thisFill = 0;
			if(_playerLifes.Count == 0 || _playerLifes.Peek().fillAmount >= 1) {
				thisFill = life >= 1 ? 1 : life;
				CreateFullLifes(thisFill);
			} else {
				float amountNeeded = 1 - _playerLifes.Peek().fillAmount;
				thisFill = life >= amountNeeded ? amountNeeded : life;
				FillLastHeart(thisFill);
			}
			life -= thisFill;
		}
	}

	private void CreateFullLifes(float fillAmount) {
		var heart = Instantiate(_playerLifePrefab, _playerLifeParent.transform);
		heart.transform.localPosition = new Vector3(_playerLifeDistance * _playerLifes.Count, 0, 0);
		var image = heart.GetComponent<Image>();
		image.fillAmount = fillAmount;
		_playerLifes.Push(image);
	}

	private void FillLastHeart(float fillAmount) {
		var lastHeart = _playerLifes.Peek().GetComponent<Image>();
		lastHeart.fillAmount = lastHeart.fillAmount + fillAmount;
	}

	private IEnumerator ScrollUpPoint(Text text, int start, int end, float time, string prefix = "") {
		int diff = end - start;
		float deltaTime = 0;
		text.text = prefix + start.ToString();
		//rolling numbers for amount of time
		while (deltaTime <= time) {
			int increment = (int)((deltaTime / time) * diff);
			deltaTime += Time.deltaTime;
			text.text = prefix + increment.ToString();
			yield return null;
		}
		text.text = prefix + end.ToString();
	}
}
