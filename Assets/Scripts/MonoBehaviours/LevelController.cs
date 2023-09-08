using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelController : MonoBehaviour
{

    public SpawnPoint playerSpawnPoint;
    public static LevelController sharedInstance = null;
    private void Awake() 
    {
        if(sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            sharedInstance = this;
        }
    }
    public GameObject gameObject;
    Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
    void Start()
    {
        SetupScene();
        Instantiate(gameObject, position, transform.rotation);
    }
    

    public void SetupScene()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if(playerSpawnPoint!=null)
        {
            GameObject player = playerSpawnPoint.SpawnObject();
        }
        
    
    }
}
