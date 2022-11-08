using TMPro;
using UnityEngine;

namespace Assets.Logic.UI {
    internal abstract class LabelBehaviour : MonoBehaviour {

        #region Data

        private protected TextMeshProUGUI p_textMeshProUGUI;

        #endregion

        protected void Awake() {
            p_textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
            if (p_textMeshProUGUI is null)
                p_textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        }

        public virtual void ChangeText(string text) {
            p_textMeshProUGUI.text = text;
        }

    }
}
