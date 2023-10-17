using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour, IPickup
{
    [SerializeField] int bonusValue = 10;

    PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    public void PickUp()
    {
        if (playerHealth.HealthPercentage() == 1) { GameManager.ModifyScore(bonusValue); }
        else { playerHealth.Heal(1); }

        Destroy(gameObject);
    }
}
