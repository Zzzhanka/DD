using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float speed = 3f;
    public float damageAmount = 10f; // ����, ��������� �����
    public float attackCooldown = 1.5f; // ����� ����� �������
    private float lastAttackTime;

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        // �����������, ���� ����� ������� �� ������� �����
        if (transform.position.z < -30f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ������� ����, ���� ������ ����������� ����� ����� ��������� �����
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                other.GetComponent<CarController>().TakeDamage((int)damageAmount); // ������� ����
                lastAttackTime = Time.time; // ��������� ����� ��������� �����
                Destroy(gameObject);
            }
        }
    }
}
