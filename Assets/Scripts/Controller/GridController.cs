using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridController : MonoBehaviour
{
    public GameObject hexagonPrefab;
    public Transform root;
    public GameObject groupItem;

    [SerializeField]
    private float hexRadius;
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;

    private float groupItemOffset = 11;

    private Dictionary<Hex, GameObject> grids = new Dictionary<Hex, GameObject>();

    Hex selectedHex = null;

    private void Start()
    {
        CreateHexagonMap();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (UICamera.selectedObject != null)
            {
                selectedHex = UICamera.selectedObject.GetComponent<Hex>();
                if (selectedHex != null)
                {
                    MakeNormal();
                    var neighbors = FindNeighbors(selectedHex);
                    neighbors.ForEach(c => c.MakeRed());
                    FindClosestThreeHex(neighbors);
                }
            }
        }
    }

    private HexGroup FindClosestThreeHex(List<Hex> hexagons)
    {
        HexGroup hexGroup = new HexGroup { MainHex = selectedHex };

        foreach (var item in hexagons)
            item.distance = Vector3.Distance(UICamera.lastWorldPosition, item.transform.position);
        hexagons.Sort((x, y) => x.distance.CompareTo(y.distance));
        // Check if neighbors have 
        foreach (var item in hexagons)
        {
            if (hexGroup.FirstNeighbor == null)
            {
                hexGroup.FirstNeighbor = item;
                item.Nominate();
            }
            else
            {
                if (Mathf.Abs(hexGroup.FirstNeighbor.x - item.x) > 1 || Mathf.Abs(hexGroup.FirstNeighbor.y - item.y) > 1) continue;
                hexGroup.SecondNeighbor = item;
                item.Nominate();
                break;
            }
        }
        RepositionGroupItem(hexGroup);
        return hexGroup;
    }

    private void RepositionGroupItem(HexGroup hexGroup)
    {
        Vector3 pos = hexGroup.MedianPos();
        groupItem.transform.position = pos;
        //if (hexGroup.FirstNeighbor.neighborNumber == )
        //{
        //    groupItem.transform.rotation = Quaternion.Euler(0, 0, 180);
        //    groupItem.transform.position = new Vector3(groupItem.transform.position.x + groupItemOffset, groupItem.transform.position.y, 0);
        //}
        //else
        //{

        //}
    }

    private List<Hex> FindNeighbors(Hex hex)
    {
        List<Hex> tempList = new List<Hex>();

        int yOffset = hex.x % 2 == 0 ? 0 : 2;
        foreach (var item in grids.Keys)
        {
            if (item.x == hex.x && item.y == hex.y - 1)
            {
                item.neighborNumber = 0;
                tempList.Add(item);
            }
            else if (item.x == hex.x - 1 && item.y == hex.y)
            {
                item.neighborNumber = 1;
                tempList.Add(item);
            }
            else if (item.x == hex.x - 1 && item.y == hex.y + yOffset - 1)
            {
                item.neighborNumber = 2;
                tempList.Add(item);
            }
            else if (item.x == hex.x && item.y == hex.y + 1)
            {
                item.neighborNumber = 3;
                tempList.Add(item);
            }
            else if (item.x == hex.x + 1 && item.y == hex.y + yOffset - 1)
            {
                item.neighborNumber = 4;
                tempList.Add(item);
            }
            else if (item.x == hex.x + 1 && item.y == hex.y)
            {
                item.neighborNumber = 5;
                tempList.Add(item);
            }
        }


        //List<Hex> returnList = grids.Keys.Where(c => //(c.x == hex.x - 1 && c.y == hex.y) || (c.x == hex.x + 1 && c.y == hex.y) ||
        //                                             //(c.x == hex.x && c.y == hex.y - 1) || //(c.x == hex.x && c.y == hex.y + 1) ||
        //                                             //(c.x == hex.x - 1 && c.y == hex.y + yOffset - 1) || //(c.x == hex.x + 1 && c.y == hex.y + yOffset - 1)).ToList();

        return tempList;
    }

    private void MakeNormal()
    {
        foreach (var item in grids)
        {
            item.Key.MakeNormal();
        }
    }

    public void CreateHexagonMap()
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

public class HexGroup
{
    public Hex MainHex;
    public Hex FirstNeighbor;
    public Hex SecondNeighbor;

    public Vector3 MedianPos()
    {
        return new Vector3((MainHex.transform.position.x + FirstNeighbor.transform.position.x + SecondNeighbor.transform.position.x) / 3, (MainHex.transform.position.y + FirstNeighbor.transform.position.y + SecondNeighbor.transform.position.y) / 3, 0);
    }
}