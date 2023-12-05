using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GameFramework.GFLuaTool
{
    /// <summary>
    /// Lua 工具类
    /// 用于自动生成 Lua .txt文件 方便AB包打包
    /// </summary>
    public class LuaTool : EditorWindow
    {
        private string luaDirName = "Lua"; // Lua原文件夹名
        private string luaNewDirName = "LuaTxt"; // Lua转存文件夹名
        private string abName = "lua"; // Lua的AB包名

        [MenuItem("GameTool/LuaTool")]
        public static void OpenLuaToolWindow()
        {
            LuaTool window = EditorWindow.GetWindowWithRect<LuaTool>(new Rect(0, 0, 250, 130));
            window.Show();
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 100, 20), "Lua原文件夹名");
            luaDirName = GUI.TextField(new Rect(120, 10, 100, 20), luaDirName);
            GUI.Label(new Rect(10, 35, 100, 20), "Lua转存文件夹名");
            luaNewDirName = GUI.TextField(new Rect(120, 35, 100, 20), luaNewDirName);
            GUI.Label(new Rect(10, 60, 100, 20), "Lua的AB包名");
            abName = GUI.TextField(new Rect(120, 60, 100, 20), abName);

            // 把 lua 文件增加 .txt后缀 并移动到指定路径
            if (GUI.Button(new Rect(10, 85, 210, 25), "Copy Lua To Txt"))
            {
                CopyLuaToTxt(luaDirName, luaNewDirName, abName);
            }
        }

        /// <summary>
        /// 把 lua 文件增加 .txt后缀 并移动到指定路径存放
        /// </summary>
        private void CopyLuaToTxt(string dirName, string newDirName, string abName)
        {
            // lua 文件的存放路径
            string path = Application.dataPath + $"/{dirName}/";

            if (!Directory.Exists(path))
            {
                return;
            }

            // 移动到的新存储路径
            string newPath = Application.dataPath + $"/{newDirName}/";
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            else
            {
                // 得到路径下的所有 .txt的文件
                string[] oldFileStrs = Directory.GetFiles(newPath, "*.txt");
                foreach (string file in oldFileStrs)
                {
                    // 删除文件
                    File.Delete(file);
                }
            }

            // 找到原路径下所有后缀为.lua的文件
            string[] strs = Directory.GetFiles(path, "*.lua");
            List<string> newFileNames = new List<string>();
            string fileName = null;
            foreach (string file in strs)
            {
                // 拼接文件新的路径且加上.txt后缀
                fileName = newPath + file.Substring(file.LastIndexOf("/") + 1) + ".txt";
                newFileNames.Add(fileName);
                File.Copy(file, fileName);
            }

            AssetDatabase.Refresh();

            // 编辑器界面后 再修改文件的AB包路径
            foreach (string newFileName in newFileNames)
            {
                // 这个API传入的路径必须是 相对于Assets文件夹 Assets/.../...
                AssetImporter import = AssetImporter.GetAtPath(newFileName.Substring(newFileName.IndexOf("Asset")));
                if (import != null)
                {
                    // 修改文件的AB包
                    import.assetBundleName = abName;
                }
            }

            Debug.Log("Lua文件转存成功");
        }
    }
}