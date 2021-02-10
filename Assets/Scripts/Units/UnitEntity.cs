using UnityEngine;
using System.Collections.Generic;
using Units.Abilities;
using Items.UtilityItems;
using Items;

namespace Units
{
    public class UnitEntity : MonoBehaviour
    {
        // Unity Editor fields ==========================
        public int _health;
        public int _maxHealth;
        public int _mana;
        public int _maxMana;

        public int _attack;
        public int _defence;
        public int _piercing;
        public int _magic;
        public int _resistance;
        // TODO: Figure what these stats are going to do.
        public int _accuracy;
        public int _agility;

        public int _sight;
        public int _movementSpeed;

        public int _level;
        public bool _playerControlled;

        public World _world;
        //==============================================

        // General Information
        public string Name { get { return name; } }

        // Items
        public Inventory Inventory { get; private set; }

        // Combat Status
        //protected List<StatusEffect> statusEffects;

        // Combat Abilities
        public List<string> Abilities { get; private set; }

        // Combat Attributes
        public int Attack { get { return _attack; } private set { _attack = value; } }
        public int Magic { get { return _magic; } private set { _magic = value; } }

        // Vision
        public int Sight { get { return _sight; } }
        public HashSet<Vector3Int> VisibleTiles { get; private set; }

        // Movement Controls
        public Vector3Int Position { get { return manager.GetPositionFor(this); } }
        public bool CanMove { get; private set; } //TODO: move to a HeroUnitEntity class
        public bool CanAttack { get; private set; }
        public int MovementSpeed { get { return _movementSpeed; } }
        public bool PlayerControlled { get { return _playerControlled; } }

        // Management
        private UnitEntityManager manager; // Manages positioning
        private UnitEntityGUI script; // Manages GUI stuff

        protected virtual void Awake()
        {
            script = GetComponent<UnitEntityGUI>();
            VisibleTiles = new HashSet<Vector3Int>();
        }

        public void SetUnitManager(UnitEntityManager manager)
        {
            this.manager = manager;
        }

        protected virtual void Start()
        {
            _world.AddUnitEntity(this);
            CanMove = true;
            CanAttack = true;

            // Testing Stuff
            // TODO: placeholder UnitEntity.Inventory weight
            Inventory = new Inventory(100);
            // TEST
            Inventory.AddItem(new UtilityItem("potion", "Health Potion", 1, 3, 1, "Potion", 10, 1, new AbilityEffect[] { new HealAbilityEffect(10) }, new HexAbilityAOE(0)));

            Abilities = new List<string>();
            //TODO: placeholder UnitEntity.Abilities
            Abilities.Add("fireball");

            UpdateVision(_world);
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
            manager.SetUnitPosition(this, destination);
            UpdateVision(world);
            script.UpdateGraphics();
        }

        public void AttackUnitEntity(UnitEntity target, World world)
        {
            CanMove = false;
            CanAttack = false;
            target.DealDamage(Attack, this, world, true); //TODO: change to basic attack ability
            script.UpdateGraphics();
        }

        public void CastAbility(Ability ability, Vector3Int target, World world)
        {
            CanMove = false;
            CanAttack = false;
            // Assumes there is enough mana available
            // TODO: UnityEntity Ability.manaCost modifiers
            _mana -= ability.ManaCost;
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
            int heal = (int) System.Math.Min(amount, _maxHealth - _health);
            _health += heal;
            return heal;
        }

        public int DealDamage(float baseDamage, UnitEntity attacker, World world, bool isPhysicalDamage = true)
        {
            if (baseDamage < 0)
                return 0;

            float combatModifier = ((TerrainTile)world.terrain.GetTile(Position)).combatModifier;

            if (isPhysicalDamage)
            {
                float reduction = Mathf.Max(0, combatModifier * (_defence - attacker._piercing));
                reduction *= (float) combatModifier * _defence / (attacker._piercing + 1);
                int damage = (int) (baseDamage - reduction);
                damage = Mathf.Max(0, damage);
                damage = Mathf.Min(damage, _health);
                _health -= damage;
                if (_health <= 0)
                    OnDeath();
                return damage;
            }
            else
            {
                float reduction = combatModifier * _resistance;
                int damage = (int)(baseDamage - reduction);
                damage = Mathf.Max(0, damage);
                damage = Mathf.Min(damage, _health);
                _health -= damage;
                if (_health <= 0)
                    OnDeath();
                return damage;
            }
        }

        public virtual void OnDeath()
        {
            Debug.Log(Name + " Died");
            Object.Destroy(script.gameObject);
            manager.RemoveUnit(this);
        }

        public string GetStatusDescription()
        {
            string text = "Health: " + _health + "/" + _maxHealth + "\n";
            text += "Mana: " + _mana + "/" + _maxMana + "\n";
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
                    world.AddFogOfWar(visible);
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

/*            // Determine sight bonus
            float bonus = 0;
            UnityEngine.Tilemaps.TileBase t = world.terrain.GetTile(Position);
            if (t != null)
            {
                bonus = ((TerrainTile)t).sightBonus;
            }*/

            // Remove FoW from tiles within line of sight
            VisibleTiles = world.GetLineOfSight(Position, Sight);

            if (PlayerControlled)
            {
                foreach (Vector3Int withinSight in VisibleTiles)
                {
                    world.RevealTerraIncognita(withinSight);
                    world.RevealFogOfWar(withinSight);
                }
            }

            // Check if the unit is visible or not
            if (world.vision.GetTile(Position) == null)
                ShowScript();
            else
                HideScript();
        }

        public bool IsEnemy(UnitEntity other)
        {
            return PlayerControlled != other.PlayerControlled;
        }
    }
}
