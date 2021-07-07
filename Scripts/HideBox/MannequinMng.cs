using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinMng : MonoBehaviour
{
    private List<GameObject> boxList_;
    private int num_;
    private float time_;

    public float coolTime_;

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

        SetMannequin();
    }

    // Update is called once per frame
    void Update()
    {
        time_ += Time.deltaTime;

        // 一定時間見つからなかった場合再配置する
        if (time_ >= coolTime_) 
        {
            SetMannequin();
        }
    }

    public void SetMannequin()
    {
        // 全ての箱のマネキンを非表示にする
        foreach (GameObject box in boxList_)
        {
            box.GetComponent<HideBox>().SetMannequin(false);
        }

        // マネキンを設置する場所を指定
        int no = Random.Range(0, num_ - 1);
        //Debug.Log("選ばれたのは" + no + "でした");
        boxList_[no].GetComponent<HideBox>().SetMannequin(true);
        time_ = 0.0f;
    }
}
