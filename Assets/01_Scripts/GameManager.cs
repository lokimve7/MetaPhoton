﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //RPC 호출 빈도
        PhotonNetwork.SendRate = 30;

        //OnPhotonSerializeView 호출 빈도
        PhotonNetwork.SerializationRate = 30;

        //나의 Player 생성
        PhotonNetwork.Instantiate("Player", new Vector3(0, 1, 0), Quaternion.identity);

        //마우스 포인터를 비활성화
        Cursor.visible = false;


        SetSpawnPos();
    }

    //spawnPosGroup Transform
    public Transform trSpawnPosGroup;
    void SetSpawnPos()
    {
        //간격 (anlge)
        float angle = 360 / 10;
        for(int i = 0; i < 10; i++)
        {
            trSpawnPosGroup.Rotate(0, angle, 0);

            Vector3 pos = trSpawnPosGroup.position + trSpawnPosGroup.forward * 5;

            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);

            go.transform.position = pos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //만약에 esc 키를 누르면 
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //마우스 포인터를 활성화
            Cursor.visible = true;
        }

        //마우스 클릭했을 때
        if(Input.GetMouseButtonDown(0))
        {
            //마우스 클릭시 해당 위치에 UI가 없으면
            if(EventSystem.current.IsPointerOverGameObject() == false)
            {
                //마우스 포인터를 비활성화
                Cursor.visible = false;
            }
        }
    }
}
