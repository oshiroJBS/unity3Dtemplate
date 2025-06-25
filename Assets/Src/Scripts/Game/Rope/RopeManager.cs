using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using YsoCorp;

[Serializable]
public class Rope {
    private RopePart[] m_parts;
    private Transform m_start;
    private Transform m_end;
    private float m_ropeLength;
    private int m_partCount;

    public Rope(ref Transform _start, ref Transform _end, int _partCount = 0) {
        m_start = _start;
        m_end = _end;
        m_ropeLength = Vector3.Distance(m_start.position, m_end.position);
        m_partCount = _partCount == 0 ? (int)this.m_ropeLength + 1 : _partCount;
        m_parts = new RopePart[m_partCount];
    }
    public float length {
        get => m_ropeLength;
        set => m_ropeLength = value;
    }
    public RopePart[] parts => this.m_parts;
    public Vector3 direction => m_end.position - m_start.position;
    public float partSize => m_ropeLength / m_partCount;
    public float partCount => m_partCount;
    public Vector3 startPosition => m_start.position;
    public Vector3 endPosition => m_end.position;
    public void Clear() {
        for(int i = 0; i < m_partCount; ++i) {
            UnityEngine.Object.DestroyImmediate(m_parts[i].gameObject);
        }
    }
}

[Serializable]
public class ControlPoint {
    public Transform p0;
    public Transform p1;
}

[ExecuteAlways]
public class RopeManager : YCBehaviour {
    [SerializeField] private GameObject m_partPrefab;
    [SerializeField] private int m_partCount = 0;

    [SerializeField] private Transform[] m_start = null;
    [SerializeField] private Transform[] m_end = null;
    [SerializeField] private ControlPoint[] m_controlPoints = null;

    [HideInInspector] public List<Rope> m_ropes = new List<Rope>();
    [HideInInspector] public List<GameObject> m_ropesParent = new List<GameObject>();

    protected override void Awake() {
        base.Awake();
        this.ClearRopes();
        if (this.m_start != null && this.m_end != null && this.m_start.Length == this.m_end.Length && this.m_start.Length == this.m_controlPoints.Length) {
            for (int i = 0; i < this.m_start.Length; ++i) { this.CreateRope(ref this.m_start[i], ref this.m_end[i]); }
        }
        this.DrawRopes();
    }

#if UNITY_EDITOR
    private void Update() {
        this.DrawRopes();
    }
#endif

    public void CreateRope(ref Transform _start, ref Transform _end) {
        if (this.m_ropes == null) { this.m_ropes = new List<Rope>(); }
        this.m_ropes.Add(new Rope(ref _start, ref _end, this.m_partCount));
        this.InstantiateRopeParts(this.m_ropes.Count-1);
    } 
    
    public void ClearRope(int _ropeIndex) {
        this.m_ropes[_ropeIndex].Clear();
        DestroyImmediate(this.m_ropesParent[_ropeIndex]);
        this.m_ropesParent.RemoveAt(_ropeIndex);
        this.m_ropes.RemoveAt(_ropeIndex);
    }

    public void ClearRopes() {
        for (int i = 0; i < this.m_ropes.Count; ++i) {
            this.m_ropes[i].Clear();
        }
        foreach (GameObject g in this.m_ropesParent) {
            DestroyImmediate(g);
        }
        this.m_ropesParent.Clear();
        this.m_ropes.Clear();
    }

    private void InstantiateRopeParts(int _ropeIndex) {
        GameObject rope = new GameObject();
        rope.name = "Rope" + (m_ropes.Count - 1);
        rope.transform.parent = this.transform;
        this.m_ropesParent.Add(rope);
        for (int i = 0; i < this.m_ropes[_ropeIndex].partCount; ++i) {
            float x = Mathf.Lerp(0f, this.m_ropes[_ropeIndex].length, i / this.m_ropes[_ropeIndex].partCount) / this.m_ropes[_ropeIndex].length;
            Vector3 pointALongLine = this.DeCasteljausAlgorithm(this.m_start[_ropeIndex].position, this.m_controlPoints[_ropeIndex].p0.position, this.m_controlPoints[_ropeIndex].p1.position, this.m_end[_ropeIndex].position, x);
            this.m_ropes[_ropeIndex].parts[i] = Instantiate(this.m_partPrefab, pointALongLine, Quaternion.identity, rope.transform).GetComponent<RopePart>();
            this.m_ropes[_ropeIndex].parts[i].gameObject.name = "Part" + i;
        }
    }

    private void DrawRopes() {
        if (this.m_ropes.Count == 0) { return; }
        for (int j = 0; j < this.m_ropes.Count; ++j) {
            this.m_ropes[j].length = Vector3.Distance(this.m_start[j].position, m_end[j].position);
            for (int i = 0; i < this.m_ropes[j].partCount; ++i) {
                float x = Mathf.Lerp(0f, this.m_ropes[j].length, i / this.m_ropes[j].partCount) / this.m_ropes[j].length;
                Vector3 pointALongLine = this.DeCasteljausAlgorithm(this.m_start[j].position, this.m_controlPoints[j].p0.position, this.m_controlPoints[j].p1.position, this.m_end[j].position, x);
                this.m_ropes[j].parts[i].UpdatePart(pointALongLine, i < this.m_ropes[j].partCount - 1 ? this.m_ropes[j].parts[i + 1].transform.position : this.m_ropes[j].endPosition);
            }
        }
    }

    private Vector3 DeCasteljausAlgorithm(Vector3 A, Vector3 B, Vector3 C, Vector3 D, float t) {
        //To make it faster
        float oneMinusT = 1f - t;

        //Layer 1
        Vector3 Q = oneMinusT * A + t * B;
        Vector3 R = oneMinusT * B + t * C;
        Vector3 S = oneMinusT * C + t * D;

        //Layer 2
        Vector3 P = oneMinusT * Q + t * R;
        Vector3 T = oneMinusT * R + t * S;

        //Final interpolated position
        Vector3 U = oneMinusT * P + t * T;

        return U;
    }

    #region Editor
#if UNITY_EDITOR
    public void CreateRopeInspector() {
        if (this.m_start != null && this.m_end != null && this.m_start.Length == this.m_end.Length && this.m_start.Length == this.m_controlPoints.Length && this.m_ropes.Count < this.m_start.Length) {
            this.CreateRope(ref this.m_start[this.m_ropes.Count], ref this.m_end[this.m_ropes.Count]);
        }
        this.DrawRopes();
    }
    public void CreateRopesInspector() {
        if (this.m_start != null && this.m_end != null && this.m_start.Length == this.m_end.Length && this.m_start.Length == this.m_controlPoints.Length && this.m_ropes.Count < this.m_start.Length) {
            for (int i = 0; i < this.m_start.Length; ++i) { this.CreateRope(ref this.m_start[i], ref this.m_end[i]); }
        }
        this.DrawRopes();
    }

    public void ClearRope() {
        this.m_ropes[this.m_ropes.Count - 1].Clear();
        DestroyImmediate(this.m_ropesParent[this.m_ropes.Count - 1]);
        this.m_ropesParent.RemoveAt(this.m_ropes.Count - 1);
        this.m_ropes.RemoveAt(this.m_ropes.Count - 1);
    }

    private void OnDrawGizmos() {
        float radius = 1f;
        Gizmos.color = Color.red;
        foreach (Transform t in this.m_start) {
            Gizmos.DrawWireSphere(t.position, radius);
        }
        foreach (Transform t in this.m_end) {
            Gizmos.DrawWireSphere(t.position, radius);
        }
        Gizmos.color = Color.green;
        foreach (ControlPoint cp in this.m_controlPoints) {
            Gizmos.DrawWireSphere(cp.p0.position, radius);
            Gizmos.DrawWireSphere(cp.p1.position, radius);
        }
    }
#endif
#endregion
}
