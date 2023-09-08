using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject ammoPrefab;
    private static List<GameObject> ammoPool;
    public int poolSize;

    public float projectileVelocity;
    void Awake() 
    {
        if(ammoPool == null)
        {
            ammoPool = new List<GameObject>();
        }   

        for(int i = 0; i<poolSize; i++)
        {
            GameObject ammoObject = Instantiate(ammoPrefab);
            ammoObject.SetActive(false);
            ammoPool.Add(ammoObject);
        }
    }

    public GameObject SpawnAmmo(Vector3 location)
    {
        foreach(GameObject ammo in ammoPool)
        {
            if(ammo.activeSelf == false)
            {
                ammo.SetActive(true);
                ammo.transform.position = location;
                return ammo;
            }
        }

        return null;
    }

    public void FireAmmo(Vector3 fireDestination)
    {
        GameObject ammo = SpawnAmmo(transform.position);
        if(ammo != null)
        {
            Arc arcScript = ammo.GetComponent<Arc>();
            float travelDuration = 1.0f/projectileVelocity;
            StartCoroutine(arcScript.TravelArc(fireDestination,travelDuration));
        }
    }

    void OnDestroy() 
    {
        ammoPool = null;
    }
}
