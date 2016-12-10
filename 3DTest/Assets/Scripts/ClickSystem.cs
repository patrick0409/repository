using UnityEngine;
using System.Collections;

public class ClickSystem : MonoBehaviour
{
	Camera ca;
	public bool isOpenUi;
	public OpenUi CurOpenObj;
	public Vector3 PrevPos;
	public Transform AntRoomInitPos;

	readonly string RayEnter = "OnRayCastEnter";
	readonly string RayClickEnter = "OnRayCastClickEnter";
	readonly string RayExit = "OnRayCastExit";

	bool _isHit;
	public Transform _hitTarget;

	// Use this for initialization
	void Start () 
	{
		ca = Camera.main;
		isOpenUi = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Ray ray = ca.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0));
		RaycastHit hit;
		GameObject startDoor = GameObject.FindGameObjectWithTag ("Doors").transform.Find ("SlidingDoors").Find("SlidingDoor0").gameObject;

		if (Physics.Raycast (ray, out hit)) 
		{
			if (GameObject.FindGameObjectWithTag ("Inven").transform.GetChild (0).childCount != 0 &&
				GameObject.FindGameObjectWithTag ("Inven").transform.Find ("Inventory0").GetChild (0).name == "birdGun2d") {
				if (hit.transform.name == "OpenButton" && (Input.GetButtonDown("JoyBtn1")|| Input.GetMouseButtonDown(0))) {
					InventoryManager.GetInstance().bShoot = true;
					GameObject.FindGameObjectWithTag("StartRoom").transform.Find("BottleCap").SetParent(GameObject.FindGameObjectWithTag("MainCamera").transform.Find("Point"));
					GameObject.FindGameObjectWithTag("BottleCap").transform.localPosition = new Vector3(0f, 0f, 0f);
					GameObject.FindGameObjectWithTag("BottleCap").transform.rotation = Quaternion.EulerAngles(new Vector3(0f, 0f, 0f));
				}
			}

			// 병뚜껑 날리기
			if (InventoryManager.GetInstance ().bShoot) {
				Vector3 target = GameObject.FindGameObjectWithTag ("StartRoom").transform.Find ("OpenButton").position;
				GameObject.FindGameObjectWithTag("BottleCap").transform.position 
				= Vector3.MoveTowards(GameObject.FindGameObjectWithTag("BottleCap").transform.position, target, 5f * Time.deltaTime);
				// 병뚜껑이 버튼을 맞추었을때
				if (GameObject.FindGameObjectWithTag("BottleCap").transform.position == target) 
				{
					InventoryManager.GetInstance ().bShoot = false;
					InventoryManager.GetInstance ().bOpenStartDoor = true;
					ClearItemList ();
				}
			}

			// 문 열기
			if (InventoryManager.GetInstance ().bOpenStartDoor) {
				HallDoorOpen (startDoor);
				return;
			}

			if (hit.distance <= 1.3f) {
				hit.transform.SendMessage (RayEnter, this, SendMessageOptions.DontRequireReceiver);
				if (Input.GetButtonDown ("JoyBtn1") || Input.GetMouseButtonDown (0)) {
					hit.transform.SendMessage (RayClickEnter, this, SendMessageOptions.DontRequireReceiver);
				}
				_isHit = true;
				if (_hitTarget != null) {
					_hitTarget.SendMessage (RayExit, this, SendMessageOptions.DontRequireReceiver);
				}

				_hitTarget = hit.transform;

				if ((Input.GetButtonDown ("JoyBtn1") || Input.GetMouseButtonDown (0)) && hit.transform.name == "OddBtn") {
					InventoryManager.GetInstance ().bOpenOddDoor = true;
				}

				if (hit.transform.parent.rotation.y >= 0.8f)
					return;
				if (hit.transform.parent.name == "Door1") {
					GameObject.Find ("Door1").transform.Rotate (0f, 3f, 0f);
				}

				if (hit.transform.parent.name == "Door2") {
					GameObject.Find ("Door2").transform.Rotate (0f, 3f, 0f);
				}

				if (hit.transform.parent.name == "Door3") {
					if (GameObject.Find ("Door3").transform.rotation.y <= -0.8f)
						return;
					GameObject.Find ("Door3").transform.Rotate (0f, -3f, 0f);
				}

				if (hit.transform.parent.name == "Door4") {
					GameObject.Find ("Door4").transform.Rotate (0f, 3f, 0f);
				}

				if (hit.transform.name == "Mattress"
				    && (Input.GetButtonDown ("JoyBtn1") || Input.GetMouseButtonDown (0))) {
					hit.collider.gameObject.SetActive (false);
					hit.transform.parent.Find ("MattressFold").gameObject.SetActive (true);
				}

				if (hit.transform.name == "MattressFold"
					&& (Input.GetButtonDown ("JoyBtn1") || Input.GetMouseButtonDown (0))) {
					hit.collider.gameObject.SetActive (false);
					hit.transform.parent.Find ("Mattress").gameObject.SetActive (true);
				}

				if (!isOpenUi) {
					if (hit.transform.parent.name == "elevPassObj") {
						if (Input.GetButtonDown ("JoyBtn1") || Input.GetMouseButtonDown (0)) {
							GameObject item = hit.transform.parent.FindChild ("elevPass").gameObject;
							CurOpenObj = item.GetComponent<OpenUi> ();
							CurOpenObj.gameObject.SetActive (true);
							PrevPos = transform.position;
							transform.position = AntRoomInitPos.transform.position;
							StartCoroutine ("PosChange", item);
							CurOpenObj.isOpen = true;
							CurOpenObj.init ();
							CurOpenObj.player = this.gameObject;
							isOpenUi = true;
						}

					}
				}

				for (int i = 0; i < InventoryManager.GetInstance ().itemArray.Length; ++i) {
					if (hit.transform.name == InventoryManager.GetInstance ().itemArray [i]) {
						GameObject item = hit.transform.gameObject;
						if (hit.transform.name == "Bottle") {
							hit.transform.position = new Vector3 (300f, 300f);
							item = hit.transform.parent.Find ("BottleCap").gameObject;
						}
						// 인벤 UI로 보내버린다
						item.transform.position = new Vector3 (300f, 300f);
						AddInven (item.name);
					}
				}
			} else 
			{
				_isHit = false;
			}
		}


	}

	void HallDoorOpen(GameObject door)
	{
		if (door.transform.localPosition.x <= -13.7f) {
			InventoryManager.GetInstance ().bOpenStartDoor = false;
			InventoryManager.GetInstance ().bOpenOddDoor = false;
			return;
		}
		Vector3 trans = -door.transform.right * Time.deltaTime * 0.3f;
		door.transform.Translate (trans, Space.Self);
	}

	IEnumerator PosChange(GameObject go)
	{
		yield return null;
		ca.transform.rotation = new Quaternion(0, ca.transform.rotation.y, 0, ca.transform.rotation.w);
		go.transform.position = new Vector3(ca.transform.FindChild("Point").position.x, ca.transform.FindChild("Point").position.y, ca.transform.FindChild("Point").position.z);
		go.transform.LookAt (ca.transform);

		yield break;
	}

	void AddInven(string itemName)
	{
		// 인벤안에 2dSprite를 넣는다
		GameObject item2d = new GameObject ();

		int listnum = InventoryManager.GetInstance ().invenList.Count;
		string invenNum = "Inventory" + listnum;

		item2d.transform.SetParent (GameObject.FindGameObjectWithTag ("Inven").transform.Find (invenNum));
		item2d.transform.localPosition = new Vector2 (0f, 0f);
		item2d.transform.localRotation = Quaternion.AngleAxis(0f, new Vector3(0f,0f,0f));
		item2d.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
		item2d.AddComponent<SpriteRenderer>().sprite = Resources.Load <Sprite> ("Textures/" + itemName + "2d");  
		item2d.GetComponent<SpriteRenderer>().sortingOrder = 1;
		item2d.name = itemName;

		InventoryManager.GetInstance ().invenList.Add (item2d);

		if (MakeBirdGun ()) {
			ClearItemList ();
			GameObject birdGun2d = new GameObject ();
			birdGun2d.transform.SetParent (GameObject.FindGameObjectWithTag ("Inven").transform.Find ("Inventory0"));
			birdGun2d.transform.localPosition = new Vector2 (0f, 0f);
			birdGun2d.transform.localRotation = Quaternion.AngleAxis(0f, new Vector3(0f,0f,0f));
			birdGun2d.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
			birdGun2d.AddComponent<SpriteRenderer>().sprite = Resources.Load <Sprite> ("Textures/birdGun2d");  
			birdGun2d.GetComponent<SpriteRenderer>().sortingOrder = 1;
			birdGun2d.name = "birdGun2d";

			InventoryManager.GetInstance ().invenList.Add (birdGun2d);
		}
	}

	void ClearItemList()
	{
		for(int i = 0; i < 8; ++i)
		{
			if (GameObject.FindGameObjectWithTag ("Inven").transform.GetChild (i).childCount == 0)
				break;
			Destroy (GameObject.FindGameObjectWithTag ("Inven").transform.GetChild (i).GetChild (0).gameObject);
		}
		InventoryManager.GetInstance ().invenList.Clear ();
	}

	bool MakeBirdGun()
	{
		int cnt = 0;
		for (int i = 0; i < InventoryManager.GetInstance ().invenList.Count; ++i) {
			if (InventoryManager.GetInstance ().invenList [i].name == "Nipper")
				cnt++;
			if (InventoryManager.GetInstance ().invenList [i].name == "Rubberband")
				cnt++;
			if (InventoryManager.GetInstance ().invenList [i].name == "BottleCap")
				cnt++;
		}

		if (cnt == 3)
			return true;

		return false;
	}
}