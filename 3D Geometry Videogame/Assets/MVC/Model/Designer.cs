using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    [System.Serializable]
    private class SaveDataDesigner
    {
        [SerializeField]
        private string username;

        [SerializeField]
        private string account = "Designer";

        [SerializeField]
        private List<MissionDesigner> listOfMissionsDesigned;

        public SaveDataDesigner(string username, List<MissionDesigner> listOfMissionsDesigned)
        {
            this.username = username;
            this.listOfMissionsDesigned = listOfMissionsDesigned;
        }
    }
}
