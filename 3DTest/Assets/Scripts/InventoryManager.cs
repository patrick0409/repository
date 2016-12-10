using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour 
{
	private static InventoryManager instance;  
	private static GameObject container;  

	public static InventoryManager GetInstance()  
	{  
		if(!instance)  
		{  
			container = new GameObject();  
			container.name = "InventoryManager";  
			instance = container.AddComponent(typeof(InventoryManager)) as InventoryManager;  
		}  
		return instance;  
	} 

	public bool bOpenStartDoor = false;
	public bool bOpenOddDoor = false;
	public bool bShoot = false;

	public string[] itemArray = new string[] {"Nipper", "Rubberband", "Bottle", "BottleCap", "birdGun"};
	public List<GameObject> invenList = new List<GameObject>();

	void Start () 
	{
		
	}
}
