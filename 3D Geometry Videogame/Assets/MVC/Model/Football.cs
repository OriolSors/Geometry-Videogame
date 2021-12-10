using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Football : Minigame
{

    public Football(int currentWave, Dictionary<int, bool> isFigureCollectedInWave, int numberOfFigures) : base(currentWave, isFigureCollectedInWave)
    {
        SetWaveNumberToSpawn(numberOfFigures);
    }

    public Football(SaveDataMinigame footballGame): base(footballGame)
    {

    }

    private void SetWaveNumberToSpawn(int numberOfFigures)
    {
        int quotient_down = Convert.ToInt32(Math.Floor((float)numberOfFigures / 2f)); //TODO: tenir en compte el joc Collect Game, aixi que s'haura de modificar aixo
        int waveSpawnNumber = quotient_down;

        var rand = new System.Random();

        foreach (int wavePos in Enumerable.Range(1, 5).OrderBy(x => rand.Next()).Take(waveSpawnNumber))
        {
            isFigureCollectedInWave[wavePos] = false;
        }

    }

    public SaveDataMinigame WriteToDB()
    {
        SaveDataMinigame saveFootballDataToDB = new SaveDataMinigame(currentWave, isFigureCollectedInWave);
        return saveFootballDataToDB;
    }
}
