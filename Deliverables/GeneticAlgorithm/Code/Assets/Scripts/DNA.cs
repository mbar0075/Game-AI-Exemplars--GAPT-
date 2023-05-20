using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA 
{

	//Array storing the gene values
	List<int> genes = new List<int>();
	int dnaLength = 0;
	int maxValues = 0;

	//Assigning the dnaLength and maxValues as well as randomsing all the gene values
	public DNA(int l, int v)
	{
		dnaLength = l;
		maxValues = v;
		SetRandom();
	}

	//Randomising all the genes that compose the chromasome
	public void SetRandom()
	{
		genes.Clear();
		for(int i = 0; i < dnaLength; i++)
		{
			genes.Add(Random.Range(-maxValues, maxValues));
		}
	}

	//Setting the gene value according to the passed parameters
	public void SetGene(int pos, int value)
	{
		genes[pos] = value;
	}

	//Combining the two parents to produce a child 
	public void Combine(DNA d1, DNA d2)
	{
		for(int i = 0; i < dnaLength; i++)
		{
			genes[i] = Random.Range(0,10) < 5 ? d1.genes[i] : d2.genes[i];
		}
	}

	//Mutates a range of genes in the DNA
	public void Mutate()
	{
		genes[Random.Range(0,dnaLength)] = Random.Range(-maxValues, maxValues);
	}

	//Retrieving the gene value according to the passed parameter
	public int GetGene(int pos)
	{
		return genes[pos];
	}
}

