using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tiles;

namespace Cities.Construction.Projects
{
    public class ConstructedTileGhost : MonoBehaviour, ProjectSelectionController
    {
        public SpriteRenderer spriteRenderer;
        public Canvas canvas;
        public Text tooltipText;

        private City city;
        private World world;
        private ConstructedTileProject project;
        private GUIStateManager state;

        public void Enable(City city, World world, ConstructedTileProject project, GUIStateManager state)
        {
            gameObject.SetActive(true);
            this.city = city;
            this.world = world;
            this.project = project;
            this.state = state;
        }

        public IEnumerator StartSelection()
        {
            state.SetState(GUIStateManager.GHOST_TILE);

            GrabProjectSprite();

            HashSet<Vector3Int> range = city.GetCityRange(world);
            HighlightValidTiles(range);

            yield return new WaitForSeconds(0.1f);

            yield return WaitForValidClick();

            ClearHighlightedTiles(range);

            Debug.Log("Tile Selected");
            state.SetState(GUIStateManager.CITY);
            Destroy(gameObject);
            yield break;
        }

        private IEnumerator WaitForValidClick()
        {
            Vector3Int gridPos = world.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            bool click = Input.GetMouseButtonUp(0);
            while (!click || !IsValidTile(gridPos, city, world, project))
            {
                gridPos = ClickOnTile();

                click = Input.GetMouseButtonUp(0);
                yield return null;
            }

            PlaceConstructedTile(gridPos);

            yield break;
        }

        private void GrabProjectSprite()
        {
            char sepChar = System.IO.Path.DirectorySeparatorChar;
            spriteRenderer.sprite = Resources.Load<Sprite>("Projects" + sepChar + "Constructed Tiles" + sepChar + project.ID);
            spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
        }

        private void HighlightValidTiles(HashSet<Vector3Int> range)
        {
            UnityEngine.Tilemaps.Tile green = Resources.Load<UnityEngine.Tilemaps.Tile>(System.IO.Path.Combine("Tiles", "Green"));
            foreach (Vector3Int pos in range)
            {
                if (IsValidTile(pos, city, world, project))
                    world.SetMovementTile(pos, green);
            }
        }

        private Vector3Int ClickOnTile()
        {
            Vector3Int gridPos = world.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            transform.position = world.CellToWorld(gridPos);

            SetToolTipText(gridPos);

            SetSpriteColor(gridPos);

            return gridPos;
        }

        private void SetToolTipText(Vector3Int gridPos)
        {
            string text = project.GetTooltipText(gridPos, world);
            if (text != null)
            {
                if (!canvas.gameObject.activeSelf)
                    canvas.gameObject.SetActive(true);
                tooltipText.text = text;
            }
            else if (canvas.gameObject.activeSelf)
                canvas.gameObject.SetActive(false);
        }

        private void SetSpriteColor(Vector3Int gridPos)
        {
            if (!IsValidTile(gridPos, city, world, project))
                spriteRenderer.color = new Color(0, 0, 0);
            else
                spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
        }

        private void PlaceConstructedTile(Vector3Int gridPos)
        {
            char sepChar = System.IO.Path.DirectorySeparatorChar;
            ConstructedTile tile = Resources.Load<ConstructedTile>("Projects" + sepChar + "Constructed Tiles" + sepChar + project.ID);
            if (world.GetConstructedTile(gridPos) != null)
                project.OnPlacement(gridPos, world.GetConstructedTile(gridPos).Project);
            else
                project.OnPlacement(gridPos);

            world.StartConstructionOfCityTile(tile, gridPos);
        }

        private void ClearHighlightedTiles(HashSet<Vector3Int> range)
        {
            foreach (Vector3Int pos in range)
                world.SetMovementTile(pos, null);
        }

        private bool IsValidTile(Vector3Int pos, City city, World world, ConstructedTileProject project)
        {
            bool upgradeable = world.GetConstructedTile(pos) == null || project.IsUpgradeableTile(pos, world);
            return city.WithinCityRange(pos) && project.IsValidTile(pos, world, city) && upgradeable;
        }
    }
}