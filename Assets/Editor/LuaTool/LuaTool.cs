using System.Collections.Generic;
using System.IO;
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
        private SerializedObject serializedObject;
        private SerializedProperty luaDirNameProperty; // Luaԭ�ļ�����
        private SerializedProperty luaNewDirNameProperty; // Luaת���ļ�����
        private SerializedProperty abNameProperty; // Lua��AB����

        //private string luaDirName = "Lua"; // Luaԭ�ļ�����
        //private string luaNewDirName = "LuaTxt"; // Luaת���ļ�����
        //private string abName = "lua"; // Lua��AB����

        [MenuItem("GameTool/LuaTool")]
        public static void OpenLuaToolWindow()
        {
            LuaTool window = EditorWindow.GetWindowWithRect<LuaTool>(new Rect(0, 0, 250, 130));
            window.autoRepaintOnSceneChange = true;
            window.Show();
        }

        private void Init()
        {
            serializedObject?.Dispose();
            serializedObject = new SerializedObject(LuaToolScriptableObject.Instance);
            luaDirNameProperty = serializedObject.FindProperty("luaDirName");
            luaNewDirNameProperty = serializedObject.FindProperty("luaNewDirName");
            abNameProperty = serializedObject.FindProperty("abName");
        }

        private void OnGUI()
        {
            if (serializedObject == null || !serializedObject.targetObject)
            {
                Init();
            }

            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            GUI.Label(new Rect(10, 10, 100, 20), "Luaԭ�ļ�����");
            luaDirNameProperty.stringValue = GUI.TextField(new Rect(120, 10, 100, 20), luaDirNameProperty.stringValue);
            GUI.Label(new Rect(10, 35, 100, 20), "Luaת���ļ�����");
            luaNewDirNameProperty.stringValue = GUI.TextField(new Rect(120, 35, 100, 20), luaNewDirNameProperty.stringValue);
            GUI.Label(new Rect(10, 60, 100, 20), "Lua��AB����");
            abNameProperty.stringValue = GUI.TextField(new Rect(120, 60, 100, 20), abNameProperty.stringValue);

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                LuaToolScriptableObject.Save();
            }

            // �� lua �ļ����� .txt��׺ ���ƶ���ָ��·��
            if (GUI.Button(new Rect(10, 85, 210, 25), "Copy Lua To Txt"))
            {
                CopyLuaToTxt(luaDirNameProperty.stringValue, luaNewDirNameProperty.stringValue, abNameProperty.stringValue);
            }
        }

        /// <summary>
        /// �� lua �ļ����� .txt��׺ ���ƶ���ָ��·�����
        /// </summary>
        private void CopyLuaToTxt(string dirName, string newDirName, string abName)
        {
            // lua �ļ��Ĵ��·��
            string path = Application.dataPath + $"/{dirName}/";

            if (!Directory.Exists(path))
            {
                return;
            }

            // �ƶ������´洢·��
            string newPath = Application.dataPath + $"/{newDirName}/";
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            else
            {
                // �õ�·���µ����� .txt���ļ�
                string[] oldFileStrs = Directory.GetFiles(newPath, "*.txt");
                foreach (string file in oldFileStrs)
                {
                    // ɾ���ļ�
                    File.Delete(file);
                }
            }

            // �ҵ�ԭ·�������к�׺Ϊ.lua���ļ�
            string[] strs = Directory.GetFiles(path, "*.lua");
            List<string> newFileNames = new List<string>();
            string fileName = null;
            foreach (string file in strs)
            {
                // ƴ���ļ��µ�·���Ҽ���.txt��׺
                fileName = newPath + file.Substring(file.LastIndexOf("/") + 1) + ".txt";
                newFileNames.Add(fileName);
                File.Copy(file, fileName);
            }

            AssetDatabase.Refresh();

            // �༭������� ���޸��ļ���AB��·��
            foreach (string newFileName in newFileNames)
            {
                // ���API�����·�������� �����Assets�ļ��� Assets/.../...
                AssetImporter import = AssetImporter.GetAtPath(newFileName.Substring(newFileName.IndexOf("Asset")));
                if (import != null)
                {
                    // �޸��ļ���AB��
                    import.assetBundleName = abName;
                }
            }

            Debug.Log("Lua�ļ�ת��ɹ�");
        }
    }
}