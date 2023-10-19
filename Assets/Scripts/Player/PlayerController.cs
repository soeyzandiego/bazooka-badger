using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float xSpeed = 5f;
    [SerializeField] float ySpeed = 5f;

    [Header("Bounds")]
    [SerializeField] float leftBound = -6.5f;
    [SerializeField] float rightBound = 6.5f;
    [SerializeField] float botBound = -3.3f;
    [SerializeField] float topBound = 4f;

    [Header("Jumping")]
    [SerializeField] float jumpForce = 3f;
    [SerializeField] float castDist = 1f;
    [SerializeField] float dismountForce = 3.5f;

    [Header("Shoot")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform shootPoint;

    [Header("Audio")]
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip shootSound;

    bool onGlider = true;
    bool grounded = true;
    bool dead = false;

    // component references
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer sprite;
    Glider glider;
    AudioSource audioSource;
    Shield shield;

    float horAxis;
    float verAxis;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        glider = GetComponentInChildren<Glider>();
        audioSource = GetComponent<AudioSource>();
        shield = GetComponentInChildren<Shield>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dead) { return; }
        
        horAxis = Input.GetAxis("Horizontal");
        verAxis = Input.GetAxis("Vertical");

        anim.SetBool("onGlider", onGlider);

        // clamp X value
        Vector3 clampPos = transform.position;
        clampPos.x = Mathf.Clamp(clampPos.x, leftBound, rightBound);
        clampPos.y = Mathf.Clamp(clampPos.y, botBound, topBound);
        transform.position = clampPos;

        if (!onGlider)
        {
            anim.SetBool("running", Mathf.Abs(horAxis) > 0.1f);
            anim.SetBool("grounded", grounded);

            if (Input.GetKeyDown(KeyCode.Space) && grounded)
            {
                rb.velocity += new Vector2(0, jumpForce);
                audioSource.PlayOneShot(jumpSound);
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
        

        if (Input.GetKeyDown(KeyCode.X))
        {
            Instantiate(bulletPrefab, shootPoint.position, Quaternion.Euler(Vector3.zero));
            anim.SetTrigger("shoot");
            audioSource.PlayOneShot(shootSound);
        }

        if (Input.GetKeyDown(KeyCode.C)) { shield.Activate(); }
        if (Input.GetKeyUp(KeyCode.C)) { shield.Deactivate(); }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onGlider)
            {
                onGlider = !onGlider;
                rb.isKinematic = false;
                rb.velocity += new Vector2(dismountForce * Mathf.Sign(rb.velocity.x), dismountForce);
                glider.Dismount();
                audioSource.PlayOneShot(jumpSound);
            }
        }

        // for debugging
        //if (Input.GetKeyDown(KeyCode.M) && !onGlider)
        //{
        //    onGlider = !onGlider;
        //    glider.Mount();
        //    rb.isKinematic = true;
        //    sprite.flipX = false;
        //}
    }

    void FixedUpdate()
    {
        if (onGlider) { GliderMovement(); }
        else { GroundMovement(); }
    }

    void GliderMovement()
    {
        rb.velocity = new Vector2(horAxis * xSpeed, verAxis * ySpeed);

        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -rb.velocity.x);
    }

    void GroundMovement()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
        rb.velocity = new Vector2(horAxis * xSpeed, rb.velocity.y);

        if (Mathf.Abs(rb.velocity.x) > 0)
        {
            if (Mathf.Sign(rb.velocity.x) == 1) { sprite.flipX = false; }
            else if (Mathf.Sign(rb.velocity.x) == -1) { sprite.flipX = true; }
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castDist, LayerMask.GetMask("Ground"));
        Debug.DrawRay(transform.position, Vector2.down * castDist, Color.red);

        if (hit.collider != null)
        {
            grounded = true;
            transform.parent = hit.collider.transform;
        }
        else
        {
            grounded = false;
            transform.parent = null;
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
                return;
            }

            IPickup pickup = collision.gameObject.GetComponent<IPickup>();
            if (pickup != null) { pickup.PickUp(); }
        }
    }

    public void PlayAudio(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }

    public void Kill()
    {
        dead = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * castDist);
    }
}
