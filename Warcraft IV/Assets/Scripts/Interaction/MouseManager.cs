using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SimpleFogOfWar;

public class MouseManager : MonoBehaviour
{
        public static MouseManager Current;

        public MouseManager()
        {
                Current = this;
        }

        [SerializeField] List<Interactive> selections = new List<Interactive>();
        [SerializeField] Camera childCamera;
        [SerializeField] RawImage portraitImage;
        bool unitPortrait = false;
        bool isDragSelecting = false;
        Vector3 mousePosition1;
        float lastClickTime;
        [SerializeField] float catchTime;

        void Update ()
        {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                        if (Input.GetMouseButtonDown(0))
                        {
                                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                RaycastHit hit;

                                if (Physics.Raycast(ray, out hit))
                                {
                                        Interactive interact = hit.transform.GetComponent<Interactive>();

                                        if (selections.Count > 0)
                                        {
                                                unitPortrait = false;
                                                childCamera.gameObject.transform.parent.gameObject.layer = 0;
                                                childCamera.gameObject.transform.parent.GetChild(0).gameObject.layer = 0;
                                                childCamera.gameObject.SetActive(unitPortrait);
                                                portraitImage.enabled = unitPortrait;
                                                foreach (Interactive sel in selections)
                                                {
                                                        sel.Deselect();
                                                }
                                                selections.Clear();
                                        }

                                        if (Time.time - lastClickTime < catchTime)
                                        {
                                                if (interact != null)
                                                {
                                                        if (FogOfWarSystem.Current.GetVisibility(hit.transform.position) == FogOfWarSystem.FogVisibility.Visible)
                                                        {
                                                                Unit unit = interact.gameObject.GetComponent<Unit>();

                                                                foreach (Unit u in FindObjectsOfType<Unit>())
                                                                {
                                                                        if (unit.name == u.name)
                                                                        {
                                                                                Interactive sel = u.gameObject.GetComponent<Interactive>();
                                                                                selections.Add(sel);
                                                                                sel.Select();  
                                                                        }
                                                                }
                                                                isDragSelecting = false;
                                                                return;
                                                        }
                                                }
                                        }
                                        else
                                        {
                                                if (interact != null)
                                                {
                                                        if (FogOfWarSystem.Current.GetVisibility(hit.transform.position) == FogOfWarSystem.FogVisibility.Visible)
                                                        {
                                                                selections.Add(interact);
                                                                interact.Select();
                                                        }
                                                }

                                        }
                                        lastClickTime = Time.time;
                                        mousePosition1 = Input.mousePosition;
                                }

                                isDragSelecting = true;
                        }
                        else if (Input.GetMouseButtonUp(0))
                        {
                                foreach (Interactive sel in FindObjectsOfType<Interactive>())
                                {
                                        if (IsWithinSelectionBounds(sel.gameObject) && sel.gameObject.tag != "Building" && sel.gameObject.GetComponent<Player>().Info == RTSManager.Current.Players[0])
                                        {
                                                selections.Add(sel);
                                                sel.Select();
                                        }
                                        if ((sel.gameObject.tag == "Building" || sel.gameObject.GetComponent<Player>().Info != RTSManager.Current.Players[0]) && selections.Count > 1)
                                        {
                                                unitPortrait = false;
                                                sel.Deselect();
                                                selections.Remove(sel);
                                        }
                                }

                                isDragSelecting = false;
                        }

                        if (Input.GetMouseButtonDown(1))
                        {
                                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                RaycastHit hit;

                                if (Physics.Raycast(ray, out hit))
                                {
                                        foreach (Interactive selection in selections)
                                        {
                                                if (selection.gameObject.GetComponent<Player>().Info == RTSManager.Current.Players[0])
                                                {
                                                        Unit unit = selection.gameObject.GetComponent<Unit>();

                                                        if (unit.IsMovable)
                                                        {
                                                                Pathfinding pathfinding = selection.gameObject.GetComponent<Pathfinding>();
                                                                pathfinding.SendToTarget(hit.point); 
                                                        }
                                                        if (hit.collider.tag == "Unit" || hit.collider.tag == "Building")
                                                        {
                                                                PlayerSetup Info = hit.collider.gameObject.GetComponent<Player>().Info;

                                                                if (Info != RTSManager.Current.Players[0])
                                                                {
                                                                        StartCoroutine(hit.collider.gameObject.GetComponent<Highlight>().Targeted());
                                                                        if (Info.Faction != PlayerSetup.Factions.Neutral)
                                                                        {
                                                                                if (unit.HasAttack)
                                                                                {
                                                                                        selection.gameObject.GetComponent<Combat>().Aggression(hit.collider.gameObject);
                                                                                }
                                                                        }
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }

                        if (selections.Count > 0 && unitPortrait == false)
                        {
                                unitPortrait = true;
                                int randomUnit = Random.Range(0, selections.Count);
                                CapsuleCollider collider = selections[randomUnit].GetComponent<CapsuleCollider>();
                                Player info = selections[randomUnit].gameObject.GetComponent<Player>();
                                selections[randomUnit].gameObject.layer = 8;
                                selections[randomUnit].transform.GetChild(0).gameObject.layer = 8; 
                                portraitImage.enabled = unitPortrait;
                                childCamera.gameObject.SetActive(unitPortrait);
                                childCamera.backgroundColor = info.Info.AccentColor;
                                childCamera.transform.parent = selections[randomUnit].transform;
                                childCamera.transform.position = selections[randomUnit].transform.position + ((collider.center.y + collider.height) * Vector3.up) + (selections[randomUnit].transform.forward * (collider.height + collider.radius));
                                childCamera.transform.LookAt(selections[randomUnit].transform.position + collider.center);
                        }
                }
        }
            
        public List<Interactive> Selections
        {
                get{ return selections; }
        }

        void OnGUI ()
        {
                if (isDragSelecting)
                {
                        Rect rect = GetScreenRectangle(mousePosition1, Input.mousePosition);
                        DrawScreenRectangle(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
                        DrawScreenRectangleBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
                }
        }

        public bool IsWithinSelectionBounds (GameObject gameObj)
        {
                Camera camera = Camera.main;
                Bounds viewportBounds = GetViewportBounds(camera, mousePosition1, Input.mousePosition);
                return viewportBounds.Contains(camera.WorldToViewportPoint(gameObj.transform.position));
        }

        public static class Utils
        {
                static Texture2D whiteTexture;
                public static Texture2D WhiteTexture
                {
                        get
                        {
                                if (whiteTexture == null)
                                {
                                        whiteTexture = new Texture2D(1, 1);
                                        whiteTexture.SetPixel(0, 0, Color.white);
                                        whiteTexture.Apply();
                                }

                                return whiteTexture;
                        }
                }
        }

        public static void DrawScreenRectangle (Rect rect, Color color)
        {
                GUI.color = color;
                GUI.DrawTexture(rect, Texture2D.whiteTexture);
                GUI.color = Color.white;
        }

        public static void DrawScreenRectangleBorder (Rect rect, float thickness, Color color)
        {
                // Top Edge Drawn
                DrawScreenRectangle(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
                // Left Edge Drawn
                DrawScreenRectangle(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
                // Right Edge Drawn
                DrawScreenRectangle(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
                // Bottom Edge Drawn
                DrawScreenRectangle(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        }
	
        public static Rect GetScreenRectangle (Vector3 screenPosition1, Vector3 screenPosition2)
        {
                screenPosition1.y = Screen.height - screenPosition1.y;
                screenPosition2.y = Screen.height - screenPosition2.y;

                Vector3 corner1 = Vector3.Min(screenPosition1, screenPosition2);
                Vector3 corner2 = Vector3.Max(screenPosition1, screenPosition2);

                return Rect.MinMaxRect(corner1.x, corner1.y, corner2.x, corner2.y);
        }

        public static Bounds GetViewportBounds (Camera camera, Vector3 screenPosition1, Vector3 screenPosition2)
        {
                Vector3 viewportPosition1 = Camera.main.ScreenToViewportPoint(screenPosition1);
                Vector3 viewportPosition2 = Camera.main.ScreenToViewportPoint(screenPosition2);
                Vector3 min = Vector3.Min(viewportPosition1, viewportPosition2);
                Vector3 max = Vector3.Max(viewportPosition1, viewportPosition2);
                min.z = camera.nearClipPlane;
                max.z = camera.farClipPlane;

                Bounds bounds = new Bounds();
                bounds.SetMinMax(min, max);
                return bounds;
        }
}
