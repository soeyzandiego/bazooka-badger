using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempGlider : MonoBehaviour
{
    Collider2D coll;

    [HideInInspector] public bool colliding = false;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        
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
