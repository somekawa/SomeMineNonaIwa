using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearDoorAnimation : MonoBehaviour
{
    public GameObject door;             // 回転するオブジェクト格納用
    public bool openFlag;               // 開くためのフラグ
    public bool closeFlag;              // 閉じるためのフラグ

    private float minAngle_;            // 回転する前の角度
    private float maxAngle_;            // 回転した後の角度
    private float tmpAngle_;            // 回転角度格納用変数
    private bool transFlag;             // 遷移前の管理フラグ

    void Start()
    {
        Debug.Log("Doorのタグ"+this.gameObject.tag);
        minAngle_ = 0f;
        maxAngle_ = -5f;
        openFlag = false;// true;
        closeFlag = false;
        transFlag = false;// true;
        tmpAngle_ = 0;
    }


    void Update()
    {
        if (openFlag == true)
        {
            float step = -120f * Time.deltaTime;
            tmpAngle_ += step;          // いくら回転したのか格納する
            //指定した方向にゆっくり回転する場合
            door.transform.rotation = Quaternion.RotateTowards(door.transform.rotation, Quaternion.Euler(0, maxAngle_, 0), step);
            //door.transform.Rotate(0f, step, 0f);

            if (tmpAngle_ <= -90f)      // 回転した角度を参照
            {
                openFlag = false;
            }
        }
        else if (closeFlag == true)
        {
            float step = 120f * Time.deltaTime;
            tmpAngle_ += step;
            //指定した方向にゆっくり回転する場合
            door.transform.rotation = Quaternion.RotateTowards(door.transform.rotation, Quaternion.Euler(0, minAngle_, 0), step);
            //door.transform.Rotate(0f, step, 0f);

            if (tmpAngle_ >= minAngle_)
            {
                closeFlag = false;
            }
        }
        else
        {
            return;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("接触したオブジェクト"+other.gameObject.tag);
        //if (transFlag == true && openFlag == false)
        //{
        //if (Input.GetKeyDown(KeyCode.E))
        //{
            if (other.gameObject.tag == "Player")
            {
                if (this.gameObject.tag == "target")
                {
                    Debug.Log("ゲームに戻るドアに接触");
                   // SceneManager.LoadScene("MainScene");

                }
                else if (this.gameObject.tag == "Door")
                {
                    Debug.Log("タイトルに戻るドアに接触");
                   // SceneManager.LoadScene("TitleSample");
                }
                else if (this.gameObject.tag == "Enemy")
                {
                    Debug.Log("プレイヤーが接触しました");
                }
            transFlag = true;
            openFlag = true;

        }
        //  }
        //}
    }


}
