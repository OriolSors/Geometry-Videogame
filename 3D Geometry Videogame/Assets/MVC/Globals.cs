using System.Collections;
using System.Text;
using UnityEngine;

public static class Globals
{
    public static StringBuilder logBuffer = new StringBuilder();

    public static int cubeCount = 0;
    public static int logIntents = 0;
    public static int wrongPositionCount = 0;
    public static int overflowPositionCount = 0;
    public static int invalidPositionCount = 0;

    public static void Reset()
    {
        cubeCount = 0;
        logIntents = 0;
        wrongPositionCount = 0;
        overflowPositionCount = 0;
        invalidPositionCount = 0;
    }
}