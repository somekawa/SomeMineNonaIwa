using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLightCtl : MonoBehaviour
{
    public GameObject comingObject;     // 自身との距離を計算するターゲットオブジェクト格納用

    private GameObject objectLight;     // 発光するための子オブジェクト格納用

    // Start is called before the first frame update
    void Start()
    {
        objectLight = gameObject.GetComponentInChildren<Light>().gameObject;     // 子オブジェクトのlightを取得
    }

    // Update is called once per frame
    void Update()
    {
        //　距離を計算
        var distance = Vector3.Distance(gameObject.transform.position, comingObject.transform.position);
        if (distance > 20)
        {
            objectLight.SetActive(false);       // 範囲外なら非表示にする
        }
        else
        {
            objectLight.SetActive(true);       // 範囲内なら表示にする
        }
    }
}
