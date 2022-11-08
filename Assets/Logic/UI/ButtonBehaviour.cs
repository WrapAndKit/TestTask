using UnityEngine;
using UnityEngine.UI;

namespace Assets.Logic.UI {
    [RequireComponent(typeof(Button))]
    internal abstract class ButtonBehaviour : MonoBehaviour {

        #region Data

        private Button p_button;

        #endregion

        private void Awake() {
            p_button = GetComponent<Button>();
            p_button.onClick.AddListener(ButtonClicked);
        }

        private protected abstract void ButtonClicked();
    }
}
