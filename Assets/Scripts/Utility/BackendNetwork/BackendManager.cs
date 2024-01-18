using BackEnd;
using LitJson;
using System;
using UnityEngine;


namespace Muks.BackEnd
{
    public class BackendManager : SingletonHandler<BackendManager>
    {

        public override void Awake()
        {
            base.Awake();

            BackendInit(10);
        }

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
                BackendInit(maxRepeatCount - 1);
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

        public void CustomLogin(string id, string pw, int maxRepeatCount = 10, Action<BackendReturnObject> onCompleted = null)
        {
            Debug.Log("�α����� ��û�մϴ�.");

            if (maxRepeatCount <= 0)
            {
                Debug.LogError("�α��� ����");
                return;
            }

            BackendReturnObject bro = Backend.BMember.CustomLogin(id, pw);

            if (bro.IsSuccess())
            {
                onCompleted?.Invoke(bro);
                Debug.Log("�α��� ����!");
            }
            else
            {
                if (bro.IsClientRequestFailError()) // Ŭ���̾�Ʈ�� �Ͻ����� ��Ʈ��ũ ���� ��
                {
                    CustomLogin(id, pw, maxRepeatCount - 1, onCompleted);
                }
                else if (bro.IsServerError()) // ������ �̻� �߻� ��
                {
                    CustomLogin(id, pw, maxRepeatCount - 1, onCompleted);
                }
                else if (bro.IsMaintenanceError()) // ���� ���°� '����'�� ��
                {
                    //���� �˾�â + �α��� ȭ������ ������
                    Debug.Log("���� �������Դϴ�.");
                    return;
                }
                else if (bro.IsTooManyRequestError()) // �ܱⰣ�� ���� ��û�� ���� ��� �߻��ϴ� 403 Forbbiden �߻� ��
                {
                    //�ܱⰣ�� ���� ��û�� ������ �߻��մϴ�. 5�е��� �ڳ��� �Լ� ��û�� �����ؾ��մϴ�.  
                    return;
                }
                else if (bro.IsBadAccessTokenError())
                {
                    bool isRefreshSuccess = RefreshTheBackendToken(3); // �ִ� 3�� �������� ����

                    if (isRefreshSuccess)
                    {
                        CustomLogin(id, pw, maxRepeatCount - 1, onCompleted);
                    }
                    else
                    {
                        Debug.Log("��ū�� ���� ���߽��ϴ�.");
                    }
                }

            }
        }

        /// <summary> id, pw, ���� ���� ���н� �ݺ�Ƚ��, �Ϸ� �� ������ �Լ��� �޾� ȸ�������� �����ϴ� �Լ� </summary>
        public void CustomSignup(string id, string pw, int maxRepeatCount = 10, Action<BackendReturnObject> onCompleted = null)
        {
            Debug.Log("ȸ�������� ��û�մϴ�.");

            if (maxRepeatCount <= 0)
            {
                Debug.LogError("ȸ������ ����");
                return;
            }

            BackendReturnObject bro = Backend.BMember.CustomSignUp(id, pw);

            if (bro.IsSuccess())
            {
                onCompleted?.Invoke(bro);
                Debug.Log("ȸ������ ����!");
            }
            else
            {
                if (bro.IsClientRequestFailError()) // Ŭ���̾�Ʈ�� �Ͻ����� ��Ʈ��ũ ���� ��
                {
                    CustomSignup(id, pw, maxRepeatCount - 1, onCompleted);
                }
                else if (bro.IsServerError()) // ������ �̻� �߻� ��
                {
                    CustomSignup(id, pw, maxRepeatCount - 1, onCompleted);
                }
                else if (bro.IsMaintenanceError()) // ���� ���°� '����'�� ��
                {
                    //���� �˾�â + �α��� ȭ������ ������
                    Debug.Log("���� �������Դϴ�.");
                    return;
                }
                else if (bro.IsTooManyRequestError()) // �ܱⰣ�� ���� ��û�� ���� ��� �߻��ϴ� 403 Forbbiden �߻� ��
                {
                    //�ܱⰣ�� ���� ��û�� ������ �߻��մϴ�. 5�е��� �ڳ��� �Լ� ��û�� �����ؾ��մϴ�.  
                    return;
                }
                else if (bro.IsBadAccessTokenError())
                {
                    bool isRefreshSuccess = RefreshTheBackendToken(3); // �ִ� 3�� �������� ����

                    if (isRefreshSuccess)
                    {
                        CustomSignup(id, pw, maxRepeatCount - 1, onCompleted);
                    }
                    else
                    {
                        Debug.Log("��ū�� ���� ���߽��ϴ�.");
                    }
                }

            }
        }





        /// <summary> ���� ���� ID�� �޾� JsonData�� �Ѱ��ִ� �Լ� </summary>
        public void GetMyData(string selectedProbabilityFileId, int maxRepeatCount = 10, Action<BackendReturnObject> onCompleted = null)
        {
            if (!Backend.IsLogin)
            {
                Debug.LogError("�ڳ��� �α��� �Ǿ����� �ʽ��ϴ�.");
                return;
            }

            if (maxRepeatCount <= 0)
            {
                Debug.LogErrorFormat("{0} ��Ʈ�� ������ �޾ƿ��� ���߽��ϴ�.", selectedProbabilityFileId);
                return;
            }

            Backend.GameData.GetMyData(selectedProbabilityFileId, new Where(), callback =>
            {
                if (callback.IsSuccess())
                {
                    onCompleted?.Invoke(callback);
                    Debug.LogFormat("{0} ��Ʈ ������ �޾ƿԽ��ϴ�.", selectedProbabilityFileId);
                }
                else
                {
                    if (callback.IsClientRequestFailError()) // Ŭ���̾�Ʈ�� �Ͻ����� ��Ʈ��ũ ���� ��
                    {
                        GetMyData(selectedProbabilityFileId, maxRepeatCount - 1, onCompleted);
                    }
                    else if (callback.IsServerError()) // ������ �̻� �߻� ��
                    {
                        GetMyData(selectedProbabilityFileId, maxRepeatCount - 1, onCompleted);
                    }
                    else if (callback.IsMaintenanceError()) // ���� ���°� '����'�� ��
                    {
                        //���� �˾�â + �α��� ȭ������ ������
                        Debug.Log("���� �������Դϴ�.");
                        return;
                    }
                    else if (callback.IsTooManyRequestError()) // �ܱⰣ�� ���� ��û�� ���� ��� �߻��ϴ� 403 Forbbiden �߻� ��
                    {
                        //�ܱⰣ�� ���� ��û�� ������ �߻��մϴ�. 5�е��� �ڳ��� �Լ� ��û�� �����ؾ��մϴ�.  
                        return;
                    }
                    else if (callback.IsBadAccessTokenError())
                    {
                        bool isRefreshSuccess = RefreshTheBackendToken(3); // �ִ� 3�� �������� ����

                        if (isRefreshSuccess)
                        {
                            GetMyData(selectedProbabilityFileId, maxRepeatCount - 1, onCompleted);
                        }
                        else
                        {
                            Debug.Log("��ū�� ���� ���߽��ϴ�.");
                        }
                    }

                }
            });
        }


        /// <summary> ��Ʈ ID�� �ݺ� Ƚ��, ������ ���� ��� ������ �Լ��� �޾� �ڳ����� ChartData�� �޾ƿ��� �Լ� </summary>
        public void GetChartData(string selectedProbabilityFileId, int maxRepeatCount = 10, Action<BackendReturnObject> onCompleted = null)
        {
            if (!Backend.IsLogin)
            {
                Debug.LogError("�ڳ��� �α��� �Ǿ����� �ʽ��ϴ�.");
                return;
            }

            if (maxRepeatCount <= 0)
            {
                Debug.LogErrorFormat("{0} ��Ʈ�� ������ �޾ƿ��� ���߽��ϴ�.", selectedProbabilityFileId);
                return;
            }


            Backend.Chart.GetOneChartAndSave(selectedProbabilityFileId, callback =>
            {
                if (callback.IsSuccess())
                {
                    onCompleted?.Invoke(callback);
                    Debug.LogFormat("{0} ��Ʈ ������ �޾ƿԽ��ϴ�.", selectedProbabilityFileId);
                }
                else
                {
                    if (callback.IsClientRequestFailError()) // Ŭ���̾�Ʈ�� �Ͻ����� ��Ʈ��ũ ���� ��
                    {
                        GetChartData(selectedProbabilityFileId, maxRepeatCount - 1, onCompleted);
                    }
                    else if (callback.IsServerError()) // ������ �̻� �߻� ��
                    {
                        GetChartData(selectedProbabilityFileId, maxRepeatCount - 1, onCompleted);
                    }
                    else if (callback.IsMaintenanceError()) // ���� ���°� '����'�� ��
                    {
                        //���� �˾�â + �α��� ȭ������ ������
                        Debug.Log("���� �������Դϴ�.");
                        return;
                    }
                    else if (callback.IsTooManyRequestError()) // �ܱⰣ�� ���� ��û�� ���� ��� �߻��ϴ� 403 Forbbiden �߻� ��
                    {
                        //�ܱⰣ�� ���� ��û�� ������ �߻��մϴ�. 5�е��� �ڳ��� �Լ� ��û�� �����ؾ��մϴ�.  
                        return;
                    }
                    else if (callback.IsBadAccessTokenError())
                    {
                        bool isRefreshSuccess = RefreshTheBackendToken(3); // �ִ� 3�� �������� ����

                        if (isRefreshSuccess)
                        {
                            GetChartData(selectedProbabilityFileId, maxRepeatCount - 1, onCompleted);
                        }
                        else
                        {
                            Debug.Log("��ū�� ���� ���߽��ϴ�.");
                        }
                    }
                }
            });
        }


        /// <summary> ��Ʈ ID�� �ݺ� Ƚ��, ������ ���� ��� ������ �Լ��� �޾� �ڳ� GameData���� ������ �߰��ϴ� �Լ� </summary>
        public void GameDataInsert(string selectedProbabilityFileId, int maxRepeatCount, Param param, Action<BackendReturnObject> onCompleted = null)
        {
            if (!Backend.IsLogin)
            {
                Debug.LogError("�ڳ��� �α��� �Ǿ����� �ʽ��ϴ�.");
                return;
            }

            if (maxRepeatCount <= 0)
            {
                Debug.LogErrorFormat("{0} ���� ������ �߰����� ���߽��ϴ�.", selectedProbabilityFileId);
                return;
            }

            Backend.GameData.Insert(selectedProbabilityFileId, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    onCompleted?.Invoke(callback);
                    Debug.LogFormat("{0} ���� ������ �߰� �߽��ϴ�..", selectedProbabilityFileId);
                }
                else
                {
                    if (callback.IsClientRequestFailError()) // Ŭ���̾�Ʈ�� �Ͻ����� ��Ʈ��ũ ���� ��
                    {
                        GameDataInsert(selectedProbabilityFileId, maxRepeatCount - 1, param, onCompleted);
                    }
                    else if (callback.IsServerError()) // ������ �̻� �߻� ��
                    {
                        GameDataInsert(selectedProbabilityFileId, maxRepeatCount - 1, param, onCompleted);
                    }
                    else if (callback.IsMaintenanceError()) // ���� ���°� '����'�� ��
                    {
                        //���� �˾�â + �α��� ȭ������ ������
                        Debug.Log("���� �������Դϴ�.");
                        return;
                    }
                    else if (callback.IsTooManyRequestError()) // �ܱⰣ�� ���� ��û�� ���� ��� �߻��ϴ� 403 Forbbiden �߻� ��
                    {
                        //�ܱⰣ�� ���� ��û�� ������ �߻��մϴ�. 5�е��� �ڳ��� �Լ� ��û�� �����ؾ��մϴ�.  
                        return;
                    }
                    else if (callback.IsBadAccessTokenError())
                    {
                        bool isRefreshSuccess = RefreshTheBackendToken(3); // �ִ� 3�� �������� ����

                        if (isRefreshSuccess)
                        {
                            GameDataInsert(selectedProbabilityFileId, maxRepeatCount - 1, param, onCompleted);
                        }
                        else
                        {
                            Debug.Log("��ū�� ���� ���߽��ϴ�.");
                        }
                    }

                }
            });
        }


        /// <summary> ��Ʈ ID�� �ݺ� Ƚ��, ������ ���� ��� ������ �Լ��� �޾� �ڳ� GameData���� ������ �߰��ϴ� �Լ� </summary>
        public void GameDataUpdate(string selectedProbabilityFileId, string inDate, int maxRepeatCount, Param param, Action<BackendReturnObject> onCompleted = null)
        {
            if (!Backend.IsLogin)
            {
                Debug.LogError("�ڳ��� �α��� �Ǿ����� �ʽ��ϴ�.");
                return;
            }

            if (maxRepeatCount <= 0)
            {
                Debug.LogErrorFormat("{0} ���� ������ �������� ���߽��ϴ�.", selectedProbabilityFileId);
                return;
            }

            Backend.GameData.UpdateV2(selectedProbabilityFileId, inDate, Backend.UserInDate, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    onCompleted?.Invoke(callback);
                    Debug.LogFormat("{0} ���� ������ ���� �߽��ϴ�..", selectedProbabilityFileId);
                }
                else
                {
                    if (callback.IsClientRequestFailError()) // Ŭ���̾�Ʈ�� �Ͻ����� ��Ʈ��ũ ���� ��
                    {
                        GameDataUpdate(selectedProbabilityFileId, inDate, maxRepeatCount - 1, param, onCompleted);
                    }
                    else if (callback.IsServerError()) // ������ �̻� �߻� ��
                    {
                        GameDataUpdate(selectedProbabilityFileId, inDate, maxRepeatCount - 1, param, onCompleted);
                    }
                    else if (callback.IsMaintenanceError()) // ���� ���°� '����'�� ��
                    {
                        //���� �˾�â + �α��� ȭ������ ������
                        Debug.Log("���� �������Դϴ�.");
                        return;
                    }
                    else if (callback.IsTooManyRequestError()) // �ܱⰣ�� ���� ��û�� ���� ��� �߻��ϴ� 403 Forbbiden �߻� ��
                    {
                        //�ܱⰣ�� ���� ��û�� ������ �߻��մϴ�. 5�е��� �ڳ��� �Լ� ��û�� �����ؾ��մϴ�.  
                        return;
                    }
                    else if (callback.IsBadAccessTokenError())
                    {
                        bool isRefreshSuccess = RefreshTheBackendToken(3); // �ִ� 3�� �������� ����

                        if (isRefreshSuccess)
                        {
                            GameDataUpdate(selectedProbabilityFileId, inDate, maxRepeatCount - 1, param, onCompleted);
                        }
                        else
                        {
                            Debug.Log("��ū�� ���� ���߽��ϴ�.");
                        }
                    }

                }
            });
        }



        public bool RefreshTheBackendToken(int maxRepeatCount)
        {
            if (maxRepeatCount <= 0)
            {
                return false;
            }
            var callback = Backend.BMember.RefreshTheBackendToken();
            if (callback.IsSuccess())
            {
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

