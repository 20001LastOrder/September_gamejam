using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
	enum MenuType {
		Main,
		Pause
	}

	private MenuType _menuType;

	[SerializeField]
	private GameObject _pauseMenu;

	[SerializeField]
	private GameObject _mainMenu;

	[Header("SceneTransfer")]
	[SerializeField]
	private string _tutorialLevelName;

	[SerializeField]
	private string _mainLevelName;

	public void GoToTutorial() {
		GameFlowManager.Instance.ChangeScene(_tutorialLevelName);
		SetInGame(true);
	}

	public void GoToMain() {
		GameFlowManager.Instance.ChangeScene(_mainLevelName);
		SetInGame(true);
	}

	public void SetInMainMenu() {
		SetInGame(false);
	}

	public void SetInGame(bool isInGame) {
		_menuType = MenuType.Pause;
		_pauseMenu.gameObject.SetActive(isInGame);
		_mainMenu.gameObject.SetActive(!isInGame);
	}
}
