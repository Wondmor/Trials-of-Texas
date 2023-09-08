using UnityEngine;
using System.Collections;

public class Enemy : Character
{

    float hitPoints;

    public int damageStrength;
    Coroutine damageCoroutine;

    private void OnEnable() 
    {
        ResetCharacter();
    }


    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        while (true)
        {
            hitPoints = hitPoints - damage;
            if (hitPoints <= float.Epsilon)
            {
                KillCharacter();
                break;
            }

            if (interval > float.Epsilon)
            {
                yield return new WaitForSeconds(interval);
            }
            else
            {
                break;
            }
        }
    }

    public override void ResetCharacter()
    {
        hitPoints = startingHitPoints;
    }

}
