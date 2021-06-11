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
        MAX
    }
    private GameObject[] item_;// シーン上にあるアイテムを探す

    public PracticMission mission;
   
    void Start()
    {
        // 配列を作るときはまず大きさを決める
        item_ = new GameObject[(int)item.MAX];

        item_[(int)item.INDUCTION] = GameObject.FindGameObjectWithTag("InductionItem").gameObject;
        item_[(int)item.ESCAPE] = GameObject.FindGameObjectWithTag("EscapeItem").gameObject;

        for (int i = 0; i < (int)item.MAX; i++)
        {
            item_[i].SetActive(false);
        }

    }

    void Update()
    {
        // 鍵を探すミッションになっていたら
        if (mission.GetMissionNum() == (int)PracticMission.practic.SEARCH_KEY)
        {
            if (item_[(int)item.ESCAPE] != null)
            {
                item_[(int)item.ESCAPE].SetActive(true);// 鍵を表示
            }
        }


        // null=鍵を取得　ボトルを表示
        if (item_[(int)item.ESCAPE] == null)
        {
            // 次のミッションに進む
            mission.SetMissionNum((int)PracticMission.practic.INDUCTION);
            if (item_[(int)item.INDUCTION] != null)
            {
                item_[(int)item.INDUCTION].SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // null=鍵は取った
        if (item_[(int)item.ESCAPE] == null)
        {
            //ドアと接触したらクリアシーンに移る
            if (other.gameObject.tag == "Door")
            {
                Debug.Log("ドアに接触");
                SceneManager.LoadScene("MainScene");
            }
        }
    }

}
