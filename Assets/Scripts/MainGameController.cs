using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Boundary
{
	public int xMin;
	public int xMax;
	public int yMin;
	public int yMax;
	public int zMin;
	public int zMax;
}

[System.Serializable]
public class PicUps
{
	public GameObject android;
	public GameObject framedPicture;
	public GameObject m9;
	public GameObject oilDrum;
	public GameObject tank;
	public GameObject enterprise;
	public GameObject heart;
	public GameObject iphone;
	public GameObject boxes;
	public GameObject stopSign;
	public GameObject sword;
	public GameObject bullet;
	public GameObject katana;
	public GameObject boat;
}

[System.Serializable]
public class Cameras
{
	public Camera mainCamera;
	public Camera loadingCamera;
}

public class MainGameController : MonoBehaviour 
{
	public Boundary boundary;
	public PicUps pickUps;
	public Cameras cameras;
	public GUIText loadingText;
	public GUIText timerText;

	private int numberOfNonPickUps = 40;
	private int numberOfPickUps = 10;
	private List<GameObject> pickUpList = new List<GameObject>();
	private List<GameObject> nonPickUpList = new List<GameObject>();
	Dictionary<string, int> pickUpDisplayList = new Dictionary<string, int> ();
	private Vector2 scrollPosition = Vector2.zero;

	private bool gameLoaded = false;
	private float loadingTimeout = 5;
	private float loadingStartTime;
	private GameObject loadingObject;
	private float timerStartTime = 0;
	private float timerTimeout = 60; // 60s

	// Use this for initialization
	void Start () 
	{
		timerText.text = "";
		timerStartTime = Time.time;
		cameras.mainCamera.enabled = false;
		cameras.loadingCamera.enabled = true;
		loadingStartTime = Time.time;

		loadingObject = GameObject.FindWithTag ("Loading");
		if (loadingObject == null)
		{
			Debug.Log("Cannot find 'Loading' object.");
            
        }

		GenerateNonPickUps ();
		GeneratePickUps ();
		GeneratePickUpsDisplayList ();
	}

	GameObject InstantiatePickUpObject()
	{
		float randomPickup = UnityEngine.Random.value;
		GameObject pickUpObject;
		if(randomPickup < 0.07f)
		{
			pickUpObject = (GameObject)Instantiate(pickUps.android);
		}
		else if(randomPickup < 0.14f)
		{
			pickUpObject = (GameObject)Instantiate(pickUps.framedPicture);
		}
		else if(randomPickup < 0.21f)
		{
			pickUpObject = (GameObject)Instantiate(pickUps.m9);
		}
		else if(randomPickup < 0.28f)
		{
			pickUpObject = (GameObject)Instantiate(pickUps.oilDrum);
		}
		else if(randomPickup < 0.35f)
		{
			pickUpObject = (GameObject)Instantiate(pickUps.tank);
		}
		else if(randomPickup < 0.42f)
		{
			pickUpObject = (GameObject)Instantiate(pickUps.enterprise);
		}
		else if(randomPickup < 0.49f)
		{
			pickUpObject = (GameObject)Instantiate(pickUps.heart);
		}
		else if(randomPickup < 0.56f)
		{
			pickUpObject = (GameObject)Instantiate(pickUps.iphone);
		}
		else if(randomPickup < 0.63f)
		{
			pickUpObject = (GameObject)Instantiate(pickUps.boxes);
		}
		else if(randomPickup < 0.70f)
		{
			pickUpObject = (GameObject)Instantiate(pickUps.stopSign);
		}
		else if(randomPickup < 0.77f)
		{
			pickUpObject = (GameObject)Instantiate(pickUps.sword);
		}
		else if(randomPickup < 0.84f)
		{
			pickUpObject = (GameObject)Instantiate(pickUps.bullet);
		}
		else if(randomPickup < 0.93f)
		{
			pickUpObject = (GameObject)Instantiate(pickUps.katana);
		}
		else
		{
			pickUpObject = (GameObject)Instantiate(pickUps.boat);
		}

		pickUpObject.name = ObjectNameParse(pickUpObject.name);
		pickUpObject.transform.position = GenerateRandomPickUpPosition();
		pickUpObject.transform.rotation = Quaternion.Euler(pickUpObject.transform.rotation.eulerAngles.x, UnityEngine.Random.Range(-90, 90), pickUpObject.transform.rotation.eulerAngles.z);
		pickUpObject.rigidbody.freezeRotation = true;
		pickUpObject.rigidbody.drag = 4.0f;

		return pickUpObject;
	}

	void GenerateNonPickUps()
	{
		for (int i= 0; i<numberOfNonPickUps; i++) 
		{
			GameObject pickUpObject = InstantiatePickUpObject();
			nonPickUpList.Add(pickUpObject);
		}
	}

	void GeneratePickUps()
	{
		for (int i= 0; i<numberOfPickUps; i++) 
		{
			GameObject pickUpObject = new GameObject();

			do
			{
				Destroy(pickUpObject);
				pickUpObject = InstantiatePickUpObject();

			} while(NonPickUpListContainsObjectByName(pickUpObject.name));

			pickUpObject.tag = "PickUp";
			pickUpList.Add(pickUpObject);
		}
	}

	bool NonPickUpListContainsObjectByName(string objName)
	{
		for (int i= 0; i<nonPickUpList.Count; i++) 
		{
			if(String.Compare( nonPickUpList[i].name, objName) == 0)
			{
				return true;
			}
		}

		return false;
	}

	Vector3 GenerateRandomPickUpPosition()
	{
		float randomX = 0;
		float randomZ = 0;
		randomX = UnityEngine.Random.Range(boundary.xMin, boundary.xMax);
		randomZ = UnityEngine.Random.Range(boundary.zMin, boundary.zMax);

		return new Vector3(randomX, boundary.yMax, randomZ);
	}

	Quaternion GenerateRandomPickUpRotation()
	{	
		//return Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
		return Quaternion.identity;
	}

	void OnGUI()
	{
		if (gameLoaded == true) 
		{
			int labelHeight = 20;
			int labelWidth = 150;
			scrollPosition = GUI.BeginScrollView (new Rect (0, 0, 200, 100), scrollPosition, new Rect (0, 0, 150, pickUpDisplayList.Count * labelHeight));
			int count = 0;
			foreach (KeyValuePair<string, int> pud in pickUpDisplayList) 
			{
				GUI.Label (new Rect (0, count * labelHeight, labelWidth, labelHeight), count + ": " + pud.Key + " x" + pud.Value.ToString ());
				count++;
			}
			GUI.EndScrollView ();

			float guiTime = Time.time - timerStartTime;
			int timerTimeLeft = Mathf.CeilToInt(timerTimeout - guiTime);
			timerText.text = "Time left: " + timerTimeLeft;
		}
	}

	void Update()
	{
		if(gameLoaded == false)
		{
			float loadingTimeSpent = Time.time - loadingStartTime;
			if(loadingTimeSpent > loadingTimeout)
			{
				foreach(GameObject pu in pickUpList)
				{
					pu.rigidbody.isKinematic = true;
				}
				foreach(GameObject pu in nonPickUpList)
				{
					pu.rigidbody.isKinematic = true;
				}
			}

			bool allStopped = true;
			foreach(GameObject pu in pickUpList)
			{
				if(!pu.rigidbody.IsSleeping())
				{
					allStopped = false;
				}
			}

			if(allStopped)
			{
				foreach(GameObject pu in pickUpList)
				{
					pu.rigidbody.isKinematic = true;
				}
				foreach(GameObject pu in nonPickUpList)
				{
					pu.rigidbody.isKinematic = true;
				}
				timerStartTime = Time.time;
				gameLoaded = true;
				cameras.mainCamera.enabled = true;
				cameras.loadingCamera.enabled = false;
				loadingText.text = "";
				if(loadingObject != null)
				{
					Destroy(loadingObject);
				}
			}
		}
	}

	void GeneratePickUpsDisplayList()
	{
		pickUpDisplayList.Clear ();

		foreach (GameObject pu in pickUpList) 
		{
			if(pickUpDisplayList.ContainsKey(pu.name))
			{
				pickUpDisplayList[pu.name] += 1;
			}

			if(!pickUpDisplayList.ContainsKey(pu.name))
			{
				pickUpDisplayList.Add(pu.name, 1);
			}
		}
	}

	public void PickUpSelected(GameObject pickUpObject)
	{
		if (!NonPickUpListContainsObjectByName (pickUpObject.name)) 
		{
			pickUpList.Remove (pickUpObject);
			Destroy (pickUpObject);
			GeneratePickUpsDisplayList ();
		}
	}

	string ObjectNameParse(string inName)
	{
		return inName.Remove (inName.Length - 7);
	}
}
