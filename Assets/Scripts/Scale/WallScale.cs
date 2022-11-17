using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScale : MonoBehaviour, InterAction
{


    int scaledvalue = 0;
    float sign = 0;
    Vector3 pos = Vector3.zero;
    [SerializeField] Vector3 localpos = Vector3.zero;

    [SerializeField] MeshRenderer LeftCorner, RightCorner, wall;

    [SerializeField] Material[] materias;

    public void Interact(int x, int z)
    {
        //Control z or x axis
        if (transform.localPosition.z == 0)
        {
            scaledvalue = z;
            sign = Mathf.Sign(localpos.x) * (0.5f * x);
            pos = new Vector3(localpos.x + sign, transform.localPosition.y, transform.localPosition.z);
        }
        else
        {
            scaledvalue = x;
            sign = Mathf.Sign(localpos.z) * (0.5f * z);
            pos = new Vector3(transform.localPosition.x, transform.localPosition.y, localpos.z + sign);
        }
        //Set Scale
        transform.localScale = new Vector3(scaledvalue + 1,
        transform.localScale.y, transform.localScale.z);
        //Set Position
        transform.localPosition = pos;



    }

    public void ChangeMat(matColor value = matColor.white)
    {
        SetMaterial(materias[(int)value]);
    }
    public void SetMaterial(Material mat)
    {
        LeftCorner.material = mat;
        RightCorner.material = mat;
        wall.material = mat;
    }
    [SerializeField]
    matColor WallColor;
    public matColor Icolor
    {
        get => WallColor;
        set
        {
            if (value == WallColor) return;
            WallColor = value;
            ChangeMat(value);

        }

    }
}
