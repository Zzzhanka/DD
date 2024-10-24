using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public GameObject zombiePrefab;        // Префаб зомби
    public Transform[] spawnPoints;           // Точка спавна зомби
    public int zombiesToDefeat = 10;       // Количество зомби для перехода на новую локацию
    public float spawnInterval = 3f;       // Интервал между спавном зомби

    private int zombiesDefeated = 0;       // Количество уничтоженных зомби

    public RoadController roadController;  // Ссылка на контроллер дорог
    public GameObject cityPrefab;          // Префаб города

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

        // Переход на локацию города после нужного количества уничтоженных зомби
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
        roadController.StopRoad(); // Останавливаем движение дороги
        Instantiate(cityPrefab, new Vector3(0, 0, 100), Quaternion.identity); // Спавн города
    }
}
