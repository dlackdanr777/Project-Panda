using BackEnd;
using LitJson;
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


        /// <summary> ���� ���� ID�� �޾� JsonData�� �Ѱ��ִ� �Լ� </summary>
        public JsonData GetMyData(string selectedProbabilityFileId)
        {
            BackendReturnObject backendReturnObject = Backend.GameData.GetMyData(selectedProbabilityFileId, new Where());

            if (!backendReturnObject.IsSuccess())
            {
                Debug.Log("������ �޾ƿ��� ���߽��ϴ�.");
                return null;
            }
                
            return backendReturnObject.FlattenRows();
        }


        /// <summary> ��ƮID�� �޾� JsonData�� �Ѱ��ִ� �Լ� </summary>
        public JsonData GetChartData(string selectedProbabilityFileId)
        {
            BackendReturnObject backendReturnObject = Backend.Chart.GetOneChartAndSave(selectedProbabilityFileId);

            if (!backendReturnObject.IsSuccess())
            {
                Debug.Log("������ �޾ƿ��� ���߽��ϴ�.");
                return null;
            }

            return backendReturnObject.FlattenRows();
        }


    }
}

