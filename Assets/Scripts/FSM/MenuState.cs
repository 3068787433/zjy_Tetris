using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 菜单状态
/// </summary>
public class MenuState : FSMState
{
    private void Awake()
    {
        stateID = StateID.Menu;     //设置状态id
        AddTransition(Transition.StartButtonClick, StateID.Play);   //添加条件和状态（开始按钮进入开始状态）
    }

    //进入状态时调用的方法
    public override void DoBeforeEntering()
    {
        ctrl.view.ShowMenu();
        ctrl.cameraManager.ZoomOut();   //缩小
    }
    //离开状态时
    public override void DoBeforeLeaving()
    {
        ctrl.view.HideMenu();   //隐藏菜单
    }

    //开始按钮点击事件（拖拽State组件绑定）
    public void OnStartButtonClick()
    {
        ctrl.audioManager.PlayCursor();
        fsm.PerformTransition(Transition.StartButtonClick);
    }

    public void OnRankButtonClick()
    {
        ctrl.audioManager.PlayCursor();
        ctrl.view.ShowRankUI(ctrl.model.HighScore, ctrl.model.Score, ctrl.model.NumberGame);
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    public void OnDeleteButtonClick()
    {
        ctrl.model.ClearData();
        OnRankButtonClick();
    }

    /// <summary>
    /// 重新开始
    /// </summary>
    public void OnResatrtButtonClick()
    {
        ctrl.audioManager.PlayCursor();
        ctrl.model.Restart();
        ctrl.gameManager.ClearShape();
        fsm.PerformTransition(Transition.StartButtonClick);
    }
}
