using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EyeMng : MonoBehaviour
{
    public RawImage eyeImage;
    public float height;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        eyeImage.rectTransform.sizeDelta = screenSize;
    }

    // Update is called once per frame
    void Update()
    {
        eyeImage.material.SetFloat("height", height);
    }
}
