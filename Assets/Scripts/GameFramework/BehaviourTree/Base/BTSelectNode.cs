/// <summary>
/// 2.选择器(Or-|):依次执行每个子行为直到某个子节点已经成功执行或返回RUNNING状态
/// </summary>
public class BTSelectNode : BTControlNode
{
    public BTSelectNode()
    {
        onInitialize += Initialize;
        onUpdate += Update;
    }

    public virtual void Initialize()
    {
    }

    public E_BT_StateType Update()
    {
        var nowState = E_BT_StateType.Waiting;
        // 遍历子结点，以此执行子结点逻辑
        if (nowIndex < childs.Count)
        {
            nowState = childs[nowIndex].Tick();
            switch (nowState)
            {
                case E_BT_StateType.Success:
                    nowIndex = 0;
                    return E_BT_StateType.Success;

                case E_BT_StateType.Failure:
                    {
                        ++nowIndex;
                        if (nowIndex == childs.Count)
                        {
                            nowIndex = 0;
                            return E_BT_StateType.Failure;
                        }
                        break;
                    }

                default:
                    return nowState;
            }
        }

        return E_BT_StateType.Failure;
    }
}