using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField][Range(1, 8)] int maxHealth = 6;
    [SerializeField] Image[] targetImages;
    [SerializeField] Sprite[] heartSprites;

    int curHealth;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = 5;
        UpdateHearts();
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
        for (int i = 1; i < maxHealth; i += 2)
        {
            if (i + 1 < curHealth) 
            { 
                targetImages[((i+1) / 2) - 1].sprite = heartSprites[0];
                Debug.Log("full heart");
            }
            else if (i == curHealth) 
            { 
                targetImages[((i + 1) / 2) - 1].sprite = heartSprites[1];
                Debug.Log("half heart");
            }
            else 
            { 
                targetImages[((i + 1) / 2) - 1].sprite = heartSprites[2];
                Debug.Log("empty heart");
            }
        }
    }
}
