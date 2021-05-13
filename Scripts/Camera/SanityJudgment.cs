using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityJudgment : MonoBehaviour
{
    public Light spotLight;
    public GameObject slenderMan;
    public Camera playerCamera;

    private Plane[] planes_;
    private Collider eCollider_;
    private float sanityTime_;
    private SlenderManCtl slenderManCtl_;

    // Start is called before the first frame update
    void Start()
    {
        planes_ = GeometryUtility.CalculateFrustumPlanes(playerCamera);
        eCollider_ = slenderMan.GetComponent<Collider>();
        slenderManCtl_ = slenderMan.GetComponent<SlenderManCtl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GeometryUtility.TestPlanesAABB(planes_, eCollider_.bounds))
        {
            Debug.Log("Enter");
            slenderManCtl_.inSightFlag = true;
        }
    }

}
