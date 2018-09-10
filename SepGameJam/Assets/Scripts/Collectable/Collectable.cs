using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    [SerializeField]
    private Color _color;

    private string _playerTagName = "Player";

    public float points;

    private void Start() {
		var material = gameObject.GetComponent<Renderer>().material;
		if(material != null) {
			material.color = _color;
		}
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag.Equals(_playerTagName)) {
            GameObject.Destroy(this.gameObject);
        }
    }
}
