// IScorable.cs
// Author: Sadikur Rahman
// Description: Interface for applying polymorphic score logic for each role.

public interface IScorable
{
    void CalculateScore(PlayerData player, bool didPoliceWin, RoleType target);
}