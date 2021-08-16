using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRange : MonoBehaviour
{
    public Barrier barrier;                     // バリア状態かを取得する際に使用する

    private bool rangeFlag_ = false;            // 範囲内か
    private float rangeTime_ = 0.0f;         // 範囲内に入ってからの時間

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnDisable()
    {
        // 非アクティブ時、敵が懐中電灯の範囲内だったら正気度低下時の処理を止める
        if (rangeFlag_)
        {
            rangeFlag_ = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Enemy")
        {
            return;         // タグがEnemy以外なら以降の処理を行わない
        }

        if (!rangeFlag_)
        {
            rangeFlag_ = true;
            rangeTime_ = 0.0f;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Enemy")
        {
            return;         // タグがEnemy以外なら以降の処理を行わない
        }

        rangeTime_ += Time.deltaTime;
        Debug.Log("範囲内時間：" + rangeTime_);

        // バリアアイテムを取得しているか確認を行う
        if (barrier.GetBarrierItemFlg())
        {
            barrier.SetBarrierItemFlg(false);
            Debug.Log("防御アイテムを使用しました。");
        }
        Debug.Log("ライトの範囲内に敵を確認しました。");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Enemy")
        {
            return;         // タグがEnemy以外なら以降の処理を行わない
        }
        rangeFlag_ = false;
        Debug.Log("敵がライトの範囲外にいきました。");
    }
}
