using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Camera fullMapCamera;        // 全体マップを映すカメラの格納用
    public Image miniMap;               // ミニマップ画像の格納用

    // Start is called before the first frame update
    void Start()
    {
        fullMapCamera.gameObject.SetActive(false);      // 基本的に非表示
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))                    // キーが押されたとき
        {
            if (fullMapCamera.gameObject.active == false)
            {
                fullMapCamera.gameObject.SetActive(true);
                Time.timeScale = 0f;
                miniMap.gameObject.SetActive(false);
            }
            else
            {
                fullMapCamera.gameObject.SetActive(false);
                Time.timeScale = 1f;
                miniMap.gameObject.SetActive(true);
            }
        }
    }
}
