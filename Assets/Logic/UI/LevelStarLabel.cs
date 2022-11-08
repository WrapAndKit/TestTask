using Assets.Logic.General;

namespace Assets.Logic.UI {

    internal class LevelStarLabel : LabelBehaviour {

        #region Data

        #endregion

        private void Start() {
            Game.Instance.StarCountChanged = ChangeText;
        }

        private void ChangeText(int number) {
            p_textMeshProUGUI.text = $"{number}/3";
        }
    }
}
