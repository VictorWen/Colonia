using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class HeroController : MonoBehaviour
{
    public Camera cam;
    public Grid grid;
    public WorldTerrain terrain;
    public Tilemap movement;
    public TileBase gold;
    public TileBase cyan;

    public GUIMaster gui; // TODO: change to GUIStateManager

    public Text pedometer;
    private int steps = 0;

    public int speed;

    private bool selected = false;
    private HashSet<Vector3Int> moveablePos = new HashSet<Vector3Int>();

    private void Start()
    {
        cam.transform.position = transform.position + new Vector3(0, 0, -500);
    }

    private void OnMouseOver()
    {
        if (gui.GUIState.UnitControl)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (!selected)
                {
                    CalculateMovables();
                    movement.SetTile(grid.WorldToCell(transform.position), gold);
                    selected = true;
                }
                else
                {
                    ClearMovables();
                    movement.SetTile(grid.WorldToCell(transform.position), null);
                    selected = false;
                }
            }
        }
    }

    private void Update()
    {
        if (selected && Input.GetMouseButtonUp(0))
        {
            Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = grid.WorldToCell(pos);
            if (moveablePos.Contains(gridPos))
            {
                movement.SetTile(grid.WorldToCell(transform.position), null);
                ClearMovables();
                transform.position = grid.CellToWorld(gridPos);
                selected = false;
                steps++;
                pedometer.text = "STEPS: " + steps;
            }
        }
    }

    private void CalculateMovables()
    {
        Vector2[] checks = new Vector2[] { new Vector2(-1, 0), new Vector2(-0.5f, 0.75f), new Vector2(0.5f, 0.75f), new Vector2(1, 0), new Vector2(0.5f, -0.75f), new Vector2(-0.5f, -0.75f) };

        List<Vector3> queue = new List<Vector3>();
        List<float> moves = new List<float>();
        Vector3 snap = grid.CellToWorld(grid.WorldToCell(transform.position));
        moveablePos.Add(grid.WorldToCell(transform.position));
        queue.Add(snap);
        moves.Add(0);

        while(queue.Count > 0)
        {
            foreach(Vector2 v in checks)
            {
                Vector3 tilePos = queue[0] + (Vector3)v;
                float cost = terrain.IsReachable(speed - moves[0], grid.WorldToCell(tilePos));
                Vector3Int gridTilePos = grid.WorldToCell(tilePos);
                if (!moveablePos.Contains(gridTilePos) && cost >= 0){
                    movement.SetTile(gridTilePos, cyan);
                    moveablePos.Add(gridTilePos);
                    
                    queue.Add(tilePos);
                    moves.Add(cost + moves[0]);
                }
            }
            queue.RemoveAt(0);
            moves.RemoveAt(0);
        }
        moveablePos.Remove(grid.WorldToCell(transform.position));
    }

    private void ClearMovables()
    {
        foreach (Vector3Int tilePos in moveablePos)
        {
            movement.SetTile(tilePos, null);
        }
        moveablePos.Clear();
    }
}
