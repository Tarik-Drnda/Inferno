using UnityEngine;

public class ScrollingTexture : MonoBehaviour
{
    public float speedX = 0.1f; 
    public float speedY = 0.11f; 
    private float curX; 
    private float curY;

    void Start()
    {
        curX = GetComponent<Renderer>().material.mainTextureOffset.x;
        curY = GetComponent<Renderer>().material.mainTextureOffset.y;
    }

    void FixedUpdate()
    {
        curX += Time.deltaTime * speedX;
        curY += Time.deltaTime * speedY;
        GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector3(curX, 0, curY));
    }
}