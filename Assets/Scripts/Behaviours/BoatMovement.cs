using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BoatMovement : MonoBehaviour
{
    public List<GameObject> points = new List<GameObject>();
    public float speed=1f;

    private float distance;

    private int index = 0;

    void Update()
    {
       
        Vector3 newPos;
        Vector3 destination = points[index].transform.position;
        newPos = Vector3.MoveTowards(this.transform.position,destination,speed * Time.deltaTime);

        Vector3 directionToTarget = this.transform.position - destination;
        Vector3 newDirection= Vector3.RotateTowards(-this.transform.forward,directionToTarget , speed * Mathf.Deg2Rad * Time.deltaTime, 0f);

        this.transform.rotation = Quaternion.LookRotation(-newDirection);
        this.transform.position = newPos;
        distance = Vector3.Distance(this.transform.position, destination);
        Debug.Log(distance);
        if (distance <= 0.05f)
        {
            index++;
            Debug.Log("Index++" + index);
        }
    }
    
}
