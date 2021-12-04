using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : User
{
    private List<MissionPlayer> listOfMissions;

    public Player(string username, string email, string password): base (username, email, password)
    {
        listOfMissions = new List<MissionPlayer>();
    }
}
