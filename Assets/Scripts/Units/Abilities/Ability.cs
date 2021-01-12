using System;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Abilities
{
    public class Ability
    {
        public string ID { get; private set; }
        public string Name { get; private set; }
        public int ManaCost { get; private set; }
        private readonly int range;
        private readonly bool ignoreLoS; // Whether to ignore line of sight
        private readonly AbilityEffect[] effects;
        private readonly AbilityAOE area;
        private readonly bool targetFriends;
        private readonly bool targetEnemies;

        public Ability(string id, string name, int manaCost, int range, bool ignoreLineOfSight, AbilityEffect[] effects, AbilityAOE areaOfEffect, bool targetFriends = false, bool targetEnemies = true)
        {
            this.ID = id;
            this.Name = name;
            this.ManaCost = manaCost;
            this.range = range;
            this.ignoreLoS = ignoreLineOfSight;
            this.effects = effects;
            this.area = areaOfEffect;
            this.targetFriends = targetFriends;
            this.targetEnemies = targetEnemies;
        }

        public virtual void Cast(UnitEntity caster, Vector3Int target, World world)
        {
            HashSet<Vector3Int> aoe = area.GetAOE(caster.Position, target, world);
            List<UnitEntity> targets = new List<UnitEntity>();
            foreach (Vector3Int tile in aoe)
            {
                if (world.UnitManager.Positions.ContainsKey(tile))
                {
                    bool isEnemy = world.UnitManager.Positions[tile].PlayerControlled != caster.PlayerControlled;
                    if (isEnemy && targetEnemies || !isEnemy && targetFriends)
                        targets.Add(world.UnitManager.Positions[tile]);
                }
            }
            foreach (AbilityEffect effect in effects)
            {
                effect.Apply(caster, targets);
            }
        }

        public HashSet<Vector3Int> GetWithinRange(UnitEntity caster, World world)
        {
            if (ignoreLoS)
                return world.GetTilesInRange(caster.Position, range);
            else if (caster.Sight <= range)
                return caster.VisibleTiles;
            else
                return world.GetLineOfSight(caster.Position, range);
        }

        /// <summary>
        /// Determines what tiles can reach the target tile with this ability. (Pretty expensive computation)
        /// </summary>
        public HashSet<Vector3Int> GetReachingTiles(UnitEntity caster, Vector3Int target, World world)
        {
            HashSet<Vector3Int> reachingTiles = new HashSet<Vector3Int>();
            foreach (Vector3Int tile in world.GetTilesInRange(target, range))
            {
                if (ignoreLoS || world.GetLineOfSight(caster.Position, Math.Min(caster.Sight, range)).Contains(target))
                {
                    reachingTiles.Add(tile);
                }
            }
            return reachingTiles;
        }

        public HashSet<Vector3Int> GetAreaOfEffect(Vector3Int caster, Vector3Int target, World world)
        {
            return area.GetAOE(caster, target, world);
        }

        public string GetDescription()
        {
            string text = "<b>" + Name + "</b>\n\n";
            text += "<b>Range:</b> " + range + "\n";
            text += area.GetDescription() + "\n";
            foreach (AbilityEffect e in effects)
            {
                text += e.GetDescription() + "\n";
            }

            return text;
        }
    }
}
