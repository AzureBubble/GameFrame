using GameFramework.MonoManager;
using System.Collections.Generic;

namespace GameFramework.GFInputManager
{
    /// <summary>
    /// 命令模式的输入管理器
    /// </summary>
    public class InputMgr
    {
        private bool canInput; // 是否可输入
        private List<ICommand> commands; // 命令存储器

        public InputMgr()
        {
            MonoMgr.Instance.AddUpdateListener(Update);
            commands = new List<ICommand>();
        }

        private void Update()
        {
            if (!canInput) return;

            foreach (ICommand command in commands)
            {
                command?.Execute();
            }
        }

        /// <summary>
        /// 注册命令
        /// </summary>
        /// <param name="command"></param>
        public void RegisterCommand(ICommand command)
        {
            commands.Add(command);
        }

        /// <summary>
        /// 取消注册命令
        /// </summary>
        /// <param name="command"></param>
        public void RemoveCommand(ICommand command)
        {
            if (commands.Contains(command))
            {
                commands.Remove(command);
            }
        }

        /// <summary>
        /// 启动键盘监听
        /// </summary>
        public void Enable()
        {
            canInput = true;
        }

        /// <summary>
        /// 取消键盘监听
        /// </summary>
        public void Disable()
        {
            canInput = false;
        }

        /// <summary>
        /// 清空容器中的命令
        /// </summary>
        public void Clear()
        {
            commands.Clear();
            canInput = false;
        }
    }
}