using UnityEngine;
using System.Collections;

public class PickUpController : MonoBehaviour 
{
	void OnMouseDown()
	{
		Destroy (gameObject);
	}
}
