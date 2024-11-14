using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour
{
    public GUI gui;
    [System.NonSerialized]
    public Tile[,] field;
    public GameObject mainCamera;
    public int tilesNumber;
    public float tileInterval;
    [System.NonSerialized]
    public float tileWidth;
    public int TotalJumps { get; set; }
    #region Components
    [System.NonSerialized]
    public TilesAppearAnimator tilesAppearAnimator;
    [System.NonSerialized]
    public FieldController fieldController;
    [System.NonSerialized]
    public ObjectSpawner[] objectSpawners;
    #endregion
    [System.NonSerialized]
    public Vector2Int player;

    public const float MIN_TIMESCALE = 2.4f;
    private static float timeScale;
    private const float MAX_TIMESCALE = 4.0f;

    void Awake()
    {
        fieldController = GetComponent<FieldController>();
        if (fieldController == null) {
            throw new System.Exception("fieldGenreator script not found");
        }
        tilesAppearAnimator = GetComponent<TilesAppearAnimator>();
        if (tilesAppearAnimator == null) {
            throw new System.Exception("TilesApearAnimator script not found");
        }
        objectSpawners = GetComponents<ObjectSpawner>();

        field = new Tile[tilesNumber, tilesNumber];
        for(int i = 0; i < tilesNumber; i++)
        {
            for(int j = 0; j < tilesNumber; j++)
            {
                field[i, j] = null;
            }
        }
        tilesAppearAnimator.setArr(field);
        tileWidth = fieldController.tilePrefabs[0].GetComponent<RectTransform>().rect.size.x;
        TotalJumps = 0;
        // setCamera();
        //fieldController.generateMap();
        //StartCoroutine("s");
        //jumpsToCreateCoin = Random.Range(15, tilesNumber * tilesNumber - 5);
        timeScale = 2.3f;
        Time.timeScale = timeScale;
        Application.targetFrameRate = 60;
        
    }

    void Start()
    {
        fieldController.generateMap();
        fieldController.animateGeneration();
    }

    public static void setTimeScale(float ts)
    {
        timeScale = ts;
    }

    public static float getTimeScale()
    {
        return timeScale;
    }
    

    public void onTileJump(Tile currentTile) {
        TotalJumps++;
        player = currentTile.getCoordinates();
        fieldController.onTileJump(currentTile);
        foreach (ObjectSpawner spawner in objectSpawners ?? System.Linq.Enumerable.Empty<ObjectSpawner>()) {
            spawner.onTileJump(currentTile);
        }

        if (timeScale < MAX_TIMESCALE)
        {
            timeScale += 0.02f;
            Time.timeScale = timeScale;
        }
        
    }

    public void ResetField() {
        TotalJumps = 0;
        fieldController.clearField();
        fieldController.generateMap();
        fieldController.animateGeneration();
    }

    //void setCamera()
    //{
    //    mainCamera.transform.position =
    //        new Vector3(2.5f * tileWidth - mainCamera.GetComponent<RectTransform>().rect.size.x,
    //            2 * (5 * tileWidth + 4 * tileInterval) / 1.732f, -1.55f * (5 * tileWidth + 4 * tileInterval) / 1.732f);
    //    mainCamera.transform.Rotate(50, 0, 0);
    //}
}