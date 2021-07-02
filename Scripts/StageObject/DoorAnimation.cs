using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorAnimation : MonoBehaviour
{
    public GameObject door;             // 回転するオブジェクト格納用
    public bool openFlag;               // 開くためのフラグ
    public bool closeFlag;              // 閉じるためのフラグ

    private float minAngle_;            // 回転する前の角度
    private float maxAngle_;            // 回転した後の角度
    private float tmpAngle_;            // 回転角度格納用変数
    private bool transFlag;             // 遷移前の管理フラグ

    // Start is called before the first frame update
    void Start()
    {
        minAngle_ = 0f;
        maxAngle_ = -5f;
        openFlag = false;
        closeFlag = false;
        transFlag = false;
        tmpAngle_ = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    openFlag = true;
        //}
        // 鎖と南京錠の数を調べて、0以下の時に扉がアニメーションするようにフラグを立てる
        if (door.transform.childCount <= 0&& transFlag == false)
        {
            transFlag = true;
            openFlag = true;
        }

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
        else if(closeFlag == true)
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

        if (transFlag == true && openFlag == false)
        {
            SceneManager.LoadScene("ClearScene");
        }
    }
}
