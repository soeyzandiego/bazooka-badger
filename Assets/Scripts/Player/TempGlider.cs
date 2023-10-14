using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempGlider : MonoBehaviour
{
    [HideInInspector] public bool colliding = false;

    void Update()
    {
        transform.position -= new Vector3(PlatformManager.scrollSpeed * Time.deltaTime, 0);
    }

    public void Activate()
    {
        StartCoroutine(ActivateTemp());
    }


    IEnumerator ActivateTemp()
    {
        yield return new WaitForSeconds(0.5f);
        colliding = true;
    }
}
