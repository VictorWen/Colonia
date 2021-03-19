﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Tiles;

namespace Units.Abilities
{
    public abstract class AbilityEffect
    {
        public abstract void Apply(BaseUnitEntity caster, List<BaseUnitEntity> targets, IWorld world);

        public abstract string GetDescription();
    }
}
