using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MobBehaviourObject))]
[CanEditMultipleObjects]
public class MobBehaviourObjectEditor : Editor {

    public SerializedProperty
        canDash_prop,
        ranged_prop,
        walkSpeed_prop,
        dashSpeed_prop,
        likeDistanceToPlayer_prop,
        aggressitvity_prop,
        patience_prop,
        fear_prop,
        canBlockDuringWait_prop,
        canBlockDuringDash_prop,
        canBlockDuringAttack_prop,
        canBlockDuringWalk_prop,
        canBlockDuringFlee_prop,
        distanceToDash_prop;
    private void OnEnable()
    {
        canDash_prop = serializedObject.FindProperty("canDash");
        ranged_prop = serializedObject.FindProperty("ranged");
        walkSpeed_prop = serializedObject.FindProperty("walkSpeed");
        dashSpeed_prop = serializedObject.FindProperty("dashSpeed");
        likeDistanceToPlayer_prop = serializedObject.FindProperty("likeDistanceToPlayer");
        aggressitvity_prop = serializedObject.FindProperty("aggressitvity");
        patience_prop = serializedObject.FindProperty("patience");
        fear_prop = serializedObject.FindProperty("fear");
        canBlockDuringAttack_prop = serializedObject.FindProperty("canBlockDuringAttack");
        canBlockDuringDash_prop = serializedObject.FindProperty("canBlockDuringDash");
        canBlockDuringFlee_prop = serializedObject.FindProperty("canBlockDuringFlee");
        canBlockDuringWait_prop = serializedObject.FindProperty("canBlockDuringWait");
        canBlockDuringWalk_prop = serializedObject.FindProperty("canBlockDuringWalk");
        distanceToDash_prop = serializedObject.FindProperty("distanceToDash");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Paramètres de comportement", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(walkSpeed_prop);
        EditorGUILayout.PropertyField(canBlockDuringWalk_prop);
        EditorGUILayout.PropertyField(canBlockDuringAttack_prop);
        EditorGUILayout.PropertyField(canBlockDuringWait_prop);
        EditorGUILayout.PropertyField(canBlockDuringFlee_prop);
        EditorGUILayout.PropertyField(canDash_prop);
        EditorGUILayout.PropertyField(ranged_prop);

        EditorGUILayout.LabelField("Caractère", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(likeDistanceToPlayer_prop);
        if (distanceToDash_prop.floatValue > likeDistanceToPlayer_prop.floatValue - 1)
            distanceToDash_prop.floatValue = likeDistanceToPlayer_prop.floatValue - 1;
        EditorGUILayout.PropertyField(aggressitvity_prop);
        EditorGUILayout.PropertyField(patience_prop);
        EditorGUILayout.PropertyField(fear_prop);
        if (canDash_prop.boolValue == true)
        {
            EditorGUILayout.LabelField("Paramètres du dash", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(dashSpeed_prop);
            EditorGUILayout.PropertyField(distanceToDash_prop);
            EditorGUILayout.PropertyField(canBlockDuringDash_prop);
        }

        serializedObject.ApplyModifiedProperties();
    }

}
