namespace GameFramework.StateMachine
{
    /// <summary>
    /// 状态接口
    /// 如果不需要成员属性 成员变量 可使用接口
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// 状态进入
        /// </summary>
        void OnEnter();

        /// <summary>
        /// 状态更新
        /// </summary>
        void OnUpdate();

        /// <summary>
        /// 状态退出
        /// </summary>
        void OnExit();
    }
}