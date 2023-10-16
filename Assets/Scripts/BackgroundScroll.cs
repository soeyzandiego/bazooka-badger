using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 0.5f;

    RawImage rawImg;

    // Start is called before the first frame update
    void Start()
    {
        rawImg = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        Rect rect = rawImg.uvRect;
        rawImg.uvRect = new Rect(rect.position += new Vector2(scrollSpeed, 0) * Time.deltaTime, rawImg.uvRect.size);
    }
}
