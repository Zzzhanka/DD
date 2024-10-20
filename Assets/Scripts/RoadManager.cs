using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public List<GameObject> roadPrefabs;  // Список префабов дорог
    public Transform spawnPoint;          // Точка спавна новых дорог
    public float roadSpeed = 5.0f;        // Скорость движения дороги
    public float roadLength = 30.0f;      // Длина одного участка дороги
    private bool isMoving = true;         // Управляет движением и генерацией

    private Queue<GameObject> activeRoads = new Queue<GameObject>();  // Очередь активных участков

    void Start()
    {
        // Генерируем начальные участки дороги
        for (int i = 0; i < 3; i++)
        {
            SpawnRoad();
        }
    }

    void Update()
    {
        if (isMoving)
        {
            MoveRoads();  // Двигаем активные участки дороги
        }
    }

    void MoveRoads()
    {
        foreach (GameObject road in activeRoads)
        {
            road.transform.Translate(Vector3.back * roadSpeed * Time.deltaTime);
        }

        // Проверка: если участок дороги прошёл за пределы камеры, удаляем его и спавним новый
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
        isMoving = false;  // Останавливаем движение и генерацию
    }

    public void StartRoad()
    {
        isMoving = true;  // Возобновляем движение и генерацию
    }
}
