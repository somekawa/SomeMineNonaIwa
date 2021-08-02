using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUI : MonoBehaviour
{
    public Barrier barrier;

    public GameObject barrierUI;

    void Start()
    {
        barrierUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (barrier.GetBarrierItemFlg() == true)
        {
            barrierUI.SetActive(true);
        }
        else
        {
            barrierUI.SetActive(false);
        }
    }
}
