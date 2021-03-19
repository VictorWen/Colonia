﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Units.Movement {
    public class UnitEntityMovementGraphics
    {
        public HashSet<Vector3Int> ShownMoveables { get; private set; }
        private HashSet<Vector3Int> visibleTiles;
        private UnitEntityGraphics graphics;

        public UnitEntityMovementGraphics(UnitEntityGraphics graphics)
        {
            this.graphics = graphics;

            graphics.Unit.OnMove += UpdateUnitPosition;

            if (graphics.Config.playerControlled)
                graphics.Unit.OnVisionUpdate += UpdateVision;
 
            ShownMoveables = new HashSet<Vector3Int>();
            visibleTiles = new HashSet<Vector3Int>();
        }

        public void UpdateUnitPosition()
        {
            bool toggledSelected = false;
            if (graphics.ShowingSelectionIndicator)
            {
                graphics.HideSelectionIndicator();
                ClearMoveables();
                toggledSelected = true;
            }

            graphics.Obj.transform.position = graphics.World.grid.CellToWorld(graphics.Unit.Position);
            
            if (toggledSelected)
                graphics.ShowSelectionIndicator();
        }

        public void ShowMoveables(HashSet<Vector3Int> moveables)
        {
            ClearMoveables();
            foreach (Vector3Int moveable in moveables)
            {
                graphics.World.movement.SetTile(moveable, graphics.Config.moveTile);
            }
            ShownMoveables = new HashSet<Vector3Int>(moveables);
        }

        public void ClearMoveables()
        {
            foreach (Vector3Int moveable in ShownMoveables)
            {
                graphics.World.movement.SetTile(moveable, null);
            }
            ShownMoveables = new HashSet<Vector3Int>();
        }

        public void UpdateVision()
        {
            // Cover up previously viewable tiles
            foreach (Vector3Int fog in visibleTiles)
            {
                graphics.World.AddFogOfWar(fog);
            }
            visibleTiles = new HashSet<Vector3Int>();

            // Reveal visible tiles
            foreach (Vector3Int visible in graphics.Unit.Visibles)
            {
                graphics.World.RevealFogOfWar(visible);
                visibleTiles.Add(visible);
            }
        }

    }
}