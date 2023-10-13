using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glider : MonoBehaviour
{
    [SerializeField] GameObject gliderPrefab;

    // whether or not this is a temp glider for when dismounting
    [HideInInspector] public bool temp = false;

    GameObject tempGlider = null;

    // Update is called once per frame
    void Update()
    {

    }

    public void Dismount()
    {
        gameObject.SetActive(false);
        if (tempGlider == null)
        {
            tempGlider = Instantiate(gliderPrefab, transform.position, transform.rotation);
            tempGlider.transform.parent = null;
            // TODO gradually rotate back to upright
            tempGlider.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(transform.rotation.z, 0, 0.5f));
            tempGlider.GetComponent<Glider>().temp = true;
        }
        else
        {
            tempGlider.SetActive(true);
            tempGlider.transform.position = transform.position;
            tempGlider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void Mount()
    {
        gameObject.SetActive(true);
        tempGlider.SetActive(false);
    }
}
