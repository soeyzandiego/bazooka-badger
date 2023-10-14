using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField][Range(1, 5)] int maxHealth = 3;

    int curHealth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (curHealth <= 0)
        {
            // game over
        }
    }

    public void ModifyHealth(int value)
    {
        curHealth += value;
    }

    void UpdateHearts()
    {

    }
}
