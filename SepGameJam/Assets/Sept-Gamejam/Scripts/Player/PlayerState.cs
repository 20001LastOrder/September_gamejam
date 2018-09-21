using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour {
	[SerializeField]
	private float _life = 2.5f;

	private void Start() {
		GameFlowManager.Instance.IncreasePlayerLifeUI(_life);
	}
}
