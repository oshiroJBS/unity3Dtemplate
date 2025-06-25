using UnityEngine;
using YsoCorp;

public class BasicAI : YCBehaviour
{


    [SerializeField] Transform[] patrol; //list of wayPoint

    [SerializeField] float m_moveFactor = 2f; //Base Speed


    [SerializeField] private int angle = 45; //angle of vision
    [SerializeField] private int vision = 50; // range of vision

    private bool is_running;
    private bool is_walking;

    private int destination;

    private float speed;

    private void Start()
    {
        this.destination = GetClosestWaypointId(this.transform.position);
    }


    private int GetClosestWaypointId(Vector3 position)
    {
        float closestDistance = Mathf.Infinity;
        int closestWaypointId = 0;

        for (var i = 0; i < patrol.Length; i++)
        {
            float dist = Vector3.Distance(position, patrol[i].transform.position);
            if (1 < dist && dist < closestDistance)
            {
                closestDistance = dist;
                closestWaypointId = i;
            }
        }
        return closestWaypointId;
    }


    private void FixedUpdate()
    {
        Vector3 CheckPos = this.game.player.m_character.transform.position - this.transform.position;

        float joueurAngle = Vector3.Angle(CheckPos, transform.forward);

        Ray rayon = new Ray(this.transform.position, CheckPos);

        float dist = Vector3.Distance(this.game.player.m_character.transform.position, this.transform.position);

        is_walking = true;

        if (is_walking)
        {
            speed = m_moveFactor;
            if (is_running)
            {
                speed = m_moveFactor * 2f;
            }
        }
        else
        {
            speed = 0f;
        }


        // see player //

        // to add for player recognition                                               ||        //if the player if close enough he will get noticed 
        /* && Physics.Raycast(rayon, out RaycastHit hitInfo) && hitInfo.collider.tag == "Player" */
        if ((joueurAngle > -angle && joueurAngle < angle && dist <= vision /* && this.game.player.m_character.m_isAlive*/) || (dist <= vision / 2 /*&& this.game.player.m_character.m_isAlive*/))
        {
            is_running = true;

            Maths.RotateToLookAt(this.transform, this.player.m_character.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, this.game.player.m_character.transform.position, speed * Time.deltaTime);

            Debug.DrawRay(this.transform.position, CheckPos, UnityEngine.Color.yellow);//debug
        }

        // don't see player //
        else
        {
            is_running = false;

            transform.position = Vector3.MoveTowards(transform.position, patrol[destination].position, speed * Time.deltaTime);
            Maths.RotateToLookAt(this.transform, patrol[destination].position);

            if (Maths.RoughlyEqual(this.transform.position, patrol[destination].position, 0.5f)) //go to next wayPoint
            {
                destination++;
            }

            Debug.DrawRay(this.transform.position, CheckPos, UnityEngine.Color.red);//debug
        }

        //get back to the first wayPoint 
        if (destination >= patrol.Length)
        {
            destination = 0;
        }

    }

    //////////////////////////////////////// DEBUG 
    public void DebugFov()
    {
        Vector3 extentLeft = Quaternion.AngleAxis(angle, Vector3.up) * this.transform.forward;
        Vector3 extentRight = Vector3.Reflect(extentLeft, this.transform.right);
        Debug.DrawRay(this.transform.position, extentLeft * vision, UnityEngine.Color.red);
        Debug.DrawRay(this.transform.position, extentRight * vision, UnityEngine.Color.red);
    }
}
