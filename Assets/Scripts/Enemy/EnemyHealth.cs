using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField][Range(1, 10)] int maxHealth = 6;

    [Header("Audio")]
    [SerializeField] AudioClip hurtSound;

    Enemy controller;

    int curHealth;
    bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
        controller = GetComponent<Enemy>();
    }


    public void Damage(int value)
    {
        if (dead) { return; }
        curHealth -= value;
        FindObjectOfType<AudioPlayer>().PlayAudio(hurtSound);

        if (curHealth <= 0)
        {
            dead = true;
            controller.Kill();
        }
    }
}
