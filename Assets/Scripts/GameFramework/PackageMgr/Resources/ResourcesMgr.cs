using SGM.MonoManager;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SGM.ResourcesManager
{
    public class ResourcesMgr : Singleton<ResourcesMgr>
    {
        /// <summary>
        /// 同步加载资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="name">资源路径</param>
        /// <param name="objName">物体实例化后名字</param>
        /// <returns></returns>
        public T LoadRes<T>(string path, string objName = null) where T : Object
        {
            T res = Resources.Load<T>(path);

            if (res is GameObject)
            {
                if (objName == null)
                {
                    return GameObject.Instantiate(res);
                }
                T obj = GameObject.Instantiate(res);
                obj.name = objName;
                return obj;
            }

            return res;
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">资源路径</param>
        /// <param name="callback">回调函数</param>
        /// <param name="objName">物体实例化后名字</param>
        public void LoadResAsync<T>(string path, UnityAction<T> callback, string objName = null) where T : Object
        {
            // 协程异步加载资源
            MonoMgr.Instance.StartCoroutine(LoadResAsyncCoroutine<T>(path, callback, objName));
        }

        private IEnumerator LoadResAsyncCoroutine<T>(string path, UnityAction<T> callback, string objName = null) where T : Object
        {
            ResourceRequest rr = Resources.LoadAsync<T>(path);
            // 等待异步加载结束
            yield return rr;
            // 根据资源类型的不同进行不同处理
            if (rr.asset is GameObject)
            {
                // 实例化一个游戏对象并通过回调函数返回
                if (objName == null)
                {
                    callback?.Invoke(GameObject.Instantiate(rr.asset as T));
                }
                else
                {
                    T obj = GameObject.Instantiate(rr.asset as T);
                    obj.name = objName;
                    callback?.Invoke(obj);
                }
            }
            else
            {
                callback?.Invoke(rr.asset as T);
            }
        }
    }
}