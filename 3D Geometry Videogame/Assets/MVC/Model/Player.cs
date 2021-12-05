using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase.Database;
using UnityEngine;

[System.Serializable]
public class Player : User
{

    private List<MissionPlayer> listOfMissions;

    public Player(string username, string email): base (username, email)
    {
    
        listOfMissions = new List<MissionPlayer>();
    }

    public override void WriteUserToLocalJSON()
    {
        SaveDataPlayer savePlayerDataToLocal = new SaveDataPlayer(username, listOfMissions);
        File.WriteAllText(Application.persistentDataPath + "/currentuser.json", JsonUtility.ToJson(savePlayerDataToLocal));
    }

    public override void WriteNewUserToDB()
    {
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            SaveDataPlayer savePlayerDataToDB = new SaveDataPlayer(username, listOfMissions);
            reference.Child("Users").Child(user.UserId).SetRawJsonValueAsync(JsonUtility.ToJson(savePlayerDataToDB));
        }
        
    }

    [System.Serializable]
    private class SaveDataPlayer
    {
        [SerializeField]
        private string username;

        [SerializeField]
        private string account = "Player";

        [SerializeField]
        private List<MissionPlayer> listOfMissions;

        public SaveDataPlayer(string username, List<MissionPlayer> listOfMissions)
        {
            this.username = username;
            this.listOfMissions = listOfMissions;
        }
    }

}
