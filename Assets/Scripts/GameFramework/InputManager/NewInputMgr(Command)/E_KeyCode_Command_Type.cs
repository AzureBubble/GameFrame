/// <summary>
/// 按键的三种状态枚举
/// </summary>
public enum E_KeyCode_Command_Type
{
    /// <summary>
    /// 按键按下
    /// </summary>
    Down,

    /// <summary>
    /// 按键按住
    /// </summary>
    Stay,

    /// <summary>
    /// 按键抬起
    /// </summary>
    Up,
}

/// <summary>
/// 按键的三种状态枚举
/// </summary>
public enum E_HotKey_Command_Type
{
    /// <summary>
    /// 获取 HorizontalAxis -1 ~ 1
    /// </summary>
    HorizontalAxis,

    /// <summary>
    /// 获取 VerticalAxis -1 ~ 1
    /// </summary>
    VerticalAxis,

    /// <summary>
    /// 获取 HorizontalAxisRaw -1 0 1
    /// </summary>
    HorizontalAxisRaw,

    /// <summary>
    /// 获取 VerticalAxisRaw -1 0 1
    /// </summary>
    VerticalAxisRaw,
}