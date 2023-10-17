using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [Header("Spawn Area")]
    [SerializeField] float maxY;

    [SerializeField] GameObject[] prefabs;

    public static float scrollSpeed = 1.15f;

    List<Platform> platforms = new List<Platform>();

    EnemySpawner enemySpawner;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawner = GetComponent<EnemySpawner>();

        foreach (Platform newPlatform in GetComponentsInChildren<Platform>())
        {
            float newY = Random.Range(-maxY, maxY);
            newPlatform.transform.position = new Vector2(newPlatform.transform.position.x, newY);
            platforms.Add(newPlatform);
        }

        enemySpawner.ChanceSpawn(platforms);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector3(9, 0), new Vector3(5, maxY * 2));
    }
}
