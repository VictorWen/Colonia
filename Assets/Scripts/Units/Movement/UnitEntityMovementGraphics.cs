using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Units.Movement {
    public class UnitEntityMovementGraphics
    {
        private readonly World world;
        private readonly GameObject obj;

        private readonly UnitEntityConfig config;
        private bool showingSelectionIndicator;

        private readonly BaseUnitEntity unit;
        public HashSet<Vector3Int> ShownMoveables { get; private set; }
        private HashSet<Vector3Int> visibleTiles;

        public UnitEntityMovementGraphics(World world, GameObject obj, BaseUnitEntity unit, UnitEntityConfig config)
        {
            this.world = world;
            this.obj = obj;

            this.unit = unit;
            unit.OnMove += UpdateUnitPosition;

            if (config.playerControlled)
                unit.OnVisionUpdate += UpdateVision;

            this.config = config;
            showingSelectionIndicator = false;

            ShownMoveables = new HashSet<Vector3Int>();
            visibleTiles = new HashSet<Vector3Int>();
        }

        public void ShowSelectionIndicator()
        {
            if (!showingSelectionIndicator)
            {
                world.movement.SetTile(world.grid.WorldToCell(obj.transform.position), config.selectTile);
                showingSelectionIndicator = true;
            }
        }

        public void HideSelectionIndicator()
        {
            if (showingSelectionIndicator)
            {
                ClearMoveables();
                //ClearAttackables();
                world.movement.SetTile(world.grid.WorldToCell(obj.transform.position), null);
                showingSelectionIndicator = false;
            }
        }

        public void UpdateUnitPosition()
        {
            bool toggledSelected = false;
            if (showingSelectionIndicator)
            {
                HideSelectionIndicator();
                ClearMoveables();
                toggledSelected = true;
            }

            obj.transform.position = world.grid.CellToWorld(unit.Position);
            
            if (toggledSelected)
                ShowSelectionIndicator();
        }

        public void ShowMoveables(HashSet<Vector3Int> moveables)
        {
            ClearMoveables();
            foreach (Vector3Int moveable in moveables)
            {
                world.movement.SetTile(moveable, config.moveTile);
            }
            ShownMoveables = new HashSet<Vector3Int>(moveables);
        }

        public void ClearMoveables()
        {
            foreach (Vector3Int moveable in ShownMoveables)
            {
                world.movement.SetTile(moveable, null);
            }
            ShownMoveables = new HashSet<Vector3Int>();
        }

        public void UpdateVision()
        {
            // Cover up previously viewable tiles
            foreach (Vector3Int fog in visibleTiles)
            {
                world.AddFogOfWar(fog);
            }
            visibleTiles = new HashSet<Vector3Int>();

            // discover recon tiles
/*            foreach (Vector3Int withinRecon in recon)
            {
                world.RevealTerraIncognita(withinRecon);
            }*/

            // Reveal visible tiles
            foreach (Vector3Int visible in unit.Visibles)
            {
                world.RevealFogOfWar(visible);
                visibleTiles.Add(visible);
            }
        }

    }
}
