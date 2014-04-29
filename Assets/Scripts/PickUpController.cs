using UnityEngine;
using System.Collections;

public class PickUpController : MonoBehaviour 
{
	private MainGameController gameController;
	//private Boundary boundary;

	void Start()
	{
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) 
		{
			gameController = gameControllerObject.GetComponent<MainGameController>();
			//boundary = gameController.GetBoundary();
			
		} else 
		{
			Debug.Log("Cannot find 'GameController' script.");
			
		}
	}

	void Update()
	{
		/*Vector3 tmpPos = transform.position;
		tmpPos.x = Mathf.Clamp (transform.position.x, boundary.xMin, boundary.xMax);
		tmpPos.z = Mathf.Clamp (transform.position.z, boundary.zMin, boundary.zMax);
		transform.position = tmpPos;*/
	}

	void OnMouseDown()
	{
		gameController.PickUpSelected(gameObject);
	}
}
