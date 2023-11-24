using SGM.ResourcesManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SGM.ObjectPoolManager
{
    /// <summary>
    /// 缓存池容器对象
    /// </summary>
    public class PoolData
    {
        public GameObject parentObj; // 缓存池结点
        public List<GameObject> poolList; // 缓存池容器
        public readonly float clearUpInterval = 300.0f; // 缓存池清理间隔时间 默认时间300秒
        private float lastUseTime = 0.0f; // 缓存池最后一次使用时间

        /// <summary>
        /// 构造函数 创建缓存池管理者对象结点，预制体缓存池结点
        /// </summary>
        /// <param name="obj">缓存池物体</param>
        /// <param name="poolMgrObj">缓存池管理游戏对象</param>
        public PoolData(GameObject obj, GameObject poolMgrObj)
        {
            // 创建父节点物体
            this.parentObj = new GameObject(obj.name + " Pool");
            // 把父节点物体作为缓存池管理对象的子节点
            this.parentObj.transform.SetParent(poolMgrObj.transform, false);
            // 初始化缓存池
            poolList = new List<GameObject>(8);
            // 把物体压入缓存池
            RealeaseObj(obj);
        }

        public GameObject GetObj()
        {
            GameObject obj = null;

            // 取出缓存池中最后一个对象
            obj = poolList[^1];
            // 从缓存池中移除最后一个对象
            poolList.RemoveAt(poolList.Count - 1);

            obj.SetActive(true);
            obj.transform.parent = null;

            // 重置最后一次使用时间
            lastUseTime = Time.time;

            return obj;
        }

        /// <summary>
        /// 压入缓存池
        /// </summary>
        /// <param name="obj"></param>
        public void RealeaseObj(GameObject obj)
        {
            poolList.Add(obj);
            obj.transform.SetParent(parentObj.transform, false);
            obj.SetActive(false);

            // 重置最后一次使用时间
            lastUseTime = Time.time;
        }

        public bool InUnUsed(float currentTime)
        {
            //Debug.Log(currentTime - lastUseTime);
            return currentTime - lastUseTime >= clearUpInterval;
        }

        public void Clear()
        {
            foreach (GameObject obj in poolList)
            {
                GameObject.Destroy(obj);
            }
            GameObject.Destroy(parentObj);
            poolList.Clear();
            parentObj = null;
        }
    }

    /// <summary>
    /// 缓存池管理器
    /// </summary>
    public class PoolMgr : SingletonAutoMono<PoolMgr>
    {
        /// <summary>
        /// 缓存池容器 键：某一类物品名字，值：游戏对象
        /// </summary>
        public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();

        private void Update()
        {
            // 定期清理缓存池
            CleanupUnusedPools();
        }

        /// <summary>
        /// 从缓存池中取物体
        /// </summary>
        /// <param name="name">物体名字</param>
        /// <param name="callback">回调函数</param>
        public void GetObj(string name, UnityAction<GameObject> callback = null, string path = "Prefabs/")
        {
            // 判断对应的对象池是否存在，且池子里有物体
            if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
            {
                callback?.Invoke(poolDic[name].GetObj());
            }
            else
            {
                // 异步加载预制体资源
                ResourcesMgr.Instance.LoadResAsync<GameObject>(path + name, (resObj) =>
                {
                    callback?.Invoke(resObj);
                }, name);
            }
        }

        /// <summary>
        /// 把物体放回对象池中
        /// </summary>
        /// <param name="name">物体名字</param>
        /// <param name="obj">归还的物体对象</param>
        public void RealeaseObj(string name, GameObject obj)
        {
            if (poolDic.ContainsKey(name))
            {
                poolDic[name].RealeaseObj(obj);
            }
            else
            {
                poolDic.Add(name, new PoolData(obj, Instance.gameObject));
            }
        }

        /// <summary>
        /// 定期清理缓存池
        /// </summary>
        public void CleanupUnusedPools()
        {
            // 记录当前时间
            float currentTime = Time.time;
            // 存储待移除键名
            List<string> poolsToRemove = new List<string>();

            // 循环缓存池容器里的所有缓存池对象
            foreach (KeyValuePair<string, PoolData> kv in poolDic)
            {
                // 判断是否长时间未使用
                if (kv.Value.InUnUsed(currentTime))
                {
                    //poolDic[kv.Key].Clear();
                    // 协程清理超时对象池资源
                    StartCoroutine(ClearUpPool(poolDic[kv.Key]));
                    poolsToRemove.Add(kv.Key);
                }
            }
            // 删除已经
            foreach (string poolName in poolsToRemove)
            {
                poolDic.Remove(poolName);
            }
        }

        /// <summary>
        /// 清空缓存池
        /// </summary>
        public void Clear()
        {
            foreach (PoolData pool in poolDic.Values)
            {
                pool.Clear();
            }
            poolDic.Clear();
        }

        /// <summary>
        /// 清除缓存协程
        /// </summary>
        /// <param name="poolData"></param>
        /// <returns></returns>
        private IEnumerator ClearUpPool(PoolData poolData)
        {
            poolData.Clear();
            yield return null;
        }
    }
}