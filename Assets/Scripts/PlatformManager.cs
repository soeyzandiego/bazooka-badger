using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [Header("Spawn Area")]
    [SerializeField] float maxY;
    [SerializeField] float minRecycleOffset = 4f;
    [SerializeField] float maxRecycleOffset = 6f;

    public static float scrollSpeed = 1.5f;

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
        SetMoving(false);
    }

    public void SetMoving(bool _moving)
    {
        foreach (Platform platform in platforms)
        {
            platform.moving = _moving;
        }
    }

    public void Recycle(Platform _platform)
    {
        Platform lastPlatform = platforms[platforms.Count - 1];

        platforms.Remove(_platform);
        platforms.Add(_platform);

        // randomize position based on the position of the last platform
        float xOffset = Random.Range(minRecycleOffset, maxRecycleOffset);
        float newY = Random.Range(-maxY, maxY);
        _platform.transform.position = new Vector2(lastPlatform.transform.position.x + xOffset, newY);

        // chance spawn
        enemySpawner.ChanceSpawn(_platform);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector3(9, 0), new Vector3(5, maxY * 2));
    }
}
