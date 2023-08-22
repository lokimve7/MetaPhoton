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
            //���࿡ ���� ���� ������ �ִٸ�
            if(TestMgr.instance.hasBallPlayer == photonView)
            {
                //��� PC ���� ���� ������� �Ѵ�.
                photonView.RPC(nameof(PassBall), RpcTarget.All, ball.transform.position, transform.forward);

                //�� ���� ������ ���� �ʴ´�
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


    ////Ball �� BallPos �� �ڽ����� �����϶�� ��� �˸���.
    //public void GetBall()
    //{
    //    photonView.RPC(nameof(RpcGetBall), RpcTarget.All);
    //}
    //[PunRPC]
    //void RpcGetBall()
    //{
    //    //���� Ball �� ������ �ִ� Player ����
    //    TestMgr.instance.hasBallPlayer = photonView;

    //    //BallPos �� �ڽ����� ���
    //    Ball b = ball.GetComponent<Ball>();
    //    b.SetBall(ballPos);
    //}
}
