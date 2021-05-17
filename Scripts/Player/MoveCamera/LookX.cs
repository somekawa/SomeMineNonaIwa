using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookX : MonoBehaviour
{
    public PauseScript pause;   // pause中の処理

    [SerializeField]
    private float x_sensitivity = 3.0f;

    private HideControl hideControl_;

    private bool rotateCheck_ = true;

    void Start()
    {
        hideControl_ = GetComponent<HideControl>();
    }

    void Update()
    {
        if (pause.SetPauseFlag() == true)
        {
            // pause中はカメラが動かないようにする
            return;
        }
        
        if ((hideControl_ != null) && (hideControl_.GetHideFlg()))
        {
            // 箱に隠れている
            return;
        }

        float x_mouse = Input.GetAxis("Mouse X");

        Vector3 newRotation = transform.localEulerAngles;
        newRotation.y += x_mouse * x_sensitivity;

        transform.localEulerAngles = newRotation;
        //Debug.Log("LookX:マウス座標"+ transform.localEulerAngles);
    }

}
