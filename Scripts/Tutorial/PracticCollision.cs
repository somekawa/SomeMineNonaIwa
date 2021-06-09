using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PracticCollision : MonoBehaviour
{
    // ミッションにかかわるオブジェクト
    public enum item
    {
        INDUCTION,
        ESCAPE,
     //   DOOR,
        MAX
    }
    private item checkItem_;
    private GameObject[] item_;// シーン上にあるアイテムを探す

    private bool startFlag_;

    public PracticMission mission;

    // Start is called before the first frame update
    void Start()
    {
        startFlag_ = false;
        // 配列を作るときはまず大きさを決める
         item_ = new GameObject[(int)item.MAX];

        item_[(int)item.INDUCTION] = GameObject.FindGameObjectWithTag("InductionItem").gameObject;
        item_[(int)item.ESCAPE] = GameObject.FindGameObjectWithTag("EscapeItem").gameObject;
        //item_[(int)item.DOOR] = GameObject.FindGameObjectWithTag("Door").gameObject;

        for (int i = 0; i < (int)item.MAX; i++)
        {
            item_[i].SetActive(false);
            checkItem_ = (item)i;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (startFlag_==false)
        {
            startFlag_ = true;
        }

        if (mission.GetMissionNum() == 2)
        {
            if (item_[(int)item.ESCAPE] != null)
            {
                Debug.Log("鍵を表示");
                // 鍵を表示
                item_[(int)item.ESCAPE].SetActive(true);
            }
        }


        if (item_[(int)item.ESCAPE] == null)
        {
            mission.SetMissionFlag(true);
            // null=鍵を取得　ボトルを表示
            if (item_[(int)item.INDUCTION] != null)
            {
                item_[(int)item.INDUCTION].SetActive(true);
            }
        }



    }
    private void OnTriggerEnter(Collider other)
    {
        //ドアと接触したらクリアシーンに移る
        if (other.gameObject.tag == "Door")
        {
            Debug.Log("ドアに接触");
            SceneManager.LoadScene("MainScene");
        }
    }


    public void SetPracticFlag(bool flag)
    {
        // tutorialmissionに置くかもしれない
        startFlag_ = flag;
    }
}
