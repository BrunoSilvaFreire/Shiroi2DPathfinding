using System;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Unity.Pathfinding2D.Editor {
    public class EditorGroupScope : IDisposable {
        public EditorGroupScope(string title) {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
        }

        public void Dispose() {
            EditorGUILayout.EndVertical();
        }
    }
}