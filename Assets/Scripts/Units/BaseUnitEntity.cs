using UnityEngine;
using System;
using System.Collections.Generic;
using Tiles;
using Units.Movement;
using Units.Combat;
using Items;

namespace Units
{
    public class BaseUnitEntity : IUnitEntity
    {
        public string Name { get; private set; }
        public Vector3Int Position { get; private set; }

        public int Sight { get { return sight; } }
        
        public int Health { get; private set; }
        public int MaxHealth { get; private set; }
        public bool IsAlive { get; private set; }
        public HashSet<Vector3Int> Visibles { get; protected set; }

        public bool IsPlayerControlled { get; private set; }
        public IUnitEntityMovement Movement { get { return movement; } }
        public IUnitEntityCombat Combat { get { return combat; } }
        public Inventory Inventory { get; }

        public event Action<int> OnDamaged;
        public event Action OnDeath;
        public event Action OnMove;
        public event Action OnVisionUpdate;

        [SerializeField] private readonly int sight;
        private readonly IWorld world;

        [SerializeReference] private readonly IUnitEntityMovement movement;
        [SerializeReference] private readonly IUnitEntityCombat combat;

        public BaseUnitEntity(string name, Vector3Int initialPosition, int maxHealth, int sight, IWorld world, bool isPlayerControlled, int movementSpeed)
        {
            Name = name;
            Position = initialPosition;
            MaxHealth = maxHealth;

            this.sight = sight;
            this.world = world;

            IsPlayerControlled = isPlayerControlled;

            movement = new UnitEntityMovement(this, world, movementSpeed);
            combat = new UnitEntityCombat(this, world, movement);
        }

        public string GetStatus()
        {
            return combat.GetStatusDescription();
        }

        public void OnNextTurn(GameMaster game)
        {
            movement.OnNextTurn(game);
        }

        public virtual void MoveTo(Vector3Int destination)
        {
            Movement.CanMove = false;

            Position = destination;
            OnMove?.Invoke();
            UpdateVision();
        }

        public virtual void UpdateVision()
        {
            Visibles = world.GetLineOfSight(Position, sight);
            OnVisionUpdate?.Invoke();
        }

        public void Damage(int damage)
        {
            if (damage > 0 && IsAlive)
            {
                Health -= damage;
                if (Health < 0)
                    IsAlive = false;
            }
        }

        public void Heal(int heal)
        {
            if (heal > 0 && IsAlive)
            {
                Health += heal;
                if (Health > MaxHealth)
                    Health = MaxHealth;
            }
        }

    }
}
