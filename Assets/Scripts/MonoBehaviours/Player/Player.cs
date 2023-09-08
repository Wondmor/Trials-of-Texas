using System.Collections;

using UnityEngine;

public class Player : Character
{
    public HitPoints hitPoints;
    
    public HealthBar healthBarPrefab;
    HealthBar healthBar;

    void Start()
    {
        ResetCharacter();
    }
    void OnTriggerEnter(Collider collision)  
    {
        if(collision.gameObject.CompareTag("CanBePickedUp"))
        {
            Item hitObject = collision.gameObject.GetComponent<Consumable>().item;
            if(hitObject != null)
            {
                bool shouldDisapper = false;
                print(hitObject.objectName + "+1");
                switch(hitObject.itemType)
                {
                    case Item.ItemType.COIN:
                        shouldDisapper = true;
                        break;
                    case Item.ItemType.HEALTH:
                        shouldDisapper = AdjustHitPoints(hitObject.quantity);
                        break;
                    default:
                        break;
                }
                if(shouldDisapper)
                {
                    collision.gameObject.SetActive(false);
                }
                
            }
            

        }
    }

    public bool AdjustHitPoints(int amount)
    {
        if (hitPoints.value < maxHitPoints)
        {
            hitPoints.value = hitPoints.value + amount;
            print(amount);
            return true;
        }
        return false;
    }

    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        while (true)
        {
            hitPoints.value = hitPoints.value - damage;
            if (hitPoints.value <= float.Epsilon)
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

    public override void KillCharacter()
    {
        base.KillCharacter();

        Destroy(healthBar.gameObject);

    }

public override void ResetCharacter()
    {
        hitPoints.value = startingHitPoints;
        healthBar = Instantiate(healthBarPrefab);
        healthBar.character = this;
    }


}

