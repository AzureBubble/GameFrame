using UnityEngine;
using UnityEngine.Events;

namespace GameFramework.MonoManager
{
    public class MonoController : MonoBehaviour
    {
        // 生命周期函数监听
        private event UnityAction updateEvent;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            updateEvent?.Invoke();
        }

        public void AddUpdateListener(UnityAction action)
        {
            updateEvent += action;
        }

        public void RemoveUpdateListener(UnityAction action)
        {
            updateEvent -= action;
        }
    }
}