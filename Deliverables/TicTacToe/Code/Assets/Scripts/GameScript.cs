using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class GameScript : MonoBehaviour {
    [SerializeField] private Text outputText;
    [SerializeField] private Text ruleName;
    [SerializeField] private GameObject[] squares;
    [SerializeField] private Button winStateButton;

    //Array of images (O and X)
    public Sprite[] images;

    //Set initial spriteIndex to false
    bool spriteIndex = false;

    //Function to switch the spriteIndex's value
    public void switchTurn() {
        spriteIndex = !spriteIndex;
    }

    //The number of moves performed by the AI
    private int aiMoveCount = 0;

    //The first move made by the AI, to be used in the second move
    private int aiFirstMove = 0;

    public void Start() {
        //Perform the first AI move
		AITurn();
        //Set the button initially to be inactive
        winStateButton.gameObject.SetActive(false);
    }

    //AI FUNCTIONALITY

    public void AITurn() {
        //First Move
        if (aiMoveCount == 0) {
            ruleName.text = "Rule Used: Choose random corner";
            //Generate a random number between one and four
            int num = new Random().Next(1, 4);
            //Depending on the number generated, one of the four corners' type and sprite are set
            //Also, set aiFirstMove to the respective square selected
            if (num == 1) {
                squares[0].GetComponent<PlayerTurn>().type = 2;
                squares[0].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
                aiFirstMove = 0;
            } else if (num == 2) {
                squares[2].GetComponent<PlayerTurn>().type = 2;
                squares[2].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
                aiFirstMove = 2;
            } else if (num == 3) {
                squares[6].GetComponent<PlayerTurn>().type = 2;
                squares[6].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
                aiFirstMove = 6;
            } else {
                squares[8].GetComponent<PlayerTurn>().type = 2;
                squares[8].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
                aiFirstMove = 8;
            }
        //Second Move
        } else if (aiMoveCount == 1) {
            //If the corner vertically opposite the first move has not been played, set its type and sprite
            //else set the horizontally opposite corner's type and sprite
            if (aiFirstMove <= 3) {
                if (squares[aiFirstMove + 6].GetComponent<PlayerTurn>().type == 1) {
                    ruleName.text = "Rule Used: Corner Vertically opposite unavailable, choose horizontally opposite";
                    if ((aiFirstMove % 3) == 0) {
                        squares[2].GetComponent<PlayerTurn>().type = 2;
                        squares[2].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
                    } else {
                        squares[0].GetComponent<PlayerTurn>().type = 2;
                        squares[0].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
                    }
                } else {
                    ruleName.text = "Rule Used: Choose Vertically opposite";
                    squares[aiFirstMove + 6].GetComponent<PlayerTurn>().type = 2;
                    squares[aiFirstMove + 6].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
                }
            } else {
                if (squares[aiFirstMove - 6].GetComponent<PlayerTurn>().type == 1) {
                    ruleName.text = "Rule Used: Corner Vertically opposite unavailable, choose horizontally opposite";
                    if ((aiFirstMove % 3) == 0) {
                        squares[8].GetComponent<PlayerTurn>().type = 2;
                        squares[8].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
                    } else {
                        squares[6].GetComponent<PlayerTurn>().type = 2;
                        squares[6].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
                    }
                } else {
                    ruleName.text = "Rule Used: Choose Vertically opposite";
                    squares[aiFirstMove - 6].GetComponent<PlayerTurn>().type = 2;
                    squares[aiFirstMove - 6].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
                }
            }
        //Third and Fourth Move
        } else if ((aiMoveCount == 2) || (aiMoveCount == 3)) {
            //Check if a line is winnable, if it is, set the empty square's type and sprite
            //Else, check if a line is loseable, if it is, set the empty square's type and sprite
            //Else, set an empty corner's type and sprite

            //The islineWinnable function is called for every possible line, once as a winnable line and once as a loseable line, by passing true or false
            int tempNumber;
            if (isLineWinnable(0, 1, 2, true) != -1) {
                ruleName.text = "Rule Used: Choose empty square of a Winnable Line";
                tempNumber = isLineWinnable(0, 1, 2, true);
                squares[tempNumber].GetComponent<PlayerTurn>().type = 2;
                squares[tempNumber].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
            } else if (isLineWinnable(3, 4, 5, true) != -1) {
                ruleName.text = "Rule Used: Choose empty square of a Winnable Line";
                tempNumber = isLineWinnable(3, 4, 5, true);
                squares[tempNumber].GetComponent<PlayerTurn>().type = 2;
                squares[tempNumber].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
            } else if (isLineWinnable(6, 7, 8, true) != -1) {
                ruleName.text = "Rule Used: Choose empty square of a Winnable Line";
                tempNumber = isLineWinnable(6, 7, 8, true);
                squares[tempNumber].GetComponent<PlayerTurn>().type = 2;
                squares[tempNumber].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
            } else if (isLineWinnable(0, 3, 6, true) != -1) {
                ruleName.text = "Rule Used: Choose empty square of a Winnable Line";
                tempNumber = isLineWinnable(0, 3, 6, true);
                squares[tempNumber].GetComponent<PlayerTurn>().type = 2;
                squares[tempNumber].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
            } else if (isLineWinnable(1, 4, 7, true) != -1) {
                ruleName.text = "Rule Used: Choose empty square of a Winnable Line";
                tempNumber = isLineWinnable(1, 4, 7, true);
                squares[tempNumber].GetComponent<PlayerTurn>().type = 2;
                squares[tempNumber].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
            } else if (isLineWinnable(2, 5, 8, true) != -1) {
                ruleName.text = "Rule Used: Choose empty square of a Winnable Line";
                tempNumber = isLineWinnable(2, 5, 8, true);
                squares[tempNumber].GetComponent<PlayerTurn>().type = 2;
                squares[tempNumber].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
            } else if (isLineWinnable(0, 4, 8, true) != -1) {
                ruleName.text = "Rule Used: Choose empty square of a Winnable Line";
                tempNumber = isLineWinnable(0, 4, 8, true);
                squares[tempNumber].GetComponent<PlayerTurn>().type = 2;
                squares[tempNumber].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
            } else if (isLineWinnable(2, 4, 6, true) != -1) {
                ruleName.text = "Rule Used: Choose empty square of a Winnable Line";
                tempNumber = isLineWinnable(2, 4, 6, true);
                squares[tempNumber].GetComponent<PlayerTurn>().type = 2;
                squares[tempNumber].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
            } else if (isLineWinnable(0, 1, 2, false) != -1) {
                ruleName.text = "Rule Used: No winnable line, choose empty square of a Loseable Line";
                tempNumber = isLineWinnable(0, 1, 2, false);
                squares[tempNumber].GetComponent<PlayerTurn>().type = 2;
                squares[tempNumber].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
            } else if (isLineWinnable(3, 4, 5, false) != -1) {
                ruleName.text = "Rule Used: No winnable line, choose empty square of a Loseable Line";
                tempNumber = isLineWinnable(3, 4, 5, false);
                squares[tempNumber].GetComponent<PlayerTurn>().type = 2;
                squares[tempNumber].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
            } else if (isLineWinnable(6, 7, 8, false) != -1) {
                ruleName.text = "Rule Used: No winnable line, choose empty square of a Loseable Line";
                tempNumber = isLineWinnable(6, 7, 8, false);
                squares[tempNumber].GetComponent<PlayerTurn>().type = 2;
                squares[tempNumber].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
            } else if (isLineWinnable(0, 3, 6, false) != -1) {
                ruleName.text = "Rule Used: No winnable line, choose empty square of a Loseable Line";
                tempNumber = isLineWinnable(0, 3, 6, false);
                squares[tempNumber].GetComponent<PlayerTurn>().type = 2;
                squares[tempNumber].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
            } else if (isLineWinnable(1, 4, 7, false) != -1) {
                ruleName.text = "Rule Used: No winnable line, choose empty square of a Loseable Line";
                tempNumber = isLineWinnable(1, 4, 7, false);
                squares[tempNumber].GetComponent<PlayerTurn>().type = 2;
                squares[tempNumber].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
            } else if (isLineWinnable(2, 5, 8, false) != -1) {
                ruleName.text = "Rule Used: No winnable line, choose empty square of a Loseable Line";
                tempNumber = isLineWinnable(2, 5, 8, false);
                squares[tempNumber].GetComponent<PlayerTurn>().type = 2;
                squares[tempNumber].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
            } else if (isLineWinnable(0, 4, 8, false) != -1) {
                ruleName.text = "Rule Used: No winnable line, choose empty square of a Loseable Line";
                tempNumber = isLineWinnable(0, 4, 8, false);
                squares[tempNumber].GetComponent<PlayerTurn>().type = 2;
                squares[tempNumber].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
            } else if (isLineWinnable(2, 4, 6, false) != -1) {
                ruleName.text = "Rule Used: No winnable line, choose empty square of a Loseable Line";
                tempNumber = isLineWinnable(2, 4, 6, false);
                squares[tempNumber].GetComponent<PlayerTurn>().type = 2;
                squares[tempNumber].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
            } else {
                ruleName.text = "Rule Used: No winnable or loseable line, choose empty corner";
                if (squares[0].GetComponent<PlayerTurn>().type == 0) {
                    squares[0].GetComponent<PlayerTurn>().type = 2;
                    squares[0].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
                } else if (squares[2].GetComponent<PlayerTurn>().type == 0) {
                    squares[2].GetComponent<PlayerTurn>().type = 2;
                    squares[2].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
                } else if (squares[6].GetComponent<PlayerTurn>().type == 0) {
                    squares[6].GetComponent<PlayerTurn>().type = 2;
                    squares[6].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
                } else if (squares[8].GetComponent<PlayerTurn>().type == 0) {
                    squares[8].GetComponent<PlayerTurn>().type = 2;
                    squares[8].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
                }
            }
        //Fifth Move
        } else if (aiMoveCount == 4) {
            //Set the remaining square's type and sprite
            ruleName.text = "Rule Used: Choose remaining empty square";
            int square = 0;
            for (int tempNumber = 0; tempNumber < 9; tempNumber++) {
                if (squares[tempNumber].GetComponent<PlayerTurn>().type == 0) {
                    square = tempNumber;
                }
            }
            squares[square].GetComponent<PlayerTurn>().type = 2;
            squares[square].GetComponent<PlayerTurn>().spriteRenderer.sprite = images[1];
        }
        //Increment aiMoveCount by one and call the checkWin function
        aiMoveCount++;
        checkWin();
    }

    //Function to determine if a line is winnable/loseable or not
    public int isLineWinnable(int num1, int num2, int num3, bool isPlayer) {
        //If we want to check if the line is winnable
        if (isPlayer) {
            //Check if the line contains two O squares and one empty square, if so, return the empty square's position
            if ((squares[num1].GetComponent<PlayerTurn>().type == 2) && (squares[num2].GetComponent<PlayerTurn>().type == 2) && (squares[num3].GetComponent<PlayerTurn>().type == 0)) {
                return num3;
            } else if ((squares[num1].GetComponent<PlayerTurn>().type == 2) && (squares[num2].GetComponent<PlayerTurn>().type == 0) && (squares[num3].GetComponent<PlayerTurn>().type == 2)) {
                return num2;
            } else if ((squares[num1].GetComponent<PlayerTurn>().type == 0) && (squares[num2].GetComponent<PlayerTurn>().type == 2) && (squares[num3].GetComponent<PlayerTurn>().type == 2)){
                return num1;
            //Otherwise, return a -1
            } else {
                return -1;
            }
        //If we want to check if the line is loseable
        } else {
            //Check if the line contains two X squares and one empty square, if so, return the empty square's position
            if ((squares[num1].GetComponent<PlayerTurn>().type == 1) && (squares[num2].GetComponent<PlayerTurn>().type == 1) && (squares[num3].GetComponent<PlayerTurn>().type == 0)) {
                return num3;
            } else if ((squares[num1].GetComponent<PlayerTurn>().type == 1) && (squares[num2].GetComponent<PlayerTurn>().type == 0) && (squares[num3].GetComponent<PlayerTurn>().type == 1)) {
                return num2;
            } else if ((squares[num1].GetComponent<PlayerTurn>().type == 0) && (squares[num2].GetComponent<PlayerTurn>().type == 1) && (squares[num3].GetComponent<PlayerTurn>().type == 1)){
                return num1;
            //Otherwise, return a -1
            } else {
                return -1;
            }
        }
    }

    //Function to check if current board contains winning conditions or if a draw has occured
    public void checkWin() {
        //For each line, check if all squares in the line have the same type
        //If they do, call the decideWinner function

        //Diagonal Lines
        if ((squares[0].GetComponent<PlayerTurn>().type == squares[4].GetComponent<PlayerTurn>().type) && (squares[0].GetComponent<PlayerTurn>().type == squares[8].GetComponent<PlayerTurn>().type)) {
            decideWinner(0);
        } else if ((squares[2].GetComponent<PlayerTurn>().type == squares[4].GetComponent<PlayerTurn>().type) && (squares[2].GetComponent<PlayerTurn>().type == squares[6].GetComponent<PlayerTurn>().type)) {
            decideWinner(2);
        } else {
            for (int i = 1; i <= 3; i++) {
                //Horizontal Lines
                if ((squares[3 * (i-1)].GetComponent<PlayerTurn>().type == squares[1 + (3 * (i-1))].GetComponent<PlayerTurn>().type) && (squares[0 + (3 * (i-1))].GetComponent<PlayerTurn>().type == squares[2 + (3 * (i-1))].GetComponent<PlayerTurn>().type)) {
                    decideWinner(3 * (i-1));
                    return;
                //Vertical Lines
                } else if ((squares[i - 1].GetComponent<PlayerTurn>().type == squares[i + 2].GetComponent<PlayerTurn>().type) && (squares[i - 1].GetComponent<PlayerTurn>().type == squares[i + 5].GetComponent<PlayerTurn>().type)) {
                    decideWinner(i - 1);
                    return;
                }
            }

            //Check if a draw has occured
            bool drawCheck = true;
            //For each square, if it is empty (type is 0), set drawCheck to false
            foreach (GameObject square in squares) {
                if (square.GetComponent<PlayerTurn>().type == 0) {
                    drawCheck = false;
                }
            }
            //If drawCheck is true
            if (drawCheck) {
                //Display the button and the specified output text
                winStateButton.gameObject.SetActive(true);
                outputText.text = "Draw!";
                //Call the endGame function
                endGame();
            }
        }
    }

    //Function to decide the winner of the game
    public void decideWinner(int num) {
        //If the square at the passed value as an element's type is X
        if (squares[num].GetComponent<PlayerTurn>().type == 1) {
            //Display the button with a winning message and call the endGame function
            winStateButton.gameObject.SetActive(true);
            outputText.text = "You Win!";
            endGame();
        //If the square at the passed value as an element's type is O
        } else if (squares[num].GetComponent<PlayerTurn>().type == 2) {
            //Display the button with a losing message and call the endGame function
            winStateButton.gameObject.SetActive(true);
            outputText.text = "You Lose!";
            endGame();
        }
    }

    //Function to set all the square's gameFinished variable to true
    public void endGame() {
        foreach (GameObject square in squares) {
            square.GetComponent<PlayerTurn>().gameFinished = true;
        }
    }
}
