using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUI : MonoBehaviour
{
    // Component取得用変数
    public Barrier barrier;
    public ItemTrhow itemTrhow;

    public GameObject barrierUI;
    public GameObject bottoleUI;

    // Start is called before the first frame update
    void Start()
    {
        barrierUI.SetActive(false);
        bottoleUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // テスト中
        //barrierUI.SetActive(barrier.GetBarrierItemFlg() == true ? true : false);
        //bottoleUI.SetActive(itemTrhow.GetTrhowItemFlg() == true ? true : false);

        if (barrier.GetBarrierItemFlg() == true)
        {
            barrierUI.SetActive(true);
        }
        else
        {
            barrierUI.SetActive(false);
        }
        if (itemTrhow.GetTrhowItemFlg() == true)
        {
            bottoleUI.SetActive(true);
        }
        else
        {
            bottoleUI.SetActive(false);
        }
    }
}
