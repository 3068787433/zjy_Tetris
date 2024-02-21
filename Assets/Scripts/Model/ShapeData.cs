using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 当前图案数据类
/// </summary>
[Serializable]
public class ShapeData
{
    public bool isDelete;
    public int index;		//图案索引
    public int indexColor;  //颜色索引
    public float x;
    public float y;
    public float z;
}
