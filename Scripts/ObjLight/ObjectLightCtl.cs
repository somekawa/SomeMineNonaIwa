using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLightCtl : MonoBehaviour
{
    public GameObject comingObject;     // 自身との距離を計算するターゲットオブジェクト格納用

    private GameObject cameraControll_;
    private CameraController cameraController_;
    private GameObject objectLight_;     // 発光するための子オブジェクト格納用

    const float displayRange = 20;

    // Start is called before the first frame update
    void Start()
    {
        cameraControll_ = GameObject.Find("CameraControll");
        cameraController_ = cameraControll_.GetComponent<CameraController>();
        objectLight_ = gameObject.GetComponentInChildren<Light>().gameObject;     // 子オブジェクトのlightを取得
    }

    // Update is called once per frame
    void Update()
    {
        // 距離を計算
        if (Vector3.Distance(gameObject.transform.position, comingObject.transform.position) > displayRange ||
            cameraController_.FullMapFlag() == true)
        {
            objectLight_.SetActive(false);
        }
        else
        {
            objectLight_.SetActive(true);
        }
    }
}
