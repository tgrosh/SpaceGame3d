using UnityEngine;
using System.Collections;

public class SpaceCamera : MonoBehaviour {

	public Transform target;
	public float lookSpeed = .5f;
	public float distanceToFollow = 50.0f;
	public float height = -10.0f;
	public float moveSpeed = 1.0f;
	public float rotationSpeed = 1.0f;	

	Transform cameraTransform;
	float positionDamping = 2f;
	float verticalLook, horizontalLook;
	Vector3 desiredRotation;
	
	bool _isLookingRight = false;
	bool _isLookingLeft = false;
	bool _isLookingForward = false;
	bool _isLookingBack = false;

	// Use this for initialization
	void Start () {
		cameraTransform = transform;
	}

	void LateUpdate() {
		verticalLook = -Input.GetAxis ("Look Vertical D");
		horizontalLook = Input.GetAxis ("Look Horizontal D");

		if (horizontalLook != 0) {
			_isLookingRight = horizontalLook > 0;
			_isLookingLeft = !_isLookingRight;
			_isLookingForward = _isLookingBack = false;
		} else if (verticalLook != 0) {
			_isLookingForward = verticalLook > 0;
			_isLookingBack = !_isLookingForward;
			_isLookingLeft = _isLookingRight = false;
		}

		if (_isLookingRight || _isLookingLeft || _isLookingBack) {
			if (_isLookingRight) {
				desiredRotation = target.right;
			} else if (_isLookingLeft) {
				desiredRotation = target.right * -1;
			} else if (_isLookingBack) {
				desiredRotation = target.forward * -1;
			}
			transform.position = Vector3.Slerp(transform.position, transform.rotation * new Vector3(0, -height, -distanceToFollow) + target.position, moveSpeed * Time.deltaTime);
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredRotation, target.up), rotationSpeed * Time.deltaTime);
		} else {
			Vector3 wantedPosition = target.position + (target.rotation * new Vector3(0f, -height, -distanceToFollow));
			Vector3 currentPosition = Vector3.Lerp(cameraTransform.position, wantedPosition, positionDamping * Time.deltaTime);
			cameraTransform.position = currentPosition;
			
			Quaternion wantedRotation = Quaternion.LookRotation(target.position - currentPosition, target.up);
			cameraTransform.rotation = wantedRotation;
		}
	}
	
	public bool isLookingRight() {
		return _isLookingRight;
	}

	public bool isLookingLeft() {
		return _isLookingLeft;
	}

	void OrbitCamera(float x, float y) {	

		Quaternion rotation = Quaternion.Euler(y - (target.rotation.eulerAngles.y * 180f), x, 0);
		//Debug.Log (ClampAngle(target.rotation.eulerAngles.y * 180f, 0, 180));

		Vector3 negDistance = new Vector3(0f, -height, -distanceToFollow * 2);
		Vector3 position = rotation * negDistance + target.position;
		
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
		transform.position = Vector3.Slerp(transform.position, position, Time.deltaTime);
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < 0f)
			angle = 360f + angle;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}




