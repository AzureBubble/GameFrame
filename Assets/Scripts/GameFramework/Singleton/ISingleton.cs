using System;

/// <summary>
/// ��ͨ����
/// </summary>
public interface ISingleton : IDisposable
{
    /// <summary>
    ///  ��ǵ����Ƿ�����
    /// </summary>
    public bool IsDisposed { get; set; }

    /// <summary>
    /// ������ʼ������
    /// </summary>
    public void Initialize();
}