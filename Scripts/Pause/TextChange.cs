using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextChange : MonoBehaviour
{
    public GameObject TextMng;
    private List<Text> textUIList_   = new List<Text>();     // 書き換え用テキスト
    private List<string> textOrigin_ = new List<string>();   // 元テキストの保存
    private string[] strArray_ = { "蟯ｩ蟠", "迹ｳ蟶", "驥惹ｸｭ", "譟灘ｷ" };   // 代入文字

    private float changeTimeMax_  = 45.0f;  // 文字の変更時間の最大値
    private float textChangeTime_ = 0.0f;   // 現在の変更時間
    private bool textChangeFlg_   = false;  // 変更中はフラグをtrueにする

    private float coolTime_    = 300.0f;    // 現在の文字を変更しない時間
    private float coolTimeMax_ = 300.0f;    // 文字を変更しない時間の最大値

    private int rangeNum_ = -1;             // 書き換えるテキストの値を保存する

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < TextMng.transform.childCount; i++)
        {
            // TextMngの子を上から順番に取り出して、listに格納していく
            textUIList_.Add(TextMng.transform.GetChild(i).GetComponentInChildren<Text>());
            textOrigin_.Add(TextMng.transform.GetChild(i).GetComponentInChildren<Text>().text);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 正気度が50を下回るまでは文字化けを発生させないでおく
        if (SanitMng.sanit_ > 50.0f)
        {
            return;
        }

        // 文字化け中の処理
        if (textChangeFlg_)
        {
            // 表示時間の加算
            textChangeTime_++;

            // 一定時間の経過
            if (textChangeTime_ >= changeTimeMax_)
            {
                Change(textOrigin_[rangeNum_]);
                textChangeTime_ = 0.0f;
                rangeNum_ = -1;
            }
        }
        else
        {
            if(coolTime_ <= coolTimeMax_)
            {
                // クールタイム以下なら下の処理に進まない
                coolTime_++;
                return;
            }

            // 範囲0～9
            if (Random.Range(0, 10) == 0)    // 0のとき
            {
                rangeNum_ = Random.Range(0, textUIList_.Count); 　　
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
        textUIList_[rangeNum_].transform.Rotate(new Vector3(180.0f, 0.0f, 0.0f));
        textUIList_[rangeNum_].text = tmpStr;
        textChangeFlg_ = !textChangeFlg_;
    }
}