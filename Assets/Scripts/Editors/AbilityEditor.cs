using UnityEngine;
using UnityEditor;

namespace Units.Abilities
{
    enum AbilityEffectType
    {
        damage,
        heal,
    }

    enum AbilityAOEType
    {
        hexAreaOfEffect,
        none,
    }

    [CustomEditor(typeof(AbilitySO))]
    public class AbilityEditor : Editor
    {
        private AbilityEffectType selectedEffectType;
        private AbilityAOEType selectedAOEType = AbilityAOEType.hexAreaOfEffect;
        private bool showingAbilityEffects = true;

        private void CreateNewAbilityEffect(AbilitySO data)
        {
            switch (selectedEffectType)
            {
                case AbilityEffectType.damage:
                    data.Effects.Add(new DamageAbilityEffect());
                    break;
                case AbilityEffectType.heal:
                    data.Effects.Add(new HealAbilityEffect());
                    break;
                default:
                    break;
            }
            serializedObject.Update();
        }

        private void ChooseAbilityAOEType(AbilitySO data)
        {
            AbilityAOEType chosen = (AbilityAOEType)EditorGUILayout.EnumPopup(selectedAOEType);
            if (chosen != selectedAOEType)
            {
                selectedAOEType = chosen;
                switch (chosen)
                {
                    case AbilityAOEType.hexAreaOfEffect:
                        data.AOE = new HexAbilityAOE();
                        break;
                    default:
                        data.AOE = null;
                        break;
                }
                serializedObject.Update();
            }
        }

        public override void OnInspectorGUI()
        {
            AbilitySO abilityData = (AbilitySO)target;

            DrawDefaultInspectorWithoutAbilityEffects();
            ChooseAbilityAOEType(abilityData);
            EditorGUILayout.Space();

            showingAbilityEffects = EditorGUILayout.Foldout(showingAbilityEffects, "Ability Effects");
            if (showingAbilityEffects)
                DrawAbilityEffectGroup(abilityData);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawDefaultInspectorWithoutAbilityEffects()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            EditorGUI.EndDisabledGroup();

            SerializedProperty property = serializedObject.GetIterator();
            property.NextVisible(true);
            do
            {
                if (property.name != "effects" && property.name != "m_Script")
                    EditorGUILayout.PropertyField(property, true);
            } while (property.NextVisible(false));
        }

        private void DrawAbilityEffectGroup(AbilitySO abilityData)
        {
            DrawAbilityEffects(abilityData);
            EditorGUILayout.Space();
            DrawAddNewAbilityEffectGroup(abilityData);
        }

        private void DrawAbilityEffects(AbilitySO abilityData)
        {
            EditorGUI.indentLevel++;
            SerializedProperty effectsProp = serializedObject.FindProperty("effects");
            for (int i = 0; i < abilityData.Effects.Count; i++)
                EditorGUILayout.PropertyField(effectsProp.GetArrayElementAtIndex(i), new GUIContent(abilityData.Effects[i].EffectTypeName), true);
            EditorGUI.indentLevel--;
        }

        private void DrawAddNewAbilityEffectGroup(AbilitySO abilityData)
        {
            EditorGUILayout.LabelField("Add New Ability Effect", EditorStyles.boldLabel);
            ChooseAbilityEffectType();

            if (GUILayout.Button("Submit Ability Effect"))
                CreateNewAbilityEffect(abilityData);
        }

        private void ChooseAbilityEffectType()
        {
            AbilityEffectType chosen = (AbilityEffectType)EditorGUILayout.EnumPopup("Choose Effect Type", selectedEffectType);
            if (selectedEffectType != chosen)
                selectedEffectType = chosen;
        }
    }
}
