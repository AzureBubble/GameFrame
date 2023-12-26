using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace GameFramework.GFEventCenter
{
    /// <summary>
    /// 单个事件接口
    /// </summary>
    public interface IEventInfo
    {
    }

    /// <summary>
    /// 不带参数的事件
    /// </summary>
    public class EventInfo : IEventInfo
    {
        public event UnityAction actions;

        public EventInfo(UnityAction action)
        {
            actions += action;
        }

        /// <summary>
        /// 事件触发
        /// </summary>
        public void EventTrigger()
        {
            actions?.Invoke();
        }
    }

    /// <summary>
    /// 带一个参数的事件
    /// </summary>
    public class EventInfo<T> : IEventInfo
    {
        public event UnityAction<T> actions;

        public EventInfo(UnityAction<T> action)
        {
            actions += action;
        }

        public void EventTrigger(T t)
        {
            actions?.Invoke(t);
        }
    }

    /// <summary>
    /// 带两个参数的事件
    /// </summary>
    public class EventInfo<T1, T2> : IEventInfo
    {
        public event UnityAction<T1, T2> actions;

        public EventInfo(UnityAction<T1, T2> action)
        {
            actions += action;
        }

        public void EventTrigger(T1 t1, T2 t2)
        {
            actions?.Invoke(t1, t2);
        }
    }

    /// <summary>
    /// 带三个参数的事件
    /// </summary>
    public class EventInfo<T1, T2, T3> : IEventInfo
    {
        public event UnityAction<T1, T2, T3> actions;

        public EventInfo(UnityAction<T1, T2, T3> action)
        {
            actions += action;
        }

        public void EventTrigger(T1 t1, T2 t2, T3 t3)
        {
            actions?.Invoke(t1, t2, t3);
        }
    }

    /// <summary>
    /// 事件中心 负责分发管理事件
    /// </summary>
    public class EventCenter : Singleton<EventCenter>
    {
        /// <summary>
        /// 事件容器
        /// key —— 事件的名字（比如：怪物死亡，玩家死亡，通关 等等）
        /// value —— 对应的是 监听这个事件 对应的委托函数
        /// </summary>
        private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

        #region 不带参数的事件监听

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="actionName">事件名字</param>
        /// <param name="action">事件</param>
        public void AddEventListener(string actionName, UnityAction action)
        {
            // 如果字典中存在该事件
            if (eventDic.TryGetValue(actionName, out IEventInfo eventInfo))
            {
                (eventInfo as EventInfo).actions += action;
            }
            else // 否则
            {
                eventDic.Add(actionName, new EventInfo(action));
            }
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="actionName">事件名字</param>
        /// <param name="action">事件</param>
        public void RemoveEventListener(string actionName, UnityAction action)
        {
            // 如果字典中存在该事件
            if (eventDic.TryGetValue(actionName, out IEventInfo eventInfo))
            {
                (eventInfo as EventInfo).actions -= action;
            }
        }

        /// <summary>
        /// 不带参数事件触发
        /// </summary>
        /// <param name="actionName">事件名字</param>
        public void EventTrigger(string actionName)
        {
            if (eventDic.TryGetValue(actionName, out IEventInfo eventInfo))
            {
                (eventDic[actionName] as EventInfo).EventTrigger();
            }
        }

        #endregion

        #region 一个参数的事件监听

        public void AddEventListener<T>(string actionName, UnityAction<T> action)
        {
            // 如果字典中存在该事件
            if (eventDic.TryGetValue(actionName, out IEventInfo eventInfo))
            {
                (eventInfo as EventInfo<T>).actions += action;
            }
            else // 否则
            {
                eventDic.Add(actionName, new EventInfo<T>(action));
            }
        }

        public void RemoveEventListener<T>(string actionName, UnityAction<T> action)
        {
            // 如果字典中存在该事件
            if (eventDic.TryGetValue(actionName, out IEventInfo eventInfo))
            {
                (eventInfo as EventInfo<T>).actions -= action;
            }
        }

        public void EventTrigger<T>(string actionName, T t)
        {
            if (eventDic.TryGetValue(actionName, out IEventInfo eventInfo))
            {
                (eventDic[actionName] as EventInfo<T>).EventTrigger(t);
            }
        }

        #endregion

        #region 两个参数的事件监听

        public void AddEventListener<T1, T2>(string actionName, UnityAction<T1, T2> action)
        {
            // 如果字典中存在该事件
            if (eventDic.TryGetValue(actionName, out IEventInfo eventInfo))
            {
                (eventInfo as EventInfo<T1, T2>).actions += action;
            }
            else // 否则
            {
                eventDic.Add(actionName, new EventInfo<T1, T2>(action));
            }
        }

        public void RemoveEventListener<T1, T2>(string actionName, UnityAction<T1, T2> action)
        {
            // 如果字典中存在该事件
            if (eventDic.TryGetValue(actionName, out IEventInfo eventInfo))
            {
                (eventInfo as EventInfo<T1, T2>).actions -= action;
            }
        }

        public void EventTrigger<T1, T2>(string actionName, T1 t1, T2 t2)
        {
            if (eventDic.TryGetValue(actionName, out IEventInfo eventInfo))
            {
                (eventDic[actionName] as EventInfo<T1, T2>).EventTrigger(t1, t2);
            }
        }

        #endregion

        #region 三个参数的事件监听

        public void AddEventListener<T1, T2, T3>(string actionName, UnityAction<T1, T2, T3> action)
        {
            // 如果字典中存在该事件
            if (eventDic.TryGetValue(actionName, out IEventInfo eventInfo))
            {
                (eventInfo as EventInfo<T1, T2, T3>).actions += action;
            }
            else // 否则
            {
                eventDic.Add(actionName, new EventInfo<T1, T2, T3>(action));
            }
        }

        public void RemoveEventListener<T1, T2, T3>(string actionName, UnityAction<T1, T2, T3> action)
        {
            // 如果字典中存在该事件
            if (eventDic.TryGetValue(actionName, out IEventInfo eventInfo))
            {
                (eventInfo as EventInfo<T1, T2, T3>).actions -= action;
            }
        }

        public void EventTrigger<T1, T2, T3>(string actionName, T1 t1, T2 t2, T3 t3)
        {
            if (eventDic.TryGetValue(actionName, out IEventInfo eventInfo))
            {
                (eventDic[actionName] as EventInfo<T1, T2, T3>).EventTrigger(t1, t2, t3);
            }
        }

        internal void EventTrigger<T>(string v, object loadingChange)
        {
            throw new NotImplementedException();
        }

        #endregion

        public override void Dispose()
        {
            if (IsDisposed) return;
            eventDic.Clear();
            base.Dispose();
        }
    }
}