using UnityEngine.Events;
using UnityEngine;

namespace GameFramework.GFInputManager
{
    /// <summary>
    /// 鼠标按键命令类
    /// </summary>
    public class MouseButtonCommand : ICommand
    {
        protected E_KeyCode_Command_Type type; // 按键状态
        /// <summary>
        /// 0->鼠标左键 1->鼠标右键 2->鼠标中键/滚轮
        /// </summary>
        protected int mouseButton;
        protected UnityAction action; // 按下触发事件

        public MouseButtonCommand(E_KeyCode_Command_Type type, int mouseButton, UnityAction action)
        {
            this.type = type;
            this.mouseButton = mouseButton;
            this.action = action;
        }

        public virtual void Execute()
        {
            switch (type)
            {
                case E_KeyCode_Command_Type.Down:
                    if (Input.GetMouseButtonDown(mouseButton))
                    {
                        action?.Invoke();
                    }
                    break;

                case E_KeyCode_Command_Type.Stay:
                    if (Input.GetMouseButton(mouseButton))
                    {
                        action?.Invoke();
                    }
                    break;

                case E_KeyCode_Command_Type.Up:
                    if (Input.GetMouseButtonUp(mouseButton))
                    {
                        action?.Invoke();
                    }
                    break;
            }
        }
    }
}