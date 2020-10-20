using UnityEngine;
public class GUIStateManager
{
    // ---CONTROLS---
    // Whether the camera can still move and zoom in/out
    public bool CameraControl { get; private set; }
    // Whether units can be selected and given actions
    public bool UnitControl { get; private set; }
    // Whether tiles and cities can be interacted with by the player
    public bool TileInteraction { get; private set; }

    // -----GUI------
    // Whether the mapHUD is displayed
    public bool MapHUD { get; private set; }
    private Canvas mapHUD;
    // Whether the cityPanel is displayed
    public bool CityPanel { get; private set; }
    private CityGUIScript cityPanel;


    //----PRESETS-----
    // GameState for when player is viewin the map
    public static readonly GUIStatePreset MAP = new GUIStatePreset(true, true, true, true);
    // GameState for when the player has a City panel open
    public static readonly GUIStatePreset CITY = new GUIStatePreset(false, false, false, false, true);
    // GameState for when the player is placing a tile improvement
    public static readonly GUIStatePreset TILE_IMPROVEMENT = new GUIStatePreset(true);

    public struct GUIStatePreset
    {
        public readonly bool camCtrl;
        public readonly bool unitCtrl;
        public readonly bool tileIntr;

        public readonly bool mapHUD;
        public readonly bool cityPanel;

        public GUIStatePreset(bool cam, bool unit = false, bool tile = false, bool map = false, bool city = false)
        {
            camCtrl = cam;
            unitCtrl = unit;
            tileIntr = tile;
            mapHUD = map;
            cityPanel = city;
        }
    }

    public GUIStateManager(CityGUIScript cityPanel, Canvas mapHUD)
    {
        this.cityPanel = cityPanel;
        this.mapHUD = mapHUD;

        SetState(MAP);
    }

    public void SetState(GUIStatePreset preset)
    {
        CameraControl = preset.camCtrl;
        UnitControl = preset.unitCtrl;
        TileInteraction = preset.tileIntr;
        MapHUD = preset.mapHUD;
        CityPanel = preset.cityPanel;
        cityPanel.gameObject.SetActive(CityPanel);
        mapHUD.gameObject.SetActive(MapHUD);
    }

}
