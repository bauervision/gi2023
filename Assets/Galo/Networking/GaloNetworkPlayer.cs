using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

namespace Galo.Networking
{
    public class GaloNetworkPlayer : NetworkBehaviour
    {
        public TextMeshPro playerNameText;

        [SyncVar(hook = nameof(HandleHOOK_DataUpdated))]
        [SerializeField]
        public GaloSyncDataModel dataModel;


        [SyncVar(hook = nameof(HandleHOOK_NameUpdated))]
        [SerializeField]
        string myLocalName;

        #region Server

        /// <summary>
        /// Server sets initial data when new player joins
        /// </summary>
        /// <param name="newData"></param>
        [Server]
        public void SetDisplayedData(GaloSyncDataModel newData) { dataModel = newData; }


        /// <summary>
        /// Server sets name update
        /// </summary>
        /// <param name="newName"></param>
        [Server]
        public void SetDisplayName(string newName)
        {
            myLocalName = newName;//trigger the hook
            //update the data
            dataModel.playerName = newName;
            //and the UI
            playerNameText.text = newName;
        }

        /// <summary>
        /// Called from a client to request change to its data globally
        /// </summary>
        /// <param name="newName"></param>
        [Command]
        void CmdSetDisplayName(string newName)
        {
            //TODO: Handle validation

            //set the data
            SetDisplayName(newName);

        }

        #endregion

        #region Client

        /// <summary>
        /// Triggered from HOOK when GaloSyncDataModel has updated
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        public void HandleHOOK_DataUpdated(GaloSyncDataModel oldValue, GaloSyncDataModel newValue)
        {
            playerNameText.text = newValue.playerName;
        }

        /// <summary>
        /// riggered from HOOK when myLocalName has updated
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        public void HandleHOOK_NameUpdated(string oldValue, string newValue)
        {
            myLocalName = newValue;
            playerNameText.text = newValue;
        }



        /// <summary>
        /// Send update status to player's log
        /// </summary>
        /// <param name="newName"></param>
        [ClientRpc]
        void RpcLogNewName(string newName)
        {
            Debug.Log(newName);
        }

        // Dev testing
        [ContextMenu("Set Ny Name")]
        void SetMyName() { CmdSetDisplayName("DevDad"); }

        #endregion
    }
}
