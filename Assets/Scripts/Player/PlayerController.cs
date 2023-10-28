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
    Glider glider;
    AudioSource audioSource;
    Shield shield;

    float horAxis;
    float verAxis;
    float faceDir = 1;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        glider = GetComponentInChildren<Glider>();
        audioSource = GetComponent<AudioSource>();
        shield = GetComponentInChildren<Shield>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("onGlider", onGlider);
        
        if (dead) 
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 1.5f * Time.deltaTime);
            return; 
        }
        if (GameManager.state != GameManager.GameState.PLAYING) { return; }

        horAxis = Input.GetAxis("Horizontal");
        verAxis = Input.GetAxis("Vertical");

        ClampPosition();

        if (!onGlider)
        {
            anim.SetBool("running", Mathf.Abs(horAxis) > 0.1f);
            anim.SetBool("grounded", grounded);

            if (Input.GetKeyDown(KeyCode.Space) && grounded)
            {
                rb.velocity += new Vector2(0, jumpForce);
                audioSource.PlayOneShot(jumpSound);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow)) { faceDir = 1; }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { faceDir = -1; }
            print(faceDir);

            Vector3 temp = transform.localScale;
            temp.x = faceDir;
            transform.localScale = temp;

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

        // shoot
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (onGlider)
            {
                Instantiate(bulletPrefab, shootPoint.position, Quaternion.Euler(Vector3.zero));
                anim.SetTrigger("shoot");
                audioSource.PlayOneShot(shootSound);
            }
            else
            {
                if (Mathf.Abs(horAxis) < 0.01f)
                {
                    GameObject newBullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.Euler(Vector3.zero));
                    newBullet.GetComponent<Bullet>().SetDir(new Vector2(transform.localScale.x, 0));
                    anim.SetTrigger("shoot");
                    audioSource.PlayOneShot(shootSound);
                }
            }
        }

        // shield
        if (onGlider)
        {
            if (Input.GetKeyDown(KeyCode.C)) { shield.Activate(); }
            if (Input.GetKeyUp(KeyCode.C)) { shield.Deactivate(); }
        }

        // dismount
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onGlider)
            {
                shield.Deactivate();
                onGlider = false;
                rb.isKinematic = false;
                rb.AddForce(new Vector2(dismountForce * Mathf.Sign(rb.velocity.x), dismountForce), ForceMode2D.Impulse);
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
        if (dead) { return; }
        if (GameManager.state != GameManager.GameState.PLAYING) { return; }

        if (onGlider) { GliderMovement(); }
        else { GroundMovement(); }
    }

    void ClampPosition()
    {
        // clamp pos values
        Vector3 clampPos = transform.position;
        clampPos.x = Mathf.Clamp(clampPos.x, leftBound, rightBound);
        if (onGlider) { clampPos.y = Mathf.Clamp(clampPos.y, botBound, topBound); }
        transform.position = clampPos;
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

        //if (Mathf.Abs(rb.velocity.x) > 0)
        //{
        //    if (Mathf.Sign(rb.velocity.x) == 1) { sprite.flipX = false; }
        //    else if (Mathf.Sign(rb.velocity.x) == -1) { sprite.flipX = true; }
        //}

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
        // mount glider
        if (!onGlider)
        {
            TempGlider collGlider = collision.gameObject.GetComponent<TempGlider>();
            if (collGlider != null && collGlider.colliding)
            {
                // face player forward
                Vector3 temp = transform.localScale;
                temp.x = 1;
                transform.localScale = temp;
                
                glider.Mount();
                onGlider = true;
                rb.isKinematic = true;
                //sprite.flipX = false;
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
