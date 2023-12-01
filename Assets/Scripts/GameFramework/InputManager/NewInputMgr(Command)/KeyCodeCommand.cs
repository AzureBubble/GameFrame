using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// 键盘按键命令类
/// </summary>
public class KeyCodeCommand : ICommand
{
    protected E_KeyCode_Command_Type type; // 按键状态
    protected KeyCode keyCode; // 某个按键
    protected UnityAction action; // 按下触发事件

    public KeyCodeCommand(E_KeyCode_Command_Type type, KeyCode keyCode, UnityAction action)
    {
        this.type = type;
        this.keyCode = keyCode;
        this.action = action;
    }

    public virtual void Execute()
    {
        switch (type)
        {
            case E_KeyCode_Command_Type.Down:
                if (Input.GetKeyDown(keyCode))
                {
                    action?.Invoke();
                }
                break;

            case E_KeyCode_Command_Type.Stay:
                if (Input.GetKey(keyCode))
                {
                    action?.Invoke();
                }
                break;

            case E_KeyCode_Command_Type.Up:
                if (Input.GetKeyUp(keyCode))
                {
                    action?.Invoke();
                }
                break;
        }
    }
}