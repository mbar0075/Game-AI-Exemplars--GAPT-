using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : MonoBehaviour {
	public SpriteRenderer spriteRenderer;
	GameObject gameController;

	//Array of images (O and X)
	[SerializeField] private Sprite[] images;

	//Set initial type to empty
	public int type = 0;

	//Set initial gameFinished to not finished
	public bool gameFinished = false;

	//Function called at the first frame update
	private void Awake() {
		//Find the gameController game object and save it as gameController
		gameController = GameObject.Find("GameController");
		//Get the SpriteRenderer game component and save it as spriteRenderer
		spriteRenderer = GetComponent<SpriteRenderer>();
		//Set the sprite renderer to null at start
		spriteRenderer.sprite = null;
	}

	//On mouse click
	private void OnMouseDown() {
		//If the square is empty and the game is not finished yet
		if ((type == 0) && (!gameFinished)) {
			//Call function SetSquareType
			SetSquareType();
			//If the game is not finished
			if (!gameFinished) {
				//Call the AITurn function from the GameController's script
				gameController.GetComponent<GameScript>().AITurn();
			}
		}
	}

	//Function to set the chosen square's image and type
	public void SetSquareType() {
		//Set the square's sprite to X
		spriteRenderer.sprite = images[0];
		//Set its type to X
		type = 1;
		//Call the checkWin function from the GameController's script
		gameController.GetComponent<GameScript>().checkWin();
	}
}
