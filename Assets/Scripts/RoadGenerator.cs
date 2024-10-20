using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public List<GameObject> roadPrefabs;  // Список префабов дороги
    public int numberOfSegments = 5;      // Количество активных сегментов
    public float roadLength = 20.0f;      // Длина одного сегмента дороги
    public float moveSpeed = 10.0f;       // Скорость движения дороги

    private List<GameObject> roadSegments = new List<GameObject>();
    private float spawnZ = 0.0f;

    void Start()
    {
        // Генерируем начальные сегменты
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
        // Выбираем случайный префаб из списка
        GameObject randomPrefab = roadPrefabs[Random.Range(0, roadPrefabs.Count)];
        GameObject segment = Instantiate(randomPrefab, new Vector3(0, 0, spawnZ), Quaternion.identity);

        roadSegments.Add(segment);
        spawnZ += roadLength;
    }

    void ReuseRoadSegment()
    {
        GameObject oldSegment = roadSegments[0];
        roadSegments.RemoveAt(0);

        // Перемещаем сегмент вперёд
        oldSegment.transform.position = new Vector3(0, 0, spawnZ);
        roadSegments.Add(oldSegment);

        spawnZ += roadLength;
    }

    void MoveRoadSegments()
    {
        // Двигаем все сегменты дороги назад
        foreach (GameObject segment in roadSegments)
        {
            segment.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
    }
}
