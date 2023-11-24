using UnityEngine;

/// <summary>
/// 继承 MonoBehaviour 的单例基类
/// </summary>
public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance => instance;

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            // 过场景不移除
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}