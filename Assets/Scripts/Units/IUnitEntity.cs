using UnityEngine;
using System;
using System.Collections.Generic;

namespace Units
{
    public interface IUnitEntity
    {
        string Name { get; }
        Vector3Int Position { get; }

        int Health { get; }
        int MaxHealth { get; }
        bool IsAlive { get; }
        HashSet<Vector3Int> Visibles { get; }

        event Action<int> OnDamaged;
        event Action OnDeath;
        event Action OnMove;
        event Action OnVisionUpdate;

        string GetStatus();

        void OnNextTurn(GameMaster game);

        void MoveTo(Vector3Int destination);

        void UpdateVision();

        void Damage(int damage);

        void Heal(int heal);
    }
}
