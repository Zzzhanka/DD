using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    public List<GameObject> roadPrefabs; 
    public List<GameObject> cityPrefabs; 
    public Transform spawnPoint;         
    public float spawnInterval = 2.0f;   
    public float citySpawnDelay = 30.0f; 

    private RoadMovementController movementController;
    private bool isCityMode = false;    

    void Start()
    {
        movementController = GetComponent<RoadMovementController>();

        for (int i = 0; i < 5; i++)
        {
            SpawnSegment(roadPrefabs);
        }
        StartCoroutine(SpawnEnvironment());
        StartCoroutine(ActivateCityMode());
    }

    IEnumerator SpawnEnvironment()
    {
        while (true)
        {
            if (isCityMode)
            {
                SpawnSegment(cityPrefabs);
            }
            else
            {
                SpawnSegment(roadPrefabs);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator ActivateCityMode()
    {
        yield return new WaitForSeconds(citySpawnDelay);
        isCityMode = true;
    }

    void SpawnSegment(List<GameObject> prefabs)
    {
        int randomIndex = Random.Range(0, prefabs.Count);
        GameObject newSegment = Instantiate(prefabs[randomIndex], spawnPoint.position, Quaternion.identity);
        movementController.AddSegment(newSegment);
    }
}
