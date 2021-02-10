using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Units.Abilities;

namespace Units
{
    public class UnitEntityGUI : MonoBehaviour
    {
        // TODO: Organize UnitEntityScript's field variables
        //public Camera cam;
        //public World world;
        //public Tilemap movement;
        public TileBase gold;
        public TileBase cyan;
        public TileBase red;

        public GUIMaster gui;

        private World world;
        private bool selected = false;
        private readonly HashSet<Vector3Int> moveablePos = new HashSet<Vector3Int>();
        private readonly HashSet<Vector3Int> attackablePos = new HashSet<Vector3Int>();

        //TODO: change to readonly
        public UnitEntity Unit { get; set;}

        private void Awake()
        {
            Unit = GetComponent<UnitEntity>();
            world = gui.Game.World;
            Debug.Log(Unit);
        }

        private void Start()
        {
/*            Debug.Log(gui);
            Debug.Log(gui.Game);
            Debug.Log(gui.Game.world);*/
            if (Unit.PlayerControlled)
                gameObject.SetActive(true);
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
                        world.movement.SetTile(world.grid.WorldToCell(transform.position), gold);
                        gui.GUIState.SetState(GUIStateManager.UNIT);
                        selected = true;
                    }
                    else
                    {
                        ClearMovables();
                        ClearAttackables();
                        gui.unitPanel.SetSelectedUnit(null);
                        world.movement.SetTile(world.grid.WorldToCell(transform.position), null);
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
                Vector3 pos = gui.playerCam.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPos = world.grid.WorldToCell(pos);
                if (moveablePos.Contains(gridPos))
                {
                    world.movement.SetTile(Unit.Position, null);
                    ClearMovables();
                    world.movement.SetTile(gridPos, gold);
                    Unit.MoveTo(gridPos, world);
                    //gui.UpdateAllUnitVisibilities();
                }
                if (attackablePos.Contains(gridPos))
                {
                    ClearAttackables();
                    Unit.AttackUnitEntity(world.GetUnitAt(gridPos), world);
                }
            }
        }

        public void UpdateGraphics()
        {
            Debug.Log(world);
            transform.position = world.grid.CellToWorld(Unit.Position);
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
                    world.movement.SetTile(gridPos, cyan);
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
                UnitEntity unitAt = world.GetUnitAt(attackPos);
                if (unitAt != null && !unitAt.PlayerControlled && !attackablePos.Contains(attackPos))
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
                    HashSet<Vector3Int> area = ability.GetAreaOfEffect(Unit.Position, gridPos, world);
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
                world.movement.SetTile(tilePos, null);
            }
            moveablePos.Clear();
            //ClearAttackables();
        }

        private void ClearAttackables()
        {
            foreach (Vector3Int tilePos in attackablePos)
            {
                world.movement.SetTile(tilePos, null);
            }
            attackablePos.Clear();
        }
    }
}