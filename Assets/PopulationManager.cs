using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour {

    public static float elapsed = 0f;

    public GameObject botPrefab;
    public float trialTime = 5;
    public int populationSize = 10;
  
    private List<GameObject> population = new List<GameObject>();
    private int generation = 1;
    GUIStyle guiStyle = new GUIStyle();


    private void OnGUI()
    {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Label(new Rect(10, 25, 200, 30), "Generation: " + generation, guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time : {0:0.00}", elapsed), guiStyle);
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
        elapsed += Time.deltaTime;
        if (elapsed >= trialTime)
        {
            BreedNewPopulation();
            elapsed = 0;
        }
    }

    GameObject Breed(GameObject p1, GameObject p2)
    {
        Vector3 startPosition = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f),
                                                transform.position.y,
                                                transform.position.z + Random.Range(-0.5f, 0.5f));

        GameObject clone = Instantiate(botPrefab, startPosition, transform.rotation);
        Brain cloneBrain = clone.GetComponent<Brain>();
        cloneBrain.Init(startPosition);
        if (Random.Range(0, 100) == 1)//1% mutation rate
        {
            cloneBrain.chromosome.Mutate();
        }
        else
        {
            cloneBrain.chromosome.CrossOver(p1.GetComponent<Brain>().chromosome, p2.GetComponent<Brain>().chromosome);
        }
        return clone;
    }

    void BreedNewPopulation()
    {


        //List<GameObject> winners = new List<GameObject>();

        //for (int i = 0; i < population.Count; i++)
        //{
        //    if (population[i].GetComponent<Brain>().goalReached)
        //    {
        //        winners.Add(population[i]);
        //    }
        //}

        List<GameObject> sortedPopulation = population.OrderBy(o => o.GetComponent<Brain>().goalReached).ToList();
        population.Clear();
        
        for (int i = (int)(sortedPopulation.Count / 2f) - 1; i < sortedPopulation.Count - 1; i++)
        {
            population.Add(Breed(sortedPopulation[i], sortedPopulation[i + 1]));
            population.Add(Breed(sortedPopulation[i + 1], sortedPopulation[i]));
        }

        for (int i = 0; i < sortedPopulation.Count; i++)
        {
            Destroy(sortedPopulation[i]);
        }
        generation++;
    }
    
}
