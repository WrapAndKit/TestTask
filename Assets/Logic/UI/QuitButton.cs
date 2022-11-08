using UnityEngine;

namespace Assets.Logic.UI {
    internal sealed class QuitButton : ButtonBehaviour {

        #region Data

        #endregion

        private protected override void ButtonClicked() {
            Application.Quit();
        }
    }
}
