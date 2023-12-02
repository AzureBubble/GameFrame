using UnityEngine;
using UnityEngine.Events;

namespace GameFramework.GFInputManager
{
    /// <summary>
    /// Unity热键监测输入类
    /// </summary>
    public class HotKeyCommand : ICommand
    {
        protected E_KeyCode_Command_Type type; // 热键输入类型：Axis/AxisRaw
        /// <summary>
        /// 热键名字
        /// Horizontal->左右箭头/AD键 Vertical->上下箭头/WS键 Mouse X/Y->鼠标水平/垂直方向
        /// </summary>
        protected string keyName;
        protected UnityAction<float> action; // 热键输入的回调函数

        public HotKeyCommand(E_KeyCode_Command_Type type, string keyName, UnityAction<float> action)
        {
            this.type = type;
            this.keyName = keyName;
            this.action = action;
        }

        public void Execute()
        {
            switch (type)
            {
                case E_KeyCode_Command_Type.Axis:
                    action?.Invoke(Input.GetAxis(keyName));
                    break;

                case E_KeyCode_Command_Type.AxisRaw:
                    action?.Invoke(Input.GetAxisRaw(keyName));
                    break;
            }
        }
    }
}