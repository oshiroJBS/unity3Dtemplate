using UnityEngine;
using YsoCorp;

[ExecuteAlways]
public class RopePart : YCBehaviour {
    [SerializeField] GameObject m_ropeVisual;

    [SerializeField] private float m_smoothPositionSpeed = 20f;
    [SerializeField] private float m_scaleFactor = 1f;

    private Vector3 m_currentRopePosition;
    private Vector3 m_nextRopePosition;

    private void Start() {
        float zScale = Vector3.Distance(this.transform.position, this.m_nextRopePosition);
        this.m_ropeVisual.transform.localScale = new Vector3(this.m_ropeVisual.transform.localScale.x, this.m_ropeVisual.transform.localScale.y, zScale * this.m_scaleFactor);
        this.transform.LookAt(this.m_nextRopePosition);
        this.transform.position = Vector3.Lerp(this.transform.position, this.m_currentRopePosition, Time.deltaTime * this.m_smoothPositionSpeed);
    }

#if UNITY_EDITOR
    private void Update() {
        float zScale = Vector3.Distance(this.transform.position, this.m_nextRopePosition);
        this.m_ropeVisual.transform.localScale = new Vector3(this.m_ropeVisual.transform.localScale.x, this.m_ropeVisual.transform.localScale.y, zScale * this.m_scaleFactor);
        this.transform.LookAt(this.m_nextRopePosition);
        this.transform.position = Vector3.Lerp(this.transform.position, this.m_currentRopePosition, Time.deltaTime * this.m_smoothPositionSpeed);
    }
#endif

    public void UpdatePart(Vector3 _newPosition, Vector3 _nextRopePosition) {
        this.m_currentRopePosition = _newPosition;
        this.m_nextRopePosition = _nextRopePosition;
    }
}
