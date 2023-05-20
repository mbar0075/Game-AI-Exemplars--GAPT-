using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
     //Variables to hold min and max width
    [SerializeField] private int minwidth,maxwidth;
    //Variables to hold min and max height
    [SerializeField] private int minheight,maxheight;
    //Variable to hold the Z axis position
    [SerializeField] private int zAxis;
    //Variable to hold the policePrefab
    [SerializeField] private GameObject policePrefab;
    //Variable to hold the robberPrefab
    [SerializeField] private GameObject robberPrefab;
    //Variable to hold the bankPrefab
    [SerializeField] private GameObject bankPrefab;
    //Array to hold the different buildings prefab
    [SerializeField] private GameObject [] buildingsPrefab;
    //Variable to hold the number of robbers to be spawned
    [SerializeField] private int noOfRobbers;
    //Variable to hold the number of caught robbers
    [SerializeField] private int caughtRobbers;
    //Variable to hold respective the Winning Text Object
    [SerializeField] private Text WinText;
    //Variable to hold the policeInfo
    private string policeState;
    private float chaseRobberValue;
    private float goToBankValue;
    private float patrolValue;

    // Start is called before the first frame update
    private void Start()
    {
        //Instantiating prefabs
        InstantiatePeople();
        InstantiateObjects();
       
    }

    //Update method
    private void Update() {
        //Retrieving the number of caught robbers
        caughtRobbers=GetAmountOfCaughtRobbers();
        //Retrieving the police information
        GetPoliceInfo();
        //Checking whether game is won
        CheckWin();
    }

    //ONGUI method to print game information for the user
    GUIStyle guiStyle = new GUIStyle();
    GUIStyle guiStyle1 = new GUIStyle();
    GUIStyle guiStyle2 = new GUIStyle();
    GUIStyle guiStyle3 = new GUIStyle();
    private void OnGUI()
    {
        guiStyle.fontSize = 15;
        guiStyle1.fontSize=15;
        guiStyle2.fontSize=15;
        guiStyle3.fontSize=15;

        guiStyle.normal.textColor = Color.white;
        guiStyle1.normal.textColor = Color.blue;
        guiStyle2.normal.textColor = Color.red;
        guiStyle3.normal.textColor = Color.green;
        GUI.BeginGroup (new Rect (10, 10, 250, 150));
        GUI.Box (new Rect (10,0,140,140), "Score: "+caughtRobbers.ToString(), guiStyle);
        GUI.Label(new Rect (10,25,200,30), "Remaining Robbers: " +(noOfRobbers-caughtRobbers).ToString(), guiStyle);
        //Setting the respective guiStyle based on percentages
        if(chaseRobberValue==Mathf.Max(chaseRobberValue, Mathf.Max(goToBankValue, patrolValue))){
            GUI.Label(new Rect (10,50,200,30), "Police State: "+policeState, guiStyle1);
        }
        else if(goToBankValue==Mathf.Max(chaseRobberValue, Mathf.Max(goToBankValue, patrolValue))){
            GUI.Label(new Rect (10,50,200,30), "Police State: "+policeState, guiStyle2);
        }
        else if(patrolValue==Mathf.Max(chaseRobberValue, Mathf.Max(goToBankValue, patrolValue))){
            GUI.Label(new Rect (10,50,200,30), "Police State: "+policeState, guiStyle3);
        }
        else{
            GUI.Label(new Rect (10,50,200,30), "Police State: "+policeState, guiStyle);
        }
        
        GUI.Label(new Rect (10,75,200,30), "Chase Robber Percentage: "+Mathf.Round(chaseRobberValue*100.0f)*0.01f, guiStyle1);
        GUI.Label(new Rect (10,100,200,30), "Go To Bank Percentage: "+Mathf.Round(goToBankValue*100.0f)*0.01f, guiStyle2);
        GUI.Label(new Rect (10,125,200,30), "Patrol Percentage: "+Mathf.Round(patrolValue*100.0f)*0.01f, guiStyle3);
        GUI.EndGroup ();
    }

    //Method which returns the amount of caught robbers in the game
    private int GetAmountOfCaughtRobbers(){
        //Retrieving all the game objects with the Robber tag
        GameObject [] robbers = GameObject.FindGameObjectsWithTag("Robber");
        //Calculating the number of robbers caught
        return noOfRobbers-robbers.Length;
    }

    //Method which retrieves the relevant police info
    private void GetPoliceInfo(){
        //Retrieving the game object with the Police Tag
        GameObject police = GameObject.FindWithTag("Police");
        //Retrieving and returning, the required police info
        if(police!= null){
            policeState= police.GetComponent<PoliceScript>().state;
            goToBankValue= police.GetComponent<PoliceScript>().goToBankValue;
            chaseRobberValue= police.GetComponent<PoliceScript>().chaseRobberValue;
            patrolValue= police.GetComponent<PoliceScript>().patrolValue;
        }
        else{
            policeState= "None";
            goToBankValue= 0;
            chaseRobberValue= 0;
            patrolValue= 0;
        }
    }

    //Method which checks whether game is won
    private void CheckWin(){
        //Checking whether the number of caught robbers is equal to noOfRobbers,
        //If so will proceed to set the Win Text witht he relevant message, and reaload current scene
        if(caughtRobbers==noOfRobbers){
            WinText.text = "Police Wins!";
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    //Method which Instantiates the game map with people
    private void InstantiatePeople(){
        //Instantiating The Police Officer, at a position on the map
        var police =Instantiate(policePrefab, new Vector3(minwidth+maxwidth/3,minheight+maxheight/3,zAxis), Quaternion.identity);
        police.transform.parent = gameObject.transform;
        //Looping through the number of robbers, and Instantiating Robbers at random positions on the Map
        for(int i=0;i<noOfRobbers;i++){
            var robber = Instantiate(robberPrefab, new Vector3(Random.Range(minwidth,maxwidth),Random.Range(minheight,maxheight),zAxis), Quaternion.identity);
            robber.transform.parent = gameObject.transform;
        }
    }

    //Method which Instantiates the game map with objects/buildings
    private void InstantiateObjects(){
        //Instantiating The Bank, at the middle of the map, along with a calculated offset of (-1.9f,3.5f) for a better presentation
        var bank =Instantiate(bankPrefab, new Vector3((maxwidth+minwidth)/2-1.9f,(maxheight+minheight)/2+3.5f,zAxis), Quaternion.identity);
        bank.transform.parent = gameObject.transform;
        //Looping through all the map, and based on a random index, a random object from the buildingsPrefab is Instantiated
        for(int x=minwidth+5;x<maxwidth;x+=7){
            for(int y=minheight+5;y<maxheight;y+=10){
                if(Random.Range(0,20)>=10){
                    var building =Instantiate(buildingsPrefab[Random.Range(0,buildingsPrefab.Length)], new Vector3(x,y,zAxis), Quaternion.identity);
                    building.transform.parent = gameObject.transform;
                }
            }
        }
    }

}
