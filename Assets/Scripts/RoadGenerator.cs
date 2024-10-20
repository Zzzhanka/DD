using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public List<GameObject> roadPrefabs;  // ������ �������� ������
    public int numberOfSegments = 5;      // ���������� �������� ���������
    public float roadLength = 20.0f;      // ����� ������ �������� ������
    public float moveSpeed = 10.0f;       // �������� �������� ������

    private List<GameObject> roadSegments = new List<GameObject>();
    private float spawnZ = 0.0f;

    void Start()
    {
        // ���������� ��������� ��������
        for (int i = 0; i < numberOfSegments; i++)
        {
            SpawnRoadSegment();
        }
    }

    void Update()
    {
        MoveRoadSegments();

        if (roadSegments[0].transform.position.z < -roadLength)
        {
            ReuseRoadSegment();
        }
    }

    void SpawnRoadSegment()
    {
        // �������� ��������� ������ �� ������
        GameObject randomPrefab = roadPrefabs[Random.Range(0, roadPrefabs.Count)];
        GameObject segment = Instantiate(randomPrefab, new Vector3(0, 0, spawnZ), Quaternion.identity);

        roadSegments.Add(segment);
        spawnZ += roadLength;
    }

    void ReuseRoadSegment()
    {
        GameObject oldSegment = roadSegments[0];
        roadSegments.RemoveAt(0);

        // ���������� ������� �����
        oldSegment.transform.position = new Vector3(0, 0, spawnZ);
        roadSegments.Add(oldSegment);

        spawnZ += roadLength;
    }

    void MoveRoadSegments()
    {
        // ������� ��� �������� ������ �����
        foreach (GameObject segment in roadSegments)
        {
            segment.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
    }
}
