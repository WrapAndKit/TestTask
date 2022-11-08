using UnityEngine;

namespace Assets.Logic.UI {
    internal sealed class ButtonStarLabel : LabelBehaviour {

        #region Data

        private NextLvlButton p_button;

        #endregion

        new void Awake() {
            base.Awake();
            p_button = GetComponentInChildren<NextLvlButton>();
            if (p_button is null)
                p_button = GetComponent<NextLvlButton>();
        }

        private void Start() {
            ChangeText($"{PlayerPrefs.GetInt($"StarsLvl{p_button.SceneNumberTransition}", 0)}/3");
        }

    }
}
