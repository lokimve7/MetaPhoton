using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
//방장 소유
public class Ball : MonoBehaviourPun
{
    Rigidbody rb;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {        
        //만약에 충돌한 물체가 BallPos 이면
        if(other.name.Contains("BallPos"))
        {
            TestMgr.instance.hasBallPlayer = other.GetComponentInParent<PhotonView>();
            SetBall(other.transform);
        }        
    }

    //모든 PC 에서 호출
    public void SetBall(Transform parent)
    {
        //중력을 끈다.
        rb.useGravity = false;
        //속도를 0으로 한다.
        rb.velocity = Vector3.zero;
        
        //부모 설정
        transform.SetParent(parent);
        //local 좌표를 0 으로 한다.
        transform.localPosition = Vector3.zero;
    }

    public void PassBall(Vector3 pos, Vector3 forward)
    {
        //부모를 없앤다.
        transform.SetParent(null);
        //볼을 위치 시킨다.
        transform.position = pos;
        //중력을 킨다.
        rb.useGravity = true;
        //속도를 준다.
        rb.velocity = forward * 100;
        print(transform.position);
    }
}
