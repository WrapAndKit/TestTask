using Assets.Logic.Creatures;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Assets.Logic.General {
    internal sealed class Game : MonoBehaviourSingleton<Game> {

        #region Data

        [HideInInspector]
        public bool isPlaying = true;

        public UnityAction<int> StarCountChanged;

        public UnityAction FinishedLevel;

        public UnityAction<bool> PathesChosen;

        [SerializeField]
        private int p_playerCount;

        private int p_starCount;
        public int StarCount {
            get => p_starCount;
            set {
                p_starCount = value;
                StarCountChanged?.Invoke(p_starCount);
            }
        }

        private List<Player> p_playersOnExit = new List<Player>();

        private List<Player> p_playersWithCompletedPath = new List<Player>();

        #endregion

        public void PlayerFinishedLevel(Player player) {
            p_playersOnExit.Add(player);
            if (p_playerCount.Equals(p_playersOnExit.Count)) {
                FinishedLevel?.Invoke();
                var prefName = $"StarsLvl{SceneManager.GetActiveScene().buildIndex}";
                if (PlayerPrefs.GetInt(prefName, 0) < p_starCount) {
                    PlayerPrefs.SetInt(prefName, p_starCount);
                }
            }
        }

        public void CompletedPath(Player player) {
            p_playersWithCompletedPath.Add(player);
            if (p_playersWithCompletedPath.Count.Equals(p_playerCount))
                PathesChosen.Invoke(true);
        }

        public void IncompletedPath(Player player) {
            p_playersWithCompletedPath.Remove(player);
            PathesChosen.Invoke(false);
        }

    }
}
