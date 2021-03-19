using System;
using System.Collections.Generic;
using UnityEngine;
using Tiles;

namespace Units.Abilities
{
    public class Ability
    {
        public string ID { get; private set; }
        public string Name { get; private set; }
        public int ManaCost { get; private set; }

        private readonly int range;
        private readonly bool ignoreLoS; // Whether to ignore line of sight
        
        private readonly bool targetFriends;
        private readonly bool targetEnemies;

        private readonly AbilityEffect[] effects;
        private readonly AbilityAOE area;

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

        public virtual void Cast(BaseUnitEntity caster, Vector3Int target, IWorld world)
        {
            HashSet<Vector3Int> aoe = area.GetAOE(caster.Position, target, world);
            List<BaseUnitEntity> targets = new List<BaseUnitEntity>();
            foreach (Vector3Int tile in aoe)
            {
                BaseUnitEntity unitAt = world.UnitManager.GetUnitAt<BaseUnitEntity>(tile);
                if (unitAt != null)
                {
                    bool isEnemy = caster.Combat.IsEnemy(unitAt.Combat);
                    if (isEnemy && targetEnemies || !isEnemy && targetFriends)
                        targets.Add(unitAt);
                }
            }
            foreach (AbilityEffect effect in effects)
            {
                effect.Apply(caster, targets, world);
            }
        }

        public HashSet<Vector3Int> GetWithinRange(BaseUnitEntity caster, IWorld world)
        {
            if (ignoreLoS)
                return world.GetTilesInRange(caster.Position, range);
            else if (caster.Sight <= range)
                return caster.Visibles;
            else
                return world.GetLineOfSight(caster.Position, range);
        }

        /// <summary>
        /// Determines what tiles can reach the target tile with this ability. (Pretty expensive computation)
        /// </summary>
        public HashSet<Vector3Int> GetReachingTiles(BaseUnitEntity caster, Vector3Int target, World world)
        {
            HashSet<Vector3Int> reachingTiles = new HashSet<Vector3Int>();
            foreach (Vector3Int tile in world.GetTilesInRange(target, range))
            {
                if (ignoreLoS || world.GetLineOfSight(tile, Math.Min(caster.Sight, range)).Contains(target))
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
