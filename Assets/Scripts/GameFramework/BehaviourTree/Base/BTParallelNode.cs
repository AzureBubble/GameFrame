public class BTParallelNode : BTControlNode
{
    public int successPolicyCount;
    public int failurePolicyCount;
    protected int successCounter;
    protected int failureCounter;

    public BTParallelNode(int SuccessPolicyCount, int FailurePolicyCount)
    {
        failurePolicyCount = FailurePolicyCount;
        successPolicyCount = SuccessPolicyCount;

        onUpdate += Update;
        onTerminate += Terminate;
    }

    public virtual E_BT_StateType Update()
    {
        for (int i = 0; i < childs.Count; i++)
        {
            if (!childs[i].IsTerminated())
            {
                childs[i].Tick();
            }

            //优先处理失败比较保险
            if (childs[i].GetStatus() == E_BT_StateType.Failure)
            {
                ++failureCounter;
                if (failureCounter >= failurePolicyCount)
                {
                    return E_BT_StateType.Failure;
                }
            }

            if (childs[i].GetStatus() == E_BT_StateType.Success)
            {
                ++successCounter;
                if (successCounter >= successPolicyCount)
                {
                    return E_BT_StateType.Success;
                }
            }
        }

        return E_BT_StateType.Running;
    }

    public virtual void Terminate(E_BT_StateType status) //终止所有运行中的子节点
    {
        for (int i = 0; i < childs.Count; i++)
        {
            BTBaseNode node = childs[i];
            if (node.IsRunning())
            {
                node.Abort();
            }
        }
    }
}