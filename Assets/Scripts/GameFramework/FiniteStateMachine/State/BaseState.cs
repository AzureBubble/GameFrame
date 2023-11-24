using UnityEngine;

namespace GameFramework.StateMachine
{
    /// <summary>
    /// 状态抽象基类
    /// 如果需要成员属性 成员变量 可使用状态基类
    /// </summary>
    public abstract class BaseState : IState
    {
        public readonly E_State_Type stateType; // 状态类型
        protected readonly string stateName; // 状态对应的 Animation 动画名称
        protected readonly float switchAnimationDuration = 0.1f; // 动画切换过渡时间
        protected int stateHash; // 状态对应的动画哈希值
        protected Animator animator; // Animator组件
        protected BaseFSM fsm; // 状态机
        protected float CurrentInfoTime => animator.GetCurrentAnimatorStateInfo(0).normalizedTime; // 当前动画状态的归一化时间

        protected float CurrentAnimationTime // 当前动画状态的播放时间
        {
            get
            {
                AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
                float animationLength = clipInfo.Length > 0 ? clipInfo[0].clip.length : 0f;
                return animationLength * CurrentInfoTime;
            }
        }

        protected bool IsAnimationFinished => CurrentAnimationTime > 0.95f; // 当前动画是否播放结束

        /// <summary>
        /// 初始化状态方法
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="fsm"></param>
        public void Init(Animator animator, BaseFSM fsm)
        {
            this.animator = animator;
            this.fsm = fsm;
            // 通过状态对应的动画名字获得其在动画系统中的哈希值
            stateHash = Animator.StringToHash(stateName);
        }

        public abstract void OnEnter();

        public abstract void OnExit();

        public abstract void OnUpdate();
    }
}