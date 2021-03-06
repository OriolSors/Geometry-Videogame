using System;
using System.Collections.Generic;
using Firebase.Database;

[System.Serializable]
public abstract class User
{
    protected string username;
    protected string email;

    protected DatabaseReference reference;
    protected Firebase.Auth.FirebaseAuth auth;

    public User(string username, string email)
    {
        this.username = username;
        this.email = email;

        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;

    }

    public abstract void WriteNewUserToDB();
    public abstract void WriteUserToLocalJSON();

    public string GetUserName()
    {
        return username;
    }

    public string GetEmail()
    {
        return email;
    }
}
