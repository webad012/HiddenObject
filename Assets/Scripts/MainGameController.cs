using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
}

public class MainGameController : MonoBehaviour 
{
	public Boundary boundary;
	public PicUps pickUps;
	//public GameObject pickUp;

	private int numberOfPickUps = 50;
	private List<GameObject> pickUpList = new List<GameObject>();

	// Use this for initialization
	void Start () 
	{
		for (int i= 0; i<numberOfPickUps; i++) 
		{
			Vector3 instantiatePosition = GenerateRandomPosition();
			Quaternion instantiateRotation = Quaternion.identity;

			GameObject pickUpObject = (GameObject)Instantiate(pickUps.pickUp1, instantiatePosition, instantiateRotation);
			pickUpObject.rigidbody.freezeRotation = true;
			pickUpObject.rigidbody.drag = 4.0f;
			pickUpList.Add((pickUpObject));
			//pickUpList.Add((GameObject)Instantiate(pickUp, instantiatePosition, instantiateRotation));
		}
	}

	Vector3 GenerateRandomPosition()
	{
		float randomX = 0;
		float randomZ = 0;
		randomX = Random.Range(boundary.xMin, boundary.xMax);
		randomZ = Random.Range(boundary.zMin, boundary.zMax);
        
		/*bool posOk = false;
		while(posOk == false)
		{
			randomX = Random.Range(boundary.xMin, boundary.xMax);
			randomZ = Random.Range(boundary.zMin, boundary.zMax);
			
			if(randomX >= -80 && randomX <= -93 && randomZ >= -105 && randomZ <= -120)
            {
				posOk = false;
			}
			else
			{
				posOk = true;
			}
        }*/

		return new Vector3(randomX, boundary.yMax, randomZ);
	}
	
	// Update is called once per frame
    void Update () 
	{
		//Debug.Log(pickUpList[0].transform.rigidbody.ToString());
		/*foreach (GameObject pu in pickUpList) 
		{
			if(pu.rigidbody.position.x >= -80 && pu.rigidbody.position.x <= -93
			   && pu.rigidbody.position.z >= -105 && pu.rigidbody.position.z <= -120)
			{
				Debug.Log(pu.transform.rigidbody.ToString());
				pu.transform.position = GenerateRandomPosition();
			}
		}*/
	}
}
