using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearDoorAnimation : MonoBehaviour
{
    public enum doorType
    {
        TITLE,
        GAME,
        NON,
        MAX
    }
    private doorType doorType_;

    public GameObject doorBackWall;
    private GameObject[] doorObj;
    private GameObject door;            // 回転するオブジェクト格納用

    private bool[] openFlag_;           // 開くためのフラグ
    private bool fullOpenFlag_ = false; // 扉が開ききったらtrue

    private float maxAngle_ = 90.0f;    // 引き扉：回転した後の角度

    public Image blackPanel;
    private float panelAlpha_ = 0.0f;
    void Start()
    {
        doorType_ = doorType.TITLE;
        doorObj = new GameObject[(int)doorType.MAX];
        openFlag_ = new bool[(int)doorType.MAX];
        for (int i = (int)doorType.TITLE; i < (int)doorType.MAX-1; i++)
        {            // 0番TitleDoor　1番GameDoor
            doorObj[i] = doorBackWall.transform.GetChild(i).gameObject;
            openFlag_[i] = false;
            Debug.Log(i+"ClearDoorAnimationScriptを呼び出しました" + doorObj[i]);
            Debug.Log("子の数" + doorBackWall.transform.childCount);
        }
    }


    void Update()
    {
        Debug.Log(doorType_ + ""+ openFlag_[(int)doorType_]);

        if (openFlag_[(int)doorType_] == true)
        {
            // SEの音を鳴らす(ドアを開く音)
            SoundScript.GetInstance().PlaySound(12);

            Debug.Log(doorType_+"のドアに接触しました");
            float step = 120.0f * Time.deltaTime;
            panelAlpha_ += 0.01f;

            // 指定した方向にゆっくり回転する場合
            door = doorObj[(int)doorType_].transform.GetChild(0).gameObject;
            door.transform.rotation = Quaternion.RotateTowards(door.transform.rotation, Quaternion.Euler(0.0f, -maxAngle_, 0.0f), step);

            if (1.0f < panelAlpha_)
            {
                openFlag_[(int)doorType_] = false;
                fullOpenFlag_ = true;
            }
            blackPanel.color = new Color(255.0f, 255.0f, 255.0f, panelAlpha_);
        }
    }

    public void SetOpenFlag(bool flag,doorType type)
    {
        doorType_ = type;
        openFlag_[(int)type] = flag;
    }

    public bool GetFullOpneFlag()
    {
        return fullOpenFlag_;
    }
}
