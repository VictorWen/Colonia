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
        public event Action<int> OnDamaged;
        public event Action OnDeath;
        public event Action OnMove;
        public event Action OnVisionUpdate;

        public string Name { get; private set; }
        public Vector3Int Position { get; private set; }

        public int Health { get; private set; }
        public int MaxHealth { get; private set; }
        public bool IsAlive { get; private set; }
        public HashSet<Vector3Int> Visibles { get; protected set; }

        // TODO: change to a more extensive faction identifier
        public bool IsPlayerControlled { get; private set; }

        public int Sight { get { return sight; } }

        public IUnitEntityMovement Movement { get { return movement; } }
        public IUnitEntityCombat Combat { get { return combat; } }
        public Inventory Inventory { get { return Inventory; } }

        [SerializeField] private readonly int sight;

        [SerializeReference] private readonly IUnitEntityMovement movement;
        [SerializeReference] private readonly IUnitEntityCombat combat;
        [SerializeReference] private readonly Inventory inventory;
        private readonly IWorld world;

        public BaseUnitEntity(string name, Vector3Int initialPosition, int maxHealth, int sight, IWorld world, bool isPlayerControlled, int movementSpeed)
        {
            Name = name;
            Position = initialPosition;
            MaxHealth = maxHealth;
            Health = maxHealth;
            IsAlive = true;

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
            combat.OnNextTurn(game);
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
                OnDamaged?.Invoke(damage);
                
                if (Health <= 0)
                {
                    IsAlive = false;
                    OnDeath?.Invoke();
                }
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
