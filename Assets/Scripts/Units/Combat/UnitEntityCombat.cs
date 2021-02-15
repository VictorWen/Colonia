using System;
using System.Collections.Generic;
using Units.Movement;
using UnityEngine;

namespace Units.Combat
{
    public class UnitEntityCombat
    {
        [Header("Health and Mana")]
        [SerializeField] private int health = 100;
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int mana = 100;
        [SerializeField] private int maxMana = 100;

        [Header("Combat Stats")]
        [SerializeField] private int attack = 10;
        [SerializeField] private int defence = 5;
        [SerializeField] private int piercing = 3;
        [SerializeField] private int magic = 10;
        [SerializeField] private int resistance = 3;

        public event Action OnDeath;
        public event Action OnAttack;

        public int Health { get { return health; } }
        public int MaxHealth { get { return maxHealth; } }
        public bool CanAttack { get; private set; }

        //public HashSet<Vector3Int> Attackables { get; private set; }

        private World world;
        private UnitEntityMovement movement;

        private bool playerControlled;
        private int attackRange = 1;

        public UnitEntityCombat(World world, UnitEntityMovement movement, bool isPlayerControlled)
        {
            this.world = world;
            this.movement = movement;
            playerControlled = isPlayerControlled;

            CanAttack = true;
            //Attackables = new HashSet<Vector3Int>();
        }

        public void Attack(Vector3Int position)
        {
            CanAttack = false;
            OnAttack?.Invoke();
        }

        public int DealDamage(float baseDamage, UnitEntityCombat attacker, float combatModifier, bool isPhysicalDamage = true)
        {
            if (baseDamage < 0)
                return 0;

            if (isPhysicalDamage)
            {
                float reduction = Mathf.Max(0, combatModifier * (defence - attacker.piercing));
                reduction *= (float)combatModifier * defence / (attacker.piercing + 1);
                int damage = (int)(baseDamage - reduction);
                damage = Mathf.Max(0, damage);
                damage = Mathf.Min(damage, health);
                health -= damage;
                if (health <= 0 && OnDeath != null)
                {
                    OnDeath.Invoke();
                }
                return damage;
            }
            else
            {
                float reduction = combatModifier * resistance;
                int damage = (int)(baseDamage - reduction);
                damage = Mathf.Max(0, damage);
                damage = Mathf.Min(damage, health);
                health -= damage;
                if (health <= 0 && OnDeath != null)
                {
                    OnDeath.Invoke();
                }
                return damage;
            }
        }

        public int Heal(float amount)
        {
            int heal = (int)System.Math.Min(amount, maxHealth - health);
            health += heal;
            return heal;
        }

        public bool IsEnemy(UnitEntityCombat other)
        {
            return other.playerControlled != playerControlled;
        }

        public HashSet<Vector3Int> GetAttackables()
        {
            HashSet<Vector3Int> attackables = new HashSet<Vector3Int>();
            foreach (Vector3Int tile in world.GetLineOfSight(movement.Position, attackRange))
            {
                if (world.UnitManager.GetUnitAt(tile, out UnitEntityManager.UnitEntityData data) && data.combat.IsEnemy(this))
                {
                    attackables.Add(tile);
                }
            }
            return attackables;
        }

        public string GetStatusDescription()
        {
            string text = "Health: " + health + "/" + maxHealth + "\n";
            text += "Mana: " + mana + "/" + maxMana + "\n";
            text += "Status Effects: \n";
            text += "* TEST STATUS EFFECT";
            return text;
        }
    }
}