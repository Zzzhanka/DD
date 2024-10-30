using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstRoadMove : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float destroyPositionZ = -30.0f; 

    private bool isMoving = true;  

    void Update()
    {
        if (isMoving)
        {
            MoveRoad();
        }
    }

    
    void MoveRoad()
    {
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

        
        if (transform.position.z < destroyPositionZ)
        {
            Destroy(gameObject);
        }
    }

    
    public void StopMovement()
    {
        isMoving = false;
    }

    public void StartMovement()
    {
        isMoving = true;
    }
}
