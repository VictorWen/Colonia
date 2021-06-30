using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cities;
using Cities.Construction;
using Units;
using Units.Intelligence;
using Items;
using Units.Abilities;
using Cities.Construction.Projects;

// Handles foreground game state (client). Also used for testing purposes
// Global client side, instantized server side
// TODO: determine Capital implementation
public class GUIMaster : MonoBehaviour
{
    [Obsolete("Should be created on new/load game. Use Game.World instead")]
    public World world;
    public CityGUIPanelScript cityGUI;
    //public static CapitalCity capital;
    //public InventoryGUI invManager;
    public CityScript cityPrefab;
    public ConstructedTileGhost ghostPrefab;

    public DistrictSelectorScript districtSelectorScript;

    public Canvas mapHUD;
    public Camera playerCam;

    public GUIStateManager GUIState { get; private set; }
    public GameMaster Game { get; private set; }

    //TESTING!
    private City capital;
    private CityScript capitalScript;

    public NPCUnitEntityController unitEntityPrefab;
    public PlayerUnitEntityController playerUnitEntityPrefab;
    public UnitPanelController unitPanel;

    public void Awake()
    {
        Debug.Log("GAME START");
        GlobalUnitEntityDictionary.LoadCombatData("Unit Entities");
        Game = new GameMaster(world);
        Game.OnUnitSpawn += CreateUnitEntityController;

        GUIState = new GUIStateManager(cityGUI, mapHUD);
    }

    public void Start()
    {
        cityGUI.gui = this;

        //--TESTING--------
        Inventory inv = Game.GlobalInventory;
        inv.AddItem(new ResourceItem("wood", 100));
        inv.AddItem(new ResourceItem("food", 150));
        inv.AddItem(new ResourceItem("stone", 25));
        //inv.AddItem(new ResourceItem("steel", 100));

        capitalScript = CityScript.Create("Test", new Vector3(0, 0, 0), this);
        capital = capitalScript.city;
        //capital.inv = inv;

        Game.PlaceStarterTileImprovements(capital);
        Game.SpawnStarterHeroes();
    }

    //Called by End Turn Button
    public void NextTurn()
    {
        Game.NextTurn();
        unitPanel.OnNextTurn();
        capitalScript.title.text = capital.Name + "(" + capital.population + ")"; //TODO: move population text update location
    }

    private void CreateUnitEntityController(UnitEntity unitEntity, string id)
    {
        if (unitEntity.IsPlayerControlled)
        {
            PlayerUnitEntityController controller = (PlayerUnitEntityController) Instantiate(playerUnitEntityPrefab);
            controller.Initialize(id, unitEntity.Position, this, Game.World, unitEntity);
        }
        else
        {
            NPCUnitEntityController controller = (NPCUnitEntityController) Instantiate(unitEntityPrefab);
            controller.Initialize(id, unitEntity.Position, this, Game.World, unitEntity);
        }
    }
}