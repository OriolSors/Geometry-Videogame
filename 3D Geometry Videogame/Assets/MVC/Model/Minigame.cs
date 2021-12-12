using System.Collections.Generic;

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

    public Minigame(SaveDataMinigame minigameData)
    {
        this.currentWave = minigameData.currentWave;
        this.isFigureCollectedInWave = minigameData.FiguresCollectedToDictionary();
    }

    public Dictionary<int,bool> GetFiguresInWaves()
    {
        return isFigureCollectedInWave;
    }
}
