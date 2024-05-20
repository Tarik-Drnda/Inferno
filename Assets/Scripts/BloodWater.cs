using System.Collections;
using UnityEngine;

public class BloodWater : MonoBehaviour
{
    public float speed = 1.0f;
    private Vector3 positionA;
    private Vector3 positionB;

    void Start()
    {
        positionA = new Vector3(transform.position.x, transform.position.y, 100);
        positionB = new Vector3(transform.position.x, transform.position.y, -100);
        StartCoroutine(MoveLoop());
    }

    IEnumerator MoveLoop()
    {
        while (true)
        {
            yield return StartCoroutine(MoveToPosition(positionA));
            yield return StartCoroutine(MoveToPosition(positionB));
        }
    }

    IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null; 
        }
    }
}