using System;
using System.Collections.Generic;
using Firebase.Database;

public class MissionListController
{
    private DatabaseReference reference;
    private Firebase.Auth.FirebaseAuth auth;

    public MissionListController()
    {
        reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    public void RetrieveAllMissionUser(User currentUser, IEnumerable<DataSnapshot> listOfMissions)
    {
        currentUser.SetAllMissions(listOfMissions);
    }

    public Dictionary<string, int[]> GetAllMissionPlayer(User currentUser)
    {
        Player currentPlayer = currentUser as Player;
        return currentPlayer.GetAllMissionPlayer();
    }

    public List<string> GetAllMissionDesigner(User currentUser)
    {
        Designer currentDesigner = currentUser as Designer;
        return currentDesigner.GetAllMissionDesigner();
    }

    public Dictionary<string, string> GetAllUserStatisticsInMission(User currentUser, string mission)
    {
        Designer currentDesigner = currentUser as Designer;
        return currentDesigner.GetAllUserStatisticsInMission(mission);
    }

    public void SaveCurrentMissionPlayer(string mission, int inventory)
    {
        
    }

    
}
