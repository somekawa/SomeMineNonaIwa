using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SetItemCtl : MonoBehaviour
{
    public GameObject escapeItem;                       // 生成するオブジェクト格納用

    private int itemCnt = 8;                            // 生成する数
    private int pointRange;                             // 配置予定地の乱数格納用
    private List<int> setPoint = new List<int>();       // 同じ場所にしないために使うリスト

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] setItemPoints = gameObject.GetComponentsInChildren<Transform>().Select(t => t.gameObject).ToArray();
        for (int i = 0; i <= setItemPoints.Length; i++)
        {
            setPoint.Add(i);                 // 配置先のオブジェクトをリストに格納
        }

        while (itemCnt-- > 0)                // 生成する数だけループ
        {
            pointRange = Random.Range(1, setPoint.Count);

            Instantiate(escapeItem, setItemPoints[pointRange].transform.position, setItemPoints[pointRange].transform.rotation);
            setPoint.RemoveAt(pointRange);   // 配置された場所はリストから除外
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
