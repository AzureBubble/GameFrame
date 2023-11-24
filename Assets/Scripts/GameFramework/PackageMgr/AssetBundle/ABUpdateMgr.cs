using SGM.MEventCenter;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

/// <summary>
/// AB�����¹�����
/// </summary>
public class ABUpdateMgr : MonoBehaviour
{
    private readonly string USER_NAME = "root"; // �û���
    private readonly string PASSWORD = "root"; //����
    private readonly string SERVER_IP = "ftp://192.168.168.128/AB/"; // �����IP��ַ

    private static ABUpdateMgr instance;

    public static ABUpdateMgr Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("ABUpdateMgr");
                instance = obj.AddComponent<ABUpdateMgr>();
            }
            return instance;
        }
    }

    private string localPath; // ���ش��·��

    private string TargetPlatform // Ŀ��ƽ̨
    {
        get
        {
#if UNITY_ANDROID
            return "Android";
#elif UNITY_IOS
            return "IOS";
#else
            return "PC";
#endif
        }
    }

    // �洢Զ��AB����Ϣ�ֵ� �����뱾��AB�����жԱȸ���
    private Dictionary<string, ABInfo> remoteABDic;

    // �洢����AB����Ϣ�ֵ� ������Զ��AB����Ϣ�Ա�
    private Dictionary<string, ABInfo> localABDic;

    // �洢���ӷ�������ص�AB����
    private List<string> downLoadABList;

    private void Awake()
    {
        // ��ʼ�����·��
        localPath = Application.persistentDataPath + "/AB/";
        remoteABDic = new Dictionary<string, ABInfo>();
        localABDic = new Dictionary<string, ABInfo>();
        downLoadABList = new List<string>();
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public void CheckUpdateABFile()
    {
        // �������
        remoteABDic.Clear();
        localABDic.Clear();
        downLoadABList.Clear();

        // ����Զ��AB���Ա��ļ�
        EventCenter.Instance.EventTrigger<string>("DownLoadABCompare", "��ʼ����AB���Ա��ļ�");
        DownLoadABCompareFile((isDownLoadABCompareFileOver) =>
        {
            // Զ�˶Ա��ļ����سɹ��� ��������ִ��
            if (isDownLoadABCompareFileOver)
            {
                EventCenter.Instance.EventTrigger<string>("DownLoadABCompare", "����AB���Ա��ļ����,��ʼ����");
                // ����Զ��AB���Ա��ļ�
                // ��ȡ���ص�AB���Ա��ļ�
                string remoteABCompareInfo = File.ReadAllText(localPath + "ABCompareInfo_TMP.txt");
                AnalysisABCompareFileInfo(remoteABCompareInfo, remoteABDic);
                EventCenter.Instance.EventTrigger<string>("DownLoadABCompare", "����Զ�˶Ա��ļ����");

                EventCenter.Instance.EventTrigger<string>("DownLoadABCompare", "��ʼ�������ضԱ��ļ�");

                // �첽Э�̽�������AB���Ա��ļ�
                AnalysisLocalABCompareFileInfoAsync((isAnalysisLocalOver) =>
                {
                    if (isAnalysisLocalOver)
                    {
                        long downLoadSize = 0;
                        EventCenter.Instance.EventTrigger<string>("DownLoadABCompare", "��ʼ�Ա�AB���ļ�");
                        foreach (string abName in remoteABDic.Keys)
                        {
                            // ������ز�����AB���������������б���
                            if (!localABDic.ContainsKey(abName))
                            {
                                downLoadABList.Add(abName);
                                downLoadSize += remoteABDic[abName].size;
                            }
                            else
                            {
                                // ���ڣ����ж�����AB����MD5���Ƿ�һ������һ����������°�
                                if (!localABDic[abName].Equals(remoteABDic[abName]))
                                {
                                    // ����������б�
                                    downLoadABList.Add(abName);
                                    downLoadSize += remoteABDic[abName].size;
                                }
                                // ���������ذ��� �ӱ����ֵ����Ƴ� ����ֵ���ʣ�µİ���Ϊ����
                                localABDic.Remove(abName);
                            }
                        }
                        // ����Ҫ���µ�AB���ļ���Сͨ���¼����Ĵ����ⲿ
                        EventCenter.Instance.EventTrigger<long>("DownLoadABFileSize", downLoadSize);
                        EventCenter.Instance.EventTrigger<string>("DownLoadABCompare", "�Ա�AB���ļ����");
                        EventCenter.Instance.EventTrigger<string>("DownLoadABCompare", "ɾ������AB�����ͷſռ�");
                        // ɾ�������AB���ļ����ͷ��ڴ�ռ�
                        foreach (string abName in localABDic.Keys)
                        {
                            if (File.Exists(localPath + abName))
                            {
                                File.Delete(localPath + abName);
                            }
                        }
                        EventCenter.Instance.EventTrigger<string>("DownLoadABCompare", "��ʼ���ظ���AB���ļ�");
                        // ���ظ���AB���ļ�
                        DownLoadABFile((isDownLoadAbFileOver) =>
                        {
                            if (isDownLoadAbFileOver)
                            {
                                EventCenter.Instance.EventTrigger<string>("DownLoadABCompare", "���±���AB���Ա��ļ�");

                                // ������AB���Ա��ļ�д�뱾��
                                File.WriteAllText(localPath + "ABCompareInfo.txt", remoteABCompareInfo);
                            }
                            // ֪ͨ�ⲿAB������״̬
                            EventCenter.Instance.EventTrigger<bool>("UpdateStatus", isDownLoadAbFileOver);
                        });
                    }
                    else
                    {
                        // ��������AB���ļ�ʧ��
                        EventCenter.Instance.EventTrigger<bool>("UpdateStatus", false);
                    }
                });
            }
            else
            {
                // Զ��AB���Ա��ļ�����ʧ��
                EventCenter.Instance.EventTrigger<bool>("UpdateStatus", false);
            }
        });
    }

    /// <summary>
    /// ���ظ���AB���ļ�
    /// </summary>
    /// <param name="callback">�ص�����</param>
    private async void DownLoadABFile(UnityAction<bool> callback)
    {
        // �ļ����·��
        string path = Application.persistentDataPath + "/AB/";
        // �����IP��ַ
        string serverIP = SERVER_IP + TargetPlatform + "/";
        // �洢���سɹ���AB���б�
        List<string> tempList = new List<string>();

        // ��ʼ���ظ���AB���ļ�
        foreach (string abName in downLoadABList)
        {
            await FtpManager.Instance.DownLoadFileAsync(abName, path + abName, serverIP, USER_NAME, PASSWORD, (result) =>
            {
                // ���سɹ� �ż���ɹ��б���
                if (result)
                {
                    tempList.Add(abName);
                }
            });
        }

        // ѭ��ɾ���Ѿ����ص�AB���ļ�
        foreach (string abName in tempList)
        {
            downLoadABList.Remove(abName);
        }

        callback(downLoadABList.Count == 0);
    }

    /// <summary>
    /// ����Զ��AB���Ա��ļ�����ȡԶ��AB����Ϣ
    /// </summary>
    private void AnalysisABCompareFileInfo(string compareInfo, Dictionary<string, ABInfo> abInfoDic)
    {
        string[] strs = compareInfo.Split("|");
        string[] infos = null;
        foreach (string str in strs)
        {
            infos = str.Split(" ");
            // ��Զ��AB����Ϣ�洢���ֵ���
            abInfoDic.Add(infos[0], new ABInfo(infos[0], infos[1], infos[2]));
        }
    }

    /// <summary>
    /// �첽Э�̽�������AB���Ա��ļ�
    /// </summary>
    /// <param name="callback">������ɻص�����</param>
    private void AnalysisLocalABCompareFileInfoAsync(UnityAction<bool> callback)
    {
        string path =
#if UNITY_EDITOR
                localPath + "ABCompareInfo.txt";
#else
                "file:///" + localPath + "/ABCompareInfo.txt";
#endif

        if (!File.Exists(path))
        {
            path =
#if UNITY_ANDROID
                Application.streamingAssetsPath+"/AB/"+TargetPlatform+"/ABCompareInfo.txt";
#elif UNITY_EDITOR
                Application.streamingAssetsPath + "/AB/" + TargetPlatform + "/ABCompareInfo.txt";
#else
                "file:///" + Application.streamingAssetsPath + "/AB/" + TargetPlatform + "/ABCompareInfo.txt";
#endif
            if (!File.Exists(path))
            {
                callback?.Invoke(true);
            }
        }

        StartCoroutine(AnalysisLocalABCompareFileInfoAsync(path, callback));
    }

    /// <summary>
    /// �첽��������AB���Ա��ļ���Э��
    /// </summary>
    /// <param name="callback">�ص�����</param>
    /// <returns></returns>
    private IEnumerator AnalysisLocalABCompareFileInfoAsync(string filePath, UnityAction<bool> callback)
    {
        // ���ر����ļ�
        UnityWebRequest req = UnityWebRequest.Get(filePath);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            // ��������AB���Ա��ļ�
            AnalysisABCompareFileInfo(req.downloadHandler.text, localABDic);
            // �����ɹ�
            callback?.Invoke(true);
            EventCenter.Instance.EventTrigger<string>("DownLoadABCompare", "�������ضԱ��ļ��ɹ�");
        }
        else
        {
            // ����ʧ��
            callback?.Invoke(false);
            EventCenter.Instance.EventTrigger<string>("DownLoadABCompare", "�������ضԱ��ļ�ʧ��");
        }
    }

    /// <summary>
    /// �ӷ��������AB���Ա��ļ�
    /// </summary>
    /// <param name="callback">������ɻص�����</param>
    public async void DownLoadABCompareFile(UnityAction<bool> callback)
    {
        // �жϱ����ļ����Ƿ���ڣ��������򴴽�
        if (!Directory.Exists(localPath))
        {
            Directory.CreateDirectory(localPath);
        }
        print(Application.persistentDataPath);

        // �ļ����·��
        string path = localPath + "ABCompareInfo_TMP.txt";
        // �����IP��ַ
        string serverIP = SERVER_IP + TargetPlatform + "/";

        await FtpManager.Instance.DownLoadFileAsync("ABCompareInfo.txt", path, serverIP, USER_NAME, PASSWORD, callback);
    }

    /// <summary>
    /// AB����Ϣ�࣬���ڼ��AB���Ƿ���Ҫ����
    /// </summary>
    public class ABInfo
    {
        public string abName; // AB����
        public long size; // AB����С
        public string md5; // AB����MD5��

        public ABInfo(string abName, string size, string md5)
        {
            this.abName = abName;
            this.size = long.Parse(size);
            this.md5 = md5;
        }

        /// <summary>
        /// ��дEquals�������Ա�AB����md5��
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            ABInfo other = obj as ABInfo;
            if (other is null) return false;
            return this.md5 == other.md5;
        }

        public override int GetHashCode()
        {
            return md5.GetHashCode();
        }
    }
}