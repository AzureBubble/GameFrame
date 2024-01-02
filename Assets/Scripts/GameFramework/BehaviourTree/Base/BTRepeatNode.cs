using UnityEngine;

public class BTRepeatNode : BTDecoratorNode
{
    [Tooltip("重复次数")] public int limit;
    [Tooltip("计数器")] public int counter;

    public BTRepeatNode()
    {
        onInitialize += Initialize;
        onUpdate += Update;
    }

    private void Initialize()
    {
        counter = 0;
    }

    private E_BT_StateType Update()
    {
        for (; ; )
        {
            child.Tick();
            if (child.GetStatus() == E_BT_StateType.Running)
                break;
            if (child.GetStatus() == E_BT_StateType.Failure)
                return E_BT_StateType.Failure;
            if (++counter == limit)
                return E_BT_StateType.Success;
            child.ResetState();
        }
        return E_BT_StateType.Waiting;
    }
}