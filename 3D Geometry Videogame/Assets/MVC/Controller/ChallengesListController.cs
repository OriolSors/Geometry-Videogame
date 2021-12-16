using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChallengesListController
{
    private List<string> characteristics, prefabNames;

    private Dictionary<string, string> currentChallenges = new Dictionary<string, string>();

    private Dictionary<string, int> scores = new Dictionary<string, int>();

    private Dictionary<string, Dictionary<string, List<string>>> challenges = new Dictionary<string, Dictionary<string, List<string>>>()
        {
            { "Convexity", new Dictionary<string, List<string>>()
            {
                { "Pick all figures that are convex",  new List<string>(){"Cube" , "Capsule", "Cylinder", "Dodecahedron", "Icosahedron", "Tetrahedron", "Octahedron", "Prism"} },
                { "Pick all figures that are not convex", new List<string>(){"Fake" } }
            }},

            { "Regularity", new Dictionary<string, List<string>>()
            {
                { "Pick all figures that are regular",  new List<string>(){"Cube", "Dodecahedron", "Icosahedron", "Tetrahedron", "Octahedron" } },
                { "Pick all figures that are not regular", new List<string>(){"Fake", "Capsule", "Cylinder", "Prism" } }
            }},

            { "Faces", new Dictionary<string, List<string>>()
            {
                { "Pick all figures that have 4 faces",new List<string>(){ "Tetrahedron" } },
                { "Pick all figures that have 6 faces", new List<string>(){"Cube" , "Prism"} },
                { "Pick all figures that have 8 faces", new List<string>(){ "Octahedron" } },
                { "Pick all figures that have 12 faces", new List<string>(){ "Dodecahedron" } },
                { "Pick all figures that have 20 faces", new List<string>(){ "Icosahedron" } }
            } },

            { "Edges", new Dictionary<string, List<string>>()
            {

                { "Pick all figures that have 6 edges",new List<string>(){ "Tetrahedron" } },
                { "Pick all figures that have 12 edges", new List<string>(){"Cube","Prism", "Octahedron" } },
                { "Pick all figures that have 30 edges", new List<string>(){ "Dodecahedron", "Icosahedron" } }

            } },

            { "Vertices", new Dictionary<string, List<string>>()
            {

                { "Pick all figures that have 4 vertices", new List<string>(){ "Tetrahedron" } },
                { "Pick all figures that have 6 vertices", new List<string>(){ "Octahedron" } },
                { "Pick all figures that have 8 vertices", new List<string>(){"Cube", "Prism" } },
                { "Pick all figures that have 12 vertices", new List<string>(){ "Icosahedron" } },
                { "Pick all figures that have 20 vertices", new List<string>(){ "Dodecahedron" } }

            } },

            { "Euler", new Dictionary<string, List<string>>()
            {
                { "Pick all figures that have Euler characteristic = 2",  new List<string>(){ "Cube", "Dodecahedron", "Icosahedron", "Tetrahedron", "Octahedron", "Prism" } },
                { "Pick all figures that have Euler characteristic != 2", new List<string>(){ "Fake", "Capsule", "Cylinder" } }

            } },

            { "Faces per vertex", new Dictionary<string, List<string>>()

            {
                { "Pick all figures that have 3 faces per vertex", new List<string>(){"Cube", "Tetrahedron", "Dodecahedron", "Prism" } },
                { "Pick all figures that have 4 faces per vertex", new List<string>(){ "Octahedron" } },
                { "Pick all figures that have 5 faces per vertex", new List<string>(){ "Icosahedron" } }

            } },

            { "Vertices per face", new Dictionary<string, List<string>>()

            {
                { "Pick all figures that have 3 vertices per face",  new List<string>(){ "Icosahedron", "Tetrahedron", "Octahedron" } },
                { "Pick all figures that have 4 vertices per face", new List<string>(){"Cube", "Prism" } },
                { "Pick all figures that have 5 vertices per face", new List<string>(){ "Dodecahedron" } }

            } },

            { "Symmetry", new Dictionary<string, List<string>>()

            {
                { "Pick all figures that have axial symmetry",  new List<string>(){ "Cube", "Dodecahedron", "Icosahedron", "Tetrahedron", "Octahedron", "Capsule", "Cylinder", "Prism" } },
                { "Pick all figures that have specular symmetry", new List<string>(){ "Cube", "Dodecahedron", "Icosahedron", "Tetrahedron", "Octahedron", "Capsule", "Cylinder", "Prism" } },
                { "Pick all figures that have no symmetry", new List<string>(){"Fake" } }

            } },

            { "Uniformity", new Dictionary<string, List<string>>()
            {
                { "Pick all figures that are uniform",  new List<string>(){ "Cube", "Dodecahedron", "Icosahedron", "Tetrahedron", "Octahedron" } },
                { "Pick all figures that are not uniform", new List<string>(){ "Fake", "Capsule", "Cylinder", "Prism" } }
            } },

            { "Duality", new Dictionary<string, List<string>>()

            {
                { "Pick all figures which its dual is the tetrahedron", new List<string>(){ "Tetrahedron" } },
                { "Pick all figures which its dual is the hexahedron", new List<string>(){ "Octahedron" } },
                { "Pick all figures which its dual is the octahedron", new List<string>(){"Cube" } },
                { "Pick all figures which its dual is the dodecahedron", new List<string>(){ "Icosahedron" } },
                { "Pick all figures which its dual is the icosahedron", new List<string>(){ "Dodecahedron" } }

            } },

        };

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

            Debug.Log(figure + ": " + scores[figure]);
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
