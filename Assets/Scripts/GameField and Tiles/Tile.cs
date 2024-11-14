using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour {
    public bool occupied;
    private bool wasStopped = false;

    protected Vector2Int coordinates;
    public int jumpsToDestroy = 1;
    protected GameField gameField;
    private Transform tr;

    private void Awake() {
        gameObject.tag = "Tile";
        occupied = false;
        tr = GetComponent<Transform>();
    }
    
    protected abstract void interact(GameObject player);

    public void setCoordinates(Vector2Int vec)
    {
        coordinates = vec;
    }

    protected bool shouldBeDestroyed()
    {
        return jumpsToDestroy == 0;
    }

    public Vector2Int getCoordinates()
    {
        return new Vector2Int(coordinates.x, coordinates.y);
    }

    public void setGameField(GameField gf)
    {
        gameField = gf;
    }

    public virtual void delete()
    {
        gameField.fieldController.deleteTile(this);
        Destroy(gameObject);
    }

    public virtual void Notify(GameObject obj) {
        interact(obj);
        gameField.onTileJump(this);
    }

    public void FixedUpdate()
    {
        
        // for generation animation to stop after force////
        if (!wasStopped && tr.position.y < 0)
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic = true;
            wasStopped = true;
        }
    }
}

