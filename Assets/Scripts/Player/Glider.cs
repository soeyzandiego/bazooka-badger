using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glider : MonoBehaviour
{
    [SerializeField] GameObject gliderPrefab;

    TempGlider tempGlider = null;

    void Start()
    {
        FindObjectOfType<ParticleFollow>().SetFollowTarget(transform);
    }

    public void Dismount()
    {
        gameObject.SetActive(false);
        if (tempGlider == null)
        {
            GameObject tempGliderObject = Instantiate(gliderPrefab, transform.position, transform.rotation);
            tempGlider = tempGliderObject.GetComponent<TempGlider>();
            tempGlider.transform.parent = null;
        }
        tempGlider.gameObject.SetActive(true);
        tempGlider.transform.position = transform.position;
        // TODO gradually rotate back to upright
        tempGlider.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(transform.rotation.z, 0, 0.5f));
        tempGlider.Activate();

        FindObjectOfType<ParticleFollow>().SetFollowTarget(tempGlider.transform);
    }

    public void Mount()
    {
        gameObject.SetActive(true);
        tempGlider.gameObject.SetActive(false);
        
        FindObjectOfType<ParticleFollow>().SetFollowTarget(transform);
    }
}
