using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 结束状态
/// </summary>
public class GameOverState : FSMState
{
    private void Awake()
    {
        stateID = StateID.GameOver;     //设置状态id
    }
}
