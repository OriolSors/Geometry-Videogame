using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenges
{
    private Dictionary<string, Dictionary<string, List<string>>> challenges;

    private Challenges()
    {
        challenges = new Dictionary<string, Dictionary<string, List<string>>>()
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
    }

    private static Challenges instance = null;
    public static Challenges Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Challenges();
            }
            return instance;
        }
    }

    public Dictionary<string, Dictionary<string, List<string>>> GetChallenges()
    {
        return challenges;
    }

}
