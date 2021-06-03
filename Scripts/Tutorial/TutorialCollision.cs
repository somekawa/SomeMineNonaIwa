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

    // チュートリアル用当たり判定
    public TutorialScript tutorialMain;

    public PlayerCollision playerCol;

    public HideControl hideCtl;

    private GameObject[] item_;
    private bool[] destroyCheckFlag_;
    private bool keyGetFlag_;
    private bool nextMissionFlag_;

    // Start is called before the first frame update
    void Start()
    {
        keyGetFlag_ = false;
        nextMissionFlag_ = false; //true;//  true:次のミッションに移動　false:まだだめ
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
        // 2巡目のミッションに入ったら
        if (tutorialMain.GetMissionFlag() == true)
        {
            // アイテムが削除されていなければ表示
            if (item_[(int)item.BATTERY] != null)
            {
                item_[(int)item.BATTERY].SetActive(true);
            }
        }

        if (playerCol.GetBatteryFlag() == true)
        {
            if (destroyCheckFlag_[(int)item.BATTERY] == true)
            {
                tutorialMain.SetItemFlag(true);// アイテムを拾った(true)を渡す
            }
        }

        if (hideCtl.GetHideFlg() == true)
        {
            if (item_[(int)item.BATTERY] = null)
            {
                return;
            }
            // ビン取得ではなく隠れる処理をしてから鍵を表示する
                item_[(int)item.ESCAPE].SetActive(true);
            
        }
        if (destroyCheckFlag_[(int)item.ESCAPE] == true)
        {
            if (tutorialMain.GetCompleteFlag() == true)
            {
                item_[(int)item.DOOR].SetActive(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Untagged")
        {
            tutorialMain.SetItemFlag(false);
            return;
        }

        if (destroyCheckFlag_[(int)item.ESCAPE] == true)
        {
            // 最後のアイテムを取ったら処理に入らないようにする
            return;
        }
        else
        {

            if (other.gameObject.tag == "Battery")
            {
                // 電池との当たり判定
                checkItem_ = item.BATTERY;
                CheckItem(checkItem_, other);
                return;
            }
            else if (other.gameObject.tag == "BarrierItem")
            {
                // 防御アイテム取得処理
                checkItem_ = item.BARRIER;
                CheckItem(checkItem_, other);
                return;
            }
            else if (other.gameObject.tag == "InductionItem")
            {
                // 誘導アイテム取得処理
                checkItem_ = item.INDUCTION;
                CheckItem(checkItem_, other);
                return;
            }
            else if (other.gameObject.tag == "EscapeItem")
            {
                checkItem_ = item.ESCAPE;
                CheckItem(checkItem_, other);
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
            SceneManager.LoadScene("MainScene");
        }

    }

    private void CheckItem(item items_, Collider other)
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
