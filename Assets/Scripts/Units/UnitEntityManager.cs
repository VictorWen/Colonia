using System.Collections.Generic;
using UnityEngine;

namespace Units
{
    public class UnitEntityManager
    {
        public List<IUnitEntity> Units { get; private set; }

        private readonly Dictionary<Vector3Int, IUnitEntity> positions;
        private readonly Dictionary<IUnitEntity, Vector3Int> lastPositions;

        public UnitEntityManager()
        {
            positions = new Dictionary<Vector3Int, IUnitEntity>();
            lastPositions = new Dictionary<IUnitEntity, Vector3Int>();
            Units = new List<IUnitEntity>();
        }

        // TODO: fix removal of NPCUnitEntityAIs
        public void AddUnit(IUnitEntity unit)
        {
            Units.Add(unit);
            positions.Add(unit.Position, unit);
            lastPositions.Add(unit, unit.Position);

            unit.OnMove += () => UpdateUnitPosition(unit);
        }

        public void RemoveUnit(Vector3Int position)
        {
            Units.Remove(positions[position]);
            positions.Remove(position);
            lastPositions.Remove(positions[position]);
        }

        public void RemoveUnit(IUnitEntity unit)
        {
            Units.Remove(unit);
            positions.Remove(unit.Position);
            lastPositions.Remove(unit);
        }

        public void UpdateUnitPosition(IUnitEntity unit)
        {
            positions.Remove(lastPositions[unit]);
            positions.Add(unit.Position, unit);
            lastPositions[unit] = unit.Position;
        }

        public bool TryGetUnitAt(Vector3Int position, out IUnitEntity data)
        {
            return positions.TryGetValue(position, out data);
        }

        public IUnitEntity GetUnitAt(Vector3Int position)
        {
            if (TryGetUnitAt(position, out IUnitEntity unit))
                return unit;
            return null;
        }

        public T GetUnitAt<T>(Vector3Int position) where T : IUnitEntity
        {
            IUnitEntity unit = GetUnitAt(position);
            if (unit is T t)
            {
                return t;
            }
            return default;
        }

        public void NextTurn(GameMaster game)
        {
            foreach (IUnitEntity data in Units)
            {
                data.OnNextTurn(game);
            }
        }
    }
}
