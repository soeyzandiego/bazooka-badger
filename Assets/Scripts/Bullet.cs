using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] float speed = 13;
    Vector2 dir = Vector2.right;

    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = dir * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bullet>() != null) { return; } // don't let bullets collide with each other
        if (collision.GetComponent<PlayerHealth>() != null) 
        { 
            collision.GetComponent<PlayerHealth>().Damage(damage);
            Destroy(gameObject);
        }
        else if (collision.GetComponent<EnemyHealth>() != null) 
        { 
            collision.GetComponent<EnemyHealth>().Damage(damage);
            Destroy(gameObject);
        }
        else if (collision.GetComponent<Shield>() != null)
        {
            collision.GetComponent<Shield>().DamageShield(damage);
            Destroy(gameObject);
        }
        else if (collision.tag == "Shredder") 
        { 
            Destroy(gameObject); 
        }
    }

    public void SetDir(Vector2 _dir)
    {
        dir = _dir;
    }
}
