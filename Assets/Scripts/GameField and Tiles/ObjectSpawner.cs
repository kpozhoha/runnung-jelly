using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// paeriodicly spawns object on gamefield it is attached to
/// </summary>
public class ObjectSpawner : MonoBehaviour
{
    public GameObject obj;
    public int jumpsToCreateObject = 1;

    private GameField gameField;
    private int jumpsAfterLastSpawn;

    private void Awake() {
        gameField = GetComponent<GameField>();
        if (gameField == null) {
            throw new System.Exception("ObjectSpawner can only be attached to gameObject with GameField component");
        }
        if (obj.GetComponent<IGeneratable>() == null) {
            throw new System.Exception("object must have component, that implemets IGeneratable interface");
        }
        jumpsAfterLastSpawn = 0;
    }

    public void onTileJump(Tile currentTile) {
        jumpsAfterLastSpawn++;
        if (shouldGenerateObject()) {
            spawnObject(currentTile);
        }
    }

    public void reset() {
        obj.GetComponent<IGeneratable>().reset();
    }

    private bool shouldGenerateObject() {
        return !obj.GetComponent<IGeneratable>().isVisible() && jumpsToCreateObject <= jumpsAfterLastSpawn;
    }

    private void spawnObject(Tile currentTile) {
        int place = Random.Range(0, gameField.tilesNumber * gameField.tilesNumber);
        Vector2Int pos = new Vector2Int();
        pos.x = place % gameField.tilesNumber;
        pos.y = place / gameField.tilesNumber;
        Tile tile = gameField.field[pos.x, pos.y];
        while (true) {
            if (tile != null && tile != currentTile && tile.occupied == false && !(tile is ThornTile) &&
                Mathf.Abs(pos.x - gameField.player.x) + Mathf.Abs(pos.y - gameField.player.y) >= 2) {
                Vector3 position = new Vector3(
                    pos.x * (gameField.tileWidth + gameField.tileInterval),
                    5,
                    pos.y * (gameField.tileWidth + gameField.tileInterval));
                obj.GetComponent<IGeneratable>().generate(position, tile);
                //animator.Play("CoinAppear");
                jumpsAfterLastSpawn = 0;
                jumpsToCreateObject = 1;
                break;
                //Random.Range(15, tilesNumber * tilesNumber - 5)
            } else {
                pos.x = (pos.x + 1) % 5;
                if (pos.x == 0) {
                    pos.y = (pos.y + 1) % 5;
                }
                tile = gameField.field[pos.x, pos.y];
            }
        }
    }
}
