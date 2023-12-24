using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static GameFramework.GameTool.GameTool;

namespace GameFramework.GameTool
{
    public class GameTool : EditorWindow
    {
        [MenuItem("GameTool/OpenGameToolWindow")]
        private static void OpenGameToolWindow()
        {
            GameTool window = EditorWindow.GetWindow<GameTool>();
            window.minSize = new Vector2(400f, 250f);
            window.maxSize = new Vector2(900f, 650f);
            window.Show();
        }

        private ToolType type;
        private ExcelTool excelTool;
        private ABTool abTool;
        private LuaTool luaTool;

        private void OnEnable()
        {
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            type = ToolType.ExcelTool;
            if (excelTool == null)
            {
                excelTool = new ExcelTool();
            }
            if (abTool == null)
            {
                abTool = new ABTool();
            }
            if (luaTool == null)
            {
                luaTool = new LuaTool();
            }
        }

        private void OnGUI()
        {
            TypeToggle();

            switch (type)
            {
                case ToolType.ExcelTool:

                    excelTool?.OnGUI();
                    break;

                case ToolType.ABTool:
                    abTool?.OnGUI();
                    break;

                case ToolType.LuaTool:
                    luaTool?.OnGUI();
                    break;
            }
        }

        private void TypeToggle()
        {
            // 绘制顶部工具选择页签
            GUILayout.BeginHorizontal();
            GUILayout.Space(30);
            float toolbarWidth = position.width - 15 * 4;
            string[] labels = new string[3] { "ExcelTool", "ABTool", "LuaTool" };
            type = (ToolType)GUILayout.Toolbar((int)type, labels, GUILayout.Width(toolbarWidth), GUILayout.Height(30f));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(20f);
        }

        /// <summary>
        /// 工具类型
        /// </summary>
        public enum ToolType
        {
            ExcelTool, ABTool, LuaTool,
        }
    }
}