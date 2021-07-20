using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearDoorAnimation : MonoBehaviour
{
    public GameObject door;             // 回転するオブジェクト格納用
    public bool openFlag;               // 開くためのフラグ

    private float maxAngle_;            // 回転した後の角度
    private float tmpAngle_;            // 回転角度格納用変数
    private bool transFlag;             // 遷移前の管理フラグ

    void Start()
    {
        maxAngle_ = 90.0f;      // 引き扉
        openFlag = false;
        tmpAngle_ = 0.0f;
    }


    void Update()
    {
        if (openFlag == true)
        {
            float step = 120.0f * Time.deltaTime;
            tmpAngle_ += step;          // いくら回転したのか格納する
           // 指定した方向にゆっくり回転する場合
            door.transform.rotation = Quaternion.RotateTowards(door.transform.rotation, Quaternion.Euler(0.0f, -maxAngle_, 0.0f), step);

            if ( tmpAngle_ <= -90.0f)      // 回転した角度を参照
            {
                openFlag = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // targetがGameScene
        // DoorがTitleSample
        // Enemyはデバッグ用

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
            //else if (this.gameObject.tag == "Enemy")
            //{
            //    Debug.Log("プレイヤーが接触しました");
            //}
            openFlag = true;
        }
    }


}
