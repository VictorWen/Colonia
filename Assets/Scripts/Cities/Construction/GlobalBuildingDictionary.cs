﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

namespace Cities.Construction
{
    public class GlobalBuildingDictionary
    {
        private static readonly Dictionary<string, BuildingData> buildings;

        static GlobalBuildingDictionary()
        {
            buildings = new Dictionary<string, BuildingData>();

            AddNewBuilding("saw mill", "Saw Mill", new BuildingEffect[] { new ResourceModifierEffect(ModifierAttributeID.HARDNESS, "wood", -0.1f) });

            //TODO: REMOVE TEST BUILDING
            AddNewBuilding("test", "TEST BUILDING", new BuildingEffect[] { new ResourceModifierEffect(ModifierAttributeID.HARDNESS, "stone", -0.9f) });
        }

        private static void AddNewBuilding(string id, string name, BuildingEffect[] effects)
        {
            buildings.Add(id, new BuildingData(name, effects));
        }

        public static List<BuildingEffect> GetCompletionEffects(string id)
        {
            List<BuildingEffect> effects = new List<BuildingEffect>();
            foreach (BuildingEffect effect in buildings[id].effects)
            {
                if (effect.OnComplete)
                {
                    effects.Add(effect);
                }
            }
            return effects;
        }

        public static List<BuildingEffect> GetNextTurnEffects(string id)
        {
            List<BuildingEffect> effects = new List<BuildingEffect>();
            foreach (BuildingEffect effect in buildings[id].effects)
            {
                if (!effect.OnComplete)
                {
                    effects.Add(effect);
                }
            }
            return effects;
        }

        public static string GetName(string id)
        {
            return buildings[id].name;
        }
    }

    public struct BuildingData
    {
        public readonly string name;
        public readonly BuildingEffect[] effects;

        public BuildingData(string name, BuildingEffect[] effects)
        {
            this.name = name;
            this.effects = effects;
        }
    }
}