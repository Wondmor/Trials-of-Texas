using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAmmo : MonoBehaviour
{
    public int damageInflicted;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<BoxCollider>())
        {
            Player player = collision.gameObject.GetComponent<Player>();
            StartCoroutine(player.DamageCharacter(damageInflicted, 0.0f));
            gameObject.SetActive(false);
            
        }
    }
}
