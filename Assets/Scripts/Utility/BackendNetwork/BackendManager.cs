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
                Debug.LogError("뒤끝을 초기화하지 못했습니다. 다시 실행");
                return;
            }

            //BackendReturnObject는 통신의 결과로 넘어오는 값을 저장하는 클래스
            //뒤끝 SKD를 이용하여 서버로 요청한 모든 기능은 BackendReturnObject클래스 형태로 리턴

            //SDK를 초기화 후 BackendReturnObject클래스 리턴
            BackendReturnObject bro = Backend.Initialize(true);

            //초기화가 성공했을 경우?
            if (bro.IsSuccess())
            {
                //성공일 경우 statusCode 204 Success
                Debug.LogFormat("초기화 성공: {0}", bro);
            }
            else
            {
                // 실패일 경우 statusCode 400대 에러 발생 
                Debug.LogFormat("초기화 실패: {0}", bro);
                BackendInit(maxRepeatCount - 1);
            }
        }


        private void Update()
        {
            //SDK가 초기화되어있을때
            if (Backend.IsInitialized)
            {
                //뒤끝 비동기 함수 사용 시, 메인쓰레드에서 콜백을 처리해주는 Dispatch을 실행
                Backend.AsyncPoll();
            }
        }

        public void CustomLogin(string id, string pw, int maxRepeatCount = 10, Action<BackendReturnObject> onCompleted = null)
        {
            Debug.Log("로그인을 요청합니다.");

            if (maxRepeatCount <= 0)
            {
                Debug.LogError("로그인 실패");
                return;
            }

            BackendReturnObject bro = Backend.BMember.CustomLogin(id, pw);

            if (bro.IsSuccess())
            {
                onCompleted?.Invoke(bro);
                Debug.Log("로그인 성공!");
            }
            else
            {
                if (bro.IsClientRequestFailError()) // 클라이언트의 일시적인 네트워크 끊김 시
                {
                    CustomLogin(id, pw, maxRepeatCount - 1, onCompleted);
                }
                else if (bro.IsServerError()) // 서버의 이상 발생 시
                {
                    CustomLogin(id, pw, maxRepeatCount - 1, onCompleted);
                }
                else if (bro.IsMaintenanceError()) // 서버 상태가 '점검'일 시
                {
                    //점검 팝업창 + 로그인 화면으로 보내기
                    Debug.Log("게임 점검중입니다.");
                    return;
                }
                else if (bro.IsTooManyRequestError()) // 단기간에 많은 요청을 보낼 경우 발생하는 403 Forbbiden 발생 시
                {
                    //단기간에 많은 요청을 보내면 발생합니다. 5분동안 뒤끝의 함수 요청을 중지해야합니다.  
                    return;
                }
                else if (bro.IsBadAccessTokenError())
                {
                    bool isRefreshSuccess = RefreshTheBackendToken(3); // 최대 3번 리프레시 실행

                    if (isRefreshSuccess)
                    {
                        CustomLogin(id, pw, maxRepeatCount - 1, onCompleted);
                    }
                    else
                    {
                        Debug.Log("토큰을 받지 못했습니다.");
                    }
                }

            }
        }

        /// <summary> id, pw, 서버 연결 실패시 반복횟수, 완료 시 실행할 함수를 받아 회원가입을 진행하는 함수 </summary>
        public void CustomSignup(string id, string pw, int maxRepeatCount = 10, Action<BackendReturnObject> onCompleted = null)
        {
            Debug.Log("회원가입을 요청합니다.");

            if (maxRepeatCount <= 0)
            {
                Debug.LogError("회원가입 실패");
                return;
            }

            BackendReturnObject bro = Backend.BMember.CustomSignUp(id, pw);

            if (bro.IsSuccess())
            {
                onCompleted?.Invoke(bro);
                Debug.Log("회원가입 성공!");
            }
            else
            {
                if (bro.IsClientRequestFailError()) // 클라이언트의 일시적인 네트워크 끊김 시
                {
                    CustomSignup(id, pw, maxRepeatCount - 1, onCompleted);
                }
                else if (bro.IsServerError()) // 서버의 이상 발생 시
                {
                    CustomSignup(id, pw, maxRepeatCount - 1, onCompleted);
                }
                else if (bro.IsMaintenanceError()) // 서버 상태가 '점검'일 시
                {
                    //점검 팝업창 + 로그인 화면으로 보내기
                    Debug.Log("게임 점검중입니다.");
                    return;
                }
                else if (bro.IsTooManyRequestError()) // 단기간에 많은 요청을 보낼 경우 발생하는 403 Forbbiden 발생 시
                {
                    //단기간에 많은 요청을 보내면 발생합니다. 5분동안 뒤끝의 함수 요청을 중지해야합니다.  
                    return;
                }
                else if (bro.IsBadAccessTokenError())
                {
                    bool isRefreshSuccess = RefreshTheBackendToken(3); // 최대 3번 리프레시 실행

                    if (isRefreshSuccess)
                    {
                        CustomSignup(id, pw, maxRepeatCount - 1, onCompleted);
                    }
                    else
                    {
                        Debug.Log("토큰을 받지 못했습니다.");
                    }
                }

            }
        }





        /// <summary> 게임 정보 ID를 받아 JsonData를 넘겨주는 함수 </summary>
        public void GetMyData(string selectedProbabilityFileId, int maxRepeatCount = 10, Action<BackendReturnObject> onCompleted = null)
        {
            if (!Backend.IsLogin)
            {
                Debug.LogError("뒤끝에 로그인 되어있지 않습니다.");
                return;
            }

            if (maxRepeatCount <= 0)
            {
                Debug.LogErrorFormat("{0} 차트의 정보를 받아오지 못했습니다.", selectedProbabilityFileId);
                return;
            }

            Backend.GameData.GetMyData(selectedProbabilityFileId, new Where(), callback =>
            {
                if (callback.IsSuccess())
                {
                    onCompleted?.Invoke(callback);
                    Debug.LogFormat("{0} 차트 정보를 받아왔습니다.", selectedProbabilityFileId);
                }
                else
                {
                    if (callback.IsClientRequestFailError()) // 클라이언트의 일시적인 네트워크 끊김 시
                    {
                        GetMyData(selectedProbabilityFileId, maxRepeatCount - 1, onCompleted);
                    }
                    else if (callback.IsServerError()) // 서버의 이상 발생 시
                    {
                        GetMyData(selectedProbabilityFileId, maxRepeatCount - 1, onCompleted);
                    }
                    else if (callback.IsMaintenanceError()) // 서버 상태가 '점검'일 시
                    {
                        //점검 팝업창 + 로그인 화면으로 보내기
                        Debug.Log("게임 점검중입니다.");
                        return;
                    }
                    else if (callback.IsTooManyRequestError()) // 단기간에 많은 요청을 보낼 경우 발생하는 403 Forbbiden 발생 시
                    {
                        //단기간에 많은 요청을 보내면 발생합니다. 5분동안 뒤끝의 함수 요청을 중지해야합니다.  
                        return;
                    }
                    else if (callback.IsBadAccessTokenError())
                    {
                        bool isRefreshSuccess = RefreshTheBackendToken(3); // 최대 3번 리프레시 실행

                        if (isRefreshSuccess)
                        {
                            GetMyData(selectedProbabilityFileId, maxRepeatCount - 1, onCompleted);
                        }
                        else
                        {
                            Debug.Log("토큰을 받지 못했습니다.");
                        }
                    }

                }
            });
        }


        /// <summary> 차트 ID와 반복 횟수, 연결이 됬을 경우 실행할 함수를 받아 뒤끝에서 ChartData를 받아오는 함수 </summary>
        public void GetChartData(string selectedProbabilityFileId, int maxRepeatCount = 10, Action<BackendReturnObject> onCompleted = null)
        {
            if (!Backend.IsLogin)
            {
                Debug.LogError("뒤끝에 로그인 되어있지 않습니다.");
                return;
            }

            if (maxRepeatCount <= 0)
            {
                Debug.LogErrorFormat("{0} 차트의 정보를 받아오지 못했습니다.", selectedProbabilityFileId);
                return;
            }


            Backend.Chart.GetOneChartAndSave(selectedProbabilityFileId, callback =>
            {
                if (callback.IsSuccess())
                {
                    onCompleted?.Invoke(callback);
                    Debug.LogFormat("{0} 차트 정보를 받아왔습니다.", selectedProbabilityFileId);
                }
                else
                {
                    if (callback.IsClientRequestFailError()) // 클라이언트의 일시적인 네트워크 끊김 시
                    {
                        GetChartData(selectedProbabilityFileId, maxRepeatCount - 1, onCompleted);
                    }
                    else if (callback.IsServerError()) // 서버의 이상 발생 시
                    {
                        GetChartData(selectedProbabilityFileId, maxRepeatCount - 1, onCompleted);
                    }
                    else if (callback.IsMaintenanceError()) // 서버 상태가 '점검'일 시
                    {
                        //점검 팝업창 + 로그인 화면으로 보내기
                        Debug.Log("게임 점검중입니다.");
                        return;
                    }
                    else if (callback.IsTooManyRequestError()) // 단기간에 많은 요청을 보낼 경우 발생하는 403 Forbbiden 발생 시
                    {
                        //단기간에 많은 요청을 보내면 발생합니다. 5분동안 뒤끝의 함수 요청을 중지해야합니다.  
                        return;
                    }
                    else if (callback.IsBadAccessTokenError())
                    {
                        bool isRefreshSuccess = RefreshTheBackendToken(3); // 최대 3번 리프레시 실행

                        if (isRefreshSuccess)
                        {
                            GetChartData(selectedProbabilityFileId, maxRepeatCount - 1, onCompleted);
                        }
                        else
                        {
                            Debug.Log("토큰을 받지 못했습니다.");
                        }
                    }
                }
            });
        }


        /// <summary> 차트 ID와 반복 횟수, 연결이 됬을 경우 실행할 함수를 받아 뒤끝 GameData란에 정보를 추가하는 함수 </summary>
        public void GameDataInsert(string selectedProbabilityFileId, int maxRepeatCount, Param param, Action<BackendReturnObject> onCompleted = null)
        {
            if (!Backend.IsLogin)
            {
                Debug.LogError("뒤끝에 로그인 되어있지 않습니다.");
                return;
            }

            if (maxRepeatCount <= 0)
            {
                Debug.LogErrorFormat("{0} 게임 정보를 추가하지 못했습니다.", selectedProbabilityFileId);
                return;
            }

            Backend.GameData.Insert(selectedProbabilityFileId, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    onCompleted?.Invoke(callback);
                    Debug.LogFormat("{0} 게임 정보를 추가 했습니다..", selectedProbabilityFileId);
                }
                else
                {
                    if (callback.IsClientRequestFailError()) // 클라이언트의 일시적인 네트워크 끊김 시
                    {
                        GameDataInsert(selectedProbabilityFileId, maxRepeatCount - 1, param, onCompleted);
                    }
                    else if (callback.IsServerError()) // 서버의 이상 발생 시
                    {
                        GameDataInsert(selectedProbabilityFileId, maxRepeatCount - 1, param, onCompleted);
                    }
                    else if (callback.IsMaintenanceError()) // 서버 상태가 '점검'일 시
                    {
                        //점검 팝업창 + 로그인 화면으로 보내기
                        Debug.Log("게임 점검중입니다.");
                        return;
                    }
                    else if (callback.IsTooManyRequestError()) // 단기간에 많은 요청을 보낼 경우 발생하는 403 Forbbiden 발생 시
                    {
                        //단기간에 많은 요청을 보내면 발생합니다. 5분동안 뒤끝의 함수 요청을 중지해야합니다.  
                        return;
                    }
                    else if (callback.IsBadAccessTokenError())
                    {
                        bool isRefreshSuccess = RefreshTheBackendToken(3); // 최대 3번 리프레시 실행

                        if (isRefreshSuccess)
                        {
                            GameDataInsert(selectedProbabilityFileId, maxRepeatCount - 1, param, onCompleted);
                        }
                        else
                        {
                            Debug.Log("토큰을 받지 못했습니다.");
                        }
                    }

                }
            });
        }


        /// <summary> 차트 ID와 반복 횟수, 연결이 됬을 경우 실행할 함수를 받아 뒤끝 GameData란에 정보를 추가하는 함수 </summary>
        public void GameDataUpdate(string selectedProbabilityFileId, string inDate, int maxRepeatCount, Param param, Action<BackendReturnObject> onCompleted = null)
        {
            if (!Backend.IsLogin)
            {
                Debug.LogError("뒤끝에 로그인 되어있지 않습니다.");
                return;
            }

            if (maxRepeatCount <= 0)
            {
                Debug.LogErrorFormat("{0} 게임 정보를 수정하지 못했습니다.", selectedProbabilityFileId);
                return;
            }

            Backend.GameData.UpdateV2(selectedProbabilityFileId, inDate, Backend.UserInDate, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    onCompleted?.Invoke(callback);
                    Debug.LogFormat("{0} 게임 정보를 수정 했습니다..", selectedProbabilityFileId);
                }
                else
                {
                    if (callback.IsClientRequestFailError()) // 클라이언트의 일시적인 네트워크 끊김 시
                    {
                        GameDataUpdate(selectedProbabilityFileId, inDate, maxRepeatCount - 1, param, onCompleted);
                    }
                    else if (callback.IsServerError()) // 서버의 이상 발생 시
                    {
                        GameDataUpdate(selectedProbabilityFileId, inDate, maxRepeatCount - 1, param, onCompleted);
                    }
                    else if (callback.IsMaintenanceError()) // 서버 상태가 '점검'일 시
                    {
                        //점검 팝업창 + 로그인 화면으로 보내기
                        Debug.Log("게임 점검중입니다.");
                        return;
                    }
                    else if (callback.IsTooManyRequestError()) // 단기간에 많은 요청을 보낼 경우 발생하는 403 Forbbiden 발생 시
                    {
                        //단기간에 많은 요청을 보내면 발생합니다. 5분동안 뒤끝의 함수 요청을 중지해야합니다.  
                        return;
                    }
                    else if (callback.IsBadAccessTokenError())
                    {
                        bool isRefreshSuccess = RefreshTheBackendToken(3); // 최대 3번 리프레시 실행

                        if (isRefreshSuccess)
                        {
                            GameDataUpdate(selectedProbabilityFileId, inDate, maxRepeatCount - 1, param, onCompleted);
                        }
                        else
                        {
                            Debug.Log("토큰을 받지 못했습니다.");
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
                if (callback.IsClientRequestFailError()) // 클라이언트의 일시적인 네트워크 끊김 시
                {
                    return RefreshTheBackendToken(maxRepeatCount - 1);
                }
                else if (callback.IsServerError()) // 서버의 이상 발생 시
                {
                    return RefreshTheBackendToken(maxRepeatCount - 1);
                }
                else if (callback.IsMaintenanceError()) // 서버 상태가 '점검'일 시
                {
                    //점검 팝업창 + 로그인 화면으로 보내기
                    return false;
                }
                else if (callback.IsTooManyRequestError()) // 단기간에 많은 요청을 보낼 경우 발생하는 403 Forbbiden 발생 시
                {
                    //너무 많은 요청을 보내는 중
                    return false;
                }
                else
                {
                    //재시도를 해도 액세스토큰 재발급이 불가능한 경우
                    //커스텀 로그인 혹은 페데레이션 로그인을 통해 수동 로그인을 진행해야합니다.  
                    //중복 로그인일 경우 401 bad refreshToken 에러와 함께 발생할 수 있습니다.  
                    Debug.Log("게임 접속에 문제가 발생했습니다. 로그인 화면으로 돌아갑니다\n" + callback.ToString());
                    return false;
                }
            }
        }
    }
}

