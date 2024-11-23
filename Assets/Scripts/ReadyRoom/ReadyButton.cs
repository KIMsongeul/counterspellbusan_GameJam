using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ReadyButton : MonoBehaviour
{
    public ReadyManager readyManager;

    void Awake(){
        readyManager = FindObjectOfType<ReadyManager>();
    }

    public void Ready_Btn()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            readyManager.isReady = true;
            readyManager.pv.RPC("MasterClientReady", RpcTarget.All);
        }
        else{
            readyManager.isOtherReady = true;
            readyManager.pv.RPC("OtherClientReady", RpcTarget.All);
        }
    }
}
