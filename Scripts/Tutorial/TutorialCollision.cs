using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialCollision : MonoBehaviour
{
    public enum item
    {
        BATTERY,
        BARRIER,
        INDUCTION,
        ESCAPE,
        DOOR,
        MAX
    }
    private item checkItem_;

    /*プレイヤー関連*/
    public GameObject player;

    private PlayerCollision playerCol_;

    private HideControl hideCtl_;

    public TutorialScript tutorialMain;    // チュートリアル用当たり判定
    public GameObject stage;        // 現在のステージのオブジェクトを非アクティブに
    public GameObject pMission;     // practicMissionのオブジェクトをアクティブに
    public GameObject pCollision;   // practiCollisionのオブジェクトをアクティブに
    public GameObject nextStage;    // 次のステージのオブジェクトをアクティブに

    private GameObject[] item_;
    private bool[] destroyCheckFlag_;// アイテムが破壊（取得）できたかのフラグ
    private bool nextMissionFlag_;// practicステージに移って良いかのフラグ

    void Start()
    {
        playerCol_ = player.GetComponentInChildren<PlayerCollision>();
        hideCtl_ = player.GetComponent<HideControl>();

        nextMissionFlag_ = false; //  true;//true:次のミッションに移動　false:まだだめ
        checkItem_ = item.MAX;
        destroyCheckFlag_ = new bool[(int)item.MAX];
        // 配列を作るときはまず大きさを決める
        item_ = new GameObject[(int)item.MAX];

        item_[(int)item.BATTERY] = GameObject.FindGameObjectWithTag("Battery").gameObject;
        item_[(int)item.BARRIER] = GameObject.FindGameObjectWithTag("BarrierItem").gameObject;
        item_[(int)item.INDUCTION] = GameObject.FindGameObjectWithTag("InductionItem").gameObject;
        item_[(int)item.ESCAPE] = GameObject.FindGameObjectWithTag("EscapeItem").gameObject;
        item_[(int)item.DOOR] = GameObject.FindGameObjectWithTag("Door").gameObject;

        for (int i = 0; i < (int)item.MAX; i++)
        {
            item_[i].SetActive(false);
            destroyCheckFlag_[i] = false;
            checkItem_ = (item)i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(nextMissionFlag_==true)
        {
            // 基本行動ミッションが終わったらオブジェクトを破壊
            // ステージを実践ミッション用に入れ替える
            stage.SetActive(false);
            nextStage.SetActive(true);// ステージを先に表示する
            pMission.SetActive(true);
            pCollision.SetActive(true);
            Destroy(this.gameObject);
            return;
        }

        // 2巡目のミッションに入ったら
        if (tutorialMain.GetMissionRound()== (int)TutorialScript.round.SECONDE)
        {
            // アイテムが削除されていなければ表示
            if (item_[(int)item.BATTERY] != null)
            {
                item_[(int)item.BATTERY].SetActive(true);
            }
        }

        if (playerCol_.GetBatteryFlag() == true)
        {
            if (destroyCheckFlag_[(int)item.BATTERY] == true)
            {
                tutorialMain.SetItemFlag(true);// アイテムを拾った(true)を渡す
            }
        }

        // 隠れたら鍵を表示する
        if (hideCtl_.GetHideFlg() == true)
        {
            if (item_[(int)item.BATTERY] = null)
            {
                return;
            }
            // ビン取得ではなく隠れる処理をしてから鍵を表示する
            item_[(int)item.ESCAPE].SetActive(true);
        }

        // 鍵の取得を確認したらドアを表示する
        if (destroyCheckFlag_[(int)item.ESCAPE] == true)
        {
            if (tutorialMain.GetCompleteFlag() == true)
            {
                item_[(int)item.DOOR].SetActive(true);
            }
        }
        ItemSttay();
    }

    private void ItemSttay()
    {
        if (destroyCheckFlag_[(int)item.ESCAPE] == true)
        {
            // 最後のアイテムを取ったら処理に入らないようにする
            return;
        }
        else
        {
            if (playerCol_.GetItemNum() == PlayerCollision.item.BATTERY)
            {
                // 電池との当たり判定
                CheckItem(item.BATTERY);
                return;
            }
            else if (playerCol_.GetItemNum() == PlayerCollision.item.BARRIER)
            {                // 防御アイテム取得処理
                CheckItem(item.BARRIER);
                return;
            }
            else if (playerCol_.GetItemNum() == PlayerCollision.item.INDUCTION)
            {                // 防御アイテム取得処理
                CheckItem(item.INDUCTION);
                return;

            }
            else if (playerCol_.GetItemNum() == PlayerCollision.item.ESCAPE)
            {                // 防御アイテム取得処理
                CheckItem(item.ESCAPE);
                return;

            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        //ドアと接触したらクリアシーンに移る
        if (other.gameObject.tag == "Door")
        {
            tutorialMain.SetDoorColFlag(true);
            nextMissionFlag_ = true;
            Debug.Log("ドアに接触");
        }
    }

    private void CheckItem(item items_)
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            destroyCheckFlag_[(int)items_] = true;
            if ((int)items_ < (int)item.INDUCTION)
            {
                // 鍵は隠れるミッション達成後に表示
                // するからここで出すのはビンまで
                item_[(int)items_ + 1].SetActive(true);
            }
        }
        Debug.Log("1つ前を削除し新しいアイテムを表示します");
    }

    public bool GetNextMissionFlag()
    {
        return nextMissionFlag_;
    }

}
