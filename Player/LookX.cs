using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookX : MonoBehaviour
{
    [SerializeField]
    private float x_sensitivity = 3.0f;

    private HideControl hideScrip_;

    void Start()
    {
        hideScrip_ = GetComponent<HideControl>();
    }

    void Update()
    {
        if (hideScrip_.GetHideFlg())
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
