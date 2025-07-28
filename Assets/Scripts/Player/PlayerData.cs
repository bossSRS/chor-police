// PlayerData.cs
// Author: Sadikur Rahman
// Description: Stores player-specific information such as ID, Name, Role, and Score.

public class PlayerData
{
    public int ID;
    public string Name;
    public RoleType Role;
    public int Score;

    public PlayerData(int id, string name)
    {
        ID = id;
        Name = name;
        Score = 0;
    }

    public void AssignRole(RoleType role)
    {
        Role = role;
    }
}