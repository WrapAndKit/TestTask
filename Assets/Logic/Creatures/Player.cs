using Assets.Logic.General;
using Assets.Logic.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Logic.Creatures {
    internal class Player : MonoBehaviour {

        #region Data

        [HideInInspector]
        public bool canSelect;

        [SerializeField]
        private PlayerConfig p_config;
        public Color PathColor {
            get => p_config.PathColor;
        }

        public Color ProposedPathColor {
            get => p_config.ProposedPathColor;
        }

        private List<Vector2Int> p_pathInCells;
        public List<Vector2Int> PathInCells {
            get => p_pathInCells;
        }

        private Vector2Int p_spawnCell;
        public Vector2Int SpawnCell {
            get => p_spawnCell;
        }

        private List<Vector3> p_path;

        #endregion

        private void Awake() {
            p_pathInCells = new List<Vector2Int>();
            p_path = new List<Vector3>();
            p_spawnCell = new Vector2Int((int)(transform.position.x / Cell.CellSize.x), (int)(transform.position.z / Cell.CellSize.z));
            canSelect = true;
#if UNITY_EDITOR
            Debug.Log($"DEBUG ::: Player ({name} in cell: {p_spawnCell}) Awake");
#endif
        }

        private void Start() {
            GameMapManager.Instance.InitPlayerCells(this);
        }

        private void Update() {
            if (Game.Instance.isPlaying)
                if (Input.GetKeyDown(KeyCode.Mouse0)) {
                    OnClick();
                }
        }

        private void OnClick() {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) &&
                hit.collider.gameObject == gameObject) {
                GameMapManager.Instance.SetCurrentPlayer(this);
            }
        }

        public void AddToPath(Vector2Int point) {
            p_pathInCells.Add(point);
            p_path.Add(new Vector3(point.x * Cell.CellSize.x, transform.position.y, point.y * Cell.CellSize.z));
        }

        public void RemoveFromPath(Vector2Int point) {
            var index = FindPointInPath(point);
            if (index < 0)
                return;
            p_pathInCells.RemoveAt(index);
            p_path.RemoveAt(index);
            if (p_pathInCells.Count > index)
                GameMapManager.Instance.ReselectCells(p_pathInCells.GetRange(index, p_pathInCells.Count - index));
        }

        private int FindPointInPath(Vector2Int point) {
            for (int i = 0; i < p_pathInCells.Count; i++) {
                if (p_pathInCells[i].Equals(point))
                    return i;
            }
            return -1;
        }

        public void StartMovement() {
            StartCoroutine(Movement());
        }

        IEnumerator Movement() {
            var listEnumerator = p_path.GetEnumerator();
            var step = p_config.Speed * Time.fixedDeltaTime;
            while (listEnumerator.MoveNext()) {
                var target = listEnumerator.Current;
                while (Vector3.Distance(transform.position, target) > 0.01f) {
                    transform.position = Vector3.MoveTowards(transform.position, target, step);
                    yield return new WaitForFixedUpdate();
                }
                yield return new WaitForSeconds(0.1f);
            }
            yield return null;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.tag.Equals("Star")) {
                Game.Instance.StarCount++;
                other.gameObject.SetActive(false);
            }
        }

    }
}
