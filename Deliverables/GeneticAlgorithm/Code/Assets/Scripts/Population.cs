using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour
{
    //Agent prefab
    public GameObject botPrefab;

    //Empty object used to calculate spawn position
    public GameObject startingPos;

    //Size of the population
    public int populationSize = 50;

    //List of agents that compose the population
    List<GameObject> population = new List<GameObject>();

    //Time that has passed since start of generation
    public static float elapsed = 0;

    //Time alloted to each generation
    public float fuelTime = 5;

    //Denotes which generation is currently running
    int generation = 1;

    //Denotes how many agents have landed safely
    public static int reachedGoal = 0;

    //Denotes the number of tournaments that are carried out during tournament selection for crossover
    public int noOfTournaments = 4;

    //Denotes the probability (1/mutationChance) to mutate the child 
    public int mutationChance = 10;

    [Range(0, (float)0.5)]
    //Percentage of the population to be carried over to next generation without any changes
    public float elitism = 0.5f;

    [Range(0, (float)0.5)]
    //Percentage of the population to be choosen and bred at random
    public float randomRate = 0.5f;

    //GUI to present the relevant information to the user 
    GUIStyle guiStyle = new GUIStyle();
    void OnGUI()
    {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Box(new Rect(10, 0, 140, 140), "Stats", guiStyle);
        GUI.Label(new Rect(10, 25, 200, 30), "Gen: " + generation, guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), "Population: " + population.Count, guiStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "Reached Goal: " + reachedGoal, guiStyle);
        GUI.EndGroup();
    }


    //Initilising the popualtion on startup
    void Start()
    {
        for (int i = 0; i < populationSize; i++)
        {
            GameObject b = Instantiate(botPrefab, startingPos.transform.position, this.transform.rotation);
            b.GetComponent<Brain>().Init(fuelTime);
            population.Add(b);
        }
    }

    //Method the breed the passed parents to produce a child 
    GameObject Breed(GameObject parent1, GameObject parent2)
    {
        GameObject offspring = Instantiate(botPrefab, startingPos.transform.position, this.transform.rotation);
        Brain b = offspring.GetComponent<Brain>();

        /*
		Note: This method serves to breed two agents together to create an offspring whilst also allowing for a chance for the offspring to mutate

		1) Generate a random number between 0 and mutationChance and check if said value is equal to 1. 
			This will result in a 1/mutationChance probability, if this is the case than call the Init method on b and pass the fuelTime parameter.
			Then call the Combine function on the two parents dna, so as to create an offspring
			Then call the Mutate function on b
			Finally do the same thing excluding the call to Mutate in the else statement
			(Hint: Use b.dna.Combine(), parent.GetComponent<Brain>().dna, b.dna.Mutate)  
		2) Return the offspring 
		*/

        //Determining if the new child should be mutated or not
        if (Random.Range(0, mutationChance) == 1)
        {
            b.Init(fuelTime);
            b.dna.Combine(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);
            b.dna.Mutate();
        }
        else
        {
            b.Init(fuelTime);
            b.dna.Combine(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);
        }
        return offspring;
    }

    //Method to produce the next generation of agents 
    void BreedNewPopulation()
    {
        //Sorting the current generation according to the positive and negative reward values
        List<GameObject> sortedList = population.OrderByDescending(o => (o.GetComponent<Brain>().reachGoal))
        .OrderBy(o => (o.GetComponent<Brain>().distanceToGoal))
        .OrderBy(o => (o.GetComponent<Brain>().crash)).ToList();

        //Clearing the popualtion array
        population.Clear();

        /*
		Note: This method serves to produce the next generation of agents 

		1) Loop according to the elitism rate and Breed the top agents with themselves, this is done so as to achieve elitism. 
			The same result can also be achieved by copying the elite agents into the new population.
		2) Loop according to the randomRate and Breed a select few random agents with eachother, this is done so as to reduce early convergence. 
		3) While the population count is less than the specified popualtion size, choose two indices one of which accoridng to the elitismRate 
			the other according to tournament selection. The latter can be achieved by generating a series of indices and choosing the fittest one. 
			Once the two indices have been chosen breed them together and add them to the population.     

		*/

        //Breeding the same agent with itself so as to create a new instance, this avoids object reference issues
        for (int i = 0; i < (int)(elitism * sortedList.Count); i++)
        {
            population.Add(Breed(sortedList[i], sortedList[i]));
        }

        //Breeding random parents together to allow for worse solution (helps in escaping local optimums)
        for (int i = 0; i < (int)(randomRate * sortedList.Count); i++)
        {
            population.Add(Breed(sortedList[Random.Range(0, populationSize)], sortedList[Random.Range(0, populationSize)]));
        }

        //Skewed tournament selection were 1 parent is elite the other random
        while (population.Count < populationSize)
        {

            //Choosing the parents to breed together 
            int index1 = Random.Range(0, (int)(elitism * populationSize));
            int index2 = Random.Range(0, populationSize);

            //Tournament Selection for index 2
            for (int i = 0; i < noOfTournaments; i++)
            {
                int secondChoice = Random.Range(0, populationSize);
                if (secondChoice < index2)
                {
                    index2 = secondChoice;
                }
            }

            population.Add(Breed(sortedList[index1], sortedList[index2]));
        }

        //Destroying the old generation
        for (int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }

        //Incrementing the generation variable to denote the start of a new generation
        generation++;

        //Resetting the reachedGoal variable
        reachedGoal = 0;
    }

    //Update is called once per frame
    void Update()
    {

        //Retrieving the number of alive ships
        GameObject[] aliveShips = GameObject.FindGameObjectsWithTag("Player");

        //Updating the elapsed time
        elapsed += Time.deltaTime;
        if (elapsed >= fuelTime + 2)
        {
            //Breeding the new generation when the time comes
            BreedNewPopulation();
            elapsed = 0;
        }
    }
}