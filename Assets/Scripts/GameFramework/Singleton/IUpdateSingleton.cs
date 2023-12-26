/// <summary>
/// 需要帧更新的单例
/// </summary>
public interface IUpdateSingleton : ISingleton
{
    /// <summary>
    /// 帧更新方法
    /// </summary>
    abstract void OnUpdate();
}