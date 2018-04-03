using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour {

    
    public GameObject botPrefab;
    public float trialTime = 5;
    public int populationSize = 10;
    public float timeElapsed = 0f;

    //How much randomness of genes when generation new population
    [Range(0, 99)]
    public int mutationRate = 1;

    //How much the fitness function need to be selective, a low value will select all the previus generation, a high value will select the winner most of the time
    [Range(0, 100)]
    public int selectionRate = 50;

    private List<GameObject> population = new List<GameObject>();
    private int generation = 1;
    GUIStyle guiStyle = new GUIStyle();


    private void OnGUI()
    {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Label(new Rect(10, 25, 200, 30), "Generation: " + generation, guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time : {0:0.00}", timeElapsed), guiStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "Population: " + population.Count, guiStyle);
        GUI.EndGroup();
    }

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 startPosition = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f),
                                                transform.position.y,
                                                transform.position.z + Random.Range(-0.5f, 0.5f));

            GameObject clone = Instantiate(botPrefab, startPosition, transform.rotation);
            clone.GetComponent<Brain>().Init(startPosition);
            population.Add(clone);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= trialTime)
        {
            ProduceNewPopulation();
            timeElapsed = 0;
        }
    }

    GameObject ProduceBot(GameObject p1, GameObject p2)
    {
        Vector3 startPosition = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f),
                                                transform.position.y,
                                                transform.position.z + Random.Range(-0.5f, 0.5f));

        GameObject clone = Instantiate(botPrefab, startPosition, transform.rotation);
        Brain cloneBrain = clone.GetComponent<Brain>();
        cloneBrain.Init(startPosition);
        if (Random.Range(0, 100) <= mutationRate)//1% default mutation rate
        {
            cloneBrain.chromosome.Mutate();
        }
        else
        {
            cloneBrain.chromosome.CrossOver(p1.GetComponent<Brain>().chromosome, p2.GetComponent<Brain>().chromosome);
        }
        return clone;
    }
    //TODO tweak this fitness function for better seleting the winners
    void ProduceNewPopulation()
    {

        //Select the winners (the bot who reach the goal), they will be at the end of the array
        List<GameObject> sortedPopulation = population.OrderBy(o => o.GetComponent<Brain>().goalReached).ToList();
        population.Clear();

        /*NEW SELECTION METHOD*/

        //cut the array based on te selection rate percentage
        int cutFrom = Mathf.FloorToInt((populationSize * selectionRate) / 100f) - 1;
        int cutTo = populationSize - cutFrom;

        List<GameObject> cutted = sortedPopulation.GetRange(cutFrom, cutTo);
        for (int i = 0; i < populationSize; i++)
        {
            //Randomly select parents
            int firstParent = Random.Range(0, cutted.Count);
            int secondParent = Random.Range(0, cutted.Count);

            //exclude itself
            while(secondParent == firstParent)
            {
                secondParent = Random.Range(0, cutted.Count);
            }

            population.Add(ProduceBot(cutted[firstParent], cutted[secondParent]));
        }
        /*OLD SELECTION METHOD*/

        //select the bots starting from the midlle of the list to the upper half, since is ordered in ascending order
        //for (int i = (int)(sortedPopulation.Count / 2f) - 1; i < sortedPopulation.Count - 1; i++)
        //{
        //    population.Add(ProduceBot(sortedPopulation[i], sortedPopulation[i + 1]));
        //    population.Add(ProduceBot(sortedPopulation[i + 1], sortedPopulation[i]));
        //}

        for (int i = 0; i < sortedPopulation.Count; i++)
        {
            Destroy(sortedPopulation[i]);
        }
        generation++;
    }

}
