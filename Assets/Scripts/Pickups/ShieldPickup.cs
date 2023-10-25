using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickup : MonoBehaviour, IPickup
{
    [SerializeField] float chargeAmount = 3f;
    [SerializeField] int bonusValue = 10;
    [SerializeField] AudioClip bonusSound;
    [SerializeField] AudioClip pickUpSound;

    Shield shield;

    // Start is called before the first frame update
    void Start()
    {
        shield = FindObjectOfType<Shield>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUp()
    {
        if (shield.ChargePercentage() == 1)
        {
            GameManager.ModifyScore(bonusValue);
            FindObjectOfType<AudioPlayer>().PlayAudio(bonusSound);
        }
        else
        {
            shield.AddCharge(chargeAmount);
            FindObjectOfType<AudioPlayer>().PlayAudio(pickUpSound);
        }

        Destroy(gameObject);
    }
}
