using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabItem : MonoBehaviour {

	public float Thrust;

	private bool _hasItem = false;
	private GameObject _pickableItem;
	private GameObject _pickedItem;
	private Transform _itemPlaceholder;
	private Transform _items;
	private bool _canPick;

	// Use this for initialization
	void Start () {
		_itemPlaceholder = transform.GetChild(1);
		_items = GameObject.Find("items").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		if (_hasItem) {
			if (Input.GetButtonDown("Action")) {
				_hasItem = false;
				_pickedItem.transform.parent = _items;
				_pickedItem.GetComponent<Rigidbody>().isKinematic = false;
				_pickedItem.GetComponent<Rigidbody>().AddForce(transform.forward * Thrust, ForceMode.Impulse);
				_pickedItem = null;
			}
		} else {
			if (Input.GetButtonDown("Action") && _canPick) {
				_pickedItem = _pickableItem;
				_pickedItem.transform.parent = _itemPlaceholder;
				_pickedItem.GetComponent<Rigidbody>().isKinematic = true;
				_pickedItem.transform.localPosition = Vector3.zero;
				_hasItem = true;
				print("grabbed "+_pickedItem.name);
			}
		}
	}

	void startOverlapping (Collider other) {
		if (other.tag == "Pickable") {
			_canPick = true;
			_pickableItem = other.gameObject;
		}
	}

	void stopOverlapping (Collider other) {
		if (other.tag == "Pickable") {
			_canPick = false;
			_pickableItem = null;
		}
	}
}
