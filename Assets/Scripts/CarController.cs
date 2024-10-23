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

    private bool isPlayerInCar = true;
    private bool isPlayerNearCar = false;

    // Ссылка на RoadController для управления движением дороги
    public RoadController roadController;

    private void Start()
    {
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
    }
    void Update()
    {
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
    }

    void ExitCar()
    {
        isPlayerInCar = false;
        player.transform.position = playerExitPoint.position;
        player.SetActive(true);

        // Остановка движения дороги
        roadController.StopRoad();
    }

    void EnterCar()
    {
        isPlayerInCar = true;
        player.SetActive(false);
        player.transform.position = playerEnterPoint.position;

        // Возобновление движения дороги
        roadController.StartRoad();
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

        Debug.Log("Health " + health);

        if (health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("PlayerDie");
    }
}
