
using System.Collections;
using System.Linq.Expressions;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour
{
    public GameObject prefab;
    public int numberOfPrefabInstances;
    public Vector3 generationAreaSize = new Vector3(100f, 1f, 100f);

    public Transform parentContainer;
    public float spawnSeconds = 0f;
    public int counter = 0;

    public float absoluteGroundLevel = 0f;

    void start()
    {
        prefab.GetComponent<EnemyAI>().playerTransform = GameObject.FindWithTag("Player").transform;
        prefab.GetComponent<EnemyAI>().bloodScreen = GameObject.FindWithTag("BloodScreen");
    }
    void Update()
    {
        if (parentContainer == null)
        {
            parentContainer = transform.root;
        }

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, absoluteGroundLevel, gameObject.transform.position.z);
        if (counter <= numberOfPrefabInstances)
        {
            StartCoroutine(GenerateCall());
            counter++;
        }
       
    }

    void Generate()
    {
        GameObject gO;
            Vector3 randomPosition = GetRandomPositionInGenerationArea();
            Quaternion randomRotation = Quaternion.Euler(-90f, 0f, 0f);
            //Instantiate(prefab, randomPosition, randomRotation);
           gO= Instantiate(prefab, randomPosition, randomRotation, parentContainer.transform);
           gO.GetComponent<EnemyAI>().playerTransform = GameObject.FindWithTag("Player").transform;
           gO.GetComponent<EnemyAI>().bloodScreen = GameObject.FindWithTag("BloodScreen");
        
    }

    Vector3 GetRandomPositionInGenerationArea()
    {
        Vector3 randomPosition = new Vector3(
           Random.Range(-generationAreaSize.x / 2, generationAreaSize.x / 2),
           0f,
           Random.Range(-generationAreaSize.z / 2, generationAreaSize.z / 2)
       );

      
        // randomPosition.y = Terrain.activeTerrain.SampleHeight(randomPosition);

        return transform.position + randomPosition;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, generationAreaSize);
    }

    public IEnumerator GenerateCall()
    {
        yield return new WaitForSeconds(spawnSeconds);
        Generate();
    }
}