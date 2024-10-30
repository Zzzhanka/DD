using System.Collections.Generic;
using UnityEngine;

public class RoadMovementController : MonoBehaviour
{
    public float roadSpeed = 5.0f;  
    public Queue<GameObject> activeSegments = new Queue<GameObject>(); 
    private bool isMoving = true; 

    void Update()
    {
        if (isMoving)
        {
            MoveSegments();  // Двигаем окружение
        }
    }

    void MoveSegments()
    {
        foreach (GameObject segment in activeSegments)
        {
            segment.transform.Translate(Vector3.back * roadSpeed * Time.deltaTime);
        }

        if (activeSegments.Count > 0 && activeSegments.Peek().transform.position.z < -30f)
        {
            GameObject oldSegment = activeSegments.Dequeue();
            Destroy(oldSegment);
        }
    }

    public void AddSegment(GameObject segment)
    {
        activeSegments.Enqueue(segment);
    }

    public void StopMovement()
    {
        isMoving = false;
    }

    public void StartMovement()
    {
        isMoving = true;
    }

    public void SetSpeed(float speed)
    {
        roadSpeed = speed;
    }
}
