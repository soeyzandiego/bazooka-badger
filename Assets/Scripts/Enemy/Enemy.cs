using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform shootPoint;
    [SerializeField] int pointValue = 10;

    [Header("Shoot")]
    [SerializeField] float minShootDelay = 0.4f;
    [SerializeField] float maxShootDelay = 1.2f;

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
}
