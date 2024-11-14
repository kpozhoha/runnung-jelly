using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IGeneratable
{
    public Tile CurrentTile { get; set; }
    private GameObject animatedCoin;
    private Renderer renderer;
    private Animator animator;
    public void Start()
    {
        animatedCoin = transform.Find("AnimatedCoin").gameObject;
        renderer = animatedCoin.GetComponent<Renderer>();
        animator = animatedCoin.GetComponent<Animator>();
        renderer.enabled = false;
    }
    
    public void generate(Vector3 position, Tile currentTile)
    {
        CurrentTile = currentTile;
        CurrentTile.occupied = true;
        transform.position = position;
        renderer.enabled = true;
        animator.Play("CoinAppear");
    }

    public bool isVisible()
    {
        return renderer.enabled;
    }

    public void interact()
    {
        renderer.enabled = false;
        transform.position = new Vector3(-10, 4, -10);
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Player")) {
            reset();
        }
    }

    public void reset() {
        if (CurrentTile != null) {
            CurrentTile.occupied = false;
        }
        renderer.enabled = false;
        transform.position = new Vector3(-10, 4, -10);
    }
}
