using UnityEngine;
using UnityEngine.Events;

namespace GameFramework.GFInputManager
{
    /// <summary>
    /// Unity热键监测输入类
    /// </summary>
    public class HotKeyCommand : ICommand
    {
        protected E_HotKey_Command_Type type;
        protected string keyName;
        protected UnityAction<float> action;

        public HotKeyCommand(E_HotKey_Command_Type type, string keyName, UnityAction<float> action)
        {
            this.type = type;
            this.keyName = keyName;
            this.action = action;
        }

        public void Execute()
        {
            switch (type)
            {
                case E_HotKey_Command_Type.Axis:
                    action?.Invoke(Input.GetAxis(keyName));
                    break;

                case E_HotKey_Command_Type.AxisRaw:
                    action?.Invoke(Input.GetAxisRaw(keyName));
                    break;
            }
        }
    }
}