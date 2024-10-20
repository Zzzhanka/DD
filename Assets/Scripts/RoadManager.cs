using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public List<GameObject> roadPrefabs;  // ������ �������� �����
    public Transform spawnPoint;          // ����� ������ ����� �����
    public float roadSpeed = 5.0f;        // �������� �������� ������
    public float roadLength = 30.0f;      // ����� ������ ������� ������
    private bool isMoving = true;         // ��������� ��������� � ����������

    private Queue<GameObject> activeRoads = new Queue<GameObject>();  // ������� �������� ��������

    void Start()
    {
        // ���������� ��������� ������� ������
        for (int i = 0; i < 3; i++)
        {
            SpawnRoad();
        }
    }

    void Update()
    {
        if (isMoving)
        {
            MoveRoads();  // ������� �������� ������� ������
        }
    }

    void MoveRoads()
    {
        foreach (GameObject road in activeRoads)
        {
            road.transform.Translate(Vector3.back * roadSpeed * Time.deltaTime);
        }

        // ��������: ���� ������� ������ ������ �� ������� ������, ������� ��� � ������� �����
        if (activeRoads.Count > 0 && activeRoads.Peek().transform.position.z < -roadLength)
        {
            GameObject oldRoad = activeRoads.Dequeue();
            Destroy(oldRoad);
            SpawnRoad();
        }
    }

    void SpawnRoad()
    {
        int randomIndex = Random.Range(0, roadPrefabs.Count);
        GameObject newRoad = Instantiate(roadPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
        activeRoads.Enqueue(newRoad);
    }

    public void StopRoad()
    {
        isMoving = false;  // ������������� �������� � ���������
    }

    public void StartRoad()
    {
        isMoving = true;  // ������������ �������� � ���������
    }
}
