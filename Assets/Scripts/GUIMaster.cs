using Cities;
using Items;
using System;
using Units;
using UnityEngine;

// Handles foreground game state (client). Also used for testing purposes
public class GUIMaster : MonoBehaviour
{
    [Obsolete("Should be created on new/load game. Use Game.World instead")]
    public World world;
    public CityGUIPanelScript cityGUI;
    public CityScript cityPrefab;

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

        GUIState = new GUIStateManager(cityGUI.gameObject.SetActive, mapHUD.gameObject.SetActive);
    }

    public void Start()
    {
        cityGUI.gui = this;

        //--TESTING--------
        Inventory inv = Game.GlobalInventory;
        inv.AddItem(new ResourceItem("wood", 25));
        inv.AddItem(new ResourceItem("food", 25));
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