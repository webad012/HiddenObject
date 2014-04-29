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
	public GameObject pickUp1;
	public GameObject pickUp2;
	public GameObject pickUp3;
	public GameObject pickUp4;
	public GameObject pickUp5;
	public GameObject pickUp6;
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

	private int numberOfPickUps = 50;
	private List<GameObject> pickUpList = new List<GameObject>();
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

		GeneratePickUps ();
		GeneratePickUpsDisplayList ();
	}

	void GeneratePickUps()
	{
		for (int i= 0; i<numberOfPickUps; i++) 
		{
			float randomPickup = UnityEngine.Random.value;
			GameObject pickUpObject;
			if(randomPickup < 0.17f)
			{
				pickUpObject = (GameObject)Instantiate(pickUps.pickUp1);
			}
			else if(randomPickup < 0.34f)
			{
				pickUpObject = (GameObject)Instantiate(pickUps.pickUp2);
			}
			else if(randomPickup < 0.51f)
			{
				pickUpObject = (GameObject)Instantiate(pickUps.pickUp3);
            }
			else if(randomPickup < 0.68f)
			{
				pickUpObject = (GameObject)Instantiate(pickUps.pickUp4);
            }
			else if(randomPickup < 0.85f)
			{
				pickUpObject = (GameObject)Instantiate(pickUps.pickUp5);
            }
            else
            {
				pickUpObject = (GameObject)Instantiate(pickUps.pickUp6);
			}

			pickUpObject.name = ObjectNameParse(pickUpObject.name);
			pickUpObject.transform.position = GenerateRandomPickUpPosition();
			pickUpObject.transform.rotation = Quaternion.Euler(pickUpObject.transform.rotation.eulerAngles.x, UnityEngine.Random.Range(0, 360), pickUpObject.transform.rotation.eulerAngles.z);
			pickUpObject.rigidbody.freezeRotation = true;
			pickUpObject.rigidbody.drag = 4.0f;
			pickUpList.Add(pickUpObject);
		}
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
			/*if(timerTimeSpent > loadingTimeout)
			{
				
				foreach(GameObject pu in pickUpList)
				{
					pu.rigidbody.isKinematic = true;
				}
			}*/
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
		pickUpList.Remove (pickUpObject);
		Destroy (pickUpObject);
		GeneratePickUpsDisplayList ();
	}

	string ObjectNameParse(string inName)
	{
		return inName.Remove (inName.Length - 7);
	}
}
