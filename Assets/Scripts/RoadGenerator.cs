using UnityEngine;
using System.Collections.Generic;

public class RoadGenerator : MonoBehaviour
{
    public List<GameObject> roadPrefabs;
    public float roadLength = 20.0f;
    public float moveSpeed = 10.0f;

    private List<GameObject> roadSegments = new List<GameObject>();
    private float spawnZ = 0.0f;
    private bool isMoving = true;  // Флаг движения дороги

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            SpawnRoadSegment();
        }
    }

    void Update()
    {
        if (isMoving)
        {
            MoveRoadSegments();

            if (roadSegments[0].transform.position.z < -roadLength)
            {
                ReuseRoadSegment();
            }
        }
    }

    void SpawnRoadSegment()
    {
        GameObject randomPrefab = roadPrefabs[Random.Range(0, roadPrefabs.Count)];
        GameObject segment = Instantiate(randomPrefab, new Vector3(0, 0, spawnZ), Quaternion.identity);
        roadSegments.Add(segment);
        spawnZ += roadLength;
    }

    void ReuseRoadSegment()
    {
        GameObject oldSegment = roadSegments[0];
        roadSegments.RemoveAt(0);
        oldSegment.transform.position = new Vector3(0, 0, spawnZ);
        roadSegments.Add(oldSegment);
        spawnZ += roadLength;
    }

    void MoveRoadSegments()
    {
        foreach (GameObject segment in roadSegments)
        {
            segment.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
    }

    // Управление движением дороги
    public void SetRoadMoving(bool isMoving)
    {
        this.isMoving = isMoving;
    }
}
