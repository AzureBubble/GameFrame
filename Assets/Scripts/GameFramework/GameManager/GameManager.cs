using SGM.MUIManager;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 游戏一开始就会创建的全局唯一的游戏管理器
/// 如有需要初始化的脚本、数据等，可以在这里进行初始化
/// </summary>
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    // 新输入系统的键盘映射
    public PlayerInputAction playerInputAction;

    /// <summary>
    /// 饿汉单例:游戏运行时加载程序唯一全局单例管理器
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitalizeGameManager()
    {
        GameObject obj = new GameObject("GameManager");
        instance = obj.AddComponent<GameManager>();
        DontDestroyOnLoad(obj);
        //UIManager.Instance.ShowPanel<LoginPanel>();
        //MusicMgr.Instance.PlayGameMusic("BGM");
        //MusicMgr.Instance.PlayAmbientMusic("a");
        //InputManager.Instance.PushStack();
    }

    private void Awake()
    {
        playerInputAction = new PlayerInputAction();
        // 从本地读取键盘设置
        string inputActionMapJson = PlayerPrefs.GetString("ActionMap", null);
        if (inputActionMapJson != null)
        {
            playerInputAction.asset.LoadBindingOverridesFromJson(inputActionMapJson);
        }
    }
}