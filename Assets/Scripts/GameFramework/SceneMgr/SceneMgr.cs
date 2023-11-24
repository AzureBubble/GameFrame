using SGM.MEventCenter;
using SGM.MonoManager;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SGM.MSceneManager
{
    /// <summary>
    /// 场景切换管理者
    /// </summary>
    public class SceneMgr : Singleton<SceneMgr>
    {
        [Header("事件名字")]
        private readonly string BEFORE_SCENE_LOAD = "BeforeSceneLoad";

        private readonly string AFTER_SCENE_LOADED = "AfterSceneLoaded";

        /// <summary>
        /// 同步切换场景
        /// </summary>
        /// <param name="sceneName">场景名字</param>
        public void LoadScene(string sceneName)
        {
            // 场景切换前事件监听
            EventCenter.Instance.EventTrigger(BEFORE_SCENE_LOAD);

            SceneManager.LoadScene(sceneName);

            // 场景切换后事件监听
            EventCenter.Instance.EventTrigger(AFTER_SCENE_LOADED);
        }

        /// <summary>
        /// 异步切换场景
        /// </summary>
        /// <param name="sceneName">场景名字</param>
        public void LoadSceneAsync(string sceneName)
        {
            // MonoMgr 启动异步加载场景协程
            MonoMgr.Instance.StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
        }

        /// <summary>
        /// 协程异步加载场景
        /// </summary>
        /// <param name="name">场景名</param>
        /// <returns></returns>
        private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
        {
            // 场景切换前事件监听
            EventCenter.Instance.EventTrigger(BEFORE_SCENE_LOAD);

            AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
            // 异步加载进度条
            while (!ao.isDone)
            {
                // 可在这里处理场景切换进度条问题
                yield return ao.progress;
            }

            // 场景切换后事件监听
            EventCenter.Instance.EventTrigger(AFTER_SCENE_LOADED);
        }
    }
}