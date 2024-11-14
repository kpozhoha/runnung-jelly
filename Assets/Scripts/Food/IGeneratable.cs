using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGeneratable {
    void generate(Vector3 position, Tile currentTile);

    bool isVisible();

    void reset();
}
