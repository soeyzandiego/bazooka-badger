using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float xSpeed = 5;
    [SerializeField] float ySpeed = 5;
    [SerializeField] float jumpForce = 3;
    [SerializeField] float dismountForce = 3.5f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform shootPoint;
    [SerializeField] Collider2D feet;

    bool onGlider = true;
    bool grounded = true;

    // Component References
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer sprite;
    Glider glider;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        glider = GetComponentInChildren<Glider>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = feet.IsTouchingLayers(LayerMask.GetMask("Ground"));
        anim.SetBool("grounded", grounded);

        if (onGlider)
        {
            GliderMovement();
        }
        else
        {
            GroundMovement();
        }
        

        if (Input.GetKeyDown(KeyCode.X))
        {
            Instantiate(bulletPrefab, shootPoint.position, Quaternion.Euler(Vector3.zero));
            anim.SetTrigger("shoot");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onGlider)
            {
                onGlider = !onGlider;
                rb.isKinematic = false;
                rb.velocity += new Vector2(dismountForce * Mathf.Sign(rb.velocity.x), dismountForce);
                glider.Dismount();
            }
        }

        if (Input.GetKeyDown(KeyCode.M) && !onGlider)
        {
            onGlider = !onGlider;
            glider.Mount();
            rb.isKinematic = true;
        }
    }

    void GliderMovement()
    {
        float horAxis = Input.GetAxis("Horizontal");
        float verAxis = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(horAxis * xSpeed, verAxis * ySpeed);

        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -rb.velocity.x);
    }

    void GroundMovement()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
        float horAxis = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horAxis * xSpeed, rb.velocity.y);

        if (Mathf.Abs(rb.velocity.x) > 0)
        {
            switch (Mathf.Sign(rb.velocity.x))
            {
                case 1:
                    sprite.flipX = false;
                break;

                case -1:
                    sprite.flipX = true;
                break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded) 
        { 
            rb.velocity += new Vector2(0, jumpForce);
            anim.SetTrigger("jump");
        }

        // cut the velocity of the jump if space is let go
        if (!grounded)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                float _yVel = rb.velocity.y / 1.5f;
                rb.velocity = new Vector2(rb.velocity.x, _yVel);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //if (!onGlider)
        //{
        //    Glider collGlider = collision.gameObject.GetComponent<Glider>();
        //    if (collGlider != null)
        //    {
        //        if (collGlider.temp)
        //        {
        //            glider.Mount();
        //            print("temp glider");
        //        }
        //    }
        //}
    }
}
