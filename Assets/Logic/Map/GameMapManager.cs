using Assets.Logic.Creatures;
using Assets.Logic.General;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Logic.Map {
    internal sealed class GameMapManager : MonoBehaviourSingleton<GameMapManager> {

        #region Data

        [SerializeField]
        private GameMapConfig p_config;

        private Cell[,] p_map;

        private Player p_currentPlayer;
        public Player CurrentPlayer {
            get => p_currentPlayer;
        }

        #endregion


        void Start() {
#if UNITY_EDITOR
            Debug.Log($"DEBUG ::: {name} - SingletonStarted - Start");
#endif
            InitMap();

#if UNITY_EDITOR
            Debug.Log($"DEBUG ::: {name} - SingletonStarted - Stop");
#endif
        }

        private void InitMap() {
            p_map = new Cell[p_config.Width, p_config.Height];
            var cells = GetComponentsInChildren<Cell>();
            if (cells.Count() != p_config.Width * p_config.Height)
                Debug.LogError($"Size of Map incorrect");
            foreach (var cell in cells) {
                p_map[cell.Index.x, cell.Index.y] = cell;
            }
        }

        public void ReselectPropesedCells() {
            if (p_currentPlayer is null)
                return;
            foreach (var cell in p_map) {
                if (cell.IsSelected && !cell.IsExit)
                    continue;
                if (cell.IsSelectable)
                    cell.SetSelectable(false, p_currentPlayer);
            }
            if (p_currentPlayer.PathInCells.Count == 0)
                SelectAdjacentCells(p_currentPlayer.SpawnCell);
            else
                SelectAdjacentCells(p_currentPlayer.PathInCells.Last());
        }

        public void SetCurrentPlayer(Player player) {
            if (player is null || player.Equals(p_currentPlayer))
                return;
            p_currentPlayer = player;
            ReselectPropesedCells();
#if UNITY_EDITOR
            Debug.Log($"DEBUG ::: Selected player - {player.name}");
#endif
        }

        public void InitPlayerCells(Player player) {
#if UNITY_EDITOR
            Debug.Log($"DEBUG ::: Init player ({player.name}) cell");
#endif
            var cell = player.SpawnCell;
            if (!p_map[cell.x, cell.y].IsSelected) {
                p_map[cell.x, cell.y].SetSelectable(true, player);
                p_map[cell.x, cell.y].Select(true, player);
            }
        }

        public void ReselectCells(List<Vector2Int> cellsToDelete) {
            cellsToDelete.ForEach(cell => {
                p_map[cell.x, cell.y].Select(false, p_currentPlayer);
                p_map[cell.x, cell.y].SetSelectable(false, p_currentPlayer);
            });
        }

        private void SelectAdjacentCells(Vector2Int index) {
            if (!p_currentPlayer.canSelect)
                return;
            if (index.x > 0)
                p_map[index.x - 1, index.y].SetSelectable(true, p_currentPlayer);
            if (index.x < p_config.Width - 1)
                p_map[index.x + 1, index.y].SetSelectable(true, p_currentPlayer);
            if (index.y > 0)
                p_map[index.x, index.y - 1].SetSelectable(true, p_currentPlayer);
            if (index.y < p_config.Height - 1)
                p_map[index.x, index.y + 1].SetSelectable(true, p_currentPlayer);
        }

    }
}
