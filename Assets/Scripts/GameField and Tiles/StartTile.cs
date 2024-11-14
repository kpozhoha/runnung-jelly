using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTile : Tile {
    private GameObject player;

    protected override void interact(GameObject player) {
        this.player = player;
        player.GetComponent<Jelly>().interactStartTile();
    }

    public override void Notify(GameObject obj) {
        interact(obj);
    }

    private void Update() {
        RectTransform rectTrans = GetComponent<RectTransform>();
        if (player != null && 
            rectTrans != null && 
            (Mathf.Abs(player.transform.localPosition.x - this.transform.localPosition.x) > rectTrans.rect.width / 2 ||
            Mathf.Abs(player.transform.localPosition.z - this.transform.localPosition.z) > rectTrans.rect.width / 2)) {
            delete();
        }
    }
}
