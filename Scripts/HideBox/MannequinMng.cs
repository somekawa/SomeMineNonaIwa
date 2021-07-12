using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinMng : MonoBehaviour
{
    private List<GameObject> boxList_;
    private int num_;
    private float time_;

    private int mannequinNo_ = -1;
    public float coolTime_;

    private string inBoxName_;
    private int redNo_ = -1;

    HideControl hideControl_;

    // デバッグ用
    // オンにしてる場合は一部の機能は行われないようになります。
    public bool debug_;

    // Start is called before the first frame update
    void Start()
    {
        // 箱をすべて取得する
        GameObject[] boxList = GameObject.FindGameObjectsWithTag("HideObj");
        int num = boxList.Length;

        // HidePositions内に入っているものだけをリストに追加する
        boxList_ = new List<GameObject>();
        for (int i = 0; i < num; i++) 
        {
            if ((boxList[i].transform.parent != null) && (boxList[i].transform.parent.name == "HidePositions")) 
            {
                boxList_.Add(boxList[i]);
            }
        }
        num_ = boxList_.Count;

        GameObject obj = GameObject.Find("Player").gameObject;
        hideControl_ = obj.GetComponent<HideControl>();

        SetMannequin();
    }

    // Update is called once per frame
    void Update()
    {        
        if (hideControl_.GetHideFlg())
        {
            // 隠れてる間はマネキンは移動させない
            for (int i = 0; i < num_; i++)
            {
                
                if (boxList_[i].GetComponent<HideBox>().GetInTime() != 0.0f)
                {
                    inBoxName_ = boxList_[i].name;

                    // 同じ箱に一定時間入った
                    if (boxList_[i].GetComponent<HideBox>().GetInTime() >= coolTime_)
                    {
                        redNo_ = i;
                    }
                    return;
                }
                else
                {
                    redNo_ = -1;
                }
            }               
            return;
        }

        if (redNo_ != -1) 
        {
            // 同じ箱に入り続けた(デバッグがオフの場合のみ実行)
            SetingMannquin(redNo_);
            redNo_ = -1;
            return;
        }

        time_ += Time.deltaTime;
        // 一定時間経ったら再配置する
        if (time_ >= coolTime_) 
        {
            SetMannequin();
        }
    }

    public void SetMannequin()
    {
        if (debug_)
        {
            // デバックがオンの時はランダム配置は行わない
            return;
        }

        // 全ての箱のマネキンを非表示にする
        foreach (GameObject box in boxList_)
        {
            box.GetComponent<HideBox>().SetMannequin(false);
        }

        // マネキンを設置する場所を指定
        int no = Random.Range(0, num_ - 1);
        while (no == mannequinNo_) 
        {
            // 前回と同じ場合は再指定
            no = Random.Range(0, num_ - 1);
        }

        //Debug.Log("選ばれたのは" + no + "でした");
        SetingMannquin(no);
    }

    private void SetingMannquin(int no)
    {
        boxList_[no].GetComponent<HideBox>().SetMannequin(true);
        mannequinNo_ = no;
        time_ = 0.0f;
    }

    public string GetInBoxName()
    {
        return inBoxName_;
    }
}
