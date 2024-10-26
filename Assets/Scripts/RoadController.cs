using System.Collections.Generic;
using UnityEngine;

public class RoadController : MonoBehaviour
{
    public List<GameObject> roadPrefabs;  // ������� ������
    public List<GameObject> cityPrefabs;  // ������� ��������� ��������
    public Transform spawnPoint;          // ����� ������ ����� ��������
    public float roadSpeed = 5.0f;        // �������� �������� ���������
    public float segmentLength = 30.0f;   // ����� ������ �������
    public int initialSegments = 5;       // ��������� ���������� ��������
    public int zombiesToCity = 10;        // ���������� ����� ��� ����� �� �����

    private Queue<GameObject> activeSegments = new Queue<GameObject>();  // ������� �������� ��������
    private bool isCityMode = false;      // ����� ��������� ������
    private bool isMoving = true;         // ���������� ��������� ���������
    private int zombiesDefeated = 0;      // ���������� ������������ �����

    void Start()
    {
        // ������� ��������� ������� ������, ����� �� ���� ��������
        for (int i = 0; i < initialSegments; i++)
        {
            SpawnSegment(i * segmentLength);
        }
    }

    void Update()
    {
        if (isMoving)
        {
            MoveSegments();  // ������� ���������
        }
    }

    // ������� ��� �������� �������
    void MoveSegments()
    {
        foreach (GameObject segment in activeSegments)
        {
            segment.transform.Translate(Vector3.back * roadSpeed * Time.deltaTime);
        }

        // ������� � ������� ����� �������, ���� �� ������� �� ������� ���������
        if (activeSegments.Count > 0 && activeSegments.Peek().transform.position.z < -segmentLength)
        {
            GameObject oldSegment = activeSegments.Dequeue();
            Destroy(oldSegment);
            SpawnSegment(segmentLength * (activeSegments.Count + 1));  // ������� ��������� �������
        }
    }

    // ����� ������� ������ ��� ������ � ����������� �� ������
    void SpawnSegment(float zOffset)
    {
        GameObject segmentPrefab;

        // ����������, ��� ��������: ����� ��� ������
        if (isCityMode)
        {
            int randomIndex = Random.Range(0, cityPrefabs.Count);
            segmentPrefab = cityPrefabs[randomIndex];
        }
        else
        {
            int randomIndex = Random.Range(0, roadPrefabs.Count);
            segmentPrefab = roadPrefabs[randomIndex];
        }

        // ������� ������� � ������ �������� �� ��� Z
        Vector3 spawnPosition = spawnPoint.position + new Vector3(0, 0, zOffset);
        GameObject newSegment = Instantiate(segmentPrefab, spawnPosition, Quaternion.identity);
        activeSegments.Enqueue(newSegment);
    }

    public void StopRoad()
    {
        isMoving = false;  // ������������� ��������
    }

    public void StartRoad()
    {
        isMoving = true;  // ������������ ��������
    }

    // ���������� ��� ����������� �����
    public void ZombieDefeated()
    {
        zombiesDefeated++;
        if (zombiesDefeated >= zombiesToCity && !isCityMode)
        {
            isCityMode = true;  // ������� � ����� ������
        }
    }
}
