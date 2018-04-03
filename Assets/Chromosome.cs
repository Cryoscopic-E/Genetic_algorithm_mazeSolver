using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chromosome {

    private List<int> genes = new List<int>();
    public int chromosomeLength = 0;
    public int maxValue = 0;

    public Chromosome(int chromosomeLength, int maxValue)
    {
        this.chromosomeLength = chromosomeLength;
        this.maxValue = maxValue;
        InitializeChromosome();
    }

    private void InitializeChromosome()
    {
        genes.Clear();

        for (int i = 0; i < chromosomeLength; i++)
        {
            genes.Add(Random.Range(0, maxValue));
        }
    }

    public void Mutate()
    {
        genes[Random.Range(0, genes.Count)] = Random.Range(0, maxValue);
    }

    public void CrossOver(Chromosome parent1, Chromosome parent2)
    {
        for (int i = 0; i < genes.Count; i++)
        {
            if(i < (genes.Count / 2.0))
            {
                genes[i] = parent1.genes[i];
            }
            else
            {
                genes[i] = parent2.genes[i];
            }
        }
    }
    
    public void SetGene(int index, int value)
    {
        if(index < genes.Count)
        {
            genes[index] = value;
        }
    }

    public int GetGene(int index)
    {
        return genes[index];
    }
}
