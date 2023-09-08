using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public HitPoints hitPoints;
    [HideInInspector]
    public Player character;
    public Image meterImage;
    float maxHitPoints;

    void Start()
    {
        maxHitPoints = character.maxHitPoints;    
    }

    // Update is called once per frame
    void Update()
    {
        if(character != null)
        {
            meterImage.fillAmount = hitPoints.value/maxHitPoints;


        }
    }
}
