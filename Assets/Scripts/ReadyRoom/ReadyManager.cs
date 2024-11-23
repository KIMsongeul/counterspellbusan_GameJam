using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using TMPro;
public class ReadyManager : MonoBehaviourPunCallbacks
{
    public bool isReady = false;
    public bool isOtherReady = false;

    public GameObject loadingUI;

    public TMP_Text roomCodeUI;
    public TMP_Text masterReadyUI;
    public TMP_Text otherReadyUI;
    [HideInInspector] public PhotonView pv;

    bool isMoveInGame;

    void Awake()
    {
        pv = GetComponent<PhotonView>();

        if(PhotonNetwork.IsMasterClient)
        {
            isReady = false;
            isOtherReady = false;
        }

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start(){
        roomCodeUI.text = $"Room Code: {PhotonNetwork.CurrentRoom.Name}";
        masterReadyUI.text = "Waiting...";
        otherReadyUI.text = "Waiting...";
    }

    void Update()
    {
        if(isReady && isOtherReady)
        {
            print("Ready is done!");
            if(PhotonNetwork.IsMasterClient && !isMoveInGame)
            {
                StartCoroutine(LoadGameScene());
            }
        }
    }
    
    IEnumerator LoadGameScene()
    {
        isMoveInGame = true;
        pv.RPC("SetLoadingUI", RpcTarget.All, true);
        yield return new WaitForSeconds(0.5f);
        PhotonNetwork.LoadLevel("Room_Game");
        // PhotonNetwork.LoadLevel()은 마스터 클라이언트에서만 호출되어도
        // AutomaticallySyncScene = true로 설정되어 있기 때문에
        // 다른 클라이언트도 자동으로 씬이 동기화됩니다.
    }

    [PunRPC]
    void MasterClientReady(){
        isReady = true;
        masterReadyUI.text = isReady ? "Done!" : "Waiting...";
    }

    [PunRPC]
    void OtherClientReady(){
        isOtherReady = true;
        otherReadyUI.text = isOtherReady ? "Done!" : "Waiting...";
    }

    [PunRPC]
    void SetLoadingUI(bool isActive){
        loadingUI.SetActive(isActive);
    }
}
