using System.Collections;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(MeshGen))]
public class MeshGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        foreach (var method in target.GetType().GetMethods().Where(m => m.GetCustomAttributes(typeof(ContextMenu), false).Length > 0))
        {
            if (GUILayout.Button(method.Name))
            {
                method.Invoke(target, null);
            }
        }
    }
}
