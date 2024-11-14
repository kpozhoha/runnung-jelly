using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// handles all spawning / destruction of tiles in the field
/// </summary>
[RequireComponent(typeof(GameField))]
public class FieldController : MonoBehaviour
{
    public Tile startTilePrefab;
    public Tile[] tilePrefabs;
    public int[] ONLY_FOR_INSPECTOR_tileLimits;

    private Dictionary<System.Type, int> tileLimits;
    private Dictionary<System.Type, int> tileCounters;

    private GameField gameField;
    private List<Vector2Int> emptyTiles;
    [SerializeField]
    private int jumpsToNewTile = 4;
    private Queue<Vector2Int> unreachableTiles;
    [SerializeField]
    private int jumpsToDeleteUnreachableTile = 4;

    private void Awake() {
        gameField = GetComponent<GameField>();

        emptyTiles = new List<Vector2Int>();
        unreachableTiles = new Queue<Vector2Int>();

        if (ONLY_FOR_INSPECTOR_tileLimits.Length != tilePrefabs.Length) {
            throw new System.Exception("tile limits must be the same size as tilePrefabs");
        }
        tileLimits = new Dictionary<System.Type, int>();
        for (int i = 0; i < tilePrefabs.Length; ++i) {
            tileLimits.Add(tilePrefabs[i].GetType(), ONLY_FOR_INSPECTOR_tileLimits[i]);
        }

        tileCounters = new Dictionary<System.Type, int>();
        for (int i = 0; i < tilePrefabs.Length; ++i) {
            tileCounters.Add(tilePrefabs[i].GetType(), 0);
        }
    }

    public void generateMap() {
        Tile type;
        for (int i = 0; i < gameField.tilesNumber; i++) {
            for (int j = 0; j < gameField.tilesNumber; j++) {
                float x = i * (gameField.tileWidth + gameField.tileInterval);
                float z = j * (gameField.tileWidth + gameField.tileInterval);
                type = tilePrefabs[UnityEngine.Random.Range(0, tilePrefabs.Length)];
                if (i == gameField.tilesNumber / 2 - 1 && i == j) {
                    gameField.field[i, j] = Instantiate(startTilePrefab, new Vector3(x, 0, z),
                                                        Quaternion.identity);
                    gameField.field[i, j].setCoordinates(new Vector2Int(i, j));
                    gameField.field[i, j].setGameField(gameField);
                } else {
                    if (type is ThornTile && TileIsNear<ThornTile>(i, j)) {
                        type = tilePrefabs[0];
                    }
                    gameField.field[i, j] = CreateNewTile(TileRandomizer(new Vector2Int(i, j)), new Vector3(x, 0, z),
                                                      Quaternion.identity, new Vector2Int(i, j), gameField);
                }
                gameField.field[i, j].GetComponent<Renderer>().enabled = false;
            }
        }
        
    }

    public void animateGeneration()
    {
        StartCoroutine(gameField.tilesAppearAnimator.animateGeneration());
    }
    
    public void deleteTile(Tile tile) {
        if (!(tile is StartTile)) {
            tileCounters[tile.GetType()]--;
        }
        emptyTiles.Add(tile.getCoordinates());
    }

    private void recreateEmptyTile() {
        Vector2Int pos;
        if (UnityEngine.Random.Range(0,3) == 0 || true) {
            pos = TileRecreationAssistance();
        } else {
            pos = RandomEmptyTilePosition();
        }
        int i = pos.x;
        int j = pos.y;

        float x = i * (gameField.tileWidth + gameField.tileInterval);
        float z = j * (gameField.tileWidth + gameField.tileInterval);
        gameField.field[i, j] = CreateNewTile(TileRandomizer(new Vector2Int(i, j)), new Vector3(x, 0, z),
                                                Quaternion.identity, new Vector2Int(i, j), gameField);
        gameField.field[i, j].GetComponent<Renderer>().enabled = false;
        TilesAppearAnimator.animateSigleTileAppearence(gameField.field[i, j]);
    }

    public void clearField() {
        foreach (ObjectSpawner spawner in gameField.objectSpawners ?? System.Linq.Enumerable.Empty<ObjectSpawner>()) {
            spawner.reset();
        }

        foreach (Tile tile in gameField.field) {
            if (tile != null) {
                tile.delete();
            }

        }

        Dictionary<System.Type, int> copy = new Dictionary<System.Type, int>(tileCounters);
        foreach (var key in copy.Keys) {
            tileCounters[key] = 0;
        }
        emptyTiles.Clear();
        unreachableTiles.Clear();
    }

    public void onTileJump(Tile currentTile) {
        if (emptyTiles.Count != 0 && gameField.TotalJumps % jumpsToNewTile == 0) {
            recreateEmptyTile();
        }
        if (unreachableTiles.Count != 0 && gameField.TotalJumps % jumpsToDeleteUnreachableTile == 0) {
            Vector2Int pos = unreachableTiles.Dequeue();
            Tile tile = gameField.field[pos.x, pos.y];
            if (tile != null) {
                tileCounters[tile.GetType()]--;
                gameField.field[pos.x, pos.y].delete();
            }
        }
    }

    private Tile CreateNewTile(Tile type, Vector3 location, Quaternion rotation, Vector2Int coords, GameField gameField) {
        Tile tile;
        if (tileCounters[type.GetType()] < tileLimits[type.GetType()]) {
            tileCounters[type.GetType()]++;
            if (type is ThornTile) {
                tile = Instantiate(type, location, rotation);
                unreachableTiles.Enqueue(coords);
            } else {
                tile = Instantiate(type, location, rotation);
            }
        } else {
            tileCounters[tilePrefabs[0].GetType()]++;
            tile = Instantiate(tilePrefabs[0], location, rotation);
        }
        tile.setCoordinates(coords);
        tile.setGameField(gameField);
        return tile;
    }

    private bool TileIsNear<T>(int x, int y)
    {
        
        if (x > 0 && y < gameField.tilesNumber - 1 && gameField.field[x - 1, y + 1] != null &&
            gameField.field[x - 1, y + 1] is T) return true;
        if (y < gameField.tilesNumber - 1 && gameField.field[x, y + 1] != null &&
            gameField.field[x, y + 1] is T) return true;
        if (x < gameField.tilesNumber - 1 && y < gameField.tilesNumber - 1 && gameField.field[x + 1, y + 1] != null &&
            gameField.field[x + 1, y + 1] is T) return true;
        if (x < gameField.tilesNumber - 1 && gameField.field[x + 1, y] != null &&
            gameField.field[x + 1, y] is T) return true;
        if (x < gameField.tilesNumber - 1 && y > 0 &&gameField.field[x + 1, y - 1] != null &&
            gameField.field[x + 1, y - 1] is T) return true;
        if (y > 0 && gameField.field[x, y - 1] != null &&
            gameField.field[x, y - 1] is T) return true;
        if (x > 0 && y > 0 && gameField.field[x - 1, y - 1] != null &&
            gameField.field[x - 1, y - 1] is T) return true;
        if (x > 0 && gameField.field[x - 1, y] != null &&
            gameField.field[x - 1, y] is T) return true;
        return false;
    }

    private Tile TileRandomizer(Vector2Int pos) {
        int availableAmounts = 0;
        List<Tile> blockedTileTypes = new List<Tile>();
        foreach(Tile tile in tilePrefabs) {
            if ((tile is ThornTile && TileIsNear<ThornTile>(pos.x, pos.y)) ||
                (tile is SpringTile && TileIsNear<SpringTile>(pos.x, pos.y))) {
                blockedTileTypes.Add(tile);
                continue;
            }
            availableAmounts += tileLimits[tile.GetType()] - tileCounters[tile.GetType()];
        }
        if (availableAmounts <= 0 && blockedTileTypes.Count > 0) {
            return blockedTileTypes[UnityEngine.Random.Range(0, blockedTileTypes.Count)];
        }
        int rand = UnityEngine.Random.Range(1, availableAmounts + 1);
        foreach (Tile tile in tilePrefabs) {
            if ((tile is ThornTile && TileIsNear<ThornTile>(pos.x, pos.y)) ||
                (tile is SpringTile && TileIsNear<SpringTile>(pos.x, pos.y))) {
                continue;
            }
            rand -= tileLimits[tile.GetType()] - tileCounters[tile.GetType()];
            if (rand <= 0) {
                return tile;
            }
        }
        throw new System.Exception("something wrong in tile type randomization");
    }

    /// <summary>
    /// returns one of empty spaces in field, after calling this function, returned coords are not considered as empty tile 
    /// </summary>
    /// <returns></returns>
    private Vector2Int RandomEmptyTilePosition() {
        //recently destroyed tile is out of random
        int ind = UnityEngine.Random.Range(0, emptyTiles.Count - 1);
        Vector2Int tileToBuild = new Vector2Int(emptyTiles[ind].x, emptyTiles[ind].y);
        emptyTiles.RemoveAt(ind);
        return tileToBuild;
    }

    private Vector2Int TileRecreationAssistance() {
        ObjectSpawner objSpawner = GetComponent<ObjectSpawner>();
        Vector2Int food;
        if (objSpawner != null && objSpawner.obj.tag.Equals("Food")) {
            food = objSpawner.obj.GetComponent<Food>().CurrentTile.getCoordinates();
        } else {
            throw new System.Exception("object Spawner not found or it doesnt spawn food");
        }
        HashSet<Vector2Int> foodComponent = ConnectedComponent(food);
        Vector2Int tileToBuild;
        if (!foodComponent.Contains(gameField.player)) {
            tileToBuild = FindShortestUnfinishedPath(gameField.player, foodComponent);
            if (!emptyTiles.Contains(tileToBuild)) {
                tileToBuild = RandomEmptyTilePosition();
            } else {
                emptyTiles.Remove(tileToBuild);
            }
        } else {
            tileToBuild = RandomEmptyTilePosition();
        }
        return tileToBuild;
    }

    /// <summary>
    /// return connected component that includes pos tile
    /// </summary>
    /// <param name="pos">member of component that will be returned</param>
    /// <returns></returns>
    private HashSet<Vector2Int> ConnectedComponent(Vector2Int pos) {
        HashSet<Vector2Int> component = new HashSet<Vector2Int>();
        Queue<Vector2Int> q = new Queue<Vector2Int>();
        q.Enqueue(pos);
        Vector2Int current;
        Vector2Int next;
        while (q.Count > 0) {
            current = q.Dequeue();
            component.Add(current);
            int koef = gameField.field[current.x, current.y] is SpringTile ? 2 : 1;
            next = new Vector2Int(current.x - koef, current.y);
            if (next.x >= 0 && gameField.field[next.x, next.y] != null && !(gameField.field[next.x, next.y] is ThornTile) && !component.Contains(next)) {
                q.Enqueue(next);
            }
            next = new Vector2Int(current.x + koef, current.y);
            if (next.x < gameField.tilesNumber && gameField.field[next.x, next.y] != null && !(gameField.field[next.x, next.y] is ThornTile) && !component.Contains(next)) {
                q.Enqueue(next);
            }
            next = new Vector2Int(current.x, current.y - koef);
            if (next.y >= 0 && gameField.field[next.x, next.y] != null && !(gameField.field[next.x, next.y] is ThornTile) && !component.Contains(next)) {
                q.Enqueue(next);
            }
            next = new Vector2Int(current.x, current.y + koef);
            if (next.y < gameField.tilesNumber && gameField.field[next.x, next.y] != null && !(gameField.field[next.x, next.y] is ThornTile) && !component.Contains(next)) {
                q.Enqueue(next);
            }
        }
        return component;
    }

    /// <summary>
    /// findsunfinished path with least empty tiles from "from" to connected component "to"
    /// </summary>
    /// <param name="from">start of path</param>
    /// <param name="to">end of path</param>
    /// <returns>one of null tile coords from unfinished path</returns>
    private Vector2Int FindShortestUnfinishedPath(Vector2Int from, HashSet<Vector2Int> to) {
        int minDist = int.MaxValue;
        Vector2Int[] coordPair = new Vector2Int[2];
        int dst;
        HashSet<Vector2Int> jellyComp = ConnectedComponent(from);
        foreach(Vector2Int start in jellyComp) {
            foreach(Vector2Int end in to) {
                dst = Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y);
                if (dst < minDist) {
                    minDist = dst;
                    coordPair[0] = start;
                    coordPair[1] = end;
                }
            }
        }
        Vector2Int tileToBuild = new Vector2Int();
        Func<int, int> zeroSign = a => a > 0 ? 1 : a < 0 ? -1 : 0;
        tileToBuild.x = coordPair[0].x + zeroSign.Invoke(coordPair[1].x - coordPair[0].x);
        tileToBuild.y = coordPair[0].y + zeroSign.Invoke(coordPair[1].y - coordPair[0].y);
        if (tileToBuild.x != coordPair[0].x && tileToBuild.y != coordPair[0].y) {
            if (UnityEngine.Random.Range(0,2) == 0) {
                tileToBuild.x = coordPair[0].x;
            } else {
                tileToBuild.y = coordPair[0].y;
            }
        }
        return tileToBuild;
    }

    public void godModeRegenerate()
    {
        int num = emptyTiles.Count;
        for (int i = 0; i < num; i++)
        {
            recreateEmptyTile();
        }
    }
}
