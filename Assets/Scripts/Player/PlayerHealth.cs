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
        curHealth = maxHealth;
        UpdateHearts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int value)
    {
        curHealth -= value;
        UpdateHearts();

        if (curHealth <= 0)
        {
            GameManager.ChangeState(GameManager.GameState.GAME_OVER);
        }
    }

    public void Heal(int value)
    {
        curHealth += value;
        UpdateHearts();
    }

    void UpdateHearts()
    {
        for (int i = 1; i < maxHealth; i += 2)
        {
            if (i + 1 <= curHealth) { targetImages[((i+1) / 2) - 1].sprite = heartSprites[0]; }
            else if (i == curHealth) { targetImages[((i + 1) / 2) - 1].sprite = heartSprites[1]; }
            else { targetImages[((i + 1) / 2) - 1].sprite = heartSprites[2]; }
        }
    }

    public float HealthPercentage()
    {
        return curHealth / maxHealth;
    }
}
