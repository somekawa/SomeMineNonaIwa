using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialCollision : MonoBehaviour
{
    public enum item
    {
        BATTERY,
        BARRIER,
        ESCAPE,
        DOOR,
        MAX
    }

    /*プレイヤー関連*/
    public GameObject player;
    private PlayerCollision playerCol_;
    private HideControl hideCtl_;

    // チュートリアル関連
    public TutorialScript tutorialMain;    // チュートリアル用当たり判定
    private GameObject[] item_;
    private bool[] destroyCheckFlag_;// アイテムが破壊（取得）できたかのフラグ
    public TutorialDoorAnimation tDoor;
    private bool nextMissionFlag_ = false; // true:次のミッションに移動　false:まだだめ

    // 実践関連
    public GameObject key;          // シーン上にある鍵を探す
    public GameObject pMission;     // practicMissionのオブジェクトをアクティブに
    private PracticMission practicMission;

    public GameObject parentStage;  // ステージがある親を代入
    private GameObject[] stages;    // parentStageから子を探して代入

    public Image monochromeUI;

    void Start()
    {
        stages = new GameObject[parentStage.transform.childCount - 1];
        stages[0] = parentStage.transform.GetChild(0).gameObject;
        stages[1] = parentStage.transform.GetChild(1).gameObject;

        key.SetActive(false);

        playerCol_ = player.GetComponentInChildren<PlayerCollision>();
        hideCtl_ = player.GetComponent<HideControl>();
        practicMission = pMission.GetComponent<PracticMission>();

        destroyCheckFlag_ = new bool[(int)item.MAX];
        // 配列を作るときはまず大きさを決める
        item_ = new GameObject[(int)item.MAX];

        item_[(int)item.BATTERY] = GameObject.FindGameObjectWithTag("Battery").gameObject;
        item_[(int)item.BARRIER] = GameObject.FindGameObjectWithTag("BarrierItem").gameObject;
        item_[(int)item.ESCAPE] = GameObject.FindGameObjectWithTag("EscapeItem").gameObject;
        item_[(int)item.DOOR] = GameObject.FindGameObjectWithTag("Door").gameObject;

        for (int i = 0; i < (int)item.MAX; i++)
        {
            item_[i].SetActive(false);
            destroyCheckFlag_[i] = false;
        }
    }

    void Update()
    {
        if (nextMissionFlag_ == true)
        {
            StartPracticMission();
        }
        else
        {
            // チュートリアルミッション中の処理
            // 2巡目のミッションに入ったら
            if (tutorialMain.GetMissionRound() == (int)TutorialScript.round.SECONDE)
            {
                if (item_[(int)item.BARRIER] != null)
                {
                    // アイテムが削除されていなければ表示
                    if (item_[(int)item.BATTERY] != null)
                    {
                        item_[(int)item.BATTERY].SetActive(true);
                    }
                    else
                    {
                        item_[(int)item.BARRIER].SetActive(true);
                    }
                }
            }

            // 3巡目のミッションに入ったら
            if (tutorialMain.GetMissionRound() == (int)TutorialScript.round.THIRD)
            {
                // 隠れたら鍵を表示するかつ鍵を持ってない状態なら
                if (hideCtl_.GetHideFlg() == true && 0 == playerCol_.GetkeyItemCnt())
                {
                    // ビン取得ではなく隠れる処理をしてから鍵を表示する
                    item_[(int)item.ESCAPE].SetActive(true);
                }
            }

            // 鍵を所持していたらドアを表示
            if (0 < playerCol_.GetkeyItemCnt())
            {
                if (tutorialMain.GetCompleteFlag() == true
                    && monochromeUI.fillAmount == 0.0f)
                {
                    item_[(int)item.DOOR].SetActive(true);
                }
            }

            //ドアと接触したら実践ミッションステージに移る
            if (tDoor.GetOpenFlag() == true)
            {
                playerCol_.SetKeyItemCnt(0);   // 鍵の所持数をリセット
                tutorialMain.SetDoorColFlag(true);

                nextMissionFlag_ = true;
                Debug.Log("ドアに接触nextMissionFlag_" + nextMissionFlag_);
            }
        }
    }

    private void StartPracticMission()
    {
        if (stages[0] == null)
        {
            // PracticMissionの準備が終わったらもう入らないようにする
            return;
        }
        // 基本行動ミッションが終わったらオブジェクトを破壊
        // ステージを実践ミッション用に入れ替える
        stages[0].SetActive(false);
        stages[1].SetActive(true);// ステージを先に表示する
        Debug.Log("stages[0]" + stages[0].activeSelf + "        stages[1]" + stages[1].activeSelf);
        pMission.SetActive(true);
        // 鍵を探すミッションになっていたら
        if (practicMission.GetMissionNum() == (int)PracticMission.practic.SEARCH_KEY)
        {
            if (key != null)
            {
                key.SetActive(true);// 鍵を表示
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (nextMissionFlag_ == true)
        {
            // null=鍵は取った
            if (key == null)
            {
                //ドアと接触したらメインシーンに移る
                if (other.gameObject.tag == "Door")
                {
                    Debug.Log("実践ミッション中ドアに接触");
                    SceneManager.LoadScene("MainScene");
                }
            }
        }
    }

}
