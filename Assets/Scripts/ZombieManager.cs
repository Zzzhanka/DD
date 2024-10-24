using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public GameObject zombiePrefab;        // ������ �����
    public Transform[] spawnPoints;           // ����� ������ �����
    public int zombiesToDefeat = 10;       // ���������� ����� ��� �������� �� ����� �������
    public float spawnInterval = 3f;       // �������� ����� ������� �����

    private int zombiesDefeated = 0;       // ���������� ������������ �����

    public RoadController roadController;  // ������ �� ���������� �����
    public GameObject cityPrefab;          // ������ ������

    void Start()
    {
        StartCoroutine(SpawnZombies());
    }

    IEnumerator SpawnZombies()
    {
        while (zombiesDefeated < zombiesToDefeat)
        {
            SpawnZombie();
            yield return new WaitForSeconds(spawnInterval);
        }

        // ������� �� ������� ������ ����� ������� ���������� ������������ �����
        GenerateCity();
    }
    void SpawnZombie()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(zombiePrefab, spawnPoints[randomIndex].position, Quaternion.identity);
    }

    public void ZombieDefeated()
    {
        zombiesDefeated++;
    }

    void GenerateCity()
    {
        roadController.StopRoad(); // ������������� �������� ������
        Instantiate(cityPrefab, new Vector3(0, 0, 100), Quaternion.identity); // ����� ������
    }
}
