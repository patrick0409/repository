using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {
	//List<GameObject> inventList = new List<GameObject>();


	// Use this for initialization
	void Start () {
		
		GetInventoryItem();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void GetInventoryItem(){
		int len = InventoryManager.GetInstance().invenList.Count;

		Debug.Log("len : " + len);
	}
}
