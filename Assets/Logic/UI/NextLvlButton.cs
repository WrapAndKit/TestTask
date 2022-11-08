

using Assets.Logic.General;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Logic.UI {
    internal sealed class NextLvlButton : ButtonBehaviour {

        #region Data

        [SerializeField]
        private int p_sceneNumber;
        public int SceneNumberTransition {
            get => p_sceneNumber;
        }

        #endregion

        private void Start() {
            if (SceneManager.GetActiveScene().buildIndex != 0) {
                Game.Instance.FinishedLevel = Activate;
                gameObject.SetActive(false);
            }
        }

        private protected override void ButtonClicked() {
            SceneManager.LoadScene(p_sceneNumber);
        }

        private void Activate() {
            gameObject.SetActive(true);
        }

    }
}
