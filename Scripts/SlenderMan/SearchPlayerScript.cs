using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchPlayerScript : MonoBehaviour
{
    private playerController playerController_;
    private SlenderManCtl slenderManCtl_;

    // Start is called before the first frame update
    void Start()
    {
        slenderManCtl_ = transform.root.gameObject.GetComponent<SlenderManCtl>();
        playerController_ = FindObjectOfType<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (slenderManCtl_.searchFlag == true)
            {
                if (playerController_.GetSlowWalkFlg() == false && playerController_.GetWalkFlg() == true)      // ゆっくり歩いていないなら
                {
                    SlenderSpawner.GetInstance().ClosestObject(other.gameObject, 0, false, false);
                    slenderManCtl_.searchFlag = false;
                }
            }
        }
    }
}
