using UnityEngine;

namespace Assets.Logic.Map {

    [CreateAssetMenu(fileName = "cfg_gameMapProperties", menuName = "Configs/Create game map config")]
    internal sealed class GameMapConfig : ScriptableObject {

        #region Data

        [SerializeField]
        private int p_mapWidth;
        public int Width {
            get => p_mapWidth;
        }

        [SerializeField]
        private int p_mapHeight;
        public int Height {
            get => p_mapHeight;
        }

        #endregion

    }
}
