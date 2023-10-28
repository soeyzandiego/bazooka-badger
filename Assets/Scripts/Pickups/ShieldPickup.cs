using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickup : MonoBehaviour, IPickup
{
    [SerializeField] float chargeAmount = 3f;
    [SerializeField] int bonusValue = 10;
    [SerializeField] AudioClip bonusSound;
    [SerializeField] AudioClip pickUpSound;

    [Header("Bonus Indicator")]
    [SerializeField] GameObject bonusIndicator;
    [SerializeField] Vector3 indicatorOffset = new Vector3(0.5f, 0.5f);

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
            Instantiate(bonusIndicator, transform.position + indicatorOffset, transform.rotation);
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
