using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cities;
using Cities.Construction;
using Units;
using Units.Intelligence;
using Items;

// Handles foreground game state (client). Also used for testing purposes
// Global client side, instantized server side
// TODO: determine Capital implementation
public class GUIMaster : MonoBehaviour
{
    [Obsolete("Should be created on new/load game. Use Game.world instead")]
    public World world;
    public CityGUIScript cityGUI;
    //public static CapitalCity capital;
    //public InventoryGUI invManager;
    public CityScript cityPrefab;
    public ConstructedTileGhost ghostPrefab;

    public DistrictSelectorScript districtSelectorScript;

    public Canvas mapHUD;

    public GUIStateManager GUIState { get; private set; }
    public GameMaster Game { get; private set; }

    //TESTING!
    //private City openedCity;
    private City capital;
    //TODO: formalize city script text updating
    private CityScript capitalScript;

    public UnitEntityScript testUnit;
    public UnitEntityScript testEnemyUnit;
    public UnitEntityScript unitEntityPrefab;
    public UnitPanelScript unitPanel;

    public void Awake()
    {
        Debug.Log("GAME START");
        
        GUIState = new GUIStateManager(cityGUI, mapHUD);
    }

    public void Start()
    {
        Game = new GameMaster(world);
        cityGUI.gui = this;

        //--TESTING--------
        Inventory inv = Game.GlobalInventory;
        inv.AddItem(new ResourceItem("wood", 100));
        inv.AddItem(new ResourceItem("food", 150));
        inv.AddItem(new ResourceItem("stone", 25));
        //inv.AddItem(new ResourceItem("steel", 100));

        capitalScript = CityScript.Create("Test", new Vector3(-1, 0, 0), this);
        capital = capitalScript.city;
        //capital.inv = inv;
        
        testUnit.gui = this;
        testUnit.Unit = Game.AddNewTestUnit("TEST HERO NAME", true, Game.World.grid.WorldToCell(testUnit.transform.position), testUnit);
        testUnit.Unit.UpdateVision(Game.World);

        testEnemyUnit.gui = this;
        testEnemyUnit.Unit = Game.AddNewTestNPCUnit("EVIL LORD", Game.World.grid.WorldToCell(testEnemyUnit.transform.position), new RecklessAI(), new BasicAttackAbilityAI(), new DijkstrasMovementAI(), testEnemyUnit);
        //testEnemyUnit.Unit.UpdateVision(Game.World);
        //UpdateAllUnitVisibilities();
        //cityGUI.OpenCityGUI(capital);
        //districtSelectorScript.Enable(capital);

        //-------------------
    }

    //Called by End Turn Button
    public void NextTurn()
    {
        Game.NextTurn(this);
        unitPanel.UpdateGUI();
        capitalScript.title.text = capital.Name + "(" + capital.population + ")"; //TODO: move population text update location

        //TODO: move enemy turn to new location
        NPCUnitEntity enemy = (NPCUnitEntity) testEnemyUnit.Unit;
/*        Vector3Int target = enemy.ai.GetTarget(enemy, Game.World);
        Debug.Log("Enemey Target: " + target);
        LinkedList<Vector3Int> movement = enemy.ai.GetMovementAction(enemy, target, Game.World);
        foreach (Vector3Int motion in movement)
        {
            Debug.Log("Enemey Motion: " + motion);
            testEnemyUnit.transform.position = Game.World.grid.CellToWorld(motion);
            enemy.MoveTo(motion, Game.World);
        }*/
        //UpdateAllUnitVisibilities();
    }

}