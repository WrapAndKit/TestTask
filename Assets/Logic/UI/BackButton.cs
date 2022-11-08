using UnityEngine.SceneManagement;

namespace Assets.Logic.UI {
    internal sealed class BackButton : ButtonBehaviour {
        #region Data

        #endregion
        private protected override void ButtonClicked() {
            SceneManager.LoadScene(0);
        }
    }
}
