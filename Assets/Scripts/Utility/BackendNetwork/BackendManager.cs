using BackEnd;
using LitJson;
using System;
using UnityEngine;


namespace Muks.BackEnd
{
    public enum BackendState
    {
        Failure,
        Maintainance,
        Retry,
        Success,
    }


    public class BackendManager : SingletonHandler<BackendManager>
    {
        /// <summary>
        /// �� ���� ���� ���� ������ ������ �����ϴ�.(�α��� �����ε� ������ ������ ���� ������ �ʱ�ȭ��)
        /// </summary>
        public bool Login;

        public override void Awake()
        {
            base.Awake();
            BackendInit(10);
        }


        /// <summary>�ڳ� �ʱ� ����</summary>

        private void BackendInit(int maxRepeatCount = 10)
        {
            if (maxRepeatCount <= 0)
            {
                Debug.LogError("�ڳ��� �ʱ�ȭ���� ���߽��ϴ�. �ٽ� ����");
                return;
            }

            //BackendReturnObject�� ����� ����� �Ѿ���� ���� �����ϴ� Ŭ����
            //�ڳ� SKD�� �̿��Ͽ� ������ ��û�� ��� ����� BackendReturnObjectŬ���� ���·� ����

            //SDK�� �ʱ�ȭ �� BackendReturnObjectŬ���� ����
            BackendReturnObject bro = Backend.Initialize(true);

            switch (ErrorCheck(bro))
            {
                case BackendState.Failure:
                    Debug.LogError("�ʱ�ȭ ����");
                    break;

                case BackendState.Maintainance:
                    Debug.LogError("���� ���� ��");
                    break;

                case BackendState.Retry:
                    Debug.LogWarning("���� ��õ�");
                    BackendInit(maxRepeatCount - 1);
                    break;

                case BackendState.Success:
                    Debug.Log("�ʱ�ȭ ����");
                    break;
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


        /// <summary> id, pw, ���� ���� ���н� �ݺ�Ƚ��, �Ϸ� �� ������ �Լ��� �޾� �α����� �����ϴ� �Լ� </summary>
        public void CustomLogin(string id, string pw, int maxRepeatCount = 10, Action<BackendReturnObject> onCompleted = null)
        {
            Debug.Log("�α����� ��û�մϴ�.");

            if (maxRepeatCount <= 0)
            {
                Debug.LogError("�α��� ����");
                return;
            }

            BackendReturnObject bro = Backend.BMember.CustomLogin(id, pw);

            switch (ErrorCheck(bro))
            {
                case BackendState.Failure:
                    Debug.LogError("�α��� ����");
                    break;

                case BackendState.Maintainance:
                    Debug.LogError("���� ���� ��");
                    break;

                case BackendState.Retry:
                    Debug.LogWarning("���� ��õ�");
                    CustomLogin(id, pw, maxRepeatCount - 1, onCompleted);
                    break;

                case BackendState.Success:
                    Debug.Log("�α��� ����");
                    onCompleted?.Invoke(bro);
                    break;
            }
        }


        /// <summary>�Խ�Ʈ �α����� �����ϴ� �Լ� </summary>
        public void GuestLogin(int maxRepeatCount = 10, Action<BackendReturnObject> onCompleted = null)
        {
            Debug.Log("�α����� ��û�մϴ�.");

            if (maxRepeatCount <= 0)
            {
                Debug.LogError("�α��� ����");
                return;
            }

            BackendReturnObject bro = Backend.BMember.GuestLogin("�Խ�Ʈ �α���");

            switch (ErrorCheck(bro))
            {
                case BackendState.Failure:
                    Debug.LogError("�α��� ����");
                    break;

                case BackendState.Maintainance:
                    Debug.LogError("���� ���� ��");
                    break;

                case BackendState.Retry:
                    Debug.LogWarning("���� ��õ�");
                    GuestLogin(maxRepeatCount - 1, onCompleted);
                    break;

                case BackendState.Success:
                    Debug.Log("�α��� ����");
                    onCompleted?.Invoke(bro);
                    break;
            }
        }




        /// <summary> id, pw, ���� ���� ���н� �ݺ�Ƚ��, �Ϸ� �� ������ �Լ��� �޾� ȸ�������� �����ϴ� �Լ� </summary>
        public void CustomSignup(string id, string pw, int maxRepeatCount = 10, Action<BackendReturnObject> onCompleted = null)
        {
            Debug.Log("ȸ�� ������ ��û�մϴ�.");

            if (maxRepeatCount <= 0)
            {
                Debug.LogError("ȸ�� ���� ����");
                return;
            }

            BackendReturnObject bro = Backend.BMember.CustomSignUp(id, pw);

            switch (ErrorCheck(bro))
            {
                case BackendState.Failure:
                    Debug.LogError("ȸ�� ���� ����");
                    break;

                case BackendState.Maintainance:
                    Debug.LogError("���� ���� ��");
                    break;

                case BackendState.Retry:
                    Debug.LogWarning("���� ��õ�");
                    CustomSignup(id, pw, maxRepeatCount - 1, onCompleted);
                    break;

                case BackendState.Success:
                    Debug.Log("ȸ������ ����");
                    onCompleted?.Invoke(bro);
                    break;
            }

        }






        /// <summary> �� ������ ID�� �޾� ���� ���� Ȯ�� �� ���� �Լ��� ó�����ִ� �Լ� </summary>
        public void GetMyData(string selectedProbabilityFileId, int maxRepeatCount = 10, Action<BackendReturnObject> onCompleted = null)
        {
            if (!Backend.IsLogin)
            {
                Debug.LogError("������ �α��� �Ǿ����� �ʽ��ϴ�.");
                return;
            }

            if (maxRepeatCount <= 0)
            {
                Debug.LogErrorFormat("{0} ��Ʈ�� ������ �޾ƿ��� ���߽��ϴ�.", selectedProbabilityFileId);
                return;
            }

            BackendReturnObject bro = Backend.GameData.GetMyData(selectedProbabilityFileId, new Where());

            switch (ErrorCheck(bro))
            {
                case BackendState.Failure:
                    Debug.LogError("���� ����");
                    break;

                case BackendState.Maintainance:
                    Debug.LogError("���� ���� ��");
                    break;

                case BackendState.Retry:
                    Debug.LogWarning("���� ��õ�");
                    GetMyData(selectedProbabilityFileId, maxRepeatCount - 1, onCompleted);
                    break;

                case BackendState.Success:
                    Debug.Log("�� ���� �޾ƿ��� ����");
                    onCompleted?.Invoke(bro);
                    break;
            }
        }


        /// <summary> ��Ʈ ID�� �ݺ� Ƚ��, ������ ���� ��� ������ �Լ��� �޾� �ڳ����� ChartData�� �޾ƿ��� �Լ� </summary>
        public void GetChartData(string selectedProbabilityFileId, int maxRepeatCount = 10, Action<BackendReturnObject> onCompleted = null)
        {
            if (!Backend.IsLogin)
            {
                Debug.LogError("������ �α��� �Ǿ����� �ʽ��ϴ�.");
                return;
            }

            if (maxRepeatCount <= 0)
            {
                Debug.LogError("���� ����");
                return;
            }

            BackendReturnObject bro = Backend.Chart.GetOneChartAndSave(selectedProbabilityFileId);

            switch (ErrorCheck(bro))
            {
                case BackendState.Failure:
                    Debug.LogError("���� ����");
                    break;

                case BackendState.Maintainance:
                    Debug.LogError("���� ���� ��");
                    break;

                case BackendState.Retry:
                    Debug.LogWarning("���� ��õ�");
                    GetChartData(selectedProbabilityFileId, maxRepeatCount - 1, onCompleted);
                    break;

                case BackendState.Success:
                    Debug.Log("��Ʈ ���� �ޱ� ����");
                    onCompleted?.Invoke(bro);
                    break;
            }
        }


        /// <summary> ��Ʈ ID�� �ݺ� Ƚ��, ������ ���� ��� ������ �Լ��� �޾� �ڳ� GameData���� ������ �߰��ϴ� �Լ� </summary>
        public void GameDataInsert(string selectedProbabilityFileId, int maxRepeatCount, Param param, Action<BackendReturnObject> onCompleted = null)
        {
            if (!Backend.IsLogin || !Login)
            {
                Debug.LogError("������ �α��� �Ǿ����� �ʽ��ϴ�.");
                return;
            }

            if (maxRepeatCount <= 0)
            {
                Debug.LogErrorFormat("{0} ���� ������ �߰����� ���߽��ϴ�.", selectedProbabilityFileId);
                return;
            }

            Backend.GameData.Insert(selectedProbabilityFileId, param, callback =>
            {
                switch (ErrorCheck(callback))
                {
                    case BackendState.Failure:
                        Debug.LogError("���� ����");
                        break;

                    case BackendState.Maintainance:
                        Debug.LogError("���� ���� ��");
                        break;

                    case BackendState.Retry:
                        Debug.LogWarning("���� ��õ�");
                        GameDataInsert(selectedProbabilityFileId, maxRepeatCount - 1, param, onCompleted);
                        break;

                    case BackendState.Success:
                        Debug.Log("���� �߰� ����");
                        onCompleted?.Invoke(callback);
                        break;
                }        
            });
        }


        /// <summary> ��Ʈ ID�� �ݺ� Ƚ��, ������ ���� ��� ������ �Լ��� �޾� �ڳ� GameData���� ������ �߰��ϴ� �Լ� </summary>
        public void GameDataUpdate(string selectedProbabilityFileId, string inDate, int maxRepeatCount, Param param, Action<BackendReturnObject> onCompleted = null)
        {
            if (!Backend.IsLogin || !Login)
            {
                Debug.LogError("������ �α��� �Ǿ����� �ʽ��ϴ�.");
                return;
            }

            if (maxRepeatCount <= 0)
            {
                Debug.LogErrorFormat("{0} ���� ������ �������� ���߽��ϴ�.", selectedProbabilityFileId);
                return;
            }

            Backend.GameData.UpdateV2(selectedProbabilityFileId, inDate, Backend.UserInDate, param, callback =>
            {
                switch (ErrorCheck(callback))
                {
                    case BackendState.Failure:
                        Debug.LogError("���� ����");
                        break;

                    case BackendState.Maintainance:
                        Debug.LogError("���� ���� ��");
                        break;

                    case BackendState.Retry:
                        Debug.LogWarning("���� ��õ�");
                        GameDataUpdate(selectedProbabilityFileId, inDate, maxRepeatCount - 1, param, onCompleted);
                        break;

                    case BackendState.Success:
                        Debug.Log("���� ���� ����");
                        onCompleted?.Invoke(callback);
                        break;
                }

            });
        }


        /// <summary> ������ ���� ���¸� üũ�ϰ� BackendState���� ��ȯ�ϴ� �Լ� </summary>
        public BackendState ErrorCheck(BackendReturnObject bro)
        {
            if (bro.IsSuccess())
            {
                Debug.Log("��û ����!");
                return BackendState.Success;
            }
            else
            {
                if (bro.IsClientRequestFailError()) // Ŭ���̾�Ʈ�� �Ͻ����� ��Ʈ��ũ ���� ��
                {
                    Debug.LogError("�Ͻ����� ��Ʈ��ũ ����");
                    return BackendState.Retry;
                }
                else if (bro.IsServerError()) // ������ �̻� �߻� ��
                {
                    Debug.LogError("���� �̻� �߻�");
                    return BackendState.Retry;
                }
                else if (bro.IsMaintenanceError()) // ���� ���°� '����'�� ��
                {
                    //���� �˾�â + �α��� ȭ������ ������
                    Debug.Log("���� ������");
                    return BackendState.Maintainance;
                }
                else if (bro.IsTooManyRequestError()) // �ܱⰣ�� ���� ��û�� ���� ��� �߻��ϴ� 403 Forbbiden �߻� ��
                {
                    //�ܱⰣ�� ���� ��û�� ������ �߻��մϴ�. 5�е��� �ڳ��� �Լ� ��û�� �����ؾ��մϴ�.  
                    Debug.LogError("�ܱⰣ�� ���� ��û�� ���½��ϴ�. 5�а� ��� �Ұ�");
                    return BackendState.Failure;
                }
                else if (bro.IsBadAccessTokenError())
                {
                    bool isRefreshSuccess = RefreshTheBackendToken(3); // �ִ� 3�� �������� ����

                    if (isRefreshSuccess)
                    {
                        Debug.LogError("��ū �߱� ����");
                        return BackendState.Retry;
                    }
                    else
                    {
                        Debug.LogError("��ū�� �߱� ���� ���߽��ϴ�.");
                        return BackendState.Failure;
                    }
                }

                //���� ��⿡�� �α��� ������ �����ִµ� ������ �����Ͱ� ������
                //��⿡ ����� �α��� ������ �����Ѵ�.
                else if (bro.GetMessage() == "bad customId, �߸��� customId �Դϴ�")
                    Backend.BMember.DeleteGuestInfo();

                else
                {
                    Debug.LogError(bro.GetErrorCode());
                }

                return BackendState.Retry;
            }
        }



        /// <summary> �ڳ� ��ū ��߱� �Լ� </summary>
        /// maxRepeatCount : ���� ���� ���н� �� �õ��� Ƚ��
        public bool RefreshTheBackendToken(int maxRepeatCount)
        {
            if (maxRepeatCount <= 0)
            {
                Debug.Log("��ū �߱� ����");
                return false;
            }
            
            BackendReturnObject callback = Backend.BMember.RefreshTheBackendToken();

            if (callback.IsSuccess())
            {
                Debug.Log("��ū �߱� ����");
                return true;
            }
            else
            {
                if (callback.IsClientRequestFailError()) // Ŭ���̾�Ʈ�� �Ͻ����� ��Ʈ��ũ ���� ��
                {
                    return RefreshTheBackendToken(maxRepeatCount - 1);
                }
                else if (callback.IsServerError()) // ������ �̻� �߻� ��
                {
                    return RefreshTheBackendToken(maxRepeatCount - 1);
                }
                else if (callback.IsMaintenanceError()) // ���� ���°� '����'�� ��
                {
                    //���� �˾�â + �α��� ȭ������ ������
                    return false;
                }
                else if (callback.IsTooManyRequestError()) // �ܱⰣ�� ���� ��û�� ���� ��� �߻��ϴ� 403 Forbbiden �߻� ��
                {
                    //�ʹ� ���� ��û�� ������ ��
                    return false;
                }
                else
                {
                    //��õ��� �ص� �׼�����ū ��߱��� �Ұ����� ���
                    //Ŀ���� �α��� Ȥ�� �䵥���̼� �α����� ���� ���� �α����� �����ؾ��մϴ�.  
                    //�ߺ� �α����� ��� 401 bad refreshToken ������ �Բ� �߻��� �� �ֽ��ϴ�.  
                    Debug.Log("���� ���ӿ� ������ �߻��߽��ϴ�. �α��� ȭ������ ���ư��ϴ�\n" + callback.ToString());
                    return false;
                }
            }
        }
    }
}

