using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Handles foreground game state (client). Also used for testing purposes
// Global client side, instantized server side
// TODO: Solve encapsulation
// TODO: determine Capital implementation
public class GUIMaster : MonoBehaviour
{
    public static GUIMaster main { get; private set; }

    public WorldTerrain world;
    public CityGUIScript cityGUI;
    //public static CapitalCity capital;
    //public InventoryGUI invManager;
    public CityScript cityPrefab;
    public TileImprovementGhostScript tileImprovementGhostScript;
    
    public Canvas mapHUD;

    //TODO: add Hero control game state?
    public GameState GUIState { get; private set; }

    private GameMaster GameMaster;
    //private City openedCity;
    private City capital;
    //TODO: formalize city script text updating
    private CityScript capitalScript;

    public void Awake()
    {
        main = this;
        //capital = new CapitalCity();
        //loadedCity = capital;

        //OpenCityGUI(testCity);
        //loadedCity = testCity;
    }

    public void Start()
    {
        Debug.Log("GAME START");
        Inventory inv = new Inventory(-1);
        inv.AddItem(new ResourceItem("wood", 100));
        inv.AddItem(new ResourceItem("food", 150));
        inv.AddItem(new ResourceItem("stone", 25));

        capitalScript = CreateCity("Test", new Vector3(-1, 0, 0));
        capital = capitalScript.city;
        capital.inv = inv;
    }

    //TODO: move to CityScript?
    public CityScript CreateCity(string name, Vector3 position)
    {
        CityScript script = Instantiate(cityPrefab, position, new Quaternion());
        script.city = new City(name, world.grid.WorldToCell(position));
        script.title.text = name + "(" + script.city.population + ")";
        return script;
    }

    //Called by End Turn Button
    public void NextTurn()
    {
        //TODO: switch to list based
        capital.OnNextTurn();
        capitalScript.title.text = capital.Name + "(" + capital.population + ")";
    }

    public void OpenCityGUI(City city)
    {
        cityGUI.Enable(city);
        SetGameState(GameState.CITY);
    }

    public void CloseCityGUI(GameState exitState = GameState.MAP)
    {
        cityGUI.gameObject.SetActive(false);
        SetGameState(exitState);
    }

    public void SetGameState(GameState gameState)
    {
        switch (GUIState)
        {
            case GameState.MAP:
                mapHUD.gameObject.SetActive(false);
                break;
            case GameState.CITY:
                //pass
                break;
        }
        switch (gameState)
        {
            case GameState.MAP:
                mapHUD.gameObject.SetActive(true);
                break;
            case GameState.CITY:
                //pass
                break;
        }
        GUIState = gameState;
    }
}

public enum GameState
{
    MAP, CITY
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