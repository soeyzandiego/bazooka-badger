using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour, IPickup
{
    [SerializeField] int bonusValue = 10;
    [SerializeField] AudioClip bonusSound;
    [SerializeField] AudioClip pickUpSound;

    PlayerHealth playerHealth;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PickUp()
    {
        // TODO move sounds somewhere else bc the object will be destroyed before the sound plays
        if (playerHealth.HealthPercentage() == 1) 
        {
            GameManager.ModifyScore(bonusValue);
            audioSource.PlayOneShot(bonusSound);
        }
        else 
        { 
            playerHealth.Heal(1);
            audioSource.PlayOneShot(pickUpSound);
        }

        Destroy(gameObject);
    }
}
