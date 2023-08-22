using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
//���� ����
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
        //���࿡ �浹�� ��ü�� BallPos �̸�
        if(other.name.Contains("BallPos"))
        {
            TestMgr.instance.hasBallPlayer = other.GetComponentInParent<PhotonView>();
            SetBall(other.transform);
        }        
    }

    //��� PC ���� ȣ��
    public void SetBall(Transform parent)
    {
        //�߷��� ����.
        rb.useGravity = false;
        //�ӵ��� 0���� �Ѵ�.
        rb.velocity = Vector3.zero;
        
        //�θ� ����
        transform.SetParent(parent);
        //local ��ǥ�� 0 ���� �Ѵ�.
        transform.localPosition = Vector3.zero;
    }

    public void PassBall(Vector3 pos, Vector3 forward)
    {
        //�θ� ���ش�.
        transform.SetParent(null);
        //���� ��ġ ��Ų��.
        transform.position = pos;
        //�߷��� Ų��.
        rb.useGravity = true;
        //�ӵ��� �ش�.
        rb.velocity = forward * 100;
        print(transform.position);
    }
}
