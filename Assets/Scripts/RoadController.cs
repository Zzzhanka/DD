using System.Collections.Generic;
using UnityEngine;

public class RoadController : MonoBehaviour
{
    public List<GameObject> roadPrefabs;  // Префабы дороги
    public List<GameObject> cityPrefabs;  // Префабы городских участков
    public Transform spawnPoint;          // Точка спавна новых участков
    public float roadSpeed = 5.0f;        // Скорость движения окружения
    public float segmentLength = 30.0f;   // Длина одного участка
    public int initialSegments = 5;       // Начальное количество участков
    public int zombiesToCity = 10;        // Количество зомби для смены на город

    private Queue<GameObject> activeSegments = new Queue<GameObject>();  // Очередь активных участков
    private bool isCityMode = false;      // Режим генерации города
    private bool isMoving = true;         // Управление движением окружения
    private int zombiesDefeated = 0;      // Количество уничтоженных зомби

    void Start()
    {
        // Спавним начальные участки вперед, чтобы не было пробелов
        for (int i = 0; i < initialSegments; i++)
        {
            SpawnSegment(i * segmentLength);
        }
    }

    void Update()
    {
        if (isMoving)
        {
            MoveSegments();  // Двигаем окружение
        }
    }

    // Двигаем все активные участки
    void MoveSegments()
    {
        foreach (GameObject segment in activeSegments)
        {
            segment.transform.Translate(Vector3.back * roadSpeed * Time.deltaTime);
        }

        // Удаляем и спавним новый участок, если он выходит за пределы видимости
        if (activeSegments.Count > 0 && activeSegments.Peek().transform.position.z < -segmentLength)
        {
            GameObject oldSegment = activeSegments.Dequeue();
            Destroy(oldSegment);
            SpawnSegment(segmentLength * (activeSegments.Count + 1));  // Спавним следующий участок
        }
    }

    // Спавн участка дороги или города в зависимости от режима
    void SpawnSegment(float zOffset)
    {
        GameObject segmentPrefab;

        // Определяем, что спавнить: город или дорогу
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

        // Спавним участок с учетом смещения по оси Z
        Vector3 spawnPosition = spawnPoint.position + new Vector3(0, 0, zOffset);
        GameObject newSegment = Instantiate(segmentPrefab, spawnPosition, Quaternion.identity);
        activeSegments.Enqueue(newSegment);
    }

    public void StopRoad()
    {
        isMoving = false;  // Останавливаем движение
    }

    public void StartRoad()
    {
        isMoving = true;  // Возобновляем движение
    }

    // Вызывается при уничтожении зомби
    public void ZombieDefeated()
    {
        zombiesDefeated++;
        if (zombiesDefeated >= zombiesToCity && !isCityMode)
        {
            isCityMode = true;  // Переход в режим города
        }
    }
}
