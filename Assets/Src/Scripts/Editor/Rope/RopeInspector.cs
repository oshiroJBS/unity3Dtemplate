using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RopeManager))]
public class RopeInspector : Editor {
    [DrawGizmo(GizmoType.Pickable)]
    public override void OnInspectorGUI() {
        RopeManager rm = (RopeManager)this.target;

        this.DrawDefaultInspector();

        if (GUILayout.Button("Create Single Rope")) {
            rm.CreateRopeInspector();
        }        
        if (GUILayout.Button("Clear Last Rope")) {
            rm.ClearRope();
        } 
        if (GUILayout.Button("Create All Ropes")) {
            rm.CreateRopesInspector();
        }        
        if (GUILayout.Button("Clear All Ropes")) {
            rm.ClearRopes();
        }
    }
}