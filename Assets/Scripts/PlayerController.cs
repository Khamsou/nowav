using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Rigidbody _rb;
	private CapsuleCollider _collider;

	// Mouvement
	public float Speed;
	public float MaxSpeed;
	private float _xMove;
	private float _zMove;
	private Vector3 _movement;
	private float _steepSlopeAngle = 40f;

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
		_collider = GetComponent<CapsuleCollider>();

		_distToGround = GetComponent<Collider>().bounds.extents.y;


		_rotation = transform.localEulerAngles;

		_camera = GameObject.Find("playerCam").transform;
		_cameraOffset = new Vector3(transform.position.x + _camera.position.x, transform.position.y + _camera.position.y, transform.position.z + _camera.position.z);
	}
	
	void FixedUpdate() {
		// Mouvement
		_xMove = Input.GetAxis("Horizontal");
		_zMove = Input.GetAxis("Vertical");
		_movement = new Vector3(_xMove, 0f, _zMove).normalized * Speed * (Time.deltaTime*60);


		if (Input.GetButton("Horizontal") || Input.GetButton("Vertical")) {
			// Rotation
			_lastXMov = _xMove;
			_lastZMov = _zMove;

			// Mouvement			
	        if (checkMoveableTerrain(transform.position, _movement, 10f))
	        {
				_rb.AddForce(_movement, ForceMode.VelocityChange);
	        }



		}
		_yRot = Mathf.Atan2(_lastXMov, _lastZMov) * (180 / Mathf.PI);
		transform.localEulerAngles = new Vector3(_rotation.x, _yRot, _rotation.z);

		// Max Vitesse
		if (_rb.velocity.x > MaxSpeed) {
			_rb.velocity = new Vector3 (MaxSpeed, _rb.velocity.y, _rb.velocity.z);
		} else if (_rb.velocity.x < -MaxSpeed) {
			_rb.velocity = new Vector3 (-MaxSpeed, _rb.velocity.y, _rb.velocity.z);
		}
		if (_rb.velocity.z > MaxSpeed) {
			_rb.velocity = new Vector3 (_rb.velocity.x,	_rb.velocity.y, MaxSpeed);
		} else if (_rb.velocity.z < -MaxSpeed) {
			_rb.velocity = new Vector3 (_rb.velocity.x, _rb.velocity.y, -MaxSpeed);
		}
		if (_rb.velocity.y > MaxSpeed*5) {
			_rb.velocity = new Vector3 (_rb.velocity.x,	MaxSpeed*5, _rb.velocity.z);
		} else if (_rb.velocity.y < -MaxSpeed*5) {
			_rb.velocity = new Vector3 (_rb.velocity.x, -MaxSpeed*5, _rb.velocity.z);
		}

		// Saut
		if (Input.GetButtonDown("Jump") && isGrounded()) {
			_rb.AddRelativeForce(Vector3.up * JumpForce * (Time.deltaTime*60), ForceMode.Impulse);
		}

	}

	void LateUpdate() {
		// Caméra
		_camera.position = new Vector3(transform.position.x + _cameraOffset.x, transform.position.y + _cameraOffset.y, transform.position.z + _cameraOffset.z);
		
	}

	bool isGrounded() {
		return Physics.Raycast(transform.position, Vector3.down, _distToGround + 0.1f);
	}

	bool checkMoveableTerrain(Vector3 position, Vector3 direction, float distance) {
		Ray checkRay = new Ray(position, direction);
		RaycastHit hit;


		// out = contient les infos sur l'objet touché
		if (Physics.Raycast(checkRay, out hit, distance) && hit.collider.gameObject.tag == "Ground")
		{
			
			print(Vector3.Angle(Vector3.up, hit.normal));

			// normal = le vecteur perpendiculaire à la surface touchée
			// Vector3.Angle = l'angle de la pente
			// Si l'angle de la pente est supérieur à la limite autorisé, le joueur ne peut pas avancer
			if (Vector3.Angle(Vector3.up, hit.normal) >= _steepSlopeAngle)
			{
				// Permet de récupérer la distance entre le joueur et la pente
				if (hit.distance - _collider.radius*2 > Mathf.Abs(Mathf.Cos(Vector3.Angle(Vector3.up, hit.normal))))
				{
					print("yes");
					return true;
				}
			return false;
			}
		}
		return true;
	}
}
