using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Scriptable Object/Level Generation/level")]
public class LevelData : ScriptableObject
{
    public short level = 0;

    public int x;
    public int y;

    public int SwipeCount=default;

    public int[] cornersColor = { 0, 0, 0, 0 };
    public List<CubeVirutalData> savedObjects = new List<CubeVirutalData>();


}

[System.Serializable]
public class CubeVirutalData
{
    public Vector3 localpositon;
    public matColor color;
}

