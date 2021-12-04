using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class User
{
    protected string username;
    protected string email;
    protected string password;

    public User(string username, string email, string password)
    {
        this.username = username;
        this.email = email;
        this.password = password;

    }
}
