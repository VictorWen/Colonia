using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Units
{
    public class UnitEntityManager
    {
        public Dictionary<Vector3Int, UnitEntity> Positions { get; private set; }
        public List<UnitEntity> Units { get; private set; }

        public UnitEntityManager()
        {
            Positions = new Dictionary<Vector3Int, UnitEntity>();
            Units = new List<UnitEntity>();
        }

        public void AddUnit(UnitEntity unit)
        {
            Units.Add(unit);
            Positions.Add(unit.Position, unit);
        }

        public void RemoveUnit(UnitEntity unit)
        {
            Units.Remove(unit);
            Positions.Remove(unit.Position);
        }

        public void MoveUnit(UnitEntity unit, Vector3Int destination)
        {
            Positions.Remove(unit.Position);
            Positions.Add(destination, unit);
        }
    }
}
