using System.Collections.Generic;
using UnityEngine;

public class RoadController : MonoBehaviour
{
    public List<GameObject> roadPrefabs;    // ������ �������� �����
    public List<GameObject> cityPrefabs;    // ������ �������� ��������� ��������
    public Transform spawnPoint;            // ����� ������ ����� ��������
    public float roadSpeed = 5.0f;          // �������� ��������
    public float roadLength = 30.0f;        // ����� ������ �������
    public int zombiesToCity = 10;          // ���������� ����� ��� ������ ������

    private Queue<GameObject> activeRoads = new Queue<GameObject>(); // �������� �������
    private bool isMoving = true;           // ���������� ���������
    private bool isCityMode = false;        // ����� ��������� ������
    private int zombiesDefeated = 0;        // ���������� ������������ �����

    void Start()
    {
        // ���������� ��������� ������� ������
        for (int i = 0; i < 3; i++)
        {
            SpawnRoadOrCity();
        }
    }

    void Update()
    {
        if (isMoving)
        {
            MoveRoads(); // ������� �������� �������
        }
    }

    void MoveRoads()
    {
        foreach (GameObject road in activeRoads)
        {
            road.transform.Translate(Vector3.back * roadSpeed * Time.deltaTime);
        }

        // �������� � ����� ������ �������, ���� �� ����� �� ������� ���������
        if (activeRoads.Count > 0 && activeRoads.Peek().transform.position.z < -roadLength)
        {
            GameObject oldRoad = activeRoads.Dequeue();
            Destroy(oldRoad);
            SpawnRoadOrCity();
        }
    }

    void SpawnRoadOrCity()
    {
        // ���� ������� � ����� ������, ���������� ��������� �������
        if (isCityMode)
        {
            int randomIndex = Random.Range(0, cityPrefabs.Count);
            GameObject newCity = Instantiate(cityPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
            activeRoads.Enqueue(newCity);
        }
        else
        {
            int randomIndex = Random.Range(0, roadPrefabs.Count);
            GameObject newRoad = Instantiate(roadPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
            activeRoads.Enqueue(newRoad);
        }
    }

    public void StopRoad()
    {
        isMoving = false; // ������������� �������� � ���������
    }

    public void StartRoad()
    {
        isMoving = true; // ������������ �������� � ���������
    }

    // ���������� ��� ����������� �����
    public void ZombieDefeated()
    {
        zombiesDefeated++;
        if (zombiesDefeated >= zombiesToCity && !isCityMode)
        {
            isCityMode = true; // ������� � ����� ��������� ������
        }
    }
}
