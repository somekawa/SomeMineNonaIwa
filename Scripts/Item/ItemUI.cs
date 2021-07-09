using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUI : MonoBehaviour
{
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
