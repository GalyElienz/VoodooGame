using System.Collections;
using UnityEngine;

namespace Root
{
    public sealed class Coroutines : MonoBehaviour
    {
        private static Coroutines Instance
        {
            get
            {
                if (_instance == null)
                {
                    var gameObject = new GameObject("[COROUTINE MANAGER]");
                    _instance = gameObject.AddComponent<Coroutines>();
                    DontDestroyOnLoad(gameObject);
                }

                return _instance;
            }
        }

        private static Coroutines _instance;

        public static Coroutine StartRoutine(IEnumerator enumerator)
        {
            return Instance.StartCoroutine(enumerator);
        }

        public static void StopRoutine(Coroutine routine)
        {
            Instance.StopCoroutine(routine);
        }
    }
}