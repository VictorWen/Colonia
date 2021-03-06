﻿using Items;
using Items.EquipmentItems;
using Items.UtilityItems;
using System;
using System.Collections.Generic;
using Tiles;
using Units.Combat;
using Units.Movement;
using UnityEngine;
using Units.Loot;

namespace Units
{
    public class UnitEntity : IUnitEntity
    {
        public event Action<int> OnDamaged;
        public event Action OnDeath;
        public event Action OnStatusChanged;
        public event Action OnMove;
        public event Action OnVisionUpdate;


        public string Name { get; private set; }
        //public string ID { get; private set; }
        public Vector3Int Position { get; private set; }

        public int Level { get; private set; }
        public int Experience { get; private set; }

        public int Health { get { return health; } private set { health = value; } }
        public int MaxHealth { get { return maxHealth; } private set { maxHealth = value; } }
        public bool IsAlive { get; private set; }
        public HashSet<Vector3Int> Visibles { get; protected set; }

        // TODO: change to a more extensive faction identifier
        public bool IsPlayerControlled { get; private set; }

        public int Sight { get { return sight; } }

        public IUnitEntityMovement Movement { get { return movement; } }
        public IUnitEntityCombat Combat { get { return combat; } }
        public Inventory Inventory { get { return inventory; } }
        
        [SerializeField] private int health;
        [SerializeField] private int maxHealth;

        [SerializeField] private int sight;

        [SerializeReference] private IUnitEntityMovement movement;
        [SerializeReference] private IUnitEntityCombat combat;
        [SerializeReference] private Inventory inventory;

        private LootTable loot;

        private readonly IWorld world;

        public UnitEntity(string name, Vector3Int initialPosition, int maxHealth, int sight, bool isPlayerControlled, int movementSpeed, IWorld world, UnitEntityCombatData combatData)
        {
            //ID = id;
            Name = name;
            Position = initialPosition;

            Level = 0;
            Experience = 0;
            
            MaxHealth = maxHealth;
            Health = maxHealth;
            IsAlive = true;

            this.sight = sight;
            this.world = world;

            IsPlayerControlled = isPlayerControlled;

            movement = new UnitEntityMovement(this, world, movementSpeed);
            inventory = new Inventory(100); //TODO: placeholder UnitEntity inventory weight

            // TODO: placeholder UnitEntity inventory initialization
            inventory.AddItem(new UtilityItem("health_potion", "Health Potion", 1, 1, 1, "Potion", 1, 0, new Abilities.AbilityEffect[] {
                new Abilities.HealAbilityEffect(10)
            }, new Abilities.HexAbilityAOE(0), true, false));

            inventory.AddItem(new EquipmentItem("Test Helment +100", new Dictionary<CombatAttributeID, int>() { { CombatAttributeID.ATTACK, 100 } }, 
                new SingleSlotEquipper(UnitEntityEquipmentSlotID.HEAD)));
            inventory.AddItem(new EquipmentItem("Test Helment +2,000", new Dictionary<CombatAttributeID, int>() { { CombatAttributeID.ATTACK, 2000 } },
                new SingleSlotEquipper(UnitEntityEquipmentSlotID.HEAD)));
            inventory.AddItem(new EquipmentItem("Test Two-hander -1,000", new Dictionary<CombatAttributeID, int>() { { CombatAttributeID.ATTACK, -1000 } },
                new AllSlotEquipper(new SlotEquipper[] {
                    new SingleSlotEquipper(UnitEntityEquipmentSlotID.WEAPON1),
                    new SingleSlotEquipper(UnitEntityEquipmentSlotID.WEAPON2)
                })));
            inventory.AddItem(new EquipmentItem("Test Two-hander -10,000", new Dictionary<CombatAttributeID, int>() { { CombatAttributeID.ATTACK, -10000 } },
                new AllSlotEquipper(new SlotEquipper[] {
                    new SingleSlotEquipper(UnitEntityEquipmentSlotID.WEAPON1),
                    new SingleSlotEquipper(UnitEntityEquipmentSlotID.WEAPON2)
                })));


            Visibles = new HashSet<Vector3Int>();
            UpdateVision();
            world.UnitManager.AddUnit(this);
            
            combat = new UnitEntityCombat(this, world, movement, combatData);
        }

        public void AssignLootTable(LootTable loot)
        {
            this.loot = loot;
        }

        public void DropLoot(UnitEntity receiver)
        {
            if (loot != null)
            {
                Debug.Log("Dropping Loot");
                loot.DropLoot(world, receiver);
            }
        }

        public string GetStatus()
        {
            return "Level: " + Level + " (" + Experience + "%)\n" + combat.GetStatusDescription();
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
            HashSet<Vector3Int> newVisibles = new HashSet<Vector3Int>();

            if (IsAlive)
                newVisibles = world.GetLineOfSight(Position, sight);

            if (IsPlayerControlled)
                world.UpdatePlayerVision(Visibles, newVisibles);

            Visibles = newVisibles;

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
                    world.UnitManager.RemoveUnit(this);
                    UpdateVision();                    
                    OnDeath?.Invoke();
                }

                OnStatusChanged?.Invoke();
            }
        }

        public void Heal(int heal)
        {
            if (heal > 0 && IsAlive)
            {
                Health += heal;
                if (Health > MaxHealth)
                    Health = MaxHealth;
                OnStatusChanged?.Invoke();
            }
        }

        public void AddExperience(int exp)
        {
            Experience += exp;
            while (Experience >= 100)
            {
                Level++;
                Experience -= 100;
            }
        }
    }
}
