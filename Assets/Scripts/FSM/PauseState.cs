using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 暂停状态
/// </summary>
public class PauseState : FSMState
{
    private void Awake()
    {
        stateID = StateID.Pause;     //设置状态id
    }
}
