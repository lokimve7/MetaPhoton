using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class PlayerMove : MonoBehaviourPun, IPunObservable
{
    //�ӷ� 
    float speed = 5;

    //Character Controller ���� ����
    CharacterController cc;

    //���� �Ŀ�
    float jumpPower = 5;
    //�߷�
    float gravity = -9.81f;
    //y �ӷ�
    float yVelocity = 0;

    //�������� �Ѿ���� ��ġ��
    Vector3 receivePos;
    //�������� �Ѿ���� ȸ����
    Quaternion receiveRot = Quaternion.identity;
    //�����ϴ� �ӷ�
    float lerpSpeed = 50;

    //NickName Text �� ��������
    public Text nickName;

    //UI Canvas
    public GameObject myUI;

    //animator
    public Animator anim;

    //���� ���̴�??
    bool isJump = false;

    //���� ������ ����
    float h;
    //���� ������ ����
    float v;

    void Start()
    {
        //Character Controller ��������
        cc = GetComponent<CharacterController>();

        //���࿡ ���� ���� Player ���
        if(photonView.IsMine == true)
        {
            //UI �� ��Ȱ��ȭ ����
            myUI.SetActive(false);
        }
        else
        {
            //nickName ����
            nickName.text = photonView.Owner.NickName;
        }

        //���� PhotonView GameManager �� �˷�����
        GameManager.instance.AddPlayer(photonView);
    }

    void Update()
    {
        //���� ���� �÷��̾���
        if (photonView.IsMine)
        {
            //���࿡ ���콺 Ŀ���� Ȱ��ȭ �Ǿ� ������ �Լ��� ������
            if (Cursor.visible == true) return;

            //W, S, A, D Ű�� ������ �յ��¿�� �����̰� �ʹ�.

            //1. ������� �Է��� ����.
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            //2. ������ �����.
            //�¿�
            Vector3 dirH = transform.right * h;
            //�յ�
            Vector3 dirV = transform.forward * v;
            //����
            Vector3 dir = dirH + dirV;
            dir.Normalize();

            //���࿡ ���� ����ִٸ�
            if (cc.isGrounded == true)
            {
                //yVeloctiy �� 0 ���� ����
                yVelocity = 0;

                //���࿡ ���� ���̶��
                if(isJump == true)
                {
                    //���� Trigger �߻�
                    photonView.RPC(nameof(SetTriggerRpc), RpcTarget.All, "Land");
                }

                //���� �ƴ϶�� ����
                isJump = false;
            }

            //�����̹ٸ� ������ ������ �ϰ� �ʹ�.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //yVelocity �� jumpPower �� ����
                yVelocity = jumpPower;

                //���� Trigger �߻�
                photonView.RPC(nameof(SetTriggerRpc), RpcTarget.All, "Jump");

                //���� ���̶�� ����
                isJump = true;
            }

            //yVelocity �� �߷¸�ŭ ���ҽ�Ű��
            yVelocity += gravity * Time.deltaTime;

            //yVelocity ���� dir �� y ���� ����
            dir.y = yVelocity;

            //3. �׹������� ��������.
            //transform.position += dir * speed * Time.deltaTime;
            cc.Move(dir * speed * Time.deltaTime);

        }
        //���� Player �� �ƴ϶��
        else
        {
            //��ġ ����
            transform.position = Vector3.Lerp(transform.position, receivePos, lerpSpeed * Time.deltaTime);
            //ȸ�� ����
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);
        }

        //�ִϸ��̼ǿ� Parameter �� ����
        anim.SetFloat("Horizontal", h);
        anim.SetFloat("Vertical", v);
    }

    [PunRPC]
    void SetTriggerRpc(string parameter)
    {
        anim.SetTrigger(parameter);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //�� Player ���
        if(stream.IsWriting)
        {
            //���� ��ġ���� ������.
            stream.SendNext(transform.position);
            //���� ȸ������ ������.
            stream.SendNext(transform.rotation);
            //h �� ������.
            stream.SendNext(h);
            //v �� ������.
            stream.SendNext(v);
        }
        //�� Player �ƴ϶��
        else
        {
            //��ġ���� ����.
            receivePos = (Vector3)stream.ReceiveNext();
            //ȸ������ ����.
            receiveRot = (Quaternion)stream.ReceiveNext();
            //h �� ����.
            h = (float)stream.ReceiveNext();
            //v �� ����.
            v = (float)stream.ReceiveNext();
        }
    }
}
