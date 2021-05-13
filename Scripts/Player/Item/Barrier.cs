﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ①敵とプレイヤーが接触時、アイテムを持っていたら減らす
// ②敵を懐中電灯で照らした時、アイテムを持っていたら減らす

// 次はアイテムを取得する処理を書く

public class Barrier : MonoBehaviour
{
    public string targeTag;
    private bool BarrierItemHaveFlg_ = false;  // アイテム所持時にtrue

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // trigger系→Collider collision系→Collision
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == targeTag)
        {
            Debug.Log("敵と衝突しました");
            if (BarrierItemHaveFlg_)
            {
                Debug.Log("防御アイテム使用");
                BarrierItemHaveFlg_ = false;
            }
            else
            {
                Debug.Log("即死処理へ");
            }
        }
    }

    public void SetBarrierItemFlg(bool flag)
    {
        // プレイヤーコントロールクラスからセットされるようにする
        BarrierItemHaveFlg_ = flag;
    }

    public bool GetBarrierItemFlg()
    {
        return BarrierItemHaveFlg_;
    }
}