using UnityEngine;

namespace Assets.Logic.General {
    /// <summary>
    /// Singleton for Scene
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour {

        #region Data

        public static T Instance {
            get {
                if (p_instance == null) {
                    Debug.LogError($"Singleton {nameof(T)} isn't defined");
                }
                return p_instance;
            }
        }

        private static T p_instance;

        #endregion


        protected void Awake() {
            if (!TryGetComponent<T>(out p_instance))
                Debug.LogError($"Object {name} should contains Component {nameof(T)}");
        }

    }
}
