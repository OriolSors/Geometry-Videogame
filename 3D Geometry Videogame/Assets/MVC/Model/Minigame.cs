using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Minigame
{
    protected int currentWave;
    protected Dictionary<int, bool> isFigureCollectedInWave;

    public Minigame(int currentWave, Dictionary<int, bool> isFigureCollectedInWave)
    {
        this.currentWave = currentWave;
        this.isFigureCollectedInWave = isFigureCollectedInWave;
    }
}
