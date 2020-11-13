using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cities.Construction
{
    public class ConstructedTileGhost : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public Canvas canvas;
        public Text tooltipText;

        public Coroutine Place(City city, WorldTerrain world, ConstructedTileProject project, GUIStateManager state)
        {
            gameObject.SetActive(true);
            return StartCoroutine(WaitForPlacement(city, world, project, state));
        }

        private IEnumerator WaitForPlacement(City city, WorldTerrain world, ConstructedTileProject project, GUIStateManager state)
        {
            state.SetState(GUIStateManager.GHOST_TILE);
            char sepChar = System.IO.Path.DirectorySeparatorChar;
            spriteRenderer.sprite = Resources.Load<Sprite>("Projects" + sepChar + "Constructed Tiles" + sepChar + project.ID);
            spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);

            Vector3Int gridPos = world.grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            yield return new WaitForSeconds(0.1f);

            bool click = Input.GetMouseButtonUp(0);
            while (!click || !IsValidTile(gridPos, city, world, project))
            {
                gridPos = world.grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                transform.position = world.grid.CellToWorld(gridPos);

                string text = project.GetTooltipText(gridPos, world);
                if (text != null)
                {
                    if (!canvas.gameObject.activeSelf)
                    {
                        canvas.gameObject.SetActive(true);
                    }
                    tooltipText.text = text;
                }
                else
                {
                    if (canvas.gameObject.activeSelf)
                    {
                        canvas.gameObject.SetActive(false);
                    }
                }

                if (!IsValidTile(gridPos, city, world, project))
                {
                    spriteRenderer.color = new Color(0, 0, 0);
                }
                else
                {
                    spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
                }

                click = Input.GetMouseButtonUp(0);
                yield return null;
            }

            ConstructedTile tile = Resources.Load<ConstructedTile>("Projects" + sepChar + "Constructed Tiles" + sepChar + project.ID);
            //TODO: Manage upgrade old constructed tile case

            if (world.cities.GetTile(gridPos) != null)
                project.OnPlacement(gridPos, ((ConstructedTile)world.cities.GetTile(gridPos)).Project);
            else
                project.OnPlacement(gridPos);

            world.cities.SetTile(gridPos, tile);
            world.cities.SetTileFlags(gridPos, UnityEngine.Tilemaps.TileFlags.None);
            world.cities.SetColor(gridPos, new Color(0.5f, 0.5f, 0.5f));

            Debug.Log("Tile Selected");
            state.SetState(GUIStateManager.CITY);
            yield break;
        }

        private bool IsValidTile(Vector3Int pos, City city, WorldTerrain world, ConstructedTileProject project)
        {
            bool upgradeable = true;
            if (world.cities.GetTile(pos) != null)
                upgradeable = project.IsUpgradeableTile(pos, world);
            return city.WithinCityRange(pos) && project.IsValidTile(pos, world, city) && world.cities.GetTile(pos) == null && upgradeable; 
        }
    }
}