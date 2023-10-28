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
            // clears pick ups and enemies
            Transform[] children = platform.GetComponentsInChildren<Transform>();
            if (children.Length > 1) { Destroy(children[1].gameObject); }

            FindObjectOfType<PlatformManager>().Recycle(platform);
        }
    }
}
