using UnityEngine;

public class CarController : MonoBehaviour
{
    public float laneDistance = 4.0f;  // Расстояние между полосами
    public float moveSpeed = 10.0f;    // Скорость движения вбок
    private int currentLane = 1;        // Текущая полоса (0 = влево, 1 = центр, 2 = вправо)

    void Update()
    {
        // Обработка ввода
        if (Input.GetKeyDown(KeyCode.LeftArrow) && currentLane > 0)
        {
            currentLane--;
            MoveToLane();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && currentLane < 2)
        {
            currentLane++;
            MoveToLane();
        }
    }

    void MoveToLane()
    {
        // Целевая позиция по X в зависимости от текущей полосы
        Vector3 targetPosition = new Vector3(currentLane * laneDistance - laneDistance, transform.position.y, transform.position.z);

        // Плавное перемещение в нужное место
        StopAllCoroutines();
        StartCoroutine(MoveToPosition(targetPosition));
    }

    System.Collections.IEnumerator MoveToPosition(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
