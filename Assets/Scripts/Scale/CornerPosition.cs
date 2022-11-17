using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerPosition : MonoBehaviour, InterAction
{
    public matColor Icolor
    {
        get;
        set;

    }
    [SerializeField] Vector3 localpos = Vector3.zero;
    [SerializeField] GameObject ParentWall;
    public void Interact(int x, int z)
    {
        float posx = localpos.x + (Mathf.Sign(localpos.x) * 0.5f * x);
        float posz = localpos.z + (Mathf.Sign(localpos.z) * 0.5f * z);

        transform.localPosition = new Vector3(posx, transform.localPosition.y, posz);
    }
    public void SetMaterial(Material mat)
    {
        ParentWall.GetComponent<InterAction>().SetMaterial(mat);
    }
    public void ChangeMat(matColor value = matColor.white)
    {

    }

}
