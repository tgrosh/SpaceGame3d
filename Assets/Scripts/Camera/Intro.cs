using UnityEngine;
using System.Collections;
using UnityStandardAssets.Cameras;

public class Intro : MonoBehaviour {

	public IntroPoint[] introPoints;
	public Vector3 positionOffset;
	public Vector3 rotationOffset;
	public GameObject hyperspaceEntrance;
	public Camera defaultCamera;
	public Canvas guiCanvas;

	GameObject playerShip;
	Transform playerShipCamTarget;
	Hyperdrive hyperDrive;

	LookatTarget camLooker;
	AutoCam cam;
	float pointDistanceThreshold = 1000f;
	float pointAngleThreshold = 5f;
	int currentIntroPoint = 0;
	float pointDistance;
	float pointAngle;
	float delayTime = 0f;
	bool completedIntroPoints;
	float defaultSpeed;
	float defaultTurnSpeed;

	float hyperspaceEntranceTime = 0f;

	// Use this for initialization
	void Start () {
		camLooker = GetComponent<LookatTarget>();
		cam = GetComponent<AutoCam>();

		defaultSpeed = cam.m_MoveSpeed;
		defaultTurnSpeed = cam.m_TurnSpeed;

		playerShip = GameObject.FindGameObjectWithTag("Player");
		playerShipCamTarget = playerShip.transform.FindChild("CameraTarget").transform;
		hyperDrive = playerShip.GetComponent<Hyperdrive>();

		guiCanvas = GameObject.Find ("GUI").GetComponent<Canvas>();

		transform.position = introPoints[currentIntroPoint].transform.position + positionOffset;
		transform.rotation = Quaternion.Euler(introPoints[currentIntroPoint].transform.rotation.eulerAngles + rotationOffset);
	}

	void Update() {
		if (completedIntroPoints) {
			hyperspaceEntranceTime += Time.deltaTime;

			if (hyperspaceEntranceTime < 4f) {
				cam.enabled = false;
				camLooker.enabled = true;
				hyperDrive.LeaveHyperspace(hyperspaceEntrance.transform);
				camLooker.SetTarget(playerShip.transform);
			} else if (hyperspaceEntranceTime < 10f) {
				cam.enabled = true;
				camLooker.enabled = false;
				cam.m_MoveSpeed = 3f;
				cam.SetTarget(playerShipCamTarget);
			} else {
				guiCanvas.enabled = true;
				defaultCamera.gameObject.SetActive(true);
				defaultCamera.enabled = true;
				cam.enabled = false;
				gameObject.SetActive(false);
				hyperDrive.Stop();
			}

			return;
		}

		cam.SetTarget(introPoints[currentIntroPoint].transform);

		pointDistance = Vector3.Distance (cam.transform.position, introPoints [currentIntroPoint].transform.position);
		pointAngle = Quaternion.Angle (cam.transform.rotation, introPoints [currentIntroPoint].transform.rotation);

		if (pointDistance < pointDistanceThreshold / cam.m_MoveSpeed && pointAngle < pointAngleThreshold) {
			delayTime += Time.deltaTime;

			if (delayTime > introPoints[currentIntroPoint].delay) {
				if (currentIntroPoint < introPoints.Length - 1){
					//dont change cam movement speed if on last one
					if (introPoints[currentIntroPoint].departureSpeed >= 0f) {
						cam.m_MoveSpeed = introPoints[currentIntroPoint].departureSpeed;
					} else {
						cam.m_MoveSpeed = defaultSpeed;
					}

					if (introPoints[currentIntroPoint].departureTurnSpeed >= 0f) {
						cam.m_TurnSpeed = introPoints[currentIntroPoint].departureTurnSpeed;
					} else {
						cam.m_TurnSpeed = defaultTurnSpeed;
					}
				}
				currentIntroPoint++;
				delayTime = 0f;

				if (currentIntroPoint > introPoints.Length - 1) {
					completedIntroPoints = true;
				}
			}
		}
	}

}
