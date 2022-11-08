using UnityEngine;

namespace Assets.Logic.Creatures {
    [CreateAssetMenu(fileName = "cfg_playerProperties", menuName = "Configs/Player properties")]
    internal class PlayerConfig : ScriptableObject {

        #region Data
        [SerializeField]
        private Color p_pathColor;
        public Color PathColor {
            get => p_pathColor;
        }

        [SerializeField]
        private Color p_proposedPathColor;
        public Color ProposedPathColor {
            get => p_proposedPathColor;
        }

        [SerializeField]
        private float p_speed;
        public float Speed {
            get => p_speed;
        }
        #endregion


    }
}
