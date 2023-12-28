using UnityEngine;

public abstract class BaseNode : ScriptableObject
{
    public E_NodeState state = E_NodeState.Waiting;// 结点初始状态为等待执行
    [TextArea] public string description;
    [HideInInspector] public bool isExcute = false; // 是否执行
    [HideInInspector] public string guid; // 唯一标识
    [HideInInspector] public Vector2 position; // 结点位置坐标
    public Sprite headIcon; // 头像
    public string speakerName; // 名字

    public BaseNode OnUpdate()
    {
        if (!isExcute)
        {
            OnEnter();
            isExcute = true;
        }
        BaseNode node = Execute();
        if (state != E_NodeState.Running)
        {
            OnExit();
            isExcute = false;
        }
        return node;
    }

    /// <summary>
    /// 结点进入
    /// </summary>
    public abstract void OnEnter();

    /// <summary>
    /// 执行逻辑的方法
    /// </summary>
    /// <returns></returns>
    public abstract BaseNode Execute();

    /// <summary>
    /// 结点退出
    /// </summary>
    public abstract void OnExit();
}