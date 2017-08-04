using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isColliding : MonoBehaviour {

	private GameObject _parent;

	void Start () {
		_parent = this.transform.parent.gameObject;
	}

    // Applies an upwards force to all rigidbodies that enter the trigger.
    void OnTriggerEnter(Collider other)
    {
    	// Déclenche la méthode startOverlapping chez le parent, et envoie en paramètre l'objet avec lequel il est entré en collision
    	_parent.SendMessage("startOverlapping", other);
    }

    // Applies an upwards force to all rigidbodies that enter the trigger.
    void OnTriggerExit(Collider other)
    {
    	_parent.SendMessage("stopOverlapping", other);
    }

}
