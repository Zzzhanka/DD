using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float laneSwitchSpeed = 5.0f;
    public GameObject player;
    public Transform playerExitPoint;
    public Transform playerEnterPoint;
    public Slider healthSlider;
    public float maxHealth = 100;
    public float health;

    public RoadController roadController; // Ссылка на RoadController

    public Transform steeringWheel;       // Руль
    public float maxSteeringAngle = 45.0f; // Максимальный угол поворота руля

    private bool isPlayerInCar = true;
    private bool isPlayerNearCar = false;
    private bool isPlayerRunning = false; // Флаг, чтобы включить бег

    private void Start()
    {
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
    }

    void Update()
    {
        if (isPlayerRunning) return; // Если игрок бежит, машина не управляется

        if (isPlayerInCar)
        {
            HandleCarMovement();
            if (Input.GetKeyDown(KeyCode.E)) ExitCar();
        }
        else if (isPlayerNearCar && Input.GetKeyDown(KeyCode.E))
        {
            EnterCar();
        }
    }

    void HandleCarMovement()
    {
        float moveX = Input.GetAxis("Horizontal") * laneSwitchSpeed * Time.deltaTime;
        transform.Translate(new Vector3(moveX, 0, 0));

        // Поворот руля в зависимости от ввода игрока
        RotateSteeringWheel(Input.GetAxis("Horizontal"));
    }

    void RotateSteeringWheel(float input)
    {
        float steeringAngle = input * maxSteeringAngle;
        steeringWheel.localRotation = Quaternion.Euler(0, 0, -steeringAngle);
    }

    void ExitCar()
    {
        isPlayerInCar = false;
        player.transform.position = playerExitPoint.position;
        player.SetActive(true);

        roadController.StopRoad(); // Остановка дороги
    }

    void EnterCar()
    {
        isPlayerInCar = true;
        player.SetActive(false);
        player.transform.position = playerEnterPoint.position;

        roadController.StartRoad(); // Запуск дороги
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerNearCar = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerNearCar = false;
        }
    }

    // Метод получения урона
    public void TakeDamage(int damage)
    {
        health -= damage;
        healthSlider.value = health;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("PlayerDie");
        isPlayerRunning = true; // Включаем режим бега

        // Активируем скрипт бега игрока
        player.GetComponent<FirstPersonMovement>().enabled = true;
        player.SetActive(true);

        // Останавливаем машину и дорогу
        StopCarAndRoad();
    }

    void StopCarAndRoad()
    {
        isPlayerInCar = false;
        roadController.StopRoad();
    }
}
