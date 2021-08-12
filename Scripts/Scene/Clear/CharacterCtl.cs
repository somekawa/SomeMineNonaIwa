using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtl : MonoBehaviour
{
    private float speed_ = 4.0f; // デフォルトの移動速度
    /*カメラ*/
    public float x_sensi; // マウスのX座標の感度格納用
    public float y_sensi; // マウスのY座標の感度格納用
    public GameObject playerCamera; // 動かすカメラ格納用
    public CameraController cameraController;

    private float x_Rotation_;
    private float y_Rotation_;

    // SE関連
    private AudioSource audioSource_;
    [SerializeField] private AudioClip effectSE_;   // 足音のSE

    private Vector3 oldPos_;
    private bool moveFlag_ = false;

    void Start()
    {
        audioSource_ = GetComponent<AudioSource>();

        this.transform.position = new Vector3(15.0f, 1.0f, 4.5f);
        Debug.Log("startプレイヤーZ座標" + this.transform.position.z);

    }

    void Update()
    {
        cameracon();
        CalculateMove();

        // moveFlag_がtrueならSEの再生を行う
        // 足音は他のSEと同時再生される可能性が高いため、SoundListに依存せず音を鳴らす
        if(moveFlag_)
        {
            if(!audioSource_.isPlaying)
            {
                audioSource_.clip = effectSE_;
                audioSource_.Play();
            }
        }
        else
        {
            // 再生中にflagがfalseになったら再生を止める
            if(audioSource_.isPlaying)
            {
                audioSource_.Stop();
            }
        }

    }

    void CalculateMove()
    {
        oldPos_ = transform.position;

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * speed_ * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * speed_ * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * speed_ * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * speed_ * Time.deltaTime;
        }

        // nowとoldの値を見て動いていたらmoveFlag_をtrueにする
        if (oldPos_ != transform.position)
        {
            moveFlag_ = true;
        }
        else
        {
            moveFlag_ = false;
        }
    }

    private void cameracon()
    {
        y_Rotation_ = Input.GetAxis("Mouse Y"); // マウスのY座標の取得
        y_Rotation_ = y_Rotation_ * y_sensi; // カメラのY座標の回転速度の計算
        playerCamera.transform.Rotate(-y_Rotation_, 0, 0); // Y座標の回転は子オブジェクトのカメラに

        x_Rotation_ = Input.GetAxis("Mouse X"); // マウスのX座標の取得
        x_Rotation_ = x_Rotation_ * x_sensi; // カメラのX座標の回転速度の計算
        this.transform.Rotate(0, x_Rotation_, 0); // X座標の回転はスクリプトをアタッチしているオブジェクトに
    }

}