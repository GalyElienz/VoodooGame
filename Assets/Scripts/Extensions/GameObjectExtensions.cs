using UnityEngine;

namespace Extensions
{
    public static class GameObjectExtensions
    {
        public static void Destroy(this GameObject gameObject)
        {
            Object.Destroy(gameObject.gameObject);
        }

        public static void Activate(this GameObject gameObject)
        {
            gameObject.SetActive(true);
        }
        
        public static void Deactivate(this GameObject gameObject)
        {
            gameObject.SetActive(false);
        }
    }
}