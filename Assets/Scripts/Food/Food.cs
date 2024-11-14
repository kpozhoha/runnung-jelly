using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour, IGeneratable {
    public Tile CurrentTile { get; set; }

    private FlashingAnimation backgroundAnimation;

    private AudioSource audioSource;

    public void Start() {
        Vibration.Init();
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null) {
            throw new System.Exception("Renderer not found");
        }
        renderer.enabled = false;

        backgroundAnimation = GameObject.Find("FlashingBackground").GetComponent<FlashingAnimation>();

        audioSource = GetComponent<AudioSource>();
    }

    public void generate(Vector3 position, Tile currentTile) {
        CurrentTile = currentTile;
        currentTile.occupied = true;
        transform.localPosition = position;
        GetComponent<Renderer>().enabled = true;
    }

    public bool isVisible() {
        return GetComponent<Renderer>().enabled;
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Player")) {
            GetComponent<ColorChanger>().changeColorToNext();
            backgroundAnimation.Play();

            audioSource.Play();

            if (VibroButton.vibroEnabled)
            {
                long[] pattern = { 0, 60 };
                Vibration.Vibrate(pattern, -1);
            }
            reset();
        }
    }

    public void reset() {
        if (CurrentTile != null) {
            CurrentTile.occupied = false;
        }
        GetComponent<Renderer>().enabled = false;
        transform.position = new Vector3(-10, 4, -10);
    }
}
