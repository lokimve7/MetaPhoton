using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class TestMgr : MonoBehaviour
{
    public static TestMgr instance;

    public Transform[] spawnPos;

    public PhotonView hasBallPlayer;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        PhotonNetwork.SendRate = PhotonNetwork.SerializationRate = 60;

        int idx = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        PhotonNetwork.Instantiate("BallPlayer", spawnPos[idx].position, spawnPos[idx].rotation);
    }

    
}
