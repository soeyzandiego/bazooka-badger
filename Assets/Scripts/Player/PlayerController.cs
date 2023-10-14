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
        if (onGlider)
        {
            GliderMovement();
            anim.SetBool("grounded", true);
        }
        else
        {
            GroundMovement();
            grounded = feet.IsTouchingLayers(LayerMask.GetMask("Ground"));
            anim.SetBool("grounded", grounded);
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
            sprite.flipX = false;
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
            if (Mathf.Sign(rb.velocity.x) == 1) { sprite.flipX = false; }
            else if (Mathf.Sign(rb.velocity.x) == -1) { sprite.flipX = true; }
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

    // TODO change to raycast, so can only mount from top
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!onGlider)
        {
            TempGlider collGlider = collision.gameObject.GetComponent<TempGlider>();
            if (collGlider != null && collGlider.colliding)
            {
                glider.Mount();
                onGlider = true;
                rb.isKinematic = true;
                sprite.flipX = false;
                collGlider.colliding = false;
            }
        }
    }
}
