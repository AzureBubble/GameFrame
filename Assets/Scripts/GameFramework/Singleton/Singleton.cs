/// <summary>
/// 懒汉单例模式:第一次请求时 才会创建
/// </summary>
/// <typeparam name="T">必须有一个公共无参构造函数</typeparam>
public class Singleton<T> where T : new()
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }
}