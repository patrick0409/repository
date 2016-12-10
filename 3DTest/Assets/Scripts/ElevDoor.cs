using UnityEngine;
using System.Collections;

public class ElevDoor : MonoBehaviour {
	public bool isOpenDoor;
	Transform leftDoor;
	Transform rightDoor;
	float time;
	float doorZPos;
	// Use this for initialization
	void Start () {
		time = 0;
		isOpenDoor = false;
		doorZPos = 0;
		rightDoor = transform.GetChild (0);
		leftDoor = transform.GetChild (1);
	}
	
	// Update is called once per frame
	void Update () {
		if (isOpenDoor) {
			time += Time.deltaTime;
			doorZPos = Mathf.Lerp (0f, 0.80f, time * 0.5f);
			leftDoor.transform.localPosition = new Vector3 (0, 0, -doorZPos);
			rightDoor.transform.localPosition = new Vector3 (0, 0, doorZPos);
			if (time >= 2f) {
				time = 0;
				isOpenDoor = false;
			}
		}
	}
}
