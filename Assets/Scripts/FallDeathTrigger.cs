using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDeathTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth player = collision.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.Damage(100);
            FindObjectOfType<ScreenShake>().ShakeScreen(0.1f, 0.2f);
        }
    }
}
