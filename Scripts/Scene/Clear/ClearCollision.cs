using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearCollision : MonoBehaviour
{
    public ClearDoorAnimation doorAnimation;
    public CharacterCtl cCtl;

    void Start()
    {
    }

    void Update()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Untagged")
        {
            return;
        }


        if (other.gameObject.tag == "TitleDoor")
        {
            Debug.Log("タイトルに戻るドアに接触");
            doorAnimation.SetOpenFlag(true, ClearDoorAnimation.doorType.TITLE);
            cCtl.enabled = false;
            if (doorAnimation.GetFullOpneFlag() == true)
            {
                Debug.Log("ドアが開ききりました。タイトルシーンに移動します");
                SceneManager.LoadScene("TitleSample");
            }
        }
        else if (other.gameObject.tag == "GameDoor")
        {
            Debug.Log("ゲームに戻るドアに接触");
            doorAnimation.SetOpenFlag(true, ClearDoorAnimation.doorType.GAME);
            cCtl.enabled = false;
            if (doorAnimation.GetFullOpneFlag() == true)
            {
                Debug.Log("ドアが開ききりました。ゲームシーンに移動します");
                SceneManager.LoadScene("MainScene");
            }
        }
        else
        {
            // 何も処理を行わない
        }
    }
}
