using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
	[SerializeField]
	private Text _playerScoreText;

    [SerializeField]
    private Text _playerGoldCount;

    [SerializeField]
    private Text _playerRubyCount;

    [SerializeField]
    private GameObject _Panel;

    [SerializeField]
    private Image _KeyImage;

    [SerializeField]
    private Image _CubeImage;

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
	private GameObject _gameOver;

	[SerializeField]
	private float _scoreRollingUpTime = 2.0f;

	private const string _defaultScoreText = "Socre: ";

	private void Awake() {
		_playerLifes = new Stack<Image>();

	}

	public void UpdateScoreText(int originalScore, int increment) {
		StartCoroutine(ScrollUpPoint(_playerScoreText, originalScore, originalScore+increment, _scoreRollingUpTime, _defaultScoreText));
	}

    public void UpdateGoldCount(int current)
    {
        
        _playerGoldCount.text = "Gold: " + current;
    }

    public void UpdateRubyCount(int current)
    {
        
        _playerGoldCount.text = "Gold: " + current;
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

    internal void UpdateKey()
    {
        _KeyImage.enabled = true;
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

    internal void UpdateCube(bool b)
    {
        if (b)
            _CubeImage.enabled = true;
        else if (!b)
            _CubeImage.enabled = false;
    }

    public void GameOver() {
		_gameOver.SetActive(true);
	}

    public void Win()
    {
        _gameOver.GetComponentInChildren<Text>().text = "You win";
        _gameOver.SetActive(true);
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
			int increment =start + (int)((deltaTime / time) * diff);
			deltaTime += Time.deltaTime;
			text.text = prefix + increment.ToString();
			yield return null;
		}
		text.text = prefix + end.ToString();
	}

    public void ShowPanel()
    {
        _Panel.SetActive(true);
    }

    public void HidePanel()
    {
        _Panel.SetActive(false);
    }

    public void GetKey()
    {
        StartCoroutine(Request.Get("https://casual-development-dogdays.c9users.io/" + GameFlowManager.Instance.GetRandomId() + "/key", CheckKey));
    }

    private void CheckKey(Dictionary<string, string> res)
    {
        string keyAcquired;
        if (!res.TryGetValue("key", out keyAcquired)) return;
        if (Convert.ToBoolean(keyAcquired))
        {
            HidePanel();
            GameFlowManager.Instance.ObtainKey();
        }
        else
        {
            HidePanel();
            GameFlowManager.Instance.Lose();
        }
    }
}
