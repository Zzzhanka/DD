using UnityEngine;

public class CarController : MonoBehaviour
{
    public float laneDistance = 4.0f;  // ���������� ����� ��������
    public float moveSpeed = 10.0f;    // �������� �������� ����
    private int currentLane = 1;        // ������� ������ (0 = �����, 1 = �����, 2 = ������)

    void Update()
    {
        // ��������� �����
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
        // ������� ������� �� X � ����������� �� ������� ������
        Vector3 targetPosition = new Vector3(currentLane * laneDistance - laneDistance, transform.position.y, transform.position.z);

        // ������� ����������� � ������ �����
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
