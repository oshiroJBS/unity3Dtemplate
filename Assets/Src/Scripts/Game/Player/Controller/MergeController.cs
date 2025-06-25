using System;
using UnityEngine;

namespace YsoCorp
{
    public class MergeController : YCBehaviour
    {

        [SerializeField] private Transform celling;

        [SerializeField] private GameObject[] forms;


        [SerializeField] private float floatingScale = 0.7f;

        private Transform ray_target = null;
        private RaycastHit mouseInGame;
        private bool hold = false;
        Ray ray;
        Vector3 previous_position;

        Color debugRayColor = Color.yellow;

        // Start is called before the first frame update
        void Start()
        {
            if (celling != null)
            {
                celling.gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (game.gameState == GameState.Playing)
            {
                this.ray = this.game.cam.m_ycCamera.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(this.ray, out mouseInGame);


                if (Input.GetMouseButtonDown(0))
                {

                    for (int i = 0; i < forms.Length; i++)
                    {
                        if (CheckTarget(this.forms[i].tag))
                        {
                            previous_position = ray_target.position;
                            hold = true;
                            return;
                        }
                    }

                    //Debug
                    debugRayColor = Color.red;
                    Debug.Log(mouseInGame.transform.tag);
                }

                if (Input.GetMouseButton(0))
                {
                    if (mouseInGame.point != null && hold == true)
                    {
                        ray_target.position = new Vector3(mouseInGame.point.x, 5f, mouseInGame.point.z);
                        ray_target.localScale = Vector3.one * floatingScale;
                    }
                }

                if (ray_target != null)
                {
                    ray_target.position = new Vector3(mouseInGame.point.x, 5f, mouseInGame.point.z);

                    if (celling != null)
                    {
                        celling.gameObject.SetActive(true);
                    }
                }
                else
                {
                    if (celling != null)
                    {
                        celling.gameObject.SetActive(false);
                    }
                }


                if (Input.GetMouseButtonUp(0))
                {
                    if (!hold) { return; }
                    this.hold = false;

                    if (Maths.RoughlyEqual(new Vector3(ray_target.position.x, 0f, ray_target.position.z), new Vector3(GetClosestTile(mouseInGame.point).position.x, 0f, GetClosestTile(mouseInGame.point).position.z), 4f))
                    {
                        if (GetClosestTile(mouseInGame.point).childCount > 0)
                        {
                            if (GetClosestTile(mouseInGame.point).GetChild(0).tag == ray_target.tag && ray_target.tag != forms[forms.Length - 1].tag)
                            {
                                ray_target.position = GetClosestTile(mouseInGame.point).position;
                                ray_target.SetParent(GetClosestTile(mouseInGame.point));

                                if (GetClosestTile(mouseInGame.point).childCount >= 2)
                                {
                                    for (var i = 0; i < forms.Length; i++)
                                    {
                                        if (ray_target.tag == this.forms[i].tag)
                                        {
                                            for (int c = 0; c < GetClosestTile(mouseInGame.point).childCount; c++)
                                            {
                                                Destroy(GetClosestTile(mouseInGame.point).GetChild(c).gameObject);
                                            }
                                            Instantiate(this.forms[i + 1], GetClosestTile(mouseInGame.point).position, Quaternion.identity, GetClosestTile(mouseInGame.point));

                                        }
                                    }
                                }
                            }
                            else
                            {
                                this.ray_target.position = previous_position;
                            }
                        }
                        else
                        {
                            ray_target.position = GetClosestTile(mouseInGame.point).position;
                            ray_target.SetParent(GetClosestTile(mouseInGame.point));
                        }
                    }
                    else
                    {
                        this.ray_target.position = previous_position;
                    }
                    this.ray_target = null;


                    //Debug
                    debugRayColor = Color.yellow;
                }
                Debug.DrawRay(this.ray.origin, this.ray.direction * 1000, debugRayColor);
            }
        }


        private Transform GetClosestTile(Vector3 position, string tagToSearch = "Tiles")
        {
            float closestDistance = Mathf.Infinity;
            Transform closestTile = null;
            GameObject[] tiles;

            tiles = GameObject.FindGameObjectsWithTag(tagToSearch);

            for (var i = 0; i < tiles.Length; i++)
            {
                float dist = Vector3.Distance(position, tiles[i].transform.position);
                if (1 < dist && dist < closestDistance)
                {
                    closestDistance = dist;
                    closestTile = tiles[i].transform;
                }
            }

            return closestTile;
        }

        public bool CheckTarget(String target_Tag)
        {
            RaycastHit hitInfo;

            if (Physics.Raycast(this.ray, out hitInfo) && hitInfo.collider.tag == target_Tag)
            {
                ray_target = hitInfo.transform;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
