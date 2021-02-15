using UnityEngine;
using UnityEngine.Tilemaps;

namespace Units
{
    [CreateAssetMenu(fileName = "UnitEntityConfig", menuName = "ScriptableObjects/Units/UnitEntityConfig", order = 1)]
    public class UnitEntityConfig : ScriptableObject
    {
        public TileBase selectTile;
        public TileBase moveTile;
        public TileBase attackTile;

        public bool playerControlled;
    }
}
