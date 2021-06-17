using UnityEngine;
using UnityEditor;

namespace Units.Abilities
{
    enum AbilityEffectType
    {
        damage,
        heal,
    }

    [CustomEditor(typeof(AbilitySO))]
    public class AbilityEditor : Editor
    {
        AbilityEffectType effectType;
        bool show = false;

        public override void OnInspectorGUI()
        {
            AbilitySO data = (AbilitySO)target;

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

            EditorGUILayout.Space();

            show = EditorGUILayout.Foldout(show, "Ability Effects");

            if (show)
            {
                EditorGUI.indentLevel++;
                SerializedProperty effectsProp = serializedObject.FindProperty("effects");
                for (int i = 0; i < data.effects.Count; i++)
                {
                    EditorGUILayout.PropertyField(effectsProp.GetArrayElementAtIndex(i), new GUIContent(data.effects[i].EffectTypeName), true);
                }
                EditorGUI.indentLevel--;

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Add New Ability Effect", EditorStyles.boldLabel);

                AbilityEffectType chosen = (AbilityEffectType)EditorGUILayout.EnumPopup("Choose Effect Type", effectType);
                if (effectType != chosen)
                    effectType = chosen;

                if (GUILayout.Button("Submit Ability Effect"))
                {
                    switch (effectType)
                    {
                        case AbilityEffectType.damage:
                            data.effects.Add(new DamageAbilityEffect());
                            break;
                        case AbilityEffectType.heal:
                            data.effects.Add(new HealAbilityEffect());
                            break;
                        default:
                            break;
                    }
                    serializedObject.Update();
                }
            }
        }
    }
}
