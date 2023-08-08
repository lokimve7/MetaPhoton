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

    void Start()
    {
        //Character Controller ��������
        cc = GetComponent<CharacterController>();

        //nickName ����
        nickName.text = photonView.Owner.NickName;
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
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

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
            }

            //�����̹ٸ� ������ ������ �ϰ� �ʹ�.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //yVelocity �� jumpPower �� ����
                yVelocity = jumpPower;
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
        }
        //�� Player �ƴ϶��
        else
        {
            //��ġ���� ����.
            receivePos = (Vector3)stream.ReceiveNext();
            //ȸ������ ����.
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
