using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音效控制
/// </summary>
public class AudioManager : MonoBehaviour
{
    private Controller ctrl;

	public AudioClip cursor;
	public AudioClip drop;
    public AudioClip balloon;
    public AudioClip lineclear;

	private AudioSource audioSource;

    private bool isMute = false;    //是否静音

    private void Awake()
    {
        ctrl = GetComponent<Controller>();
        audioSource = GetComponent<AudioSource>();

        int ismute = PlayerPrefs.GetInt("isMute", 0);
        if(ismute == 1) isMute = true;
    }

    private void Start()
    {
        ctrl.view.SetMuteActive(isMute);
    }

    //鼠标声音
    public void PlayCursor()
	{
        PlayAudio(cursor);
	}
    //下落声音
    public void PlayDrop()
    {
        PlayAudio(drop);
    }
    //移动
    public void PlayBalloon()
    {
        PlayAudio(balloon);
    }
    //消除
    public void PlayLineClear()
    {
        PlayAudio(lineclear);
    }

    //播放声音
    private void PlayAudio(AudioClip clip)
    {
        if (isMute) return;
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void OnAudioButtonClick()
    {
        isMute = !isMute;
        ctrl.view.SetMuteActive(isMute);
        if (!isMute)
        {
            PlayCursor();
            PlayerPrefs.SetInt("isMute", 0);
        }
        else
        {
            PlayerPrefs.SetInt("isMute", 1);
        }
    }
}
