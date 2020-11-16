using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cities;
using Cities.Construction;

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

        capitalScript = CityScript.Create("Test", new Vector3(-1, 0, 0), this);
        capital = capitalScript.city;
        //capital.inv = inv;

        //cityGUI.OpenCityGUI(capital);
        //districtSelectorScript.Enable(capital);
        //-------------------
    }

    //Called by End Turn Button
    public void NextTurn()
    {
        Game.NextTurn(this);
        capitalScript.title.text = capital.Name + "(" + capital.population + ")"; //TODO: move population text update location
    }
}

/*[CustomEditor(typeof(GameMaster))]
public class NextTurnButton : Editor
{
    public override void OnInspectorGUI()
    {
        GameMaster game = (GameMaster)target;

        *//*if (GUILayout.Button("Simulate Next Turn"))
        {
            game.NextTurn();
        }*/

        /*if (GUILayout.Button("Test Construction"))
        {
            game.loadedCity.UpdateConstruction();
        }*//*
    }
}*/