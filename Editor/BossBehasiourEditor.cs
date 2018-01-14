using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BossBehaviour))]
[CanEditMultipleObjects]
public class BossBehasiourEditor : Editor {

    public SerializedProperty
        nombreDePhase_prop,
        phaseUn_prop,
        phaseDeux_prop,
        phaseTrois_prop,
        touched_prop,
        waveRange_prop,
        attaqueHitBox_prop;

    private void OnEnable()
    {
        nombreDePhase_prop = serializedObject.FindProperty("nombreDePhase");
        phaseUn_prop = serializedObject.FindProperty("phaseUn");
        phaseDeux_prop = serializedObject.FindProperty("phaseDeux");
        phaseTrois_prop = serializedObject.FindProperty("phaseTrois");
        touched_prop = serializedObject.FindProperty("touched");
        waveRange_prop = serializedObject.FindProperty("waveRange");
        attaqueHitBox_prop = serializedObject.FindProperty("attaqueHitBox");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(nombreDePhase_prop);
        EditorGUILayout.PropertyField(phaseUn_prop);
        if(nombreDePhase_prop.enumValueIndex >= 1)
        {
            EditorGUILayout.PropertyField(phaseDeux_prop);
        }
        if(nombreDePhase_prop.enumValueIndex == 2)
        {
            EditorGUILayout.PropertyField(phaseTrois_prop);
        }
        EditorGUILayout.PropertyField(touched_prop);
        EditorGUILayout.PropertyField(waveRange_prop);
        EditorGUILayout.PropertyField(attaqueHitBox_prop);

        serializedObject.ApplyModifiedProperties();
    }

}
