using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cities.Construction
{
    public abstract class ConstructedTileProject : IProject
    { 
        public string ID { get; protected set; }
        public abstract string Type { get; }

        protected Vector3Int position;

        public ConstructedTileProject(string id)
        {
            ID = id;
        }

        public virtual void OnPlacement(Vector3Int position)
        {
            this.position = position;
        }

        public abstract bool IsValidTile(Vector3Int position, WorldTerrain world, City city);
        public abstract string GetTooltipText(Vector3Int position, WorldTerrain world);

        public virtual void OnSelect(City city, GUIMaster gui)
        {
            ConstructedTileGhost ghost = Object.Instantiate(gui.ghostPrefab);
            city.UpdateCityRange(gui.Game.world);
            ghost.Place(city, gui.Game.world, this, gui.GUIState);
        }

        public virtual void OnDeselect(City city, GUIMaster gui)
        {
            gui.Game.world.cities.SetTile(position, null);
        }
        
        public virtual void Complete(City city, GUIMaster gui)
        {
            CityTile tile = (CityTile) gui.Game.world.cities.GetTile(position);
            tile.FinishConstruction(city, Type);
            gui.Game.world.cities.SetColor(position, new Color(1, 1, 1));
        }
        
        public abstract IProject Copy();
        public abstract string GetDescription();
        public abstract string GetSelectionInfo(GUIMaster gui);
    }
}
