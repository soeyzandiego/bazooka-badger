using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnInfo
    {
        [SerializeField] public GameObject prefab;
        [SerializeField] public int chance; // out of 100
    }

    [SerializeField] SpawnInfo[] enemies;

    public void ChanceSpawn(List<Platform> platforms)
    {
        foreach (Platform curPlatform in platforms)
        {
            int roll = Random.Range(0, 101);

            if (!curPlatform.ignoreSpawn)
            {
                foreach (SpawnInfo enemy in enemies)
                {
                    if (roll <= enemy.chance)
                    {
                        Vector3 pos = curPlatform.transform.position + new Vector3(0, 1.15f, 0);
                        GameObject newEnemy = Instantiate(enemy.prefab, pos, Quaternion.Euler(0, 0, 0));
                        newEnemy.transform.SetParent(curPlatform.transform);
                        break;
                    }
                }
            }
        }
    }

    public void ChanceSpawn(Platform platform)
    {
        int roll = Random.Range(0, 101);

        if (!platform.ignoreSpawn)
        {
            foreach (SpawnInfo enemy in enemies)
            {
                if (roll <= enemy.chance)
                {
                    Vector3 pos = platform.transform.position + new Vector3(0, 1.15f, 0);
                    GameObject newEnemy = Instantiate(enemy.prefab, pos, Quaternion.Euler(0, 0, 0));
                    newEnemy.transform.SetParent(platform.transform);
                    break;
                }
            }
        }
    }
}
