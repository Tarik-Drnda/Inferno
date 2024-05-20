using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneratorFloor : MonoBehaviour
{
    public GameObject DesertBlock;
    public GameObject FloorDetail;
    public GameObject Pillar;
    void Start()
    {
        Generate();
    }

    private void Generate()
    {
        for (int i = -20; i <= 20; i++)
        {
            
            for (int j = -20; j <= 20; j++)
            {
                if (j == -20)
                {
                    Instantiate(Pillar, new Vector3(i, 0, 0), Quaternion.identity);
                }

                Instantiate(DesertBlock, new Vector3(i,0,j), Quaternion.identity);

                var rnd = Random.Range(0, 10);
                if (rnd >5)
                {
                    Instantiate(FloorDetail, new Vector3(i, 0, j), Quaternion.identity);
                }
            }
        }
        
    }
    

}
