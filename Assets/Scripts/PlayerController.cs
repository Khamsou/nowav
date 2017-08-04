using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Rigidbody _rb;

	// Mouvement
	public float Speed;
	private float _xMove;
	private float _zMove;
	private Vector3 _movement;

	// Saut
	public float JumpForce;
	private float _distToGround;

	// Rotation
	private float _lastXMov;
	private float _lastZMov;
	private float _yRot;
	private Vector3 _rotation;

	// Caméra
	private Transform _camera;
	private Vector3	_cameraOffset;

	// Use this for initialization
	void Start () {
		_rb = GetComponent<Rigidbody>();

		_distToGround = GetComponent<Collider>().bounds.extents.y;


		_rotation = transform.localEulerAngles;

		_camera = GameObject.Find("playerCam").transform;
		_cameraOffset = new Vector3(transform.position.x + _camera.position.x, transform.position.y + _camera.position.y, transform.position.z + _camera.position.z);
	}
	
	void FixedUpdate() {
		// Mouvement
		_xMove = Input.GetAxis("Horizontal");
		_zMove = Input.GetAxis("Vertical");
		_movement = new Vector3(_xMove, 0f, _zMove);
		_rb.velocity = _movement * Speed * (Time.deltaTime*60);

			print(_rb.velocity.y);
		// Saut
		if (Input.GetButtonDown("Jump") && isGrounded()) {
			print("jump");
			_rb.AddForce(Vector3.up * JumpForce * (Time.deltaTime*60), ForceMode.Impulse);
		}

		// Rotation
		if (Input.GetButton("Horizontal") || Input.GetButton("Vertical")) {
			_lastXMov = _xMove;
			_lastZMov = _zMove;
		}
		_yRot = Mathf.Atan2(_lastXMov, _lastZMov) * (180 / Mathf.PI);
		transform.localEulerAngles = new Vector3(_rotation.x, _yRot, _rotation.z);

		// Caméra
		_camera.position = new Vector3(transform.position.x + _cameraOffset.x, transform.position.y + _cameraOffset.y, transform.position.z + _cameraOffset.z);

	}

	bool isGrounded() {
		return Physics.Raycast(transform.position, Vector3.down, _distToGround + 0.1f);
	}
}
