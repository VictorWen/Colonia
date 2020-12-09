using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


namespace Units
{
    public class UnitEntityScript : MonoBehaviour
    {
        public Camera cam;
        public Grid grid;
        public World world;
        public Tilemap movement;
        public TileBase gold;
        public TileBase cyan;

        public GUIMaster gui; 

        private bool selected = false;
        private readonly HashSet<Vector3Int> moveablePos = new HashSet<Vector3Int>();

        //TODO: change to readonly
        public UnitEntity Unit { get; set;}

        public static UnitEntityScript Create(UnitEntity unitData, UnitEntityScript prefab, GUIMaster gui)
        {
            UnitEntityScript unit = Instantiate(prefab);
            unit.Unit = unitData;
            unit.gui = gui;
            if (unitData.PlayerControlled)
                unit.gameObject.SetActive(false);
            return unit;
        }

        private void Awake()
        {
            //TODO: TESTING UNIT CONTROLLER
            //Unit = 
        }

        private void Start()
        {
            //cam.transform.position = transform.position + new Vector3(0, 0, -500);
        }

        private void OnMouseOver()
        {
            if (gui.GUIState.UnitControl)
            {
                if (Unit.PlayerControlled && Input.GetMouseButtonUp(0))
                {
                    if (!selected)
                    {
                        //MoveAction();
                        gui.unitPanel.SetSelectedUnit(this);
                        movement.SetTile(grid.WorldToCell(transform.position), gold);
                        gui.GUIState.SetState(GUIStateManager.UNIT);
                        selected = true;
                    }
                    else
                    {
                        ClearMovables();
                        gui.unitPanel.SetSelectedUnit(null);
                        movement.SetTile(grid.WorldToCell(transform.position), null);
                        gui.GUIState.SetState(GUIStateManager.MAP);
                        selected = false;
                    }
                }
            }
        }

        public void OnMouseEnter()
        {
            //TODO: fix second unit panel call
            if (gui.GUIState.UnitControl)
                gui.unitPanel.ShowUnitInfo(Unit);
        }

        public void OnMouseExit()
        {
            gui.unitPanel.HideUnitInfo();
        }

        private void Update()
        {
            if (selected && Input.GetMouseButtonUp(0))
            {
                Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPos = grid.WorldToCell(pos);
                if (moveablePos.Contains(gridPos))
                {
                    movement.SetTile(grid.WorldToCell(transform.position), null);
                    ClearMovables();
                    movement.SetTile(gridPos, gold);
                    Unit.MoveTo(gridPos, gui.Game.World);
                    UpdateGraphics();
                    gui.unitPanel.moveButton.interactable = false;
                    //gui.UpdateAllUnitVisibilities();
                }
            }
        }

        public void UpdateGraphics()
        {
            transform.position = grid.CellToWorld(Unit.Position);
        }

        public void MoveAction()
        {
            if (!Unit.CanMove)
                return;

            ClearMovables();

            PathfinderBFS pathfinder = new PathfinderBFS(Unit.Position, Unit.MovementSpeed, world, true);

            foreach (Vector3Int gridPos in pathfinder.Reachables)
            {
                movement.SetTile(gridPos, cyan);
                moveablePos.Add(gridPos);
            }

            moveablePos.Remove(grid.WorldToCell(transform.position));
        }

        private void ClearMovables()
        {
            foreach (Vector3Int tilePos in moveablePos)
            {
                movement.SetTile(tilePos, null);
            }
            moveablePos.Clear();
        }
    }
}