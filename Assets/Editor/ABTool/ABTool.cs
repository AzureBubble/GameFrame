using GameFramework.NetManager;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ABTool : EditorWindow
{
    private string serverIP = "ftp://192.168.168.128/"; // 远程服务器地址
    private string userName = "root"; // 用户名
    private string password = "root"; // 密码

    private int nowSelectedIndex = 0;
    private string[] targetStrs = new string[] { "PC", "IOS", "Android" };

    [MenuItem("GameTool/ABTool")]
    private static void OpenABToolWindow()
    {
        ABTool window = EditorWindow.GetWindowWithRect<ABTool>(new Rect(0, 0, 320, 220));
        window.autoRepaintOnSceneChange = true;
        window.Show();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 25), "平台选择");
        nowSelectedIndex = GUI.Toolbar(new Rect(80, 10, 230, 25), nowSelectedIndex, targetStrs);

        GUI.Label(new Rect(10, 40, 100, 20), "资源服务器IP地址");
        serverIP = GUI.TextField(new Rect(110, 40, 200, 20), serverIP);

        GUI.Label(new Rect(10, 70, 50, 20), "用户名");
        userName = GUI.TextField(new Rect(60, 70, 100, 20), userName);
        GUI.Label(new Rect(170, 70, 50, 20), "密码");
        password = GUI.TextField(new Rect(210, 70, 100, 20), password);

        if (GUI.Button(new Rect(10, 100, 100, 50), "创建对比文件"))
        {
            GenerateABCompareFile();
        }

        if (GUI.Button(new Rect(110, 100, 200, 50), "保存默认资源到SteamingAssets"))
        {
            MoveABFileToStreamingAssets();
        }

        if (GUI.Button(new Rect(10, 160, 300, 50), "上传AB包和资源对比文件"))
        {
            UpLoadAllABFile();
        }
    }

    /// <summary>
    /// 移动默认资源到StreamingAssets
    /// </summary>
    private void MoveABFileToStreamingAssets()
    {
        string path = Application.streamingAssetsPath + "/AB/" + targetStrs[nowSelectedIndex] + "/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        // 获取选中的资源
        UnityEngine.Object[] selectedAsset = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        if (selectedAsset.Length == 0) return;
        // ab包对比文件字符串
        string abCompareInfo = "";
        foreach (UnityEngine.Object asset in selectedAsset)
        {
            // 获取资源路径
            string assetPath = AssetDatabase.GetAssetPath(asset);
            // 通过路径截取文件名
            string fileName = assetPath.Substring(assetPath.LastIndexOf('/') + 1);
            // 判断是否是ab包 才进行处理
            if (fileName.IndexOf(".") != -1) continue;
            // 拷贝到streamingAssetsPath路径
            AssetDatabase.CopyAsset(assetPath, "Assets/StreamingAssets/AB/" + targetStrs[nowSelectedIndex] + "/" + fileName);
            FileInfo fileInfo = new FileInfo(path + fileName);
            // 拼接字符串
            abCompareInfo += fileInfo.Name + " " + fileInfo.Length + " " + GetMD5(fileInfo.FullName);
            abCompareInfo += "|";
        }
        // 保存到streamingAssetsPath路径
        abCompareInfo = abCompareInfo.Substring(0, abCompareInfo.Length - 1);
        File.WriteAllText(path + "ABCompareInfo.txt", abCompareInfo);
        AssetDatabase.Refresh();
        Debug.Log("已经把AB包拷贝到streamingAssets文件夹");
    }

    /// <summary>
    /// 上传AB包和对比文件到服务器
    /// </summary>
    private void UpLoadAllABFile()
    {
        string serverPath = serverIP + "AB/" + targetStrs[nowSelectedIndex] + "/";
        // 获取文件夹信息
        DirectoryInfo dInfo = Directory.CreateDirectory(Application.dataPath + "/ArtRes/AB/" + targetStrs[nowSelectedIndex] + "/");
        // 获取文件夹中所有的文件信息
        FileInfo[] files = dInfo.GetFiles();

        // 只对AB包和对比文件进行处理
        foreach (FileInfo file in files)
        {
            if (file.Name == "ABCompareInfo.txt" || file.Extension == "")
            {
                FtpManager.Instance.UpLoadFileAsync(file.Name, file.FullName, serverPath, userName, password);
            }
        }
    }

    /// <summary>
    /// 生成AB包对比文件
    /// </summary>
    private void GenerateABCompareFile()
    {
        // 获取文件夹信息
        DirectoryInfo dInfo = Directory.CreateDirectory(Application.dataPath + "/ArtRes/AB/" + targetStrs[nowSelectedIndex] + "/");
        // 获取文件夹中所有的文件信息
        FileInfo[] files = dInfo.GetFiles();
        // 遍历所有的文件，只对AB包文件进行处理
        string abCompareInfo = "";
        foreach (FileInfo file in files)
        {
            if (file.Extension == "")
            {
                // 拼接对比文件信息
                abCompareInfo += file.Name + " " + file.Length + " " + GetMD5(file.FullName);
                abCompareInfo += "|";
            }
        }
        // 删除最后一个 | 分隔符
        abCompareInfo = abCompareInfo.Substring(0, abCompareInfo.Length - 1);
        // 把对比文件写入本地
        File.WriteAllText(Application.dataPath + "/ArtRes/AB/" + targetStrs[nowSelectedIndex] + "/ABCompareInfo.txt", abCompareInfo);
        // 刷新编辑器
        AssetDatabase.Refresh();
        Debug.Log("对比文件信息生成完毕");
    }

    /// <summary>
    /// 获取文件的MD5码
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns></returns>
    private string GetMD5(string filePath)
    {
        // 打开文件
        using (FileStream fs = new FileStream(filePath, FileMode.Open))
        {
            // new 一个MD5对象
            MD5 md5 = new MD5CryptoServiceProvider();
            // 得到文件的MD5码 16个字节的数组
            byte[] bytes = md5.ComputeHash(fs);
            fs.Close();

            // 把MD5码的16个字节数组 转成16进制拼接成字符串存储
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }
}