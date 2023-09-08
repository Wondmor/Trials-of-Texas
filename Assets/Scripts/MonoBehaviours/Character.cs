using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour
{
    
    public float maxHitPoints;
    public float startingHitPoints;

    public virtual void KillCharacter()
    {
        Destroy(gameObject);
    }

    public abstract void ResetCharacter();


    public abstract IEnumerator DamageCharacter(int damage, float interval);


}
