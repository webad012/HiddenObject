using UnityEngine;
using System.Collections;

public class PickUpController : MonoBehaviour 
{
	private MainGameController gameController;

	void Start()
	{
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) 
		{
			gameController = gameControllerObject.GetComponent<MainGameController>();
			
		} else 
		{
			Debug.Log("Cannot find 'GameController' script.");
			
		}
	}

	void OnMouseDown()
	{
		//Destroy (gameObject);
		gameController.PickUpSelected(gameObject);
	}
}
