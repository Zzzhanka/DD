using System.Collections.Generic;
using UnityEngine;

public class RoadController : MonoBehaviour
{
    public List<GameObject> roadPrefabs;    // Список префабов дорог
    public List<GameObject> cityPrefabs;    // Список префабов городских участков
    public Transform spawnPoint;            // Точка спавна новых участков
    public float roadSpeed = 5.0f;          // Скорость движения
    public float roadLength = 30.0f;        // Длина одного участка
    public int zombiesToCity = 10;          // Количество зомби для начала города

    private Queue<GameObject> activeRoads = new Queue<GameObject>(); // Активные участки
    private bool isMoving = true;           // Управление движением
    private bool isCityMode = false;        // Режим генерации города
    private int zombiesDefeated = 0;        // Количество уничтоженных зомби

    void Start()
    {
        // Генерируем начальные участки дороги
        for (int i = 0; i < 3; i++)
        {
            SpawnRoadOrCity();
        }
    }

    void Update()
    {
        if (isMoving)
        {
            MoveRoads(); // Двигаем активные участки
        }
    }

    void MoveRoads()
    {
        foreach (GameObject road in activeRoads)
        {
            road.transform.Translate(Vector3.back * roadSpeed * Time.deltaTime);
        }

        // Удаление и спавн нового участка, если он вышел за пределы видимости
        if (activeRoads.Count > 0 && activeRoads.Peek().transform.position.z < -roadLength)
        {
            GameObject oldRoad = activeRoads.Dequeue();
            Destroy(oldRoad);
            SpawnRoadOrCity();
        }
    }

    void SpawnRoadOrCity()
    {
        // Если перешли в режим города, генерируем городские участки
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
        isMoving = false; // Останавливаем движение и генерацию
    }

    public void StartRoad()
    {
        isMoving = true; // Возобновляем движение и генерацию
    }

    // Вызывается при уничтожении зомби
    public void ZombieDefeated()
    {
        zombiesDefeated++;
        if (zombiesDefeated >= zombiesToCity && !isCityMode)
        {
            isCityMode = true; // Переход в режим генерации города
        }
    }
}
