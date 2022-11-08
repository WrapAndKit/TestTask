using Assets.Logic.General;

namespace Assets.Logic.UI {
    internal sealed class StartButton : ButtonBehaviour {

        #region Data

        #endregion

        private void Start() {
            Game.Instance.PathesChosen = gameObject.SetActive;
            gameObject.SetActive(false);
        }

        private protected override void ButtonClicked() {
            Game.Instance.isPlaying = false;
            gameObject.SetActive(false);
        }

    }
}
