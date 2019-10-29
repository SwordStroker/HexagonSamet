using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridController : MonoBehaviour
{
    public GameObject hexagonPrefab;
    public Transform root;

    [SerializeField]
    private float hexRadius;
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;

    private Dictionary<Hex, GameObject> grids = new Dictionary<Hex, GameObject>();

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (UICamera.selectedObject != null)
            {
                var selectedHex = UICamera.selectedObject.GetComponent<Hex>();
                if (selectedHex != null)
                {
                    MakeNormal();
                    var neighbors = FindNeighbors(selectedHex);
                    neighbors.ForEach(c => c.MakeRed());
                    FindClosestThreeHex(neighbors).ForEach(c => c.Nominate());

                }
            }
        }
    }

    private List<Hex> FindClosestThreeHex(List<Hex> hexagons)
    {
        List<Hex> returnList = new List<Hex>();

        foreach (var item in hexagons)
            item.distance = Vector3.Distance(UICamera.lastWorldPosition, item.transform.position);
        hexagons.Sort((x, y) => x.distance.CompareTo(y.distance));
        // Check if neighbors have 
        foreach (var item in hexagons)
        {
            if (returnList.Count == 0)
                returnList.Add(item);
            else
            {
                var firstNeighbor = returnList[0];
                if (Mathf.Abs(firstNeighbor.x - item.x) > 1 || Mathf.Abs(firstNeighbor.y - item.y) > 1) continue;
                returnList.Add(item);
                break;
            }
        }
        return returnList;
    }

    private List<Hex> FindNeighbors(Hex hex)
    {
        int yOffset = 2;
        if (hex.x % 2 == 0)
            yOffset = 0;
        List<Hex> returnList = grids.Keys.Where(c => (c.x == hex.x - 1 && c.y == hex.y) || (c.x == hex.x + 1 && c.y == hex.y) ||
                                                     (c.x == hex.x && c.y == hex.y - 1) || (c.x == hex.x && c.y == hex.y + 1) ||
                                                     (c.x == hex.x - 1 && c.y == hex.y + yOffset - 1) || (c.x == hex.x + 1 && c.y == hex.y + yOffset - 1)).ToList();

        return returnList;
    }

    private void MakeNormal()
    {
        foreach (var item in grids)
        {
            item.Key.MakeNormal();
        }
    }

    public void CreateMap()
    {
        ClearGrid();

        Vector3 pos = Vector3.zero;
        for (int q = 0; q < width; q++)
        {
            int qOff = q >> 1;
            for (int r = -qOff; r < height - qOff; r++)
            {
                pos.x = hexRadius * 3.0f / 2.0f * q;
                pos.y = hexRadius * Mathf.Sqrt(3.0f) * (r + q / 2.0f);

                var obj = Instantiate(hexagonPrefab, pos, Quaternion.identity);
                var hex = obj.AddComponent<Hex>();
                obj.transform.parent = root;
                obj.name = $"Hex[{q}, {r + qOff}]";
                obj.transform.localScale = Vector3.one;
                obj.transform.position = pos;
                hex.x = q;
                hex.y = r + qOff;

                grids.Add(hex, obj);
            }
        }
    }

    public void ClearGrid()
    {
        foreach (var grid in grids)
            DestroyImmediate(grid.Value, false);
        grids.Clear();
    }
}
