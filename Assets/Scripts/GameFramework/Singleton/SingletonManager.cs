using GameFramework.MonoManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

/// <summary>
/// ����ȫ�ֵ� Singleton
/// </summary>
public static class SingletonManager
{
    private static bool isInitialize;
    private static List<IUpdateSingleton> updateSingletons = new List<IUpdateSingleton>(50);
    private static Dictionary<Type, ISingleton> singletons = new Dictionary<Type, ISingleton>(50);
    private static MonoController monoController;

    /// <summary>
    /// ��ʼ�� Singleton ������
    /// </summary>
    public static void Initialize()
    {
        if (isInitialize) return;

        isInitialize = true;
        GameObject obj = new GameObject("MonoController");
        monoController = obj.AddComponent<MonoController>();
        monoController?.AddUpdateListener(OnUpdate);
    }

    /// <summary>
    /// ֡���·���
    /// </summary>
    public static void OnUpdate()
    {
        if (updateSingletons.Count > 0)
        {
            foreach (var item in updateSingletons)
            {
                item?.OnUpdate();
            }
        }
    }

    /// <summary>
    /// ������������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T CreateSingleton<T>() where T : class, ISingleton
    {
        Type type = typeof(T);
        if (singletons.ContainsKey(type))
        {
            return singletons[type] as T;
        }

        T singleton = Activator.CreateInstance<T>();
        singleton.Initialize();
        switch (singleton)
        {
            case IUpdateSingleton UpdateSingleton:
                {
                    updateSingletons.Add(singleton as IUpdateSingleton);
                    break;
                }
        }

        singletons.Add(type, singleton);
        return singleton;
    }

    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetSingleton<T>() where T : class, ISingleton
    {
        if (ContainsSingleton<T>())
        {
            if (ContainsUpdateSingleton<T>())
            {
                foreach (var item in updateSingletons)
                {
                    if (item.GetType() == typeof(T))
                    {
                        return item as T;
                    }
                }
            }
            return singletons[typeof(T)] as T;
        }
        return null;
    }

    /// <summary>
    /// �Ƿ������ͨ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool ContainsSingleton<T>() where T : class, ISingleton
    {
        if (singletons.ContainsKey(typeof(T)))
        {
            return true;
        }

        Debug.Log($"������{typeof(T)}��������");
        return false;
    }

    /// <summary>
    /// �Ƿ���� UpdateSingleton
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool ContainsUpdateSingleton<T>() where T : class, ISingleton
    {
        foreach (var item in updateSingletons)
        {
            if (item.GetType() == typeof(T))
            {
                return true;
            }
        }

        Debug.Log($"������{typeof(T)}Update��������");
        return false;
    }

    /// <summary>
    /// ɾ��ĳһ�ض���������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="isUpdateSingleton">�Ƿ���Update�ĵ�������</param>
    /// <returns></returns>
    public static bool DestorySingleton<T>(bool isUpdateSingleton = false) where T : class, ISingleton
    {
        Type type = typeof(T);
        if (singletons.ContainsKey(type))
        {
            ISingleton tempSingleton = singletons[type];
            if (isUpdateSingleton)
            {
                updateSingletons.Remove(tempSingleton as IUpdateSingleton);
            }
            tempSingleton.Dispose();
            singletons.Remove(type);
            return true;
        }

        return false;
    }

    #region ����Э��

    public static Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return monoController.StartCoroutine(coroutine);
    }

    public static Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
    {
        return monoController.StartCoroutine(methodName, value);
    }

    public static Coroutine StartCoroutine(string methodName)
    {
        return monoController.StartCoroutine(methodName);
    }

    #endregion

    #region ֹͣЭ��

    public static void StopCoroutine(IEnumerator coroutine)
    {
        monoController.StopCoroutine(coroutine);
    }

    public static void StopCoroutine(Coroutine coroutine)
    {
        monoController.StopCoroutine(coroutine);
    }

    public static void StopCoroutine(string methodName)
    {
        monoController.StopCoroutine(methodName);
    }

    public static void StopAllCoroutines()
    {
        monoController.StopAllCoroutines();
    }

    #endregion

    /// <summary>
    /// �������е���
    /// </summary>
    private static void DestoryAllSingleton()
    {
        foreach (var singleton in singletons.Values)
        {
            singleton?.Dispose();
        }
    }

    /// <summary>
    /// ���ٵ���������
    /// </summary>
    public static void Dispose()
    {
        if (!isInitialize) return;

        DestoryAllSingleton();
        if (monoController != null)
        {
            monoController.RemoveUpdateListener(OnUpdate);
            GameObject.Destroy(monoController.gameObject);
            monoController = null;
        }
        updateSingletons.Clear();
        singletons.Clear();
        isInitialize = false;
    }
}