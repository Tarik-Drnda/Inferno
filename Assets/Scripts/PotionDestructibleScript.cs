using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionDestructibleScript : MonoBehaviour
{
   
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Destroy(this.transform.gameObject);
            
        }
    }
}
