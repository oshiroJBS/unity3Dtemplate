using UnityEngine;

public static class Maths
{
    public static Vector3 PositionOnParabola(Vector3 _start, Vector3 _end, float _currentZ, float _speed, float _arcHeight)
    {
        float startZ = _start.z;
        float endZ = _end.z;
        float dist = endZ - startZ;
        float nextZ = Mathf.MoveTowards(_currentZ, endZ, Time.deltaTime * _speed);
        float nextX = Mathf.Lerp(_start.x, _end.x, (nextZ - startZ) / dist);
        float baseY = Mathf.Lerp(_start.y, _end.y, (nextZ - startZ) / dist);
        float arc = (_arcHeight * (nextZ - startZ) * (nextZ - endZ)) / -(dist * dist);
        return new Vector3(nextX, baseY + arc, nextZ);
    }

    public static bool RoughlyEqual(float _a, float _b, float _threshold = 0.001f)
    {
        return Mathf.Abs(_a - _b) < _threshold;
    }

    public static bool RoughlyEqual(Vector3 _a, Vector3 _b, float _threshold = 0.001f)
    {
        return RoughlyEqual(_a.x, _b.x, _threshold)
            && RoughlyEqual(_a.y, _b.y, _threshold)
            && RoughlyEqual(_a.z, _b.z, _threshold);
    }

    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, digits);
        return Mathf.Round(value * mult) / mult;
    }

    public static float ClampAngle(float _angle, float _from, float _to)
    {
        if (_angle < 0f) { _angle = 360 + _angle; }
        return _angle > 180f ? Mathf.Max(_angle, 360 + _from) : Mathf.Min(_angle, _to);
    }

    public static void RotateToLookAt(Transform toRotate,Vector3 toLookAt,float rotateSpeed = 5f, bool FullRotation = false)
    {
        if (FullRotation)
        {
            Vector3 Direction = toLookAt - toRotate.position;

            float angleY = Vector3.SignedAngle(Direction, toRotate.forward, Vector3.up);
            int Yrotate = 0;

            float angleX = Vector3.SignedAngle(Direction, toRotate.forward, Vector3.forward);
            int Xrotate = 0;


            if (Mathf.Abs(angleY) > 0.05f)
            {
                Yrotate = 1;
            }
            if (Mathf.Abs(angleX) > 0.05f)
            {
                Xrotate = 1;
            }

            toRotate.Rotate(-angleX * rotateSpeed * Time.deltaTime * Xrotate, -angleY * rotateSpeed * Time.deltaTime * Yrotate, 0f);
        }
        else
        {
            Vector3 ArrangePos = new Vector3(toLookAt.x, toRotate.position.y, toLookAt.z);
            Vector3 Direction = ArrangePos - toRotate.position;

            float angle = Vector3.SignedAngle(Direction, toRotate.forward, Vector3.up);

            if (Mathf.Abs(angle) > 0.05f && !FullRotation)
            {
                toRotate.Rotate(0, -angle * rotateSpeed * Time.deltaTime, 0f);
            }
        }
    }

    public static Transform GetClosestObject(Vector3 position, string Tag = "", float ScaleMin = 0.001f) // used to search for object
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
            if (dist < closestDistance)
            {
                closestDistance = dist;
                if (Object[i].transform.localScale.x >= ScaleMin)
                {
                    closestObject = Object[i].transform;
                }
            }
        }

        return closestObject;
    }


    public static Transform GetFarestObject(Vector3 position, string Tag = "", float ScaleMin = 0.001f) // used to search for object
    {
        float FarestDistance = 0;
        Transform FarestObject = null;
        GameObject[] Object;

        if (Tag == "")
        {
            Tag = "Untagged";
            Debug.Log("warning, No tag as been set for GetFarestObject");
        }

        Object = GameObject.FindGameObjectsWithTag(Tag);

        for (var i = 0; i < Object.Length; i++)
        {
            float dist = Vector3.Distance(position, Object[i].transform.position);

            if (dist > FarestDistance)
            {
                FarestDistance = dist;
                if (Object[i].transform.localScale.x >= ScaleMin)
                {
                    FarestObject = Object[i].transform;
                }
            }
        }

        return FarestObject;
    }

}