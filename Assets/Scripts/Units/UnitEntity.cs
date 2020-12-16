using UnityEngine;
using System.Collections.Generic;

namespace Units
{
    public class UnitEntity
    {
        public string Name { get; private set; }
        protected int level;

        // Combat Status
        protected int health;
        protected int maxHealth;
        protected int mana;
        protected int maxMana;
        //protected List<StatusEffect> statusEffects;

        // Combat Attributes
        protected int attack;
        protected int armor;
        protected int piercing;
        protected int magic;
        protected int resistance;
        protected int accuracy;
        protected int agility;

        // Vision
        protected int sight;
        protected int reconRange;
        public HashSet<Vector3Int> VisibleTiles { get; private set; }

        private readonly UnitEntityManager manager;
        private readonly UnitEntityScript script;

        // Movement Controls
        public Vector3Int Position { get; private set; }
        public bool CanMove { get; private set; } //TODO: move to a HeroUnitEntity class
        public int MovementSpeed { get; private set; }
        public bool PlayerControlled { get; private set; }

        // Abilities
        //protected List<Ability> abilities

        public UnitEntity(string name, bool playerControlled, Vector3Int position, UnitEntityManager manager, UnitEntityScript script) 
        {
            this.Name = name;
            this.PlayerControlled = playerControlled;
            level = 0;
            
            health = 100;
            maxHealth = 100;
            mana = 100;
            maxMana = 100;

            attack = 1;
            armor = 1;
            piercing = 1;
            magic = 1;
            resistance = 1;
            accuracy = 1;
            agility = 1;

            sight = 3;
            reconRange = 5;
            VisibleTiles = new HashSet<Vector3Int>();

            CanMove = true;
            MovementSpeed = 3;
            Position = position;

            this.manager = manager;
            this.script = script;
        }

        public virtual void OnNextTurn(GameMaster game)
        {
            CanMove = true;
            UpdateVision(game.World);
        }

        public void Attack(UnitEntity target)
        {

        }

        public string GetStatusDescription()
        {
            string text = "Health: " + health + "/" + maxHealth + "\n";
            text += "Mana: " + mana + "/" + maxMana + "\n";
            text += "Status Effects: \n";
            text += "* TEST STATUS EFFECT";
            return text;
        }

        public void MoveTo(Vector3Int destination, World world)
        {
            CanMove = false;
            manager.MoveUnit(this, destination);
            Position = destination;
            UpdateVision(world);
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
            if (PlayerControlled)
            {
                foreach (Vector3Int visible in VisibleTiles)
                {
                    world.AddFogOfWar(visible, manager);
                }
            }
            VisibleTiles = new HashSet<Vector3Int>();

/*            if (PlayerControlled)
            {
                PathfinderBFS reconPath = new PathfinderBFS(Position, reconRange, world);
                foreach (Vector3Int withinRecon in reconPath.Reachables)
                {
                    
                }
            }*/

            float bonus = 0;
            UnityEngine.Tilemaps.TileBase t = world.terrain.GetTile(Position);
            if (t != null)
            {
                bonus = ((TerrainTile)t).sightBonus;
            }

            VisibleTiles = world.GetLineOfSight(Position, sight + (int) bonus);

            if (PlayerControlled)
            {
                foreach (Vector3Int withinSight in VisibleTiles)
                {
                    world.RevealTerraIncognita(withinSight);
                    world.RevealFogOfWar(withinSight, manager);
                }
            }

            if (world.vision.GetTile(Position) == null)
                ShowScript();
            else
                HideScript();
        }
    }
}
