using UnityEngine;
using System.Collections;

public class LoadingController : MonoBehaviour 
{
	// Update is called once per frame
	void Update () 
	{
		transform.RotateAround (new Vector3(0, 0, 20), Vector3.forward, 200*Time.deltaTime);
	}
}
