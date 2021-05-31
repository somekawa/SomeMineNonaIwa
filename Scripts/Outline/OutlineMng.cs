using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineMng : MonoBehaviour
{
    private Outline outline_;

    // Start is called before the first frame update
    void Start()
    {
        outline_ = gameObject.GetComponent<Outline>();
        outline_.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "ItemHitArea")
        {
            outline_.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ItemHitArea")
        {
            outline_.enabled = false;
        }
    }

}
