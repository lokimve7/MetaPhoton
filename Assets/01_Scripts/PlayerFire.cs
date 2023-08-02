using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    //��ź ����
    public GameObject bombFactory;
    //���� ����
    public GameObject fragmentFactory;

    void Start()
    {
        
    }

    void Update()
    {
        //1��Ű�� ������
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            //��ź���忡�� ��ź�� �����
            GameObject bomb = Instantiate(bombFactory);
            //������� ��ź�� ī�޶� �չ������� 1��ŭ ������ ������ ���´�.
            bomb.transform.position = 
                Camera.main.transform.position + Camera.main.transform.forward * 1;
            //������� �Ѿ��� �չ����� ī�޶� ���� �������� ����
            bomb.transform.forward = Camera.main.transform.forward;
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
}
