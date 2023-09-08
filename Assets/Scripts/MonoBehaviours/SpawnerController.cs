using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{

    [System.Serializable]
    public class Spawner
    {
        public GameObject enemy;
        public Transform spawnPoint;
        public AnimationCurve spawnCurve;
        public float spawnInterval = 5f;
        public float spawnNumber;
        public float timer;
        public void SpawnObject()
        {
            timer -= Time.deltaTime;
            if(timer <= 0 && spawnNumber>0)
            {
                float gap = spawnInterval * (float)spawnCurve.Evaluate(Time.time);
                timer = gap;
                spawnNumber --;
                Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
                
            }
        }
    }

    public Spawner[] spawners;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < spawners.Length; i++)
        {
            spawners[i].SpawnObject();
        }



    }
}
