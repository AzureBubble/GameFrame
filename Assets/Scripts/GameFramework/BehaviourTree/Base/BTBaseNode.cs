using System;
using UnityEngine;

public abstract class BTBaseNode : ScriptableObject
{
    // 结点持有者
    public GameObject Owner { get; private set; }

    public BehaviourTree OwnerTree { get; private set; }

    [ReadOnly] public string guid;
    [ReadOnly] public Vector2 position;
    [ReadOnly, Tooltip("结点状态")] public E_BT_StateType state;
    [ReadOnly] public bool isRootNode;

    [Tooltip("结点描述信息")] public string description;

    //行为事件
    public event Action onInitialize; // 结点初始化事件

    public event Func<E_BT_StateType> onUpdate; // 结点帧更新事件

    public event Action<E_BT_StateType> onTerminate; // 结点结束事件

    /// <summary>
    /// 结点创建时候 进行结点初始化
    /// </summary>
    /// <param name="owner"></param>
    public void OnCreate(GameObject owner, BehaviourTree tree)
    {
        Owner = owner;
        OwnerTree = tree;
        OnStart();
    }

    /// <summary>
    /// 创建结点时候的初始化
    /// </summary>
    public virtual void OnStart()
    {
    }

    public void AddEvent(Action Initialize, Func<E_BT_StateType> Update, Action<E_BT_StateType> Terminate)
    {
        if (Initialize != null)
            onInitialize += Initialize;
        if (Update != null)
            onUpdate += Update;
        if (Terminate != null)
            onTerminate += Terminate;
    }

    public E_BT_StateType Tick()
    {
        // 结点状态不为运行状态 且初始化函数不为空时候执行初始化函数
        if (state != E_BT_StateType.Running && onInitialize != null)
            onInitialize();

        // 结点帧更新事件
        state = onUpdate();

        // 结点状态不为运行状态 且结束函数不为空时候执行结束函数
        if (state != E_BT_StateType.Running && onTerminate != null)
            onTerminate(state);

        return state;
    }

    /// <summary>
    /// 重置结点状态为等待
    /// </summary>
    public virtual void ResetState()
    {
        state = E_BT_StateType.Waiting;
    }

    /// <summary>
    /// 终止执行结点
    /// </summary>
    public void Abort()
    {
        onTerminate(E_BT_StateType.Abort);
        state = E_BT_StateType.Abort;
    }

    /// <summary>
    /// 结点状态是否处于已经终止
    /// </summary>
    /// <returns></returns>
    public bool IsTerminated()
    {
        return state == E_BT_StateType.Success || state == E_BT_StateType.Failure;
    }

    /// <summary>
    /// 是否在运行
    /// </summary>
    /// <returns></returns>
    public bool IsRunning()
    {
        return state == E_BT_StateType.Running;
    }

    /// <summary>
    /// 得到当前结点的运行状态
    /// </summary>
    /// <returns></returns>
    public E_BT_StateType GetStatus()
    {
        return state;
    }
}