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
}

public class MainGameController : MonoBehaviour 
{
	public Boundary boundary;
	public PicUps pickUps;

	private int numberOfPickUps = 50;
	private List<GameObject> pickUpList = new List<GameObject>();
	Dictionary<string, int> pickUpDisplayList = new Dictionary<string, int> ();
	private Vector2 scrollPosition = Vector2.zero;

	// Use this for initialization
	void Start () 
	{
		GeneratePickUps ();
		GeneratePickUpsDisplayList ();
	}

	void GeneratePickUps()
	{
		for (int i= 0; i<numberOfPickUps; i++) 
		{
			Vector3 instantiatePosition = GenerateRandomPosition();
			//Quaternion instantiateRotation = Quaternion.identity;
			//Quaternion instantiateRotation = Random.rotation;
			//Quaternion instantiateRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), 0);
			
			float randomPickup = UnityEngine.Random.value;
			GameObject pickUpObject;
			if(randomPickup < 0.3f)
			{
				//pickUpObject = (GameObject)Instantiate(pickUps.pickUp1, instantiatePosition, instantiateRotation);
				pickUpObject = (GameObject)Instantiate(pickUps.pickUp1);
			}
			else if(randomPickup < 0.6f)
			{
				//pickUpObject = (GameObject)Instantiate(pickUps.pickUp2, instantiatePosition, instantiateRotation);
				pickUpObject = (GameObject)Instantiate(pickUps.pickUp2);
			}
			else
			{
				//pickUpObject = (GameObject)Instantiate(pickUps.pickUp3, instantiatePosition, instantiateRotation);
				pickUpObject = (GameObject)Instantiate(pickUps.pickUp3);
			}
			pickUpObject.transform.position = instantiatePosition;
			pickUpObject.rigidbody.freezeRotation = true;
			pickUpObject.rigidbody.drag = 4.0f;
			pickUpList.Add(pickUpObject);
		}
	}

	Vector3 GenerateRandomPosition()
	{
		float randomX = 0;
		float randomZ = 0;
		randomX = UnityEngine.Random.Range(boundary.xMin, boundary.xMax);
		randomZ = UnityEngine.Random.Range(boundary.zMin, boundary.zMax);

		return new Vector3(randomX, boundary.yMax, randomZ);
	}

	void OnGUI()
	{
		int labelHeight = 20;
		int labelWidth = 150;
		scrollPosition = GUI.BeginScrollView (new Rect(0, 0, Screen.width, 100), scrollPosition, new Rect(0, 0, Screen.width, pickUpDisplayList.Count*labelHeight));
		int count = 0;
		foreach (KeyValuePair<string, int> pud in pickUpDisplayList) 
		{
			GUI.Label(new Rect(0, count*labelHeight, labelWidth, labelHeight), count + ": " + pud.Key + " x" + pud.Value.ToString());
			count++;
		}
		GUI.EndScrollView ();
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
}
