using System;
using System.Collections.Generic;
using UnityEngine;
using Tiles;

namespace Units.Abilities
{
    [Serializable]
    public abstract class AbilityEffect
    {
        public abstract string EffectTypeName { get; }

        public abstract void Apply(UnitEntity caster, List<UnitEntity> targets, IWorld world);

        public abstract string GetDescription();
    }
}
