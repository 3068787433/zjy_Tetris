using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Model数据
/// </summary>
[Serializable]
public class ModelData
{
    public int score;          //当前分数
    public int highScore;      //最高分数
    public int numberGame;		//游戏次数
    public bool isSaveMap;

    public List<TransformData> map;
}
