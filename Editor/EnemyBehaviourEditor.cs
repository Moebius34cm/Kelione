using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyBehaviourComponent))]
[CanEditMultipleObjects]
public class EnemyBehaviourEditor : Editor {

    public SerializedProperty
        body_prop,
        sideNumber_prop,
        frontBehaviour_prop,
        sidesBehaviour_prop,
        backBehaviour_prop,
        attaque_prop,
        anim_prop,
        feedbacks_prop;

    private void OnEnable()
    {
        anim_prop = serializedObject.FindProperty("anim");
        body_prop = serializedObject.FindProperty("body");
        sideNumber_prop = serializedObject.FindProperty("sideNumber");
        frontBehaviour_prop = serializedObject.FindProperty("frontBehaviour");
        sidesBehaviour_prop = serializedObject.FindProperty("sidesBehaviour");
        backBehaviour_prop = serializedObject.FindProperty("backBehaviour");
        attaque_prop = serializedObject.FindProperty("attaque");
        feedbacks_prop = serializedObject.FindProperty("feedbacks");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(feedbacks_prop);
        EditorGUILayout.PropertyField(body_prop);
        EditorGUILayout.PropertyField(anim_prop);
        EditorGUILayout.PropertyField(sideNumber_prop);
        EditorGUILayout.PropertyField(frontBehaviour_prop);
        if(sideNumber_prop.enumValueIndex >= 1)
        {
            EditorGUILayout.PropertyField(backBehaviour_prop);
        }
        if(sideNumber_prop.enumValueIndex == 2)
        {
            EditorGUILayout.PropertyField(sidesBehaviour_prop);
        }

        EditorGUILayout.PropertyField(attaque_prop);

        serializedObject.ApplyModifiedProperties();
    }
}
