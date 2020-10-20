using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileImprovementGhostScript : MonoBehaviour
{
    //public Camera cam;
    private WorldTerrain world;
    public SpriteRenderer spriteRenderer;
    public Canvas canvas;
    public Text aspectText;
    //public City city;

    public Vector3Int Position
    {
        get
        {
            return world.grid.WorldToCell(transform.position);
        }
    }

    public void PlaceTileImprovement(City city, WorldTerrain world, TileImprovement tileImprovement, GUIStateManager state)
    {
        StartCoroutine(WaitForPlacement(city, world, tileImprovement, state));
    }

    private IEnumerator WaitForPlacement(City city, WorldTerrain world, TileImprovement tileImprovement, GUIStateManager state)
    {
        state.SetState(GUIStateManager.TILE_IMPROVEMENT);

        this.world = world;
        spriteRenderer.sprite = Resources.Load<Sprite>("Projects" + System.IO.Path.DirectorySeparatorChar + tileImprovement.ID);
        spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
        gameObject.SetActive(true);

        Vector3Int gridPos = world.grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        yield return new WaitForSeconds(0.1f);
        while (!Input.GetMouseButtonUp(0) || !city.WithinCityRange(gridPos) || !tileImprovement.IsValidTile(gridPos, world))
        {
            gridPos = world.grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            transform.position = world.grid.CellToWorld(gridPos);
            float aspect = tileImprovement.UseFertility ? world.GetFertilityAtTile(gridPos) : world.GetRichnessAtTile(gridPos);
            string title = tileImprovement.UseFertility ? "Fertility: " : "Richness: ";
            aspectText.text = title + Mathf.Round(aspect * 100) / 100;
            if (!city.WithinCityRange(gridPos) || !tileImprovement.IsValidTile(gridPos, world))
            {
                spriteRenderer.color = new Color(0, 0, 0);
            }
            else
            {
                spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
            }
            yield return null;
        }
        canvas.gameObject.SetActive(false);

        state.SetState(GUIStateManager.CITY);
    }
}
