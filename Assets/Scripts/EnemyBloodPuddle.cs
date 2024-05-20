using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBloodPuddle : MonoBehaviour
{
    private GameObject _BloodPuddle;
    // Start is called before the first frame update
    void Start()
    {
        _BloodPuddle = Instantiate(Resources.Load<GameObject>("blood_puddle"), this.transform.position,
            Resources.Load<GameObject>("blood_puddle").transform.rotation);
        _BloodPuddle.transform.SetParent(this.transform);
        _BloodPuddle.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
