using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornTile : Tile
{
    protected override void interact(GameObject player)
    {
        player.GetComponent<Jelly>().interactThornTile();
        delete();
    }
}
