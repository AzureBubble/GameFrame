using System;
using UnityEngine;

public class LogActionNode : BTActionNode
{
    public string logMessage;

    public LogActionNode()
    {
        onInitialize += OnInitialize;
        onUpdate += OnUpdate;
    }

    private void OnInitialize()
    {
    }

    private E_BT_StateType OnUpdate()
    {
        Debug.Log(logMessage);
        return E_BT_StateType.Success;
    }
}