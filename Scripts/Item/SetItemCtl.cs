using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SetItemCtl : MonoBehaviour
{
    public GameObject escapeItem;                       // 生成するオブジェクト格納用
    public int itemCnt;                                 // 生成する数

    private int pointRange;                             // 配置予定地の乱数格納用
    private List<int> setPoint = new List<int>();       // 同じ場所にしないために使うリスト
    private int[] rangeStock_;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] setItemPoints = this.gameObject.GetComponentsInChildren<Transform>().Select(t => t.gameObject).ToArray();
        for (int i = 1; i < setItemPoints.Length; i++)
        {
            setPoint.Add(i);                 // 配置先のオブジェクトをリストに格納
        }
        rangeStock_ = new int[setPoint.Count];
        for (int x = 0; x < setPoint.Count; x++)
        {
            rangeStock_[x] = x;
        }

        while (itemCnt > 0)                // 生成する数だけループ
        {
            pointRange = Random.Range(1, setPoint.Count);
            if (rangeStock_[pointRange] == -1)
            {
                pointRange = Random.Range(1, setPoint.Count);
            }
            else
            {
                rangeStock_[pointRange] = -1;
                Instantiate(escapeItem, setItemPoints[pointRange].transform.position, setItemPoints[pointRange].transform.rotation);
                Debug.Log("" + pointRange + ":" + this.gameObject.name);
                itemCnt--;
            }
            //setPoint.RemoveAt(pointRange);   // 配置された場所はリストから除外
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
