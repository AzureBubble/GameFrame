using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace SGM.XMLManager
{
    public class XmlDataMgr : Singleton<XmlDataMgr>
    {
        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="fileName">文件名</param>
        public void SaveData(object data, string fileName)
        {
            // 存储路径
            string path = Application.persistentDataPath + "/" + fileName + ".xml";

            Debug.Log(Application.persistentDataPath);

            using (StreamWriter writer = new StreamWriter(path))
            {
                // xml翻译器
                XmlSerializer s = new XmlSerializer(data.GetType());
                // 序列化数据
                s.Serialize(writer, data);
            }
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public object LoadData(Type type, string fileName)
        {
            // 判断文件是否存在
            string path = Application.persistentDataPath + "/" + fileName + ".xml";
            if (!File.Exists(path))
            {
                path = Application.streamingAssetsPath + "/" + fileName + ".xml";

                if (!File.Exists(path))
                {
                    // 不存在文件，则反射生成一个空对象返回
                    return Activator.CreateInstance(type);
                }
            }

            // 如果存在文件
            using (StreamReader reader = new StreamReader(path))
            {
                // xml翻译器
                XmlSerializer s = new XmlSerializer(type);
                // 返回一个反序列化对象
                return s.Deserialize(reader);
            }
        }
    }
}