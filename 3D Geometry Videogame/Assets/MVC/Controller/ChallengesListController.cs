using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChallengesListController
{
    private List<string> characteristics, prefabNames;

    private Dictionary<string, string> currentChallenges = new Dictionary<string, string>();

    private Dictionary<string, int> scores = new Dictionary<string, int>();

    private Dictionary<string, Dictionary<string, List<string>>> challenges = Challenges.Instance.GetChallenges();

    public ChallengesListController(List<string> characteristics, List<string> prefabNames)
    {
        this.characteristics = characteristics;
        this.prefabNames = prefabNames;

        SetChallengesAndScores();
    }

    public ChallengesListController(List<string> characteristics)
    {
        this.characteristics = characteristics;

    }

    public Dictionary<string, string> GetCurrentChallenges()
    {

        return currentChallenges;
    }

    public Dictionary<string, int> GetCurrentScores()
    {

        return scores;
    }

    

    private void SetChallengesAndScores()
    {
        var rand = new System.Random();
        bool found = false;
        int attemptCount = 1000;

        Dictionary<string, List<string>> chosenChallenges = new Dictionary<string, List<string>>();

        while (!found && attemptCount != 0)
        {
            chosenChallenges = new Dictionary<string, List<string>>();
            IEnumerable<string> chosenCharacteristics = characteristics.OrderBy(x => rand.Next()).Take(3);
            List<List<string>> figuresToCompare = new List<List<string>>();

            foreach (string characteristic in chosenCharacteristics)
            {
                KeyValuePair<string, List<string>> challenge = challenges[characteristic].ElementAt(rand.Next(0, challenges[characteristic].Count));
                chosenChallenges[challenge.Key] = challenge.Value;
                figuresToCompare.Add(challenge.Value);
            }
            if (!ThereIsSubset(figuresToCompare)) found = true;
            attemptCount--;
        }

        currentChallenges["good"] = chosenChallenges.ElementAt(0).Key;
        currentChallenges["neutral"] = chosenChallenges.ElementAt(1).Key;
        currentChallenges["bad"] = chosenChallenges.ElementAt(2).Key;

        foreach (string figure in prefabNames)
        {
            if (chosenChallenges.ElementAt(2).Value.Contains(figure)) scores[figure] = -999;
            else if (chosenChallenges.ElementAt(1).Value.Contains(figure)) scores[figure] = 0;
            else if (chosenChallenges.ElementAt(0).Value.Contains(figure)) scores[figure] = 1;
            else scores[figure] = -1;
        }

    }

    public bool CheckValidCharacteristics()
    {
        var rand = new System.Random();
        bool found = false;
        int attemptCount = 1000;

        Dictionary<string, List<string>> chosenChallenges = new Dictionary<string, List<string>>();

        while (!found && attemptCount != 0)
        {
            chosenChallenges = new Dictionary<string, List<string>>();
            IEnumerable<string> chosenCharacteristics = characteristics.OrderBy(x => rand.Next()).Take(3);
            List<List<string>> figuresToCompare = new List<List<string>>();

            foreach (string characteristic in chosenCharacteristics)
            {
                KeyValuePair<string, List<string>> challenge = challenges[characteristic].ElementAt(rand.Next(0, challenges[characteristic].Count));
                chosenChallenges[challenge.Key] = challenge.Value;
                figuresToCompare.Add(challenge.Value);
            }
            if (!ThereIsSubset(figuresToCompare)) found = true;
            attemptCount--;
        }

        return found;
    }

    private bool ThereIsSubset(List<List<string>> listOfFigures)
    {
        bool thereIsSubset = false;

        for (int i = 0; i < listOfFigures.Count; i++)
        {
            var groupFigures = listOfFigures.ElementAt(i);
            List<string> joinedFigures = new List<string>();
            for (int j = 0; j < listOfFigures.Count; j++)
            {

                if (listOfFigures.ElementAt(i).Equals(listOfFigures.ElementAt(j)))
                {
                    continue;
                }

                joinedFigures.AddRange(listOfFigures.ElementAt(j));

            }
            if (!groupFigures.Except(joinedFigures).Any())
            {
                thereIsSubset = true;
                break;
            }
        }

        return thereIsSubset;
    }
}
