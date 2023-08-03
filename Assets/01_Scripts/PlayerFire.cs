using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerFire : MonoBehaviourPun
{
    //��ź ����
    public GameObject bombFactory;
    //���� ����
    public GameObject fragmentFactory;

    void Start()
    {
        //���� ���� Player �� �ƴҶ�
        if(photonView.IsMine == false)
        {
            //PlayerFire ������Ʈ�� ��Ȱ��ȭ
            this.enabled = false;
        }
    }

    void Update()
    {
        //1��Ű�� ������
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            //FireBulletByInstantiate();
            photonView.RPC(nameof(FireBulletByRpc), RpcTarget.All);
        }

        //2��Ű ������
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            //ī�޶���ġ, ī�޶� �չ������� Ray �� ������.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            //���࿡ Ray �� �߻��ؼ� �ε��� ���� �ִٸ�
            RaycastHit hitInfo;
            if(Physics.Raycast(ray, out hitInfo))
            {
                //�� ��ġ�� ����ȿ�����忡�� ����ȿ���� �����.
                GameObject fragment = Instantiate(fragmentFactory);
                //������� ����ȿ���� �ε��� ��ġ�� ���´�.
                fragment.transform.position = hitInfo.point;
                //����ȿ���� ������ �ε��� ��ġ�� normal �������� ����
                fragment.transform.forward = hitInfo.normal;
                //2�� �ڿ� ����ȿ���� �ı�����
                Destroy(fragment, 2);
            }
        }
    }

    void FireBulletByInstantiate()
    {
        //������� ��ź�� ī�޶� �չ������� 1��ŭ ������ ������ ���´�.
        Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward * 1;

        //������� �Ѿ��� �չ����� ī�޶� ���� �������� ����
        Quaternion rot = Camera.main.transform.rotation;

        //��ź���忡�� ��ź�� �����
        GameObject bomb = PhotonNetwork.Instantiate("Bomb", pos, rot);
    }

    [PunRPC]
    void FireBulletByRpc()
    {
        //������� ��ź�� ī�޶� �չ������� 1��ŭ ������ ������ ���´�.
        Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward * 1;

        //������� �Ѿ��� �չ����� ī�޶� ���� �������� ����
        Quaternion rot = Camera.main.transform.rotation;

        GameObject bomb = Instantiate(bombFactory);
        bomb.transform.position = pos;
        bomb.transform.rotation = rot;
    }
}
