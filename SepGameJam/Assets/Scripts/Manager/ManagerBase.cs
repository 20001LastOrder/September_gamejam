using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ManagerBase<T> : MonoBehaviour where T : ManagerBase<T>{

	private static T _manager;

	public T Instance {
		get {
			return _manager;
		}
	}

	private void Start() {
		_manager = (T)this;
	}
	
	public bool Exist() {
		return _manager != null;
	}
}
