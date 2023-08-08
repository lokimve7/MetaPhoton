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

            //������� ��ź�� ī�޶� �չ������� 1��ŭ ������ ������ ���´�.
            Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward * 1;

            //������� �Ѿ��� �չ����� ī�޶� ���� �������� ����
            Vector3 forward = Camera.main.transform.forward;

            photonView.RPC(nameof(FireBulletByRpc), RpcTarget.All, pos, forward);
        }

        //2��Ű ������
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            photonView.RPC(nameof(FireRayByRpc), RpcTarget.All, 
                Camera.main.transform.position, Camera.main.transform.forward);
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
    void FireBulletByRpc(Vector3 firePos, Vector3 fireFoward)
    {
        GameObject bomb = Instantiate(bombFactory);
        bomb.transform.position = firePos;
        bomb.transform.forward = fireFoward;
    }

    [PunRPC]
    void FireRayByRpc(Vector3 firePos, Vector3 fireFoward)
    {
        //ī�޶���ġ, ī�޶� �չ������� Ray �� ������.
        Ray ray = new Ray(firePos, fireFoward);
        //���࿡ Ray �� �߻��ؼ� �ε��� ���� �ִٸ�
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            //�� ��ġ�� ����ȿ�����忡�� ����ȿ���� �����.
            GameObject fragment = Instantiate(fragmentFactory);
            //������� ����ȿ���� �ε��� ��ġ�� ���´�.
            fragment.transform.position = hitInfo.point;
            //����ȿ���� ������ �ε��� ��ġ�� normal �������� ����
            fragment.transform.forward = hitInfo.normal;
            //2�� �ڿ� ����ȿ���� �ı�����
            Destroy(fragment, 2);

            //���࿡ �������� �̸��� Player �� �����ϰ� �ִٸ�
            if (hitInfo.transform.gameObject.name.Contains("Player"))
            {
                //�÷��̾ ������ �ִ� PlayerHP ������Ʈ ��������
                PlayerHP hp = hitInfo.transform.GetComponent<PlayerHP>();
                //������ �������� UpdateHP �Լ��� ����               
                hp.UpdateHP(-10);
            }
        }
    }
}
