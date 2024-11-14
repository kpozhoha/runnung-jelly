using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {
    public int highScore;
    public int level;
    public int coins;
    public int activeSkin;
    public bool[] unlockedSkins;
    public int upgradePrice;
    public int lastUpgradedEdge;
    public int upgradedEdgeScore;
    public bool firstVisit;
    public System.DateTime freeSpinLastTime;

    public PlayerData(Jelly player) {
        highScore = player.HighScore;
        level = player.Level;
        coins = player.Coins;
        activeSkin = player.ActiveSkin;
        lastUpgradedEdge = player.lastUpgradedEdge;
        upgradePrice = player.upgradePrice;
        firstVisit = player.firstVisit;
        freeSpinLastTime = player.freeSpinLastTime;
        Edge[] edges = player.edges;
        upgradedEdgeScore = edges[lastUpgradedEdge].getScoresPerJump();
        if (player.UnlockedSkins != null) {
            unlockedSkins = new bool[player.UnlockedSkins.Length];
            for (int i = 0; i < player.UnlockedSkins.Length; ++i) {
                unlockedSkins[i] = player.UnlockedSkins[i];
            }
        } else {
            unlockedSkins = null;
        }
        firstVisit = player.firstVisit;
    }
}
