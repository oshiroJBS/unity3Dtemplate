using System;
using UnityEngine;

namespace YsoCorp
{
    public enum FingerState
    {
        None,
        Began,
        Executing,
        Ended
    }

    public class FingerArgs : EventArgs
    {
        public FingerState m_fingerState;
        public Vector2 m_startPosition;
        public Vector2 m_currentPosition;
        public Vector2 m_previousPosition;
        public Vector2 m_direction;
        public Vector2 m_delta;

        public FingerArgs()
        {
            m_fingerState = FingerState.None;
            m_startPosition = Vector2.zero;
            m_currentPosition = Vector2.zero;
            m_previousPosition = Vector2.zero;
            m_direction = Vector2.zero;
            m_delta = Vector2.zero;
        }
    }

    public class Controller : YCBehaviour
    {
        public bool m_showCursorInGame = false;
        public bool m_showJoystickUI = false;
        public bool m_isJoystickFixe = false;
        public bool m_usePanInput = false;

        [SerializeField]
        private float m_tolerance = 200f;
        private bool m_canUse;
        private readonly FingerArgs m_fingerArgs = new FingerArgs();

        public event EventHandler<FingerArgs> FingerEvent;

        bool started = false;
        private int touchID = -1;
        private int idTracked = -1;

        private FingerState FingerState
        {
            get => m_fingerArgs.m_fingerState;
            set
            {
                m_fingerArgs.m_fingerState = value;
                FingerEvent?.Invoke(this, m_fingerArgs);
            }
        }
        public bool CanUse
        {
            get => m_canUse;
            set => m_canUse = value;
        }

        public FingerArgs fingerArgs => this.m_fingerArgs;


        private void Update()
        {
            if (this.CanUse)
            {
                if (m_usePanInput)
                {
                    this.ManagePanInput();
                }
                else
                {

#if UNITY_EDITOR
                    this.ManageJoystickInput();

#else

                    if (Input.touchCount > 0)
                    {
                                                    // joystick usable only on the left part of the screen //
                        if (this.CanUse  /* && Input.GetTouch(Input.touchCount - 1).position.x < Screen.width / 2 */)
                        {
                            // touch = Input.GetTouch(0);
                            this.ManageJoystickTouch();
                        }

                        if (touchID != -1)
                        {

                            IdTracker();

                            if (Input.GetTouch(touchID).phase == TouchPhase.Moved)
                            {
                                if (started)
                                {

                                    this.m_fingerArgs.m_currentPosition = Input.GetTouch(touchID).position;
                                    if (Vector2.SqrMagnitude(this.m_fingerArgs.m_currentPosition - (this.m_fingerArgs.m_startPosition)) > this.GetTolerance() * this.GetTolerance())
                                    {
                                        if (this.m_isJoystickFixe)
                                        {
                                            this.m_fingerArgs.m_currentPosition = this.m_fingerArgs.m_startPosition + Vector2.ClampMagnitude(this.m_fingerArgs.m_currentPosition - this.m_fingerArgs.m_startPosition, this.GetTolerance());
                                        }
                                        else
                                        {
                                            this.m_fingerArgs.m_startPosition = this.m_fingerArgs.m_currentPosition + (Vector2.ClampMagnitude(this.m_fingerArgs.m_startPosition - (this.m_fingerArgs.m_currentPosition), this.GetTolerance()));
                                        }
                                    }
                                    this.m_fingerArgs.m_direction = (this.m_fingerArgs.m_currentPosition - (this.m_fingerArgs.m_startPosition)) / (this.GetTolerance());
                                    this.FingerState = FingerState.Executing;
                                }
                            }

                            if (Input.GetTouch(touchID).phase == TouchPhase.Ended)
                            {
                                started = false;
                                this.FingerState = FingerState.Ended;
                                touchID = -1;
                            }
                        }
                    }
#endif

                }
            }


        }

        private void ManageJoystickInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.m_fingerArgs.m_startPosition = Input.mousePosition;
                this.m_fingerArgs.m_currentPosition = Input.mousePosition;
                this.FingerState = FingerState.Began;
            }
            else if (Input.GetMouseButton(0))
            {
                this.m_fingerArgs.m_currentPosition = Input.mousePosition;
                if (Vector2.SqrMagnitude(this.m_fingerArgs.m_currentPosition - (this.m_fingerArgs.m_startPosition)) > this.GetTolerance() * this.GetTolerance())
                {
                    if (this.m_isJoystickFixe) { this.m_fingerArgs.m_currentPosition = this.m_fingerArgs.m_startPosition + Vector2.ClampMagnitude(this.m_fingerArgs.m_currentPosition - this.m_fingerArgs.m_startPosition, this.GetTolerance()); }
                    else { this.m_fingerArgs.m_startPosition = this.m_fingerArgs.m_currentPosition + (Vector2.ClampMagnitude(this.m_fingerArgs.m_startPosition - (this.m_fingerArgs.m_currentPosition), this.GetTolerance())); }
                }
                this.m_fingerArgs.m_direction = (this.m_fingerArgs.m_currentPosition - (this.m_fingerArgs.m_startPosition)) / (this.GetTolerance());
                this.FingerState = FingerState.Executing;

            }
            else if (Input.GetMouseButtonUp(0))
            {
                this.FingerState = FingerState.Ended;
            }
        }

        private void ManagePanInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.m_fingerArgs.m_startPosition = Input.mousePosition;
                this.m_fingerArgs.m_currentPosition = Input.mousePosition;
                this.m_fingerArgs.m_previousPosition = Input.mousePosition;
                this.FingerState = FingerState.Began;
            }
            else if (Input.GetMouseButton(0))
            {
                this.m_fingerArgs.m_currentPosition = Input.mousePosition;
                this.m_fingerArgs.m_delta = this.m_fingerArgs.m_currentPosition - (this.m_fingerArgs.m_previousPosition);
                this.m_fingerArgs.m_startPosition = this.m_fingerArgs.m_currentPosition + (this.m_fingerArgs.m_delta);
                this.m_fingerArgs.m_direction = (this.m_fingerArgs.m_currentPosition - (this.m_fingerArgs.m_previousPosition)) / (this.GetTolerance());
                this.FingerState = FingerState.Executing;
                this.m_fingerArgs.m_previousPosition = this.m_fingerArgs.m_currentPosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                this.FingerState = FingerState.Ended;
            }
        }


        private void ManageJoystickTouch()
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(Input.touchCount - 1).phase == TouchPhase.Began)
                {

                    if (touchID == -1)
                    {
                        touchID = (Input.touchCount - 1);
                        idTracked = Input.GetTouch(touchID).fingerId;
                    }

                    if (touchID != -1 && !started)
                    {
                        this.m_fingerArgs.m_startPosition = Input.GetTouch(touchID).position;
                        this.m_fingerArgs.m_currentPosition = Input.GetTouch(touchID).position;
                        this.FingerState = FingerState.Began;
                        started = true;
                    }
                }
            }
        }

        private void IdTracker()
        {
            if (idTracked != Input.GetTouch(touchID).fingerId)
            {
                touchID--;
            }
        }


        private float GetTolerance()
        {
            return (this.m_tolerance + 1) * this.ScreenScaleH();
        }
        public float ScreenScaleW()
        {
            return Screen.width / 1248f;
        }

        private Transform GetClosestObject(Vector3 position, string Tag = "") // used to search for object
        {
            float closestDistance = Mathf.Infinity;
            Transform closestObject = null;
            GameObject[] Object;

            if (Tag == "")
            {
                Tag = "Untagged";
                Debug.Log("warning, No tag as been set for GetClosetObject");
            }

            Object = GameObject.FindGameObjectsWithTag(Tag);

            for (var i = 0; i < Object.Length; i++)
            {
                float dist = Vector3.Distance(position, Object[i].transform.position);
                if (1 < dist && dist < closestDistance)
                {
                    closestDistance = dist;
                    if (Object[i].transform.localScale.x > 0.6f && Object[i].transform.position.y < 4f)
                    {
                        closestObject = Object[i].transform;
                    }
                }
            }

            return closestObject;
        }


        public float ScreenScaleH()
        {
            return Screen.height / 2688f;
        }
    }
}
