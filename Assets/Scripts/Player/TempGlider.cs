using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempGlider : MonoBehaviour
{
    [HideInInspector] public bool colliding = false;

    void Update()
    {
        float speed = PlatformManager.scrollSpeed / 1.75f;
        transform.position -= new Vector3(speed * Time.deltaTime, 0);
    }

    public void Activate()
    {
        StartCoroutine(ActivateTemp());
    }


    IEnumerator ActivateTemp()
    {
        yield return new WaitForSeconds(0.6f);
        colliding = true;
    }
}
