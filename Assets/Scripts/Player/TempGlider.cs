using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempGlider : MonoBehaviour
{
    [HideInInspector] public bool colliding = false;
    [HideInInspector] public bool lost = false;

    void Update()
    {
        float speed = PlatformManager.scrollSpeed / 1.75f;
        if (!lost) { transform.position -= new Vector3(speed * Time.deltaTime, 0); }

        if (transform.position.x < -7.9f) { lost = true; }
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

    public void NewGlider(Vector2 newPos)
    {
        lost = false;
        transform.position = newPos;
    }
}
