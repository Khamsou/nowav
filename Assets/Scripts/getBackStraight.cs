using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getBackStraight : MonoBehaviour {

	public float MaxRotation;

	// private Quaternion _originalRotationValue;

	private Rigidbody _rb;
	private float _adjustRotX;
	private float _adjustRotZ;
	private Vector3 _currentRot;
	private float _minRot = 0.1f;
	private float _turnSpeed = 1.0f;

	private float _angleX;
	private float _angleZ;


	// Use this for initializtion
	void Start () {
		_rb = GetComponent<Rigidbody>();


		// _originalRotationValue = transform.rotation; // save the initial rotation
	}
	
	void FixedUpdate () {
		
		_currentRot = transform.localEulerAngles;

		// _angleX = Mathf.Floor(Mathf.Abs(_currentRot.x)/32);
		// _angleZ = Mathf.Floor(Mathf.Abs(_currentRot.z)/32);


		// _adjustRotX = _angleX * (Time.deltaTime * 60);
		// _adjustRotZ = _angleZ * (Time.deltaTime * 60);

		_adjustRotX = 10 * (Time.deltaTime * 60) /** -_rb.angularVelocity*/;
		_adjustRotZ = 10 * (Time.deltaTime * 60) /** -_rb.angularVelocity*/;
		
		// transform.rotation = Quaternion.Slerp(transform.rotation, _originalRotationValue, Time.time * _turnSpeed); 

		if (transform.rotation.x < _minRot) {

			_rb.AddRelativeTorque(new Vector3(_adjustRotX,0f,0f), ForceMode.Force);

			// if(isRotXStable()){
			// 	transform.localEulerAngles = new Vector3(0f, transform.rotation.y, transform.rotation.z);
			// }

		} else if (transform.rotation.x > _minRot) {

			_rb.AddRelativeTorque(new Vector3(-_adjustRotX,0f,0f), ForceMode.Force);

			// if(isRotXStable()){
			// 	transform.localEulerAngles = new Vector3(0f, transform.rotation.y, transform.rotation.z);
			// }
		}



		if (transform.rotation.z < _minRot) {

			_rb.AddRelativeTorque(new Vector3(0f,0f,_adjustRotZ), ForceMode.Acceleration);

			// if(isRotZStable()){
			// 	transform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, 0f);
			// }
		} else if (transform.rotation.z > _minRot) {

			_rb.AddRelativeTorque(new Vector3(0f,0f,-_adjustRotZ), ForceMode.Acceleration);

		// 	if(isRotZStable()){
		// 		transform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, 0f);
		// 	}
		}
			


	}

	bool isRotXStable(){
		if (transform.rotation.x > -_minRot && transform.rotation.x < _minRot) {
			return true;
		} else { return false; }
	}
	bool isRotZStable(){
		if (transform.rotation.z > -_minRot && transform.rotation.z < _minRot) {
			return true;
		} else { return false; }
	}


}
