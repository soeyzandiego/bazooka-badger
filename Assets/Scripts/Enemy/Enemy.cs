using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [System.Serializable]
    public class PickUpDrop 
    {
        [SerializeField] public GameObject pickUpPrefab;
        [SerializeField] public int dropChance;
    }

    [SerializeField] PickUpDrop[] drops;

    [SerializeField] int pointValue = 10;

    [Header("Shoot")]
    [SerializeField] float minShootDelay = 0.4f;
    [SerializeField] float maxShootDelay = 1.2f;
    [SerializeField] Transform shootPoint;
    [SerializeField] GameObject bulletPrefab;


    float shootTimer;

    PlayerController playerRef;
    int playerDir;

    // Start is called before the first frame update
    void Start()
    {
        playerRef = FindObjectOfType<PlayerController>();
        shootTimer = Random.Range(minShootDelay, maxShootDelay);
    }

    // Update is called once per frame
    void Update()
    {
        // whether the player is to the left or right of enemy
        playerDir = Mathf.FloorToInt(Mathf.Sign(playerRef.transform.position.x - transform.position.x));

        Vector3 temp = transform.localScale;
        temp.x = -playerDir;
        transform.localScale = temp;


        if (GameManager.state != GameManager.GameState.PLAYING) { return; }

        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }
        else
        {
            Shoot();
            shootTimer = Random.Range(minShootDelay, maxShootDelay);
        }
    }

    void Shoot()
    {
        GameObject newBulletObject = Instantiate(bulletPrefab, shootPoint.position, Quaternion.Euler(Vector3.zero));
        Bullet newBullet = newBulletObject.GetComponent<Bullet>();
        newBullet.SetDir(new Vector2(playerDir, 0));
    }

    public void Kill()
    {
        GameManager.ModifyScore(pointValue);

        if (drops.Length > 0)
        {
            foreach (PickUpDrop drop in drops)
            {
                int roll = Random.Range(0, 101);

                if (roll <= drop.dropChance)
                {
                    Instantiate(drop.pickUpPrefab, transform.position, transform.rotation, transform.parent);
                    break;
                }
            }
        }

        Destroy(gameObject);
        Destroy(this); // if for later, want to do a dead state where the body is on the ground
    }
}
