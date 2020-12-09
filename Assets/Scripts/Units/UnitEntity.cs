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
        protected HashSet<Vector3Int> Visible;

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
            Visible = new HashSet<Vector3Int>();

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
                foreach (Vector3Int visible in Visible)
                {
                    world.AddFogOfWar(visible, manager);
                }
                Visible = new HashSet<Vector3Int>();

                PathfinderBFS reconPath = new PathfinderBFS(Position, reconRange, world);
                foreach (Vector3Int withinRecon in reconPath.Reachables)
                {
                    world.RevealTerraIncognita(withinRecon);
                }

                //BFS
                List<Vector3Int> queue = new List<Vector3Int>();
                HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
                Dictionary<Vector3Int, float> moves = new Dictionary<Vector3Int, float>();
                queue.Add(Position);
                visited.Add(Position);
                Visible.Add(Position);
                moves.Add(Position, 0);

                float bonus = 0;
                UnityEngine.Tilemaps.TileBase t = world.terrain.GetTile(Position);
                if (t != null)
                {
                    bonus = ((TerrainTile)t).sightBonus;
                }

                while (queue.Count > 0)
                {
                    foreach (Vector3Int gridTilePos in world.GetAdjacents(queue[0]))
                    {
                        float currentMoves = moves[queue[0]];
                        float cost = world.IsViewable(sight + bonus - currentMoves, gridTilePos);
                        float moveCost = cost + currentMoves;
                        if (!visited.Contains(gridTilePos) && cost >= 0)
                        {
                            visited.Add(gridTilePos);

                            Visible.Add(gridTilePos);

                            queue.Add(gridTilePos);
                            moves.Add(gridTilePos, moveCost);
                        }
                        else if (visited.Contains(gridTilePos) && cost >= 0 && moveCost < moves[gridTilePos])
                        {
                            moves[gridTilePos] = moveCost;
                        }
                    }
                    queue.RemoveAt(0);
                }

                foreach (Vector3Int withinSight in Visible)
                {
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
