using UnityEngine;
using Unity.Netcode;

public class Network_Player : NetworkBehaviour
{
    public enum Team { Attacker, Defender }

    public NetworkVariable<Team> playerTeam = new(Team.Attacker, NetworkVariableReadPermission.Everyone);

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            AssignTeam();
        }
    }

    private void AssignTeam()
    {
        int playerCount = NetworkManager.Singleton.ConnectedClients.Count;
        playerTeam.Value = (playerCount % 2 == 0) ? Team.Attacker : Team.Defender;
        Debug.Log($"Player assigned to team: {playerTeam.Value}");
    }
}
