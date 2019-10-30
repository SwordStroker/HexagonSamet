using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    public int x;
    public int y;
    public float distance;
    public int neighborNumber;

    UISprite sprite;

    private void Start()
    {
        sprite = GetComponent<UISprite>();
    }

    public void MakeRed()
    {
        sprite.color = Color.red;
    }

    public void MakeNormal()
    {
        neighborNumber = 0;
        sprite.color = Color.yellow;
    }

    public void Nominate()
    {
        sprite.color = Color.green;
    }
}
