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
        [MenuItem("GameTool/LuaTool")]
        public static void OpenLuaToolWindow()
        {
            LuaTool window = EditorWindow.GetWindowWithRect<LuaTool>(new Rect(0, 0, 300, 200));
            window.Show();
        }

        private void OnGUI()
        {
        }
    }
}