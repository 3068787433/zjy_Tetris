using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏状态
/// </summary>
public class PlayState : FSMState
{
    private void Awake()
    {
        stateID = StateID.Play;     //设置状态id
        AddTransition(Transition.PauseButtonClick, StateID.Menu);
    }

    public override void DoBeforeEntering()
    {
        ctrl.view.ShowGameUI(ctrl.model.Score, ctrl.model.HighScore);
        ctrl.cameraManager.ZoomIn();
        ctrl.gameManager.StartGame();
    }

    public override void DoBeforeLeaving()
    {
        ctrl.view.HideGameUI();
        ctrl.view.ShowRestartButton();
        ctrl.gameManager.PauseGame();
    }

    /// <summary>
    /// 暂停按钮点击事件
    /// </summary>
    public void OnPauseButtonClick()
    {
        ctrl.dataManager.SaveData();
        ctrl.audioManager.PlayCursor();
        fsm.PerformTransition(Transition.PauseButtonClick);     //切换状态
    }
    
    //重新开始
    public void OnRestartButtonClick()
    {
        ctrl.view.ShowADButton();
        ctrl.audioManager.PlayCursor();
        ctrl.view.HideGameOverUI();
        ctrl.model.Restart();
        ctrl.gameManager.ClearShape();
        ctrl.gameManager.StartGame();
        ctrl.view.UpdateGameUI(0, ctrl.model.HighScore);
    }
}
