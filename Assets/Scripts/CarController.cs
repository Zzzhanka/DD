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

    public RoadController roadController; // ������ �� RoadController

    public Transform steeringWheel;       // ����
    public float maxSteeringAngle = 45.0f; // ������������ ���� �������� ����

    private bool isPlayerInCar = true;
    private bool isPlayerNearCar = false;
    private bool isPlayerRunning = false; // ����, ����� �������� ���

    private void Start()
    {
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
    }

    void Update()
    {
        if (isPlayerRunning) return; // ���� ����� �����, ������ �� �����������

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

        // ������� ���� � ����������� �� ����� ������
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

        roadController.StopRoad(); // ��������� ������
    }

    void EnterCar()
    {
        isPlayerInCar = true;
        player.SetActive(false);
        player.transform.position = playerEnterPoint.position;

        roadController.StartRoad(); // ������ ������
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

    // ����� ��������� �����
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
        isPlayerRunning = true; // �������� ����� ����

        // ���������� ������ ���� ������
        player.GetComponent<FirstPersonMovement>().enabled = true;
        player.SetActive(true);

        // ������������� ������ � ������
        StopCarAndRoad();
    }

    void StopCarAndRoad()
    {
        isPlayerInCar = false;
        roadController.StopRoad();
    }
}
