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
}
