using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRecycle : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Platform platform = collision.GetComponent<Platform>();
        if (platform != null)
        {
            FindObjectOfType<PlatformManager>().Recycle(platform);
        }
    }
}
