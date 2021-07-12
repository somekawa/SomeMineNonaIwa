using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextChange : MonoBehaviour
{
    private List<Text> textUIList = new List<Text>();
    private List<string> textOrigin = new List<string>();   // 元テキストの保存
    public GameObject TextMng;

    private string[] strArray_ = { "蟯ｩ蟠", "迹ｳ蟶", "驥惹ｸｭ", "譟灘ｷ" };
    //private float changeTimeMax_  = 0.3f;
    private float changeTimeMax_ = 40.0f;
    private float textChangeTime_ = 0.0f;
    private bool textChangeFlg_   = false;

    //private float coolTime_    = 5.0f;
    private float coolTime_ = 300.0f;
    private float coolTimeMax_ = 5.0f;

    private int rangeNum_ = -1;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < TextMng.transform.childCount; i++)
        {
            // TextMngの子を上から順番に取り出して、listに格納していく
            textUIList.Add(TextMng.transform.GetChild(i).GetComponentInChildren<Text>());
            textOrigin.Add(TextMng.transform.GetChild(i).GetComponentInChildren<Text>().text);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 文字化け中の処理
        if (textChangeFlg_)
        {
            // 表示時間の加算
            //textChangeTime_ += Time.deltaTime;
            textChangeTime_++;

            // 一定時間の経過
            if (textChangeTime_ >= changeTimeMax_)
            {
                Change(textOrigin[rangeNum_]);
                textChangeTime_ = 0.0f;
                rangeNum_ = -1;
            }
        }
        else
        {
            if(coolTime_ <= coolTimeMax_)
            {
                // クールタイム以下なら下の処理に進まない
                //coolTime_ += Time.deltaTime;
                coolTime_++;
                return;
            }

            // 範囲0～9
            if (Random.Range(0, 10) == 0)    // 0のとき
            {
                rangeNum_ = Random.Range(0, textUIList.Count); 　　
                Change(strArray_[Random.Range(0, 4)]);    
                coolTime_ = 0.0f;
            }
        }
    }

    void Change(string tmpStr)
    {
        // 範囲外チェック
        if(rangeNum_ <= -1)
        {
            return;
        }

        // 文字反転,出力文字変更
        textUIList[rangeNum_].transform.Rotate(new Vector3(180.0f, 0.0f, 0.0f));
        textUIList[rangeNum_].text = tmpStr;
        textChangeFlg_ = !textChangeFlg_;
    }
}