using System;
using System.Collections.Generic;
using Units.Movement;
using UnityEngine;
using Tiles;

namespace Units.Combat
{
    public class UnitEntityCombat : IUnitEntityCombat
    {
        [Header("Mana")]
        [SerializeField] private int mana = 100;
        [SerializeField] private int maxMana = 100;

        [Header("Combat Stats")]
        [SerializeField] private int attack = 10;
        [SerializeField] private int defence = 5;
        [SerializeField] private int piercing = 3;
        [SerializeField] private int magic = 10;
        [SerializeField] private int resistance = 3;

        public event Action OnAttack;

        public int Attack { get { return attack; } }
        public int Piercing { get { return piercing; } }
        public int Magic { get { return magic; } }
        public bool CanAttack { get; private set; }

        //TODO: fix placeholder abilities list
        public List<string> Abilities { get { return new List<string>(); } }

        public BaseUnitEntity Unit { get; private set; }
        private readonly IWorld world;
        private readonly IUnitEntityMovement movement;

        private readonly int attackRange = 1;

        public UnitEntityCombat(BaseUnitEntity unit, IWorld world, IUnitEntityMovement movement)
        {
            this.Unit = unit;
            this.world = world;
            this.movement = movement;

            CanAttack = true;
        }

        public void OnNextTurn(GameMaster game)
        {
            CanAttack = true;
        }

        public void BasicAttackOnPosition(Vector3Int position)
        {
            CanAttack = false;
            world.UnitManager.GetUnitAt<BaseUnitEntity>(position).Combat.DealDamage(attack, this, true);
            OnAttack?.Invoke();
        }

        public void DealDamage(float baseDamage, IUnitEntityCombat attacker, bool isPhysicalDamage = true)
        {
            if (baseDamage < 0)
                return;

            float combatModifier = world.GetCombatModifierAt(Unit.Position);

            int damage;
            if (isPhysicalDamage)
                damage = CalculatePhysicalDamage(baseDamage, attacker, combatModifier);
            else
                damage = CalculateMagicalDamage(baseDamage, combatModifier);
            Unit.Damage(damage);
        }

        public bool IsEnemy(IUnitEntityCombat other)
        {
            return other.Unit.IsPlayerControlled != Unit.IsPlayerControlled;
        }

        public HashSet<Vector3Int> GetAttackables()
        {
            HashSet<Vector3Int> attackables = new HashSet<Vector3Int>();
            foreach (Vector3Int tile in world.GetLineOfSight(Unit.Position, attackRange))
            {
                BaseUnitEntity unitAt = world.UnitManager.GetUnitAt<BaseUnitEntity>(tile);
                if (unitAt != null && unitAt.Combat.IsEnemy(this))
                {
                    attackables.Add(tile);
                }
            }
            return attackables;
        }

        public string GetStatusDescription()
        {
            string text = "Health: " + Unit.Health + "/" + Unit.MaxHealth + "\n";
            text += "Mana: " + mana + "/" + maxMana + "\n";
            text += "Status Effects: \n";
            text += "* TEST STATUS EFFECT";
            return text;
        }

        private int CalculatePhysicalDamage(float baseDamage, IUnitEntityCombat attacker, float combatModifier)
        {
            float reduction = Mathf.Max(0, combatModifier * (defence - attacker.Piercing));
            reduction *= (float)combatModifier * defence / (attacker.Piercing + 1);
            int damage = (int)(baseDamage - reduction);
            return damage;
        }

        private int CalculateMagicalDamage(float baseDamage, float combatModifier)
        {
            float reduction = combatModifier * resistance;
            int damage = (int)(baseDamage - reduction);
            return damage; 
        }
    }
}