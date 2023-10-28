using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(PlayerController))]
public class PlayerHealth : MonoBehaviour
{
    [SerializeField][Range(1, 8)] int maxHealth = 6;
    [SerializeField] Image[] targetImages;
    [SerializeField] Sprite[] heartSprites;

    [Header("Audio")]
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip deathSound;

    int curHealth;

    PlayerController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
        curHealth = maxHealth;
        UpdateHearts();
    }

    void Update()
    {
        foreach (Image heart in targetImages)
        {
            if (GameManager.state == GameManager.GameState.WAITING)
            {
                heart.enabled = false;
            }
            else
            {
                heart.enabled = true;
            }
        }
    }

    public void Damage(int value)
    {
        curHealth -= value;
        UpdateHearts();
        GetComponent<Animator>().SetTrigger("hurt");

        if (curHealth <= 0)
        {
            controller.Kill();
            controller.PlayAudio(deathSound);
            GameManager.ChangeState(GameManager.GameState.GAME_OVER);
            Destroy(this);
        }

        controller.PlayAudio(hurtSound);
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
