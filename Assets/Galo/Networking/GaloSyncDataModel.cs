using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Galo.Networking
{
    [System.Serializable]
    public class GaloSyncDataModel
    {
        public string playerName;
        public GaloSyncDataModel() { }
        public GaloSyncDataModel(string newName)
        {
            this.playerName = newName;
        }
    }
}
