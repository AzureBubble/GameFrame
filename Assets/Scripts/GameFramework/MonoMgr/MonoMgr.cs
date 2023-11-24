using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

namespace GameFramework.MonoManager
{
    /// <summary>
    /// Mono 管理器
    /// 类似于适配器模式
    /// 配合事件中心 让没有继承 MonoBehaviour
    /// 提供给没有继承 MonoBehaviour 的类使用 MonoBehaviour 的生命周期函数
    /// </summary>
    public class MonoMgr : Singleton<MonoMgr>
    {
        private MonoController monoController;

        public MonoMgr()
        {
            GameObject obj = new GameObject("MonoController");
            monoController = obj.AddComponent<MonoController>();
        }

        /// <summary>
        /// 给外部提供的 添加帧更新事件的函数
        /// </summary>
        /// <param name="action"></param>
        public void AddUpdateListener(UnityAction action)
        {
            monoController.AddUpdateListener(action);
        }

        /// <summary>
        /// 给外部提供的 移除帧更新事件的函数
        /// </summary>
        /// <param name="action"></param>
        public void RemoveUpdateListener(UnityAction action)
        {
            monoController.RemoveUpdateListener(action);
        }

        #region 开启协程

        public Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return monoController.StartCoroutine(coroutine);
        }

        public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
        {
            return monoController.StartCoroutine(methodName, value);
        }

        public Coroutine StartCoroutine(string methodName)
        {
            return monoController.StartCoroutine(methodName);
        }

        #endregion

        #region 停止协程

        public void StopCoroutine(IEnumerator coroutine)
        {
            monoController.StopCoroutine(coroutine);
        }

        public void StopCoroutine(Coroutine coroutine)
        {
            monoController.StopCoroutine(coroutine);
        }

        public void StopCoroutine(string methodName)
        {
            monoController.StopCoroutine(methodName);
        }

        public void StopAllCoroutines()
        {
            monoController.StopAllCoroutines();
        }

        #endregion
    }
}