using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject ammoPrefab;
    public float projectileVelocity;

    public GameObject SpawnAmmo(Vector3 location)
    {
        GameObject ammo = Instantiate(ammoPrefab);
        ammo.transform.position = location;
        return ammo;
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

    

}
