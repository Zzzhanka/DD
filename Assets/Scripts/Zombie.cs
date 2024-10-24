using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float speed = 3f;
    public float damageAmount = 10f; // Урон, наносимый зомби
    public float attackCooldown = 1.5f; // Время между атаками
    private float lastAttackTime;

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        // Уничтожение, если зомби выходит за пределы карты
        if (transform.position.z < -30f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Наносим урон, если прошло достаточное время после последней атаки
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                other.GetComponent<CarController>().TakeDamage((int)damageAmount); // Наносим урон
                lastAttackTime = Time.time; // Обновляем время последней атаки
                Destroy(gameObject);
            }
        }
    }
}
