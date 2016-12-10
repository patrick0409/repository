using UnityEngine;
using System.Collections;

public class CabinetItem : MonoBehaviour {

	bool isWorking = false;

	void OnRayCastClickEnter(ClickSystem ray)
	{
		if (!isWorking) {
			//InventoryManager.GetInstance()
		}
	}

	// Use this for initialization
	void Start () {
		isWorking = false;
	}
}
