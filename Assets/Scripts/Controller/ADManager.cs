using System.Collections.Generic;
using TapTap.TapAd;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// 广告管理
/// </summary>
public class ADManager : MonoBehaviour
{
    public int age = 16;
    private static TapRewardVideoAd _tapRewardAd;

    private Controller ctrl;

    private void Awake()
    {
        ctrl = GetComponent<Controller>();
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        TapAdConfig config = new TapAdConfig.Builder()
            .MediaId(1007745)             // 必选参数，为 TapADN 注册的媒体 ID
            .MediaName("Tetris")         // 必选参数，为 TapADN 注册的媒体名称
            .MediaKey("lNwENytFY9UBGii3pkOwtbv7lenJpY7H7sxyXQY6r81KiztRg9HqxnRcDBEZK2Aq")           // 必选参数，媒体密钥，可以在 TapADN 后台查看（用于传输数据的解密）
            .MediaVersion("1")                  // 必选参数，默认值 "1"
            .Channel("taptap2")             // 必选参数，渠道
            .TapClientId("o7ymyxyh6teg4ns6vx")    // 可选参数，TapTap 开发者中心的游戏 Client ID 
            .EnableDebugLog(true)              // 可选参数，是否打开原生 debug 调试信息输出：true 打开、false 关闭。默认 false 关闭
            .Build();

        // CustomControllerWrapper 为实现了 TapTap.TapAd.ICustomController 的类
        // onInitedCallback 为可选回调函数，类型是 System.Action，
        TapAdSdk.Init(config, new ADCustomControllerWrapper(this), OnInitedCallBack);
    }
    //初始化完成回调函数
    private void OnInitedCallBack()
    {
        Debug.Log("Unity:广告初始化完成！");
    }

    //播放视频
    public void LoadAD()
    {
        if (TapAdSdk.IsInited == false)
        {
            Debug.Log("TapAd 需要先初始化");
            return;
        }
        if (_tapRewardAd != null)
        {   //有缓存，说明上个视频还在，释放掉
            _tapRewardAd.Dispose();
            _tapRewardAd = null;
        }
        int adId = 1006397;
        var request = new TapAdRequest.Builder()
            .SpaceId(adId)
            .RewardName("Teris-Android-test")
            .RewardCount(9)
            .Build();
        _tapRewardAd = new TapRewardVideoAd(request);       //新建广告，传入
        _tapRewardAd.SetLoadListener(new ADRewardVideoAdLoadListener(this));
        _tapRewardAd.Load();
    }

    //显示
    public void Show()
    {
        if (TapAdSdk.IsInited == false)
        {
            Debug.Log("TapAd 需要先初始化");
            return;
        }
        if (_tapRewardAd != null)
        {   //有缓存，说明上个视频还在
            _tapRewardAd.SetInteractionListener(new ADRewardVideoInteractionListener(ctrl));
            _tapRewardAd.Show();
        }
        else
        {
            Debug.LogErrorFormat($"[Unity::AD] 未加载好视频，无法播放！");
        }
    }
}


//初始化接口
public sealed class ADCustomControllerWrapper : ICustomController
{
    private readonly ADManager example;

    //方便外部接口传入
    public ADCustomControllerWrapper(ADManager context)
    {
        this.example = context;
    }

    bool ICustomController.CanUseLocation => false;

    TapAdLocation ICustomController.GetTapAdLocation => null;

    bool ICustomController.CanUsePhoneState => false;

    string ICustomController.GetDevImei => "";

    bool ICustomController.CanUseWifiState => true;

    bool ICustomController.CanUseWriteExternal => true;

    string ICustomController.GetDevOaid => null;

    bool ICustomController.Alist => true;

    bool ICustomController.CanUseAndroidId => true;

    //根据用户自定义信息优化广告
    CustomUser ICustomController.ProvideCustomer() => new CustomUser() { realAge = example.age };   //年龄
}

//加载接口
public sealed class ADRewardVideoAdLoadListener : IRewardVideoAdLoadListener
{
    private readonly ADManager example;

    public ADRewardVideoAdLoadListener(ADManager example)
    {
        this.example = example;
    }

    public void OnError(int code, string message)
    {
        message = message ?? "NULL";
        Debug.Log($"加载激励视频错误！错误 code：{code} message: {message}");
    }

    public void OnRewardVideoAdCached(TapRewardVideoAd ad)
    {
        Debug.Log($"{ad.AdType}素材 Cached 完毕！ ad ≠ null：{(ad != null).ToString()}");
        Assert.IsTrue(ad.IsReady, "Cached ad.IsReady");
    }

    public void OnRewardVideoAdLoad(TapRewardVideoAd ad)
    {
        Debug.Log($"{ad.AdType}素材 Load 完毕！ ad ≠ null:{(ad != null).ToString()}");
        Assert.IsTrue(ad.IsReady, "Cached ad.IsReady");
        //播放
        example.Show();
    }
}

//播放接口
public sealed class ADRewardVideoInteractionListener : IRewardVideoInteractionListener
{
    private readonly Controller ctrl;
    public ADRewardVideoInteractionListener(Controller ctrl)
    {
        this.ctrl = ctrl;
    }

    //开始展示
    public void OnAdShow(TapRewardVideoAd ad)
    {
        Debug.Log($"[Unity::AD] {ad.AdType} OnAdShow");
    }
    //关闭
    public void OnAdClose(TapRewardVideoAd ad)
    {
        Debug.Log($"[Unity::AD] {ad.AdType} OnAdClose");
    }
    //奖励发放
    public void OnRewardVerify(TapRewardVideoAd ad, bool rewardVerify, int rewardAmount, string rewardName, int code, string msg)
    {
        Debug.Log($"[Unity::AD] {ad.AdType} OnRewardVerify，rewardVerify: {rewardVerify}" +
            $"，rewardAmount: {rewardAmount}，rewardName: {rewardName}，code: {code}，msg: {msg}");
        ctrl.gameManager.ResetAll();    //重置
    }
    //跳过
    public void OnSkippedVideo(TapRewardVideoAd ad)
    {
        Debug.Log($"[Unity::AD] {ad.AdType} OnSkippedVideo");
    }
    //完成
    public void OnVideoComplete(TapRewardVideoAd ad)
    {
        Debug.Log($"[Unity::AD] {ad.AdType} OnVideoComplete");

    }
    //视频出错
    public void OnVideoError(TapRewardVideoAd ad)
    {
        Debug.Log($"[Unity::AD] {ad.AdType} OnVideoError");
    }
    //激励视频缓存完毕
    public void OnRewardVideoAdCached(TapRewardVideoAd ad)
    {
        Debug.Log($"[Unity::AD] {ad.AdType} OnRewardVideoAdCached");
    }
    //视频加载完毕
    public void OnRewardVideoAdLoad(TapRewardVideoAd ad)
    {
        Debug.Log($"[Unity::AD] {ad.AdType} OnRewardVideoAdLoad");
    }
    //点击
    public void OnAdClick(TapRewardVideoAd ad)
    {
        Debug.Log($"[Unity::AD] {ad.AdType} OnAdClick");
    }
    //出错
    public void OnError(int code, string message)
    {
        message = message ?? "NULL";
        Debug.Log($"加载激励视频错误！错误 code：{code} message: {message}");
    }
}
