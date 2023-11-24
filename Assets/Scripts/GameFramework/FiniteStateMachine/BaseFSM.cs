using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Machine
{
    /// <summary>
    /// 状态机基类
    /// </summary>
    public class BaseFSM : MonoBehaviour
    {
        /// <summary>
        /// 状态机字典
        /// </summary>
        private Dictionary<E_State_Type, IState> stateDict = new Dictionary<E_State_Type, IState>(50);

        /// <summary>
        /// 黑板 共享数据信息
        /// </summary>
        private Dictionary<string, System.Object> blackboardDict = new Dictionary<string, System.Object>(50);

        /// <summary>
        /// 当前状态
        /// </summary>
        private IState currentState;

        public IState CurrentState
        {
            get { return currentState; }
        }

        protected virtual void Awake()
        {
            // 初始化状态机方法
            Init();
        }

        protected virtual void Update()
        {
            // 当前状态更新
            currentState?.OnUpdate();
        }

        /// <summary>
        /// 初始化状态机方法
        /// </summary>
        protected virtual void Init()
        {
        }

        #region 启动状态

        /// <summary>
        /// 启动状态
        /// </summary>
        /// <param name="newState">新状态</param>
        protected void StateOn(E_State_Type newState)
        {
            if (stateDict.TryGetValue(newState, out currentState))
            {
                currentState?.OnEnter();
            }
            else
            {
                Debug.Log($"不存在{newState}状态");
            }
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="newState">新状态</param>
        public void SwitchState(E_State_Type newState)
        {
            currentState?.OnExit();

            StateOn(newState);
        }

        #endregion

        #region 设置黑板

        /// <summary>
        /// 设置黑板数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetBlackboardValue(string key, System.Object value)
        {
            if (blackboardDict.ContainsKey(key))
            {
                blackboardDict[key] = value;
            }
            else
            {
                blackboardDict.Add(key, value);
            }
        }

        /// <summary>
        /// 获取黑板数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public System.Object GetBlackboardValue(string key)
        {
            if (blackboardDict.ContainsKey(key))
            {
                return blackboardDict[key];
            }
            else
            {
                Debug.Log($"Not found blackboard value : {key}");
                return null;
            }
        }

        #endregion
    }
}