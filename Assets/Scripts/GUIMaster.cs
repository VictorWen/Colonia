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
    public Camera playerCam;

    public GUIStateManager GUIState { get; private set; }
    public GameMaster Game { get; private set; }

    //TESTING!
    //private City openedCity;
    private City capital;
    //TODO: formalize city script text updating
    private CityScript capitalScript;

    public UnitEntityPlayerController testUnit;
    public UnitEntityPlayerController testEnemyUnit;
    public UnitEntityPlayerController unitEntityPrefab;
    public UnitPanelController unitPanel;

    public void Awake()
    {
        Debug.Log("GAME START");
        Game = new GameMaster(world);

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

        capitalScript = CityScript.Create("Test", new Vector3(-1, 0, 0), this);
        capital = capitalScript.city;
        //capital.inv = inv;
        

/*        testUnit.Unit.UpdateVision(Game.World);

        testEnemyUnit.gui = this;*/
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
        unitPanel.OnNextTurn();
        capitalScript.title.text = capital.Name + "(" + capital.population + ")"; //TODO: move population text update location
    }

}