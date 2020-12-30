﻿using UnityEngine;
using System.Collections.Generic;
using Units.Abilities;
using Items.UtilityItems.Potions;
using Items;

namespace Units
{
    public class UnitEntity
    {
        // General Information
        public string Name { get; private set; }
        protected int level;

        // Items
        public Inventory Inventory { get; private set; }

        // Combat Status
        protected int health;
        protected int maxHealth;
        protected int mana;
        protected int maxMana;
        //protected List<StatusEffect> statusEffects;

        // Combat Abilities
        public List<string> Abilities { get; private set; }

        // Combat Attributes
        public int Attack { get; private set; }
        protected int defence;
        protected int piercing;
        public int Magic { get; private set; }
        protected int resistance;
        protected int accuracy;
        protected int agility;

        // Vision
        public int Sight { get; private set; }
        public HashSet<Vector3Int> VisibleTiles { get; private set; }

        private readonly UnitEntityManager manager;
        private readonly UnitEntityScript script;

        // Movement Controls
        public Vector3Int Position { get; private set; }
        public bool CanMove { get; private set; } //TODO: move to a HeroUnitEntity class
        public bool CanAttack { get; private set; }
        public int MovementSpeed { get; private set; }
        public bool PlayerControlled { get; private set; }

        public UnitEntity(string name, bool playerControlled, Vector3Int position, UnitEntityManager manager, UnitEntityScript script) 
        {
            this.Name = name;
            this.PlayerControlled = playerControlled;
            level = 0;

            // TODO: placeholder UnitEntity.Inventory weight
            Inventory = new Inventory(100);
            // TEST
            Inventory.AddItem(new Potion("potion", "Health Potion", 1, 3, 1, 10, new AbilityEffect[] { new HealAbilityEffect(10) }));

            health = 30;
            maxHealth = 100;
            mana = 100;
            maxMana = 100;

            Abilities = new List<string>();
            //TODO: placeholder UnitEntity.Abilities
            Abilities.Add("fireball");

            Attack = 1;
            defence = 1;
            piercing = 1;
            Magic = 1;
            resistance = 1;
            accuracy = 1;
            agility = 1;

            Sight = 5;
            VisibleTiles = new HashSet<Vector3Int>();

            CanMove = true;
            CanAttack = true;
            MovementSpeed = 3;
            Position = position;

            this.manager = manager;
            this.script = script;
        }

        public virtual void OnNextTurn(GameMaster game)
        {
            CanMove = true;
            CanAttack = true;
            UpdateVision(game.World);
        }

        public void MoveTo(Vector3Int destination, World world)
        {
            CanMove = false;
            manager.MoveUnit(this, destination);
            Position = destination;
            UpdateVision(world);
            script.UpdateGraphics();
        }

        public void AttackUnitEntity(UnitEntity target)
        {
            CanMove = false;
            CanAttack = false;
            int reduction = Mathf.Max((target.defence - piercing), 0) * (target.defence / (piercing + 1));
            int damage = Mathf.Max(0, Attack - reduction);
            Debug.Log("COMBAT: ATTACKED: " + target.Name + " for " + damage + " Damage");
            target.health = Mathf.Max(0, target.health - damage);
            script.UpdateGraphics();
        }

        public void CastAbility(Ability ability, Vector3Int target, World world)
        {
            CanMove = false;
            CanAttack = false;
            // Assumes there is enough mana available
            // TODO: UnityEntity Ability.manaCost modifiers
            mana -= ability.ManaCost;
            Debug.Log("COMBAT: " + Name + " casted " + ability.Name + " at " + target);
            ability.Cast(this, target, world);
            script.UpdateGraphics();
        }

        public void OnUtilityItemUse()
        {
            CanAttack = false;
            script.UpdateGraphics();
        }

        public int Heal(float amount)
        {
            int heal = (int) System.Math.Min(amount, maxHealth - health);
            health += heal;
            return heal;
        }

        public int DealDamage(float baseDamage, UnitEntity attacker, bool isPhysicalDamage = true)
        {
            if (baseDamage < 0)
                return 0;
            if (isPhysicalDamage)
            {
                float reduction = Mathf.Max(0, defence - attacker.piercing);
                reduction *= (float) defence / (attacker.piercing + 1);
                int damage = (int) (baseDamage - reduction);
                damage = Mathf.Max(0, damage);
                damage = Mathf.Min(damage, health);
                health -= damage;
                return damage;
            }
            else
            {
                float reduction = resistance;
                int damage = (int)(baseDamage - reduction);
                damage = Mathf.Max(0, damage);
                damage = Mathf.Min(damage, health);
                health -= damage;
                return damage;
            }
        }

        public string GetStatusDescription()
        {
            string text = "Health: " + health + "/" + maxHealth + "\n";
            text += "Mana: " + mana + "/" + maxMana + "\n";
            text += "Status Effects: \n";
            text += "* TEST STATUS EFFECT";
            return text;
        }

        public void HideScript()
        {
            script.gameObject.SetActive(false);
        }

        public void ShowScript()
        {
            script.gameObject.SetActive(true);
            script.UpdateGraphics();
        }

        public void UpdateVision(World world)
        {
            // Place FoW on previously viewable tiles
            if (PlayerControlled)
            {
                foreach (Vector3Int visible in VisibleTiles)
                {
                    world.AddFogOfWar(visible, manager);
                }
            }
            VisibleTiles = new HashSet<Vector3Int>();

            // Reveal Terra Incognita within movement
            if (PlayerControlled)
            {
                BFSPathfinder reconPath = new BFSPathfinder(Position, MovementSpeed, world);
                foreach (Vector3Int withinRecon in reconPath.Reachables)
                {
                    world.RevealTerraIncognita(withinRecon);
                }
            }

            // Determine sight bonus
            float bonus = 0;
            UnityEngine.Tilemaps.TileBase t = world.terrain.GetTile(Position);
            if (t != null)
            {
                bonus = ((TerrainTile)t).sightBonus;
            }

            // Remove FoW from tiles within line of sight
            VisibleTiles = world.GetLineOfSight(Position, Sight + (int) bonus);

            if (PlayerControlled)
            {
                foreach (Vector3Int withinSight in VisibleTiles)
                {
                    world.RevealTerraIncognita(withinSight);
                    world.RevealFogOfWar(withinSight, manager);
                }
            }

            // Check if the unit is visible or not
            if (world.vision.GetTile(Position) == null)
                ShowScript();
            else
                HideScript();
        }
    }
}
