using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class TilesAppearAnimator : MonoBehaviour
{
    private GameField gameField;
    private Tile[,] tiles;
    [SerializeField]
    private GameObject inputManager;

    private void Awake() {
        gameField = GetComponent<GameField>();
        if (gameField == null) {
            throw new System.Exception("script GameField must be attached to gameField object");
        }
    }

    public void setArr(Tile[,] tiles)
    {
        this.tiles = tiles;
    }
    

/// <summary>
/// string animation
/// </summary>
/// <returns></returns>
    // public IEnumerator animateGeneration()
    // {
    //     Time.timeScale = 2;
    //     for (int i = 0; i < tiles.GetLength(1); i++)
    //     {
    //         yield return new WaitForSeconds(0.1f);
    //         for (int j = 0; j < tiles.GetLength(1); j++)
    //         {
    //             tiles[j,j].GetComponent<Rigidbody>().AddForce(new Vector3(0, 2, 0));
    //             tiles[j, j].GetComponent<Rigidbody>().useGravity = true;
    //             tiles[j, j].GetComponent<Renderer>().enabled = true;
    //         }
    //     }
    // }

    public IEnumerator animateGeneration()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 3;
        for (int j = 0; j < tiles.GetLength(1); j++)
        {
            for (int k = 0; k <= j; k++)
            {
                Rigidbody rb = tiles[j-k, k].GetComponent<Rigidbody>(); 
                rb.AddForce(new Vector3(0, 3, 0));
                rb.useGravity = true;
                tiles[j-k, k].GetComponent<Renderer>().enabled = true;
            }
            yield return new WaitForSeconds(0.15f);
        }
        
        for (int j = 1; j < tiles.GetLength(1); j++)
        {
            for (int k = j; k <= tiles.GetLength(1) - 1; k++)
            {
                Rigidbody rb = tiles[k,j+tiles.GetLength(1) - 1 - k].GetComponent<Rigidbody>(); 
                rb.AddForce(new Vector3(0, 3, 0));
                rb.useGravity = true;
                tiles[k , j+tiles.GetLength(1) - 1 - k].GetComponent<Renderer>().enabled = true;
            }
            yield return new WaitForSeconds(0.15f);
        }
        Time.timeScale = GameField.getTimeScale();
    }

public static void animateSigleTileAppearence(Tile tile)
{
    Rigidbody rb = tile.GetComponent<Rigidbody>(); 
    rb.AddForce(new Vector3(0, 3, 0));
    rb.useGravity = true;
    tile.GetComponent<Renderer>().enabled = true;
}
}