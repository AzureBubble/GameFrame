using UnityEngine;

[System.Serializable]
public class DialoguePiece
{
    /// <summary>
    /// 片段 id
    /// </summary>
    public int id;

    /// <summary>
    /// 片段 ID 用于寻找
    /// </summary>
    public string pieceId;

    /// <summary>
    /// 头像路径
    /// </summary>
    public string imgRes;

    /// <summary>
    /// 归属名字
    /// </summary>
    public string name;

    /// <summary>
    /// 片段内容
    /// </summary>
    [TextArea]
    public string text;

    /// <summary>
    /// 选项数组
    /// </summary>
    public DialogueOption[] options;
}