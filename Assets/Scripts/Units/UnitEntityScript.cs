using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Units.Abilities;

namespace Units
{
    public class UnitEntityScript : MonoBehaviour
    {
        // TODO: Organize UnitEntityScript's field variables
        public Camera cam;
        public Grid grid;
        public World world;
        public Tilemap movement;
        public TileBase gold;
        public TileBase cyan;
        public TileBase red;

        public GUIMaster gui; 

        private bool selected = false;
        private readonly HashSet<Vector3Int> moveablePos = new HashSet<Vector3Int>();
        private readonly HashSet<Vector3Int> attackablePos = new HashSet<Vector3Int>();

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
                        ClearAttackables();
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
                    movement.SetTile(Unit.Position, null);
                    ClearMovables();
                    movement.SetTile(gridPos, gold);
                    Unit.MoveTo(gridPos, world);
                    //gui.UpdateAllUnitVisibilities();
                }
                if (attackablePos.Contains(gridPos))
                {
                    ClearAttackables();
                    Unit.AttackUnitEntity(world.UnitManager.Positions[gridPos]);
                }
            }
        }

        public void UpdateGraphics()
        {
            transform.position = grid.CellToWorld(Unit.Position);
            if (selected)
            {
                gui.unitPanel.UpdateGUI();
            }
        }

        public void MoveAction()
        {
            if (!Unit.CanMove)
                return;

            ClearMovables();
            ClearAttackables();

            BFSPathfinder pathfinder = new BFSPathfinder(Unit.Position, Unit.MovementSpeed, world, true);
            foreach (Vector3Int gridPos in pathfinder.Reachables)
            {
                if (!gridPos.Equals(Unit.Position))
                {
                    movement.SetTile(gridPos, cyan);
                    moveablePos.Add(gridPos);
                }
            }

            //AttackAction();
        }

        public void AttackAction()
        {
            if (!Unit.CanAttack)
                return;

            ClearMovables();
            ClearAttackables();

            const int testAttackrange = 1;
/*            foreach (Vector3Int movePos in moveablePos)
            {
                foreach (Vector3Int attackPos in world.GetLineOfSight(movePos, testAttackrange)) {
                    if (world.UnitManager.Positions.ContainsKey(attackPos) && !world.UnitManager.Positions[attackPos].PlayerControlled && !attackablePos.Contains(attackPos))
                    {
                        attackablePos.Add(attackPos);
                        world.movement.SetTile(attackPos, red);
                    }
                }
            }*/
            foreach (Vector3Int attackPos in world.GetLineOfSight(Unit.Position, testAttackrange))
            {
                if (world.UnitManager.Positions.ContainsKey(attackPos) && !world.UnitManager.Positions[attackPos].PlayerControlled && !attackablePos.Contains(attackPos))
                {
                    attackablePos.Add(attackPos);
                    world.movement.SetTile(attackPos, red);
                }
            }
        }

        public void CastAbility(Ability ability)
        {
            gui.unitPanel.HideAbilityMenu();
            StartCoroutine(CastAbilityCoroutine(ability));
        }

        private IEnumerator CastAbilityCoroutine(Ability ability)
        {
            HashSet<Vector3Int> range = ability.GetWithinRange(Unit, world);
            bool hasSelected = false;
            while (!hasSelected)
            {
                yield return new WaitForSecondsRealtime(0.05f);
                world.movement.ClearAllTiles();
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPos = world.grid.WorldToCell(worldPos);
                if (range.Contains(gridPos))
                {
                    Vector3Int[] area = ability.GetAreaOfEffect(Unit.Position, gridPos, world);
                    foreach (Vector3Int tile in area)
                    {
                        world.movement.SetTile(tile, red);
                    }
                    if (Input.GetMouseButtonUp(0))
                    {
                        Unit.CastAbility(ability, gridPos, world);
                        hasSelected = true;
                    }
                }
            }
            world.movement.ClearAllTiles();
            UpdateGraphics();
            yield break;
        }

        public void ShowInventory()
        {
            gui.unitPanel.heroInventory.Enable(this);
        }

        private void ClearMovables()
        {
            foreach (Vector3Int tilePos in moveablePos)
            {
                movement.SetTile(tilePos, null);
            }
            moveablePos.Clear();
            //ClearAttackables();
        }

        private void ClearAttackables()
        {
            foreach (Vector3Int tilePos in attackablePos)
            {
                movement.SetTile(tilePos, null);
            }
            attackablePos.Clear();
        }
    }
}