using BackEnd;
using UnityEngine;


namespace Muks.BackEnd
{
    public class BackendManager : SingletonHandler<BackendManager>
    {

        public override void Awake()
        {
            base.Awake();
            //BackendReturnObject�� ����� ����� �Ѿ���� ���� �����ϴ� Ŭ����
            //�ڳ� SKD�� �̿��Ͽ� ������ ��û�� ��� ����� BackendReturnObjectŬ���� ���·� ����

            //SDK�� �ʱ�ȭ �� BackendReturnObjectŬ���� ����
            BackendReturnObject bro = Backend.Initialize(true);

            //�ʱ�ȭ�� �������� ���?
            if (bro.IsSuccess())
            {
                //������ ��� statusCode 204 Success
                Debug.LogFormat("�ʱ�ȭ ����: {0}", bro);
            }
            else
            {
                // ������ ��� statusCode 400�� ���� �߻� 
                Debug.LogFormat("�ʱ�ȭ ����: {0}", bro);
            }
        }


        private void Update()
        {
            //SDK�� �ʱ�ȭ�Ǿ�������
            if (Backend.IsInitialized)
            {
                //�ڳ� �񵿱� �Լ� ��� ��, ���ξ����忡�� �ݹ��� ó�����ִ� Dispatch�� ����
                Backend.AsyncPoll();
            }
        }
    }
}

