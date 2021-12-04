using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Football : Minigame
{
    public Football(int currentWave, Dictionary<int, bool> isFigureCollectedInWave) : base(currentWave, isFigureCollectedInWave)
    {

    }
}
