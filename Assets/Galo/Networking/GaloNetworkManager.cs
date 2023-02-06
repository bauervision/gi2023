using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


namespace Galo.Networking
{
    public class GaloNetworkManager : NetworkManager
    {
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
            GaloNetworkPlayer newPlayer = conn.identity.GetComponent<GaloNetworkPlayer>();

            // set the networked name
            string newPlayerName = $"Player {numPlayers}";
            newPlayer.name = newPlayerName;

            // generate the new synced data
            GaloSyncDataModel newPlayerData = new GaloSyncDataModel(newPlayerName);

            //update the network data
            newPlayer.SetDisplayedData(newPlayerData);
        }

    }
}
