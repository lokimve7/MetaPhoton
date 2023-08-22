using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BallPlayer : MonoBehaviourPun
{
    GameObject ball;
    public Transform ballPos;

    void Start()
    {
        ball = GameObject.Find("Ball");
    }

    void Update()
    {
        if (photonView.IsMine == false) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();
        transform.position += dir * 5 * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E))
        {
            //만약에 내가 공을 가지고 있다면
            if(TestMgr.instance.hasBallPlayer == photonView)
            {
                //모든 PC 에게 볼을 던지라고 한다.
                photonView.RPC(nameof(PassBall), RpcTarget.All, ball.transform.position, transform.forward);

                //난 공을 가지고 있지 않는다
                TestMgr.instance.hasBallPlayer = null;
            }
        }
    }

    [PunRPC]
    void PassBall(Vector3 pos, Vector3 forward)
    {
        Ball b = ball.GetComponent<Ball>();
        b.PassBall(pos, forward);
    }


    ////Ball 을 BallPos 의 자식으로 셋팅하라고 모두 알린다.
    //public void GetBall()
    //{
    //    photonView.RPC(nameof(RpcGetBall), RpcTarget.All);
    //}
    //[PunRPC]
    //void RpcGetBall()
    //{
    //    //현재 Ball 을 가지고 있는 Player 저장
    //    TestMgr.instance.hasBallPlayer = photonView;

    //    //BallPos 에 자식으로 등록
    //    Ball b = ball.GetComponent<Ball>();
    //    b.SetBall(ballPos);
    //}
}
