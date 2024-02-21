using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 视图层
/// </summary>
public class View : MonoBehaviour
{
    private Controller ctrl;

	private RectTransform logoName;
	private RectTransform menuUI;
    private RectTransform gameUI;
    private GameObject gameOverUI;
    private GameObject settingUI;
    private GameObject rankUI;

    private GameObject restartButton;
    private GameObject mute;

    private GameObject ADButton;
    private Text score;
    private Text highScore;
    private Text gameOverScore;

    private Text rankScore;
    private Text rankHighScore;
    private Text rankNumberGame;

    private void Awake()
    {
        ctrl = GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>();

        logoName = transform.Find("Canvas/LogoName") as RectTransform;
        menuUI = transform.Find("Canvas/MenuUI") as RectTransform;
        gameUI = transform.Find("Canvas/GameUI") as RectTransform;
        restartButton = transform.Find("Canvas/MenuUI/RestartButton").gameObject;
        score = transform.Find("Canvas/GameUI/ScoreLable/Score").GetComponent<Text>();
        highScore = transform.Find("Canvas/GameUI/HighScoreLable/Score").GetComponent<Text>();
        gameOverUI = transform.Find("Canvas/GameOverUI").gameObject;
        ADButton = transform.Find("Canvas/GameOverUI/ADButton").gameObject;
        gameOverScore = transform.Find("Canvas/GameOverUI/Score").GetComponent<Text>();
        settingUI = transform.Find("Canvas/SettingUI").gameObject;
        mute = transform.Find("Canvas/SettingUI/AudioButton/Mute").gameObject;
        rankUI = transform.Find("Canvas/RankUI").gameObject;

        rankScore = transform.Find("Canvas/RankUI/ScoreLable/Score").GetComponent<Text>();
        rankHighScore = transform.Find("Canvas/RankUI/HighLable/Score").GetComponent<Text>();
        rankNumberGame = transform.Find("Canvas/RankUI/CountLable/Count").GetComponent<Text>();
    }

    /// <summary>
    /// 显示菜单UI
    /// </summary>
    public void ShowMenu()
    {
        logoName.DOAnchorPosY(-287f, 0.5f);  //移动到屏幕

        menuUI.DOAnchorPosY(142.74f, 0.5f);
    }

    public void HideMenu()
    {
        //logoName.DOAnchorPosY(165.68f, 0.5f)
        //    .OnComplete(delegate { logoName.gameObject.SetActive(false); });    //动画结束时调用委托

        //menuUI.DOAnchorPosY(-142.74f, 0.5f)
        //    .OnComplete(delegate { menuUI.gameObject.SetActive(false); });

        logoName.DOAnchorPosY(165.68f, 0.5f);
        menuUI.DOAnchorPosY(-142.74f, 0.5f);
    }

    #region 游戏UI
    /// <summary>
    /// 显示游戏UI
    /// </summary>
    public void ShowGameUI(int score = 0, int highScore = 0)
    {
        this.score.text = score.ToString();
        this.highScore.text = highScore.ToString();
        gameUI.DOAnchorPosY(-237.1108f, 0.5f);
    }
    //更新成绩
    public void UpdateGameUI(int score, int highScore)
    {
        this.score.text = score.ToString();
        this.highScore.text = highScore.ToString();
    }
    //隐藏
    public void HideGameUI()
    {
        gameUI.DOAnchorPosY(237.11f, 0.5f);
    }
    #endregion

    //显示重新开始按钮
    public void ShowRestartButton()
    {
        restartButton.gameObject.SetActive(true);
    }

    //显示游戏结束UI
    public void ShowGameOverUI(int score = 0)
    {
        gameOverUI.gameObject.SetActive(true);
        gameOverScore.text = score.ToString();
    }

    //返回首页按钮事件
    public void OnHomeButtonClick()
    {
        ctrl.audioManager.PlayCursor();
        ctrl.dataManager.SaveData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ctrl.gameManager.ClearShape();
    }

    //播放广告
    public void ShowADButtonClick()
    {
        HideADButton();
        ctrl.adManager.LoadAD();
    }
    //显示播放广告按钮
    public void ShowADButton()
    {
        ADButton.gameObject.SetActive(true);
    }
    //隐藏播放广告按钮
    public void HideADButton()
    {
        ADButton.gameObject.SetActive(false);
    }

    //设置
    public void OnSettingButtonClick()
    {
        ctrl.audioManager.PlayCursor();
        settingUI.gameObject.SetActive(true);
    }

    //隐藏游戏结束
    public void HideGameOverUI()
    {
        gameOverUI.gameObject.SetActive(false);
    }

    //声音斜杠图片
    public void SetMuteActive(bool isActive)
    {
        mute.SetActive(isActive);
    }

    //记录
    public void ShowRankUI(int highScore, int score, int numberGame)
    {
        rankUI.SetActive(true);
        rankHighScore.text = highScore.ToString();
        rankScore.text = score.ToString();
        rankNumberGame.text = numberGame.ToString();
    }
}
