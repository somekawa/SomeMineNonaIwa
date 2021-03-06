using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectLightCtl : MonoBehaviour
{
    public float displayRange;

    private GameObject comingObject;

    private CameraController cameraController_;
    private GameObject objectLight_;     // 発光するための子オブジェクト格納用
    private bool lightFlag_;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "TutorialScene")
        {
            comingObject = GameObject.Find("Player");
        }
        else
        {
            comingObject = GameObject.Find("tPlayer");
        }
        cameraController_ = CameraController.FindObjectOfType<CameraController>();
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
            if (this.gameObject.tag == "Battery"       ||
                this.gameObject.tag == "BarrierItem"   ||
                this.gameObject.tag == "InductionItem" ||
                this.gameObject.tag == "EscapeItem")
            {
                if (lightFlag_ == true)
                {
                    objectLight_.SetActive(true);
                }
            }
        }
        else
        {
            objectLight_.SetActive(true);
            lightFlag_ = true;
        }
    }
}
