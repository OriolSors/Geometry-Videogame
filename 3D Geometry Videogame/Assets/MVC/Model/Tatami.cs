using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tatami : Minigame
{
    public Tatami(int currentWave, Dictionary<int, bool> isFigureCollectedInWave): base(currentWave, isFigureCollectedInWave)
    {

    }
}
