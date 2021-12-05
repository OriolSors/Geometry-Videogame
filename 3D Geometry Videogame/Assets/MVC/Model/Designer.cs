using System;
using System.Collections.Generic;
using System.IO;
using Firebase.Database;
using UnityEngine;

[System.Serializable]
public class Designer : User
{
    private List<MissionDesigner> listOfMissionsDesigned;

    public Designer(string username, string email): base (username, email)
    {
        listOfMissionsDesigned = new List<MissionDesigner>();
    }

    public override void WriteUserToLocalJSON()
    {
        SaveDataDesigner saveDesignerDataToLocal = new SaveDataDesigner(username, listOfMissionsDesigned);
        File.WriteAllText(Application.persistentDataPath + "/currentuser.json", JsonUtility.ToJson(saveDesignerDataToLocal));
    }

    public override void WriteNewUserToDB()
    {
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            SaveDataDesigner saveDesignerDataToDB = new SaveDataDesigner(username, listOfMissionsDesigned);
            reference.Child("Users").Child(user.UserId).SetRawJsonValueAsync(JsonUtility.ToJson(saveDesignerDataToDB));
        }
    }

    public override void SetAllMissions(IEnumerable<DataSnapshot> listOfMissions)
    {
        foreach (DataSnapshot missionDesigner in listOfMissions)
        {

            MissionDesigner newMissionDesigner = missionDesigner.Value as MissionDesigner;

            /*
            string missionNameDesigned = missionDesigner.Child("missionName").Value.ToString();

            int numberOfFiguresDesigned = Convert.ToInt32(missionDesigner.Child("missionName").Value);

            Dictionary<string, MissionPlayer> listOfPlayers = missionDesigner.Child("listOfPlayers").Value as Dictionary<string, MissionPlayer>;
            
            //TODO: retrieve correctament les MissionPlayer del diccionari guardat a la Firebase


            MissionDesigner newMissionDesigner = new MissionDesigner(missionNameDesigned, numberOfFiguresDesigned, listOfPlayers);

            */

            listOfMissionsDesigned.Add(newMissionDesigner);
        }
    }
}
