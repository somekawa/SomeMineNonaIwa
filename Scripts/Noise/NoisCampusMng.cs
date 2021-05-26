using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ノイズなどのフィルターのサイズを画面サイズに合わせる処理
public class NoisCampusMng : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        RawImage[] rawImageList = gameObject.GetComponentsInChildren<RawImage>();
        foreach (RawImage rawImage in rawImageList)
        {
            rawImage.rectTransform.sizeDelta = screenSize;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
