using Assets.Logic.Creatures;
using Assets.Logic.General;
using UnityEngine;

namespace Assets.Logic.Map {
    internal sealed class Exit : MonoBehaviour {

        #region Data

        #endregion

        private void OnTriggerEnter(Collider other) {
            if (other.tag.Equals("Player")) {
                Game.Instance.PlayerFinishedLevel(other.gameObject.GetComponent<Player>());
            }
        }

    }
}
