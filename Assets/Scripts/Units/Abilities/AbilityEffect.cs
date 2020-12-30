﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Abilities
{
    public abstract class AbilityEffect
    {
        public abstract void Apply(UnitEntity caster, List<UnitEntity> targets);

        public abstract string GetDescription();
    }
}