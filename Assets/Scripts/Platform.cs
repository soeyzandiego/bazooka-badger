using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] public bool ignoreSpawn = false;

    bool moving = true;

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            transform.position -= new Vector3(PlatformManager.scrollSpeed * Time.deltaTime, 0);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.transform.parent = null;
        }
    }
}
