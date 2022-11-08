using Assets.Logic.Creatures;
using Assets.Logic.General;
using UnityEngine;

namespace Assets.Logic.Map {

    [RequireComponent(typeof(Collider), typeof(MeshRenderer))]
    internal sealed class Cell : MonoBehaviour {

        #region Data

        private static MeshRenderer ps_staticMeshRenderer;

        private Color p_defaultColor;

        public static Vector3 CellSize {
            get => ps_staticMeshRenderer.bounds.size;
        }

        private bool p_isExit;
        public bool IsExit {
            get => p_isExit;
        }

        private MeshRenderer p_meshRenderer;

        private Player p_cellOwner;

        private Obstacle p_obstacle;
        public bool IsAvailable {
            get => p_obstacle is null;
        }

        private bool p_isSelected;
        public bool IsSelected {
            get => p_isSelected;
        }

        private bool p_isSelectable;
        public bool IsSelectable {
            get => p_isSelectable;
        }

        private Vector2Int p_index;
        public Vector2Int Index {
            get => p_index;
        }

        #endregion

        private void Awake() {
            InitStaticFields();
            p_meshRenderer = GetComponent<MeshRenderer>();
            p_defaultColor = p_meshRenderer.material.color;
            p_index = new Vector2Int((int)(transform.position.x / CellSize.x), (int)(transform.position.z / CellSize.z));
            p_obstacle = GetComponentInChildren<Obstacle>();
            if (GetComponentInChildren<Exit>()) {
                p_isExit = true;
                p_defaultColor = Color.black;
                SetColor(p_defaultColor);
            }
#if UNITY_EDITOR
            Debug.Log($"DEBUG ::: Cell ({name} with index {p_index}) Awake");
#endif
        }

        private void InitStaticFields() {
            if (IsNullOrDestroyed(ps_staticMeshRenderer)) {
                ps_staticMeshRenderer = GetComponent<MeshRenderer>();
            }
        }

        private bool IsNullOrDestroyed(Object obj) {
            return obj is null || (obj as Object) == null;
        }


        private void Update() {
            if (Game.Instance.isPlaying)
                if (p_isSelectable) {
                    if (Input.GetKeyDown(KeyCode.Mouse0)) {
                        if (GameMapManager.Instance.CurrentPlayer is null ||
                            GameMapManager.Instance.CurrentPlayer.SpawnCell.Equals(Index))
                            return;
                        OnClick();
                    }
                }
        }

        private void OnClick() {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) &&
                hit.collider.gameObject == gameObject) {
                if (!p_isExit)
                    Select(!IsSelected, GameMapManager.Instance.CurrentPlayer);
                else {
                    Select(GameMapManager.Instance.CurrentPlayer.canSelect, GameMapManager.Instance.CurrentPlayer);
                }
                GameMapManager.Instance.ReselectPropesedCells();
            }
        }

        public void Select(bool selected, Player player) {
            if (!p_isSelectable || player is null)
                return;
            if (player.Equals(p_cellOwner) || p_cellOwner is null) {
                if (selected) {
                    SelectWithoutOwner(player);
                }
                else {
                    DeselectWithoutOwner();
                }
                p_isSelected = selected;
                SetColor(selected ?
                    (!IsExit) ? p_cellOwner.PathColor : p_defaultColor :
                    (IsSelectable) ? player.ProposedPathColor : p_defaultColor);
            }
            else if (p_isExit && p_cellOwner != null) {
                if (selected) {
                    SelectWithOwner(player);
                }
                else {
                    DeselectWithOwner(player);
                }
                SetColor(selected ? Color.black : p_cellOwner.PathColor);
            }
        }

        private void SelectWithoutOwner(Player player) {
            if (!player.canSelect && !p_isExit)
                return;
            p_cellOwner = player;
            p_cellOwner.AddToPath(p_index);
            p_cellOwner.canSelect = !p_isExit;
            if (p_isExit)
                Game.Instance.CompletedPath(player);
        }
        private void DeselectWithoutOwner() {
            if (p_cellOwner.SpawnCell.Equals(Index))
                return;
            if (p_isExit) {
                p_cellOwner.canSelect = p_isExit;
                Game.Instance.IncompletedPath(p_cellOwner);
            }
            p_cellOwner.RemoveFromPath(p_index);
            p_cellOwner = null;
        }
        private void SelectWithOwner(Player player) {
            player.AddToPath(p_index);
            player.canSelect = !p_isExit;
            Game.Instance.CompletedPath(player);
        }
        private void DeselectWithOwner(Player player) {
            player.canSelect = p_isExit;
            player.RemoveFromPath(p_index);
            Game.Instance.IncompletedPath(player);
        }

        public void SetSelectable(bool selectable, Player player) {
            if (IsAvailable) {
                if (!p_isSelected) {
                    SetColor(selectable ? player.ProposedPathColor : p_defaultColor);
                    p_isSelectable = selectable;
                }
                else {
                    if (!player.Equals(p_cellOwner) && p_isExit)
                        SetColor(selectable ? player.ProposedPathColor : p_defaultColor);
                }
            }
        }

        private void SetColor(Color color) {
            p_meshRenderer.material.color = color;
        }

    }
}
