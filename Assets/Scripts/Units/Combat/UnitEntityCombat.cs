using System;
using System.Collections.Generic;
using Tiles;
using Units.Abilities;
using Units.Movement;
using UnityEngine;

namespace Units.Combat
{
    public class UnitEntityCombatData
    {
        public int maxMana;
        public int attack;
        public int defence;
        public int piercing;
        public int magic;
        public int resistance;
        public string[] abilities;

        public static UnitEntityCombatData LoadFromSO(UnitEntityData so)
        {
            UnitEntityCombatData data = new UnitEntityCombatData()
            {
                maxMana = so.maxMana,
                attack = so.attack,
                defence = so.defence,
                piercing = so.piercing,
                magic = so.magic,
                resistance = so.resistance,
                abilities = so.abilities,
            };
            return data;
        }
    }

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

        [Header("Experience")]
        [SerializeField] private int baseExpDrop = 100;
        [SerializeField] private float proficienyAdvantage = 0.20f;

        public event Action OnAttack;

        public int Mana { get { return mana; } }

        public int Attack { get { return attack; } }
        public int Piercing { get { return piercing; } }
        public int Magic { get { return magic; } }
        public bool CanAttack { get; private set; }

        public float BaseExperienceDrop { get { return baseExpDrop; } }

        public List<string> Abilities { get; private set; }

        public UnitEntity Unit { get; private set; }
        private readonly IWorld world;
        private readonly IUnitEntityMovement movement;

        private readonly int attackRange = 1;

        public UnitEntityCombat(UnitEntity unit, IWorld world, IUnitEntityMovement movement)
        {
            this.Unit = unit;
            this.world = world;
            this.movement = movement;
            Abilities = new List<string>();

            CanAttack = true;

            Unit.OnDeath += DistributeExperienceOnDeath;
        }

        public UnitEntityCombat(UnitEntity unit, IWorld world, IUnitEntityMovement movement, UnitEntityCombatData data) : this(unit, world, movement)
        {
            mana = data.maxMana;
            maxMana = data.maxMana;

            attack = data.attack;
            defence = data.defence;
            piercing = data.piercing;
            magic = data.magic;
            resistance = data.resistance;

            foreach (string id in data.abilities)
            {
                Abilities.Add(id);
            }
        }

        public void OnNextTurn(GameMaster game)
        {
            CanAttack = true;
        }

        public void CastAbility(Ability ability, Vector3Int target)
        {
            ability.Cast(Unit, target, world);
            UseMana(ability.ManaCost);
            movement.CanMove = false;
            CanAttack = false;
            OnAttack?.Invoke();
        }

        public void BasicAttackOnPosition(Vector3Int position)
        {
            CanAttack = false;
            movement.CanMove = false;
            world.UnitManager.GetUnitAt<UnitEntity>(position).Combat.DealDamage(attack, this, true);
            OnAttack?.Invoke();
        }

        public void DealDamage(float baseDamage, IUnitEntityCombat attacker, bool isPhysicalDamage = true)
        {
            if (baseDamage < 0)
                return;

            float combatModifier = world.GetCombatModifierAt(Unit.Position);

            float damage;
            if (isPhysicalDamage)
                damage = CalculatePhysicalDamage((int)baseDamage, attacker, combatModifier);
            else
                damage = CalculateMagicalDamage(baseDamage, combatModifier);

            int levelDiff = attacker.Unit.Level - Unit.Level;
            float proficienyMultiplier = 1 + levelDiff * proficienyAdvantage;
            damage *= proficienyMultiplier;

            Unit.Damage((int)damage);
        }

        private void DistributeExperienceOnDeath()
        {
            const int DISTRIBUTION_RADIUS = 7;

            foreach (Vector3Int position in world.GetTilesInRange(Unit.Position, DISTRIBUTION_RADIUS)) {
                UnitEntity unitAt = world.UnitManager.GetUnitAt<UnitEntity>(position);
                if (unitAt != null && unitAt.Combat.IsEnemy(this))
                    unitAt.Combat.GainExperience(Unit);
            }
        }

        public void GainExperience(UnitEntity fallen)
        {
            float gainMultiplier = Mathf.Pow(1.5f, fallen.Level - Unit.Level);
            Unit.AddExperience((int)(gainMultiplier * fallen.Combat.BaseExperienceDrop));
        }

        public HashSet<Vector3Int> GetAttackables()
        {
            HashSet<Vector3Int> attackables = new HashSet<Vector3Int>();
            foreach (Vector3Int tile in world.GetLineOfSight(Unit.Position, attackRange))
            {
                UnitEntity unitAt = world.UnitManager.GetUnitAt<UnitEntity>(tile);
                if (unitAt != null && unitAt.Combat.IsEnemy(this))
                {
                    attackables.Add(tile);
                }
            }
            return attackables;
        }

        public bool IsEnemy(IUnitEntityCombat other)
        {
            return other.Unit.IsPlayerControlled != Unit.IsPlayerControlled;
        }

        public string GetStatusDescription()
        {
            string text = "Health: " + Unit.Health + "/" + Unit.MaxHealth + "\n";
            text += "Mana: " + mana + "/" + maxMana + "\n";
            text += "Status Effects: \n";
            text += "* TEST STATUS EFFECT";
            return text;
        }

        private void UseMana(int manaCost)
        {
            if (manaCost <= mana)
                mana -= manaCost;
            else
                Debug.LogError(Unit.Name + " overused mana, cost: " + manaCost);
        }

        private float CalculatePhysicalDamage(float damage, IUnitEntityCombat attacker, float combatModifier)
        {
            return (Mathf.Max(damage - defence, 0) + attacker.Piercing) / combatModifier;

            //float reduction = Mathf.Max(0, combatModifier * (defence - attacker.Piercing));
            //reduction *= (float)combatModifier * defence / (attacker.Piercing + 1);
            //int damage = (int)(damage - reduction);
            //return damage;
        }

        private float CalculateMagicalDamage(float damage, float combatModifier)
        {
            float reduction = combatModifier * resistance;
            return damage - reduction; 
        }
    }
}