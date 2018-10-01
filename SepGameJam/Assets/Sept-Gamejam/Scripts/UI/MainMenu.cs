using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	enum MenuType {
		Main,
		Pause
	}

	private MenuType _menuType;

	[Header("Menu")]
	[SerializeField]
	private GameObject _pauseMenu;

	[SerializeField]
	private GameObject _mainMenu;

	[SerializeField]
	private GameObject _inGameMenu;


	[Header("UI Camera")]
	[SerializeField]
	private Camera _uiCamera;

	public void Update() {
		//Listen for pause menu
		if (_inGameMenu.activeSelf && !_pauseMenu.activeSelf && Input.GetKeyDown(KeyCode.Escape)) {
			Pause();
		} else if (!_inGameMenu.activeSelf && _pauseMenu.activeSelf && Input.GetKeyDown(KeyCode.Escape)) {
			UnPause();
		}
	}

	public void StartGame() {
		GameFlowManager.Instance.StartGame();
		//deactive main menu components
		_uiCamera.gameObject.SetActive(false);
		_mainMenu.gameObject.SetActive(false);
		//activate in game menu
		_inGameMenu.gameObject.SetActive(true);
	}

	public void Pause() {
		GameFlowManager.Instance.Pause();
		_uiCamera.gameObject.SetActive(true);
		_inGameMenu.gameObject.SetActive(false);
		_pauseMenu.gameObject.SetActive(true);
	}

	public void UnPause() {
		GameFlowManager.Instance.Resume();
		_uiCamera.gameObject.SetActive(false);
		_inGameMenu.gameObject.SetActive(true);
		_pauseMenu.gameObject.SetActive(false);
	}

	public void Restart() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void ExitGame() {
		Application.Quit();
	}
}
