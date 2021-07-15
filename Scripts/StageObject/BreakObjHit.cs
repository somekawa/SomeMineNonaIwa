using System.Collections;
using UnityEngine;

public class BreakObjHit : MonoBehaviour
{

    public GameObject DestructionObj;
    public GameObject DestructionEffect;

    // アクティブにするだけ
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "RecognitionCylinder")
        {
            if (DestructionObj != null) DestructionObj.SetActive(true);  // 破壊後のPrefabをアクティブ
            if (DestructionEffect != null) DestructionEffect.SetActive(true);  // 破壊時のエフェクト
            SoundScript.GetInstance().PlaySound(5);
            Destroy(this.gameObject);
        }
    }
}