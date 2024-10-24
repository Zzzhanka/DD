using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float speed = 3f;
    public float destroyThreshold = -30f; // Координата для удаления

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        if (transform.position.z < destroyThreshold)
        {
            Destroy(gameObject);
            FindObjectOfType<RoadController>().ZombieDefeated();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Bullet"))
        {
            FindObjectOfType<RoadController>().ZombieDefeated();
            Destroy(gameObject);
        }
    }
}
