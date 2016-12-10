using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	Camera Mcamera;

	public float XSensitivity = 10f;
	public float YSensitivity = 2f;
	public bool clampVerticalRotation = true;
	public float MinimumX = -90F;
	public float MaximumX = 90F;
	public bool smooth;
	public float smoothTime = 5f;
	public Transform Target;

	float stepTime = 0f;
	float stepYPos = 0f;
	bool stopStep = false;

	public int iSpeed = 10;

	private Quaternion m_CharacterTargetRot;
	private Quaternion m_CameraTargetRot;
	ClickSystem clicksys;


	// Use this for initialization
	void Start () 
	{
		Mcamera = Camera.main;
		Init(transform, Mcamera.transform);
		clicksys = GetComponent<ClickSystem> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		LookRotation (Target, Mcamera.transform);
		TouchButton ();
		if (!clicksys.isOpenUi) {
			MovePlayer ();
		}
	}

	void MovePlayer()
	{
		float fMove = iSpeed * Time.deltaTime;
		float vet = Input.GetAxis("Vertical");
		float hor = Input.GetAxis("Horizontal");
		transform.Translate(Vector3.forward * vet * fMove);
		transform.Translate(Vector3.right * hor * fMove);
		/*if (vet != 0 || hor != 0) {
			stepTime += Time.deltaTime;

			if (stepTime > 0.3f) {
				stopStep = true;
				GetComponent<Rigidbody> ().useGravity = true;
				if (stepTime > 0.6f) {
					stopStep = false;
					GetComponent<Rigidbody> ().useGravity = false;
					stepTime = 0f;
				}
			}
			if (!stopStep) {
				stepYPos = Mathf.Lerp (0, fMove * 1.6f, stepTime);
				transform.Translate (Vector3.up * stepYPos);
			}
		} else {
			stopStep = false;
			//stepYPos = 0;
			GetComponent<Rigidbody> ().useGravity = true;
			stepTime = 0f;
		}*/
	}



	public void Init(Transform character, Transform _camera)
	{
		m_CharacterTargetRot = character.localRotation;
		m_CameraTargetRot = _camera.localRotation;
	}

	public void TouchButton()
	{
		if (Input.GetButtonDown("JoyBtn1")) {
			if (clicksys.isOpenUi) {
				clicksys.CurOpenObj.ClickEvn ();
			}
		}

	}


	public void LookRotation(Transform character, Transform _camera)
	{
		float yRot = 0;
		if (Input.GetAxis ("Submit") != 0) {
			yRot = Input.GetAxis("Submit") * XSensitivity;
		}
		if (Input.GetAxis ("Jump") != 0) {
			yRot = -(Input.GetAxis("Jump") * XSensitivity);
		}

		m_CharacterTargetRot *= Quaternion.Euler (0f, yRot, 0f);

		//if(clampVerticalRotation)
		//	m_CameraTargetRot = ClampRotationAroundXAxis (m_CameraTargetRot);

		if(smooth)
		{
			character.localRotation = Quaternion.Slerp (character.localRotation, m_CharacterTargetRot,
				smoothTime * Time.deltaTime);
			_camera.localRotation = Quaternion.Slerp (_camera.localRotation, m_CameraTargetRot,
				smoothTime * Time.deltaTime);
		}
		else
		{
			character.localRotation = m_CharacterTargetRot;
			transform.rotation = new Quaternion(0f, _camera.rotation.y, 0f, _camera.rotation.w);
		}
	}


	Quaternion ClampRotationAroundXAxis(Quaternion q)
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1.0f;

		float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

		angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);

		q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

		return q;
	}

}