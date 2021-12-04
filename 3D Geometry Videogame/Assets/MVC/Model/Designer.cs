using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Designer : User
{
    private List<MissionDesigner> listOfMissionsDesigned;

    public Designer(string username, string email, string password): base (username, email, password)
    {
        listOfMissionsDesigned = new List<MissionDesigner>();
    }

}
