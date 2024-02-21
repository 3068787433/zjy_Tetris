namespace TapTap.TapAd.Internal
{
    public interface ISplashAdInteractionListener : ICommonInteractionListener
    {
        /// <summary>
        /// 点击跳过
        /// </summary>
        /// <param name="ad"></param>
        void OnAdSkip(TapSplashAd ad);
        /// <summary>
        /// 广告时间到
        /// </summary>
        /// <param name="ad"></param>
        void OnAdTimeOver(TapSplashAd ad);
    }
}