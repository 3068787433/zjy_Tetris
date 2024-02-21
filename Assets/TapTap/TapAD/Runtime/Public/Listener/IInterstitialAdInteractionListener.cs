namespace TapTap.TapAd.Internal
{
    public interface IInterstitialAdInteractionListener : ICommonInteractionListener
    {
        /// <summary>
        /// 当广告播放
        /// </summary>
        /// <param name="ad"></param>
        void OnAdShow(TapInterstitialAd ad);
        /// <summary>
        /// 当广告关闭
        /// </summary>
        /// <param name="ad"></param>
        void OnAdClose(TapInterstitialAd ad);
        /// <summary>
        /// 当广告出错
        /// </summary>
        /// <param name="ad"></param>
        void OnAdError(TapInterstitialAd ad);
    }
}