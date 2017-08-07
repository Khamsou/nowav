using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Rigidbody _rb;

	// Mouvement
	public float Speed;
	public float MaxSpeed;
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
		_movement = new Vector3(_xMove, 0f, _zMove) * Speed * (Time.deltaTime*60);

		// Max Vitesse
		if (_rb.velocity.x > MaxSpeed) {
			_rb.velocity = new Vector3 (MaxSpeed,	_rb.velocity.y, _rb.velocity.z);
		} else if (_rb.velocity.x < -MaxSpeed) {
			_rb.velocity = new Vector3 (-MaxSpeed, _rb.velocity.y, _rb.velocity.z);
		}
		if (_rb.velocity.z > MaxSpeed) {
			_rb.velocity = new Vector3 (_rb.velocity.x,	_rb.velocity.y, MaxSpeed);
		} else if (_rb.velocity.z < -MaxSpeed) {
			_rb.velocity = new Vector3 (_rb.velocity.x, _rb.velocity.y, -MaxSpeed);
		}

		// Saut
		if (Input.GetButtonDown("Jump") && isGrounded()) {
			_rb.AddForce(Vector3.up * JumpForce * (Time.deltaTime*60), ForceMode.Impulse);
		}

		if (Input.GetButton("Horizontal") || Input.GetButton("Vertical")) {
			// Rotation
			_lastXMov = _xMove;
			_lastZMov = _zMove;
			// Mouvement
			_rb.AddForce(_movement, ForceMode.VelocityChange);				
		}
		_yRot = Mathf.Atan2(_lastXMov, _lastZMov) * (180 / Mathf.PI);
		transform.localEulerAngles = new Vector3(_rotation.x, _yRot, _rotation.z);
	}

	void LateUpdate() {
		// Caméra
		_camera.position = new Vector3(transform.position.x + _cameraOffset.x, transform.position.y + _cameraOffset.y, transform.position.z + _cameraOffset.z);
		
	}

	bool isGrounded() {
		return Physics.Raycast(transform.position, Vector3.down, _distToGround + 0.1f);
	}
}
