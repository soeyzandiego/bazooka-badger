using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [SerializeField] int spawnAmount;
    [SerializeField] GameObject[] prefabs;

    List<Platform> platforms = new List<Platform>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            GameObject rPrefab = prefabs[Random.Range(0, prefabs.Length)];
            GameObject newPlatformObject = Instantiate(rPrefab, transform);
            Platform newPlatform = newPlatformObject.GetComponent<Platform>();
            platforms.Add(newPlatform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
