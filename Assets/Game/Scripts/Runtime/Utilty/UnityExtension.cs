using System;
using UnityEngine;


namespace Game.Runtime
{
    /// <summary>
    /// Unity 扩展。
    /// </summary>
    public static class UnityExtension
    {
        public static void Destroy(this GameObject gameObject)
        {
            GameObject.Destroy(gameObject);
            gameObject = null;
        }

        public static void DestroyImmediate(this GameObject gameObject)
        {
            GameObject.DestroyImmediate(gameObject);
            gameObject = null;
        }

        public static void DestroyComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();

            if (component != null)
                GameObject.Destroy(component);
            component = null;
        }

        public static void DestroyImmediateComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();

            if (component != null)
                GameObject.DestroyImmediate(component);

            component = null;
        }

    }
}
