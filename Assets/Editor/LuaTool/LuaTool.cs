using UnityEditor;
using UnityEngine;

namespace GameFramework.GFLuaTool
{
    /// <summary>
    /// Lua ������
    /// �����Զ����� Lua .txt�ļ� ����AB�����
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