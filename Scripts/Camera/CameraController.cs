using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Image fullMap;        // 全体マップを映すカメラの格納用
    public Image miniMap;               // ミニマップ画像の格納用

    // Start is called before the first frame update
    void Start()
    {
        fullMap.gameObject.SetActive(false);      // 基本的に非表示
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))            // キーが押されたとき
        {
            if (fullMap.gameObject.active == false)
            {
                Active(true, 0.0f, false);
            }
            else
            {
                Active(false, 1.0f, true);
            }
        }
    }

    void Active(bool fullMapFlg,float time,bool miniMapFlg)
    {
        // 共通処理
        fullMap.gameObject.SetActive(fullMapFlg);
        Time.timeScale = time;
        miniMap.gameObject.SetActive(miniMapFlg);
    }

    public bool FullMapFlag()
    {
        return fullMap.gameObject.active;
    }
}
