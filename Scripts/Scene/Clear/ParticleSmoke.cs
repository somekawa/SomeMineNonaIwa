using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// ヒエラルキー上で0,1,2と名前のついたオブジェクトの事を、ここではsmokePointと記述します

public class ParticleSmoke : MonoBehaviour
{
    private bool moveFlg_ = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // moveFlg_がtrueになっていたら、子のsmokeを右に移動させていく
        if(moveFlg_)
        {
            //現在の位置を取得
            Vector3 pos = this.gameObject.transform.localPosition;
            //現在の位置からx方向に1移動する
            this.gameObject.transform.localPosition = new Vector3(pos.x + 1.0f, pos.y, pos.z);
        }

        if (Input.GetMouseButtonDown(0))   // マウスの左クリックをしたとき
        {
            // Activeをtrueにする
            transform.GetChild(0).gameObject.SetActive(true);

            // 親のテキスト情報を見て、長さを出す
            int textLength = this.transform.parent.GetComponent<Text>().text.Length;

            Vector3 pos = this.gameObject.transform.localPosition;

            // smokePointの座標調整(ローカルポジションで設定していく)
            switch (int.Parse(this.name))  // そもそもどのテキストのsmokepointかの番号を取得する
            {
                case 0: // SanitText
                    // 基準値が-60
                    if (textLength <= 12)
                    {
                        this.gameObject.transform.localPosition = pos;
                    }
                    else
                    {
                        this.gameObject.transform.localPosition = new Vector3(pos.x - 10.0f, 0.0f, 0.0f);
                    }
                    moveFlg_ = true;
                    break;
                case 1: // PlayTimeText
                    // 基準値が-65
                    if (textLength <= 15)
                    {
                        this.gameObject.transform.localPosition = pos;
                    }
                    else
                    {
                        this.gameObject.transform.localPosition = new Vector3(pos.x - 10.0f, 0.0f, 0.0f);
                    }
                    moveFlg_ = true;
                    break;
                case 2: // EnemyEncountText
                    // 基準値が-165
                    if (textLength <= 16)
                    {
                        this.gameObject.transform.localPosition = pos;
                    }
                    else
                    {
                        this.gameObject.transform.localPosition = new Vector3(pos.x - 10.0f, 0.0f, 0.0f);
                    }
                    moveFlg_ = true;
                    break;
                case 3: // HideEncountText
                    // 基準値が-80
                    if (textLength <= 13)
                    {
                        this.gameObject.transform.localPosition = pos;
                    }
                    else
                    {
                        this.gameObject.transform.localPosition = new Vector3(pos.x - 10.0f, 0.0f, 0.0f);
                    }
                    moveFlg_ = true;
                    break;
                default:
                    Debug.Log("エラー番号が検出されました");
                    break;
            }
        }
    }
}
