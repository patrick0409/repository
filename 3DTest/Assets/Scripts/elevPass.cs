using UnityEngine;
using System.Collections;

public class elevPass : OpenUi {
	int CurPassPos;
	SpriteRenderer CurObj;
	Sprite spr;
	public ElevDoor Door;
	int[] CurPass;
	int[] CompletePass;
	// Use this for initialization
	void Awake()
	{
		CurPass = new int[4];
		CompletePass = new int[4]{1, 2, 3, 1};
	}

	void Start () {
		ca = Camera.main;
		CurPassPos = 0;
	}

	override public void init()
	{
		CurPassPos = 0;
		for (int i = 0; i < transform.FindChild ("Password").childCount; i++) {
			transform.FindChild ("Password").GetChild (i).GetComponent<SpriteRenderer> ().gameObject.SetActive(false);
		}
	}

	override public void ClickEvn()
	{
		if (isOpen) {
			Ray ray = ca.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0));
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				if (hit.transform.parent.name == "PassNum") {
					spr = (Sprite)Resources.Load (hit.transform.name, typeof(Sprite));
					CurPass [CurPassPos] = int.Parse(hit.transform.name);
					CurObj = transform.FindChild("Password").GetChild(CurPassPos).GetComponent<SpriteRenderer>();
					CurObj.gameObject.SetActive (true);
					CurObj.sprite = spr;
					CurPassPos++;
				}
			}

			if (CurPassPos == 4) {
				bool isCom = true;
				for (int i = 0; i < 3; i++) {
					if (CurPass [i] != CompletePass [i]) {
						isCom = false;
						break;
					}
				}
				isOpen = false;
				player.GetComponent<ClickSystem> ().isOpenUi = false;
				if (isCom) {
					Door.isOpenDoor = true;
				} else {
					Door.isOpenDoor = false;
				}
				player.transform.position = player.GetComponent<ClickSystem> ().PrevPos;
				gameObject.SetActive (false);
			}

		}
	}
}
