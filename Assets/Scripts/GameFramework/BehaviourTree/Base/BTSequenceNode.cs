/// <summary>
/// 1.顺序器(And-&):按照设计顺序执行子节点行为直到所有子节点全部完成或者到某一个失败为止
/// </summary>
public class BTSequenceNode : BTControlNode
{
    public BTSequenceNode()
    {
        onInitialize += Initialize;
        onUpdate += Update;
    }

    private void Initialize()
    {
    }

    private E_BT_StateType Update()
    {
        E_BT_StateType nowState = E_BT_StateType.Waiting;
        if (nowIndex < childs.Count)
        {
            nowState = childs[nowIndex].Tick();
            switch (nowState)
            {
                case E_BT_StateType.Success:
                    {
                        ++nowIndex;
                        if (nowIndex == childs.Count)
                        {
                            nowIndex = 0;
                            return E_BT_StateType.Success;
                        }
                        break;
                    }

                case E_BT_StateType.Failure:
                    nowIndex = 0;
                    return E_BT_StateType.Failure;

                default:
                    return nowState;
            }
        }

        return E_BT_StateType.Success;
    }
}