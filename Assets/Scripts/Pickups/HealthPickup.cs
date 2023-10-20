using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour, IPickup
{
    [SerializeField] int bonusValue = 10;
    [SerializeField] AudioClip bonusSound;
    [SerializeField] AudioClip pickUpSound;

    PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    public void PickUp()
    {
        if (playerHealth.HealthPercentage() == 1) 
        {
            GameManager.ModifyScore(bonusValue);
            FindObjectOfType<AudioPlayer>().PlayAudio(bonusSound);
        }
        else 
        { 
            playerHealth.Heal(1);
            FindObjectOfType<AudioPlayer>().PlayAudio(pickUpSound);
        }

        Destroy(gameObject);
    }
}
