using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringTile : Tile
{
    void Start()
    {
        jumpsToDestroy = 1;
    }

    protected override void interact(GameObject player)
    {
        player.GetComponent<Jelly>().interactSpringTile();
        --jumpsToDestroy;
        delete();
        if (player.tag.Equals("Player")) {
            player.GetComponent<ScoreStreak>().RecentTileWasDestroyed(true);
        }
    }
}