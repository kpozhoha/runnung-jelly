using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableTile : Tile
{
    [SerializeField]
    private Mesh[] skins;

    private MeshFilter meshFilter;
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        jumpsToDestroy = 3;
        Vector3 angles = transform.rotation.eulerAngles;
        angles.y = Random.Range(0, 4) * 90f;
        Quaternion rotation = transform.rotation;
        rotation.eulerAngles = angles;
        transform.rotation = rotation;
    }

    protected override void interact(GameObject player)
    {
        //player.interactBreakableTile();
        --jumpsToDestroy;
        if (player.tag.Equals("Player")) {
            player.GetComponent<ScoreStreak>().RecentTileWasDestroyed(shouldBeDestroyed());
        }
        if (shouldBeDestroyed()) {
            delete();
            return;
        }
        meshFilter.mesh = skins[jumpsToDestroy - 1];
    }
}