using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGliderPickup : MonoBehaviour, IPickup
{
    public void PickUp()
    {
        FindObjectOfType<TempGlider>().NewGlider(transform.position);
        Destroy(gameObject);
    }
}
