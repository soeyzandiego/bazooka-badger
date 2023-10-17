using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] int maxShield = 5;

    int shieldHealth;
    bool active = false;

    Collider2D coll;
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        shieldHealth = maxShield;
        coll = GetComponent<Collider2D>();
        Deactivate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        coll.enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
    }

    public void Deactivate()
    {
        coll.enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Toggle()
    {
        active = !active;

        if (active) { Deactivate(); }
        else { Activate(); }
    }

    public void DamageShield(int value)
    {
        shieldHealth -= value;
    }
}
