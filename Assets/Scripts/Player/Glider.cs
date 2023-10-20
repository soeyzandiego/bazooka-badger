using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glider : MonoBehaviour
{
    [SerializeField] GameObject gliderPrefab;

    TempGlider tempGlider = null;

    // Update is called once per frame
    void Update()
    {

    }

    public void Dismount()
    {
        gameObject.SetActive(false);
        if (tempGlider == null)
        {
            GameObject tempGliderObject = Instantiate(gliderPrefab, transform.position, transform.rotation);
            tempGlider = tempGliderObject.GetComponent<TempGlider>();
            tempGlider.transform.parent = null;
            // TODO gradually rotate back to upright
            tempGlider.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(transform.rotation.z, 0, 0.5f));
            tempGlider.Activate();
        }
        else
        {
            tempGlider.gameObject.SetActive(true);
            tempGlider.transform.position = transform.position;
            tempGlider.transform.rotation = Quaternion.Euler(0, 0, 0);
            tempGlider.Activate();
        }
    }

    public void Mount()
    {
        gameObject.SetActive(true);
        tempGlider.gameObject.SetActive(false);
    }
}
