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

    public RoadController roadController; // Новый контроллер генерации дороги
    public FirstRoadMove[] initialRoads;  // Ссылки на начальные сегменты дороги

    public Transform steeringWheel;
    public float maxSteeringAngle = 45.0f;

    private bool isPlayerInCar = true;
    private bool isPlayerNearCar = false;
    private bool isPlayerRunning = false;

    private void Start()
    {
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
    }

    void Update()
    {
        if (isPlayerRunning) return;

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

        StopAllMovement();
    }

    void EnterCar()
    {
        isPlayerInCar = true;
        player.SetActive(false);
        player.transform.position = playerEnterPoint.position;

        StartAllMovement();
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
        isPlayerRunning = true;

        player.GetComponent<FirstPersonMovement>().enabled = true;
        player.SetActive(true);

        StopAllMovement();
    }

    void StopAllMovement()
    {
        isPlayerInCar = false;
        roadController.StopRoad(); // Остановка основной дороги

        // Остановка движения для всех начальных сегментов
        foreach (FirstRoadMove initialRoad in initialRoads)
        {
            initialRoad.StopMovement();
        }
    }

    void StartAllMovement()
    {
        roadController.StartRoad(); // Запуск основной дороги

        // Запуск движения для всех начальных сегментов
        foreach (FirstRoadMove initialRoad in initialRoads)
        {
            initialRoad.StartMovement();
        }
    }
}
