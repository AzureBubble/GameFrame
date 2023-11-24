using SGM.ObjectPoolManager;
using SGM.ResourcesManager;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SGM.MusicManager
{
    /// <summary>
    /// 音乐音效管理器
    /// </summary>
    public class MusicMgr : Singleton<MusicMgr>
    {
        private GameObject musicMgrObj;

        // 游戏背景音乐
        private AudioSource gameMusic = null;

        private float gameMusicVolume = 1f;
        //private bool gameMusicIsMute = false;

        // 游戏环境音
        private AudioSource ambientMusic = null;

        private float ambientMusicVolume = 1f;
        //private bool ambientMusicIsMute = false;

        // 游戏音效
        private List<AudioSource> soundList;

        private float soundMusicVolume = 1f;
        private bool soundMusicIsMute = false;

        public MusicMgr()
        {
            GameObject obj = new GameObject("Music Manager");
            GameObject.DontDestroyOnLoad(obj);
            musicMgrObj = obj;
            soundList = new List<AudioSource>(10);
        }

        #region 游戏背景音乐

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="name">音乐名字</param>
        public void PlayGameMusic(string name)
        {
            if (gameMusic == null)
            {
                GameObject gameMusicObj = new GameObject("Game Music");
                gameMusic = gameMusicObj.AddComponent<AudioSource>();
                gameMusicObj.transform.SetParent(musicMgrObj.transform, false);
            }
            ResourcesMgr.Instance.LoadResAsync<AudioClip>("Music/BK/" + name, (clip) =>
            {
                gameMusic.clip = clip;
                gameMusic.loop = true;
                gameMusic.volume = gameMusicVolume;
                gameMusic.Play();
            });
        }

        /// <summary>
        /// 暂停播放背景音乐
        /// </summary>
        /// <param name="name">音乐名字</param>
        public void PauseGameMusic(string name)
        {
            gameMusic?.Pause();
        }

        /// <summary>
        /// 停止播放背景音乐
        /// </summary>
        /// <param name="name">音乐名字</param>
        public void StopGameMusic(string name)
        {
            gameMusic?.Stop();
        }

        public void ChangeGameMusicVolume(float volume)
        {
            gameMusicVolume = volume;
            if (gameMusic != null)
            {
                gameMusic.volume = gameMusicVolume;
            }
        }

        /// <summary>
        /// 这是背景音乐是否静音
        /// </summary>
        /// <param name="isMute"></param>
        public void SetGameMusicMute(bool isMute)
        {
            if (gameMusic != null)
            {
                gameMusic.mute = isMute;
            }
        }

        #endregion

        #region 游戏环境音乐

        /// <summary>
        /// 播放环境音乐
        /// </summary>
        /// <param name="name">音乐名字</param>
        public void PlayAmbientMusic(string name)
        {
            if (ambientMusic == null)
            {
                GameObject ambientMusicObj = new GameObject("Ambient Music");
                ambientMusic = ambientMusicObj.AddComponent<AudioSource>();
                ambientMusicObj.transform.SetParent(musicMgrObj.transform, false);
            }
            ResourcesMgr.Instance.LoadResAsync<AudioClip>("Music/Ambient/" + name, (clip) =>
            {
                ambientMusic.clip = clip;
                ambientMusic.loop = true;
                ambientMusic.volume = ambientMusicVolume;
                ambientMusic.Play();
            });
        }

        /// <summary>
        /// 暂停播放环境音乐
        /// </summary>
        /// <param name="name">音乐名字</param>
        public void PauseAmbientMusic(string name)
        {
            ambientMusic?.Pause();
        }

        /// <summary>
        /// 停止播放环境音乐
        /// </summary>
        /// <param name="name">音乐名字</param>
        public void StopAmbientMusic(string name)
        {
            ambientMusic?.Stop();
        }

        public void ChangeAmbientMusicVolume(float volume)
        {
            ambientMusicVolume = volume;
            if (ambientMusic != null)
            {
                ambientMusic.volume = ambientMusicVolume;
            }
        }

        /// <summary>
        /// 这是环境音乐是否静音
        /// </summary>
        /// <param name="isMute"></param>
        public void SetAmbientMusicMute(bool isMute)
        {
            if (ambientMusic != null)
            {
                ambientMusic.mute = isMute;
            }
        }

        #endregion

        #region 游戏音效

        /// <summary>
        /// 播放游戏音效
        /// </summary>
        /// <param name="name">音效名字</param>
        /// <param name="isLoop">是否循环</param>
        /// <param name="callback">回调函数</param>
        public void PlaySoundMusic(string name, bool isLoop = false, UnityAction<AudioSource> callback = null)
        {
            // 先加载音效资源
            ResourcesMgr.Instance.LoadResAsync<AudioClip>("Music/Sound/" + name, (soundClip) =>
            {
                // 然后通过对象池管理音效播放组件
                PoolMgr.Instance.GetObj("Sound", (soundObj) =>
                {
                    AudioSource audioSource = soundObj.GetComponent<AudioSource>();
                    audioSource.loop = isLoop;
                    audioSource.volume = soundMusicVolume;
                    audioSource.mute = soundMusicIsMute;
                    audioSource.PlayOneShot(soundClip);
                    soundList.Add(audioSource);
                    callback?.Invoke(audioSource);
                });
            });
        }

        /// <summary>
        /// 停止播放游戏音效
        /// </summary>
        /// <param name="name">音效名字</param>
        public void StopSoundMusic(AudioSource audioSource)
        {
            if (soundList.Contains(audioSource))
            {
                // 音效停止播放，并放回对象池
                audioSource.Stop();
                PoolMgr.Instance.RealeaseObj(audioSource.name, audioSource.gameObject);
                soundList.Remove(audioSource);
            }
        }

        public void ChangeSoundMusicVolume(float volume)
        {
            soundMusicVolume = volume;
            foreach (AudioSource source in soundList)
            {
                source.volume = volume;
            }
        }

        /// <summary>
        /// 这是环境音乐是否静音
        /// </summary>
        /// <param name="isMute"></param>
        public void SetSoundMusicMute(bool isMute)
        {
            soundMusicIsMute = isMute;
            if (soundList.Count > 0)
            {
                foreach (AudioSource source in soundList)
                {
                    source.mute = isMute;
                }
            }
        }

        #endregion
    }
}