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

    private GameObject[] item_;
    private bool[] destroyCheckFlag_;
    private bool keyGetFlag_;
   // private string itemTag_;

    // Start is called before the first frame update
    void Start()
    {
        keyGetFlag_ = false;
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

        //   test = GameObject.Find("ItemMng/Battery");
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialMain.GetMissionFlag() == true)
        {
                if (item_[(int)item.BATTERY] != null)
                {
                    item_[(int)item.BATTERY].SetActive(true);
                }
        }

        if(playerCol.GetBatteryFlag()==true)
        {
            if (destroyCheckFlag_[(int)item.BATTERY] == true)
            {
                tutorialMain.SetItemFlag(true);// アイテムを拾った(true)を渡す
            }
        }

        if(destroyCheckFlag_[(int)item.ESCAPE] == true
            && tutorialMain.GetCompleteFlag() == true)
        {
            keyGetFlag_ = true;
           item_[(int)item.DOOR].SetActive(true);
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
        {             // 脱出アイテムとの当たり判定
            if (other.gameObject.tag == "Battery")
            {
                checkItem_ = item.BATTERY;
                CheckItem(checkItem_, other);
                //      destroyCheckFlag_[(int)item.BATTERY] = true;
                return;
            }
            // 電池との当たり判定
            else if (other.gameObject.tag == "BarrierItem")
            {
                checkItem_ = item.BARRIER;
                CheckItem(checkItem_, other);
                //   destroyCheckFlag_[(int)item.BARRIER] = true;
                return;
            }
            // 防御アイテム取得処理
            else if (other.gameObject.tag == "InductionItem")
            {
                checkItem_ = item.INDUCTION;
                CheckItem(checkItem_, other);
                //  destroyCheckFlag_[(int)item.INDUCTION] = true;
                return;
            }
            // 誘導アイテム取得処理
            else if (other.gameObject.tag == "EscapeItem")
            {
                checkItem_ = item.ESCAPE;
                CheckItem(checkItem_, other);
                // destroyCheckFlag_[(int)item.ESCAPE] = true;
                return;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // ミッションがすべて終了したら
        if (keyGetFlag_ == true)
        {
            //ドアと接触したらクリアシーンに移る
            if (other.gameObject.tag == "Door")
            {
                Debug.Log("ドアに接触");
                SceneManager.LoadScene("MainScene");
            }
        }
    }

    private void CheckItem(item items_, Collider other)
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
          //itemTag_ = other.gameObject.tag;

            destroyCheckFlag_[(int)items_] = true;
          //  Destroy(item_[(int)items_]);
            if (destroyCheckFlag_[(int)item.ESCAPE] != true)
            {
                item_[(int)items_ + 1].SetActive(true);
            }
        }
        Debug.Log("1つ前を削除し新しいアイテムを表示します");
    }

}
