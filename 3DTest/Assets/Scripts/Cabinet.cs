using UnityEngine;
using System.Collections;

public class Cabinet : MonoBehaviour {
	bool _isOpen = false;
	bool _isWorking = false;
	[SerializeField]
	float OpenDelay = 2f;
	float _time = 0;
	Quaternion targetRot;
	Quaternion openRot;
	Quaternion closeRot;

	void Awake()
	{
		openRot = Quaternion.Euler (new Vector3 (0, 0, -145));
		closeRot = Quaternion.Euler (Vector3.zero);
	}

	void OnRayCastClickEnter(ClickSystem ray)
	{
		if (!_isWorking) {
			if (_isOpen) {
				targetRot = closeRot;
				_isOpen = false;
			} else {
				targetRot = openRot;
				_isOpen = true;
			}

			_isWorking = true;
			ActiveDoor ();
		}
	}

	public void ActiveDoor()
	{
		StartCoroutine("ActiveDoorCo");
	}

	IEnumerator ActiveDoorCo()
	{
		_time = 0;
		Quaternion curRot = transform.localRotation;
		while (true) {
			if (_isWorking) {
				_time += Time.deltaTime;

				transform.localRotation = Quaternion.Lerp (curRot, targetRot, _time / OpenDelay);
				if (_time >= OpenDelay) {
					_isWorking = false;
					yield break;
				}
				yield return null;
			}
		}
	}
}
