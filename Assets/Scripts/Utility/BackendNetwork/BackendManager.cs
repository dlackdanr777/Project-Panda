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
        /// 이 값이 참일 때만 서버에 정보를 보냅니다.(로그인 실패인데 정보를 보내면 서버 정보가 초기화됨)
        /// </summary>
        public bool Login;

        public override void Awake()
        {
            base.Awake();
            BackendInit(10);
        }


        /// <summary>뒤끝 초기 설정</summary>

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

            switch (ErrorCheck(bro))
            {
                case BackendState.Failure:
                    Debug.LogError("초기화 실패");
                    break;

                case BackendState.Maintainance:
                    Debug.LogError("서버 점검 중");
                    break;

                case BackendState.Retry:
                    Debug.LogWarning("연결 재시도");
                    BackendInit(maxRepeatCount - 1);
                    break;

                case BackendState.Success:
                    Debug.Log("초기화 성공");
                    break;
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


        /// <summary> id, pw, 서버 연결 실패시 반복횟수, 완료 시 실행할 함수를 받아 로그인을 진행하는 함수 </summary>
        public void CustomLogin(string id, string pw, int maxRepeatCount = 10, Action<BackendReturnObject> onCompleted = null)
        {
            Debug.Log("로그인을 요청합니다.");

            if (maxRepeatCount <= 0)
            {
                Debug.LogError("로그인 실패");
                return;
            }

            BackendReturnObject bro = Backend.BMember.CustomLogin(id, pw);

            switch (ErrorCheck(bro))
            {
                case BackendState.Failure:
                    Debug.LogError("로그인 실패");
                    break;

                case BackendState.Maintainance:
                    Debug.LogError("서버 점검 중");
                    break;

                case BackendState.Retry:
                    Debug.LogWarning("연결 재시도");
                    CustomLogin(id, pw, maxRepeatCount - 1, onCompleted);
                    break;

                case BackendState.Success:
                    Debug.Log("로그인 성공");
                    onCompleted?.Invoke(bro);
                    break;
            }
        }


        /// <summary>게스트 로그인을 진행하는 함수 </summary>
        public void GuestLogin(int maxRepeatCount = 10, Action<BackendReturnObject> onCompleted = null)
        {
            Debug.Log("로그인을 요청합니다.");

            if (maxRepeatCount <= 0)
            {
                Debug.LogError("로그인 실패");
                return;
            }

            BackendReturnObject bro = Backend.BMember.GuestLogin("게스트 로그인");

            switch (ErrorCheck(bro))
            {
                case BackendState.Failure:
                    Debug.LogError("로그인 실패");
                    break;

                case BackendState.Maintainance:
                    Debug.LogError("서버 점검 중");
                    break;

                case BackendState.Retry:
                    Debug.LogWarning("연결 재시도");
                    GuestLogin(maxRepeatCount - 1, onCompleted);
                    break;

                case BackendState.Success:
                    Debug.Log("로그인 성공");
                    onCompleted?.Invoke(bro);
                    break;
            }
        }




        /// <summary> id, pw, 서버 연결 실패시 반복횟수, 완료 시 실행할 함수를 받아 회원가입을 진행하는 함수 </summary>
        public void CustomSignup(string id, string pw, int maxRepeatCount = 10, Action<BackendReturnObject> onCompleted = null)
        {
            Debug.Log("회원 가입을 요청합니다.");

            if (maxRepeatCount <= 0)
            {
                Debug.LogError("회원 가입 실패");
                return;
            }

            BackendReturnObject bro = Backend.BMember.CustomSignUp(id, pw);

            switch (ErrorCheck(bro))
            {
                case BackendState.Failure:
                    Debug.LogError("회원 가입 실패");
                    break;

                case BackendState.Maintainance:
                    Debug.LogError("서버 점검 중");
                    break;

                case BackendState.Retry:
                    Debug.LogWarning("연결 재시도");
                    CustomSignup(id, pw, maxRepeatCount - 1, onCompleted);
                    break;

                case BackendState.Success:
                    Debug.Log("회원가입 성공");
                    onCompleted?.Invoke(bro);
                    break;
            }

        }






        /// <summary> 내 데이터 ID를 받아 서버 연결 확인 후 받은 함수를 처리해주는 함수 </summary>
        public void GetMyData(string selectedProbabilityFileId, int maxRepeatCount = 10, Action<BackendReturnObject> onCompleted = null)
        {
            if (!Backend.IsLogin)
            {
                Debug.LogError("서버에 로그인 되어있지 않습니다.");
                return;
            }

            if (maxRepeatCount <= 0)
            {
                Debug.LogErrorFormat("{0} 차트의 정보를 받아오지 못했습니다.", selectedProbabilityFileId);
                return;
            }

            BackendReturnObject bro = Backend.GameData.GetMyData(selectedProbabilityFileId, new Where());

            switch (ErrorCheck(bro))
            {
                case BackendState.Failure:
                    Debug.LogError("연결 실패");
                    break;

                case BackendState.Maintainance:
                    Debug.LogError("서버 점검 중");
                    break;

                case BackendState.Retry:
                    Debug.LogWarning("연결 재시도");
                    GetMyData(selectedProbabilityFileId, maxRepeatCount - 1, onCompleted);
                    break;

                case BackendState.Success:
                    Debug.Log("내 정보 받아오기 성공");
                    onCompleted?.Invoke(bro);
                    break;
            }
        }


        /// <summary> 차트 ID와 반복 횟수, 연결이 됬을 경우 실행할 함수를 받아 뒤끝에서 ChartData를 받아오는 함수 </summary>
        public void GetChartData(string selectedProbabilityFileId, int maxRepeatCount = 10, Action<BackendReturnObject> onCompleted = null)
        {
            if (!Backend.IsLogin)
            {
                Debug.LogError("서버에 로그인 되어있지 않습니다.");
                return;
            }

            if (maxRepeatCount <= 0)
            {
                Debug.LogError("연결 실패");
                return;
            }

            BackendReturnObject bro = Backend.Chart.GetOneChartAndSave(selectedProbabilityFileId);

            switch (ErrorCheck(bro))
            {
                case BackendState.Failure:
                    Debug.LogError("연결 실패");
                    break;

                case BackendState.Maintainance:
                    Debug.LogError("서버 점검 중");
                    break;

                case BackendState.Retry:
                    Debug.LogWarning("연결 재시도");
                    GetChartData(selectedProbabilityFileId, maxRepeatCount - 1, onCompleted);
                    break;

                case BackendState.Success:
                    Debug.Log("차트 정보 받기 성공");
                    onCompleted?.Invoke(bro);
                    break;
            }
        }


        /// <summary> 차트 ID와 반복 횟수, 연결이 됬을 경우 실행할 함수를 받아 뒤끝 GameData란에 정보를 추가하는 함수 </summary>
        public void GameDataInsert(string selectedProbabilityFileId, int maxRepeatCount, Param param, Action<BackendReturnObject> onCompleted = null)
        {
            if (!Backend.IsLogin || !Login)
            {
                Debug.LogError("서버에 로그인 되어있지 않습니다.");
                return;
            }

            if (maxRepeatCount <= 0)
            {
                Debug.LogErrorFormat("{0} 게임 정보를 추가하지 못했습니다.", selectedProbabilityFileId);
                return;
            }

            Backend.GameData.Insert(selectedProbabilityFileId, param, callback =>
            {
                switch (ErrorCheck(callback))
                {
                    case BackendState.Failure:
                        Debug.LogError("연결 실패");
                        break;

                    case BackendState.Maintainance:
                        Debug.LogError("서버 점검 중");
                        break;

                    case BackendState.Retry:
                        Debug.LogWarning("연결 재시도");
                        GameDataInsert(selectedProbabilityFileId, maxRepeatCount - 1, param, onCompleted);
                        break;

                    case BackendState.Success:
                        Debug.Log("정보 추가 성공");
                        onCompleted?.Invoke(callback);
                        break;
                }        
            });
        }


        /// <summary> 차트 ID와 반복 횟수, 연결이 됬을 경우 실행할 함수를 받아 뒤끝 GameData란에 정보를 추가하는 함수 </summary>
        public void GameDataUpdate(string selectedProbabilityFileId, string inDate, int maxRepeatCount, Param param, Action<BackendReturnObject> onCompleted = null)
        {
            if (!Backend.IsLogin || !Login)
            {
                Debug.LogError("서버에 로그인 되어있지 않습니다.");
                return;
            }

            if (maxRepeatCount <= 0)
            {
                Debug.LogErrorFormat("{0} 게임 정보를 수정하지 못했습니다.", selectedProbabilityFileId);
                return;
            }

            Backend.GameData.UpdateV2(selectedProbabilityFileId, inDate, Backend.UserInDate, param, callback =>
            {
                switch (ErrorCheck(callback))
                {
                    case BackendState.Failure:
                        Debug.LogError("연결 실패");
                        break;

                    case BackendState.Maintainance:
                        Debug.LogError("서버 점검 중");
                        break;

                    case BackendState.Retry:
                        Debug.LogWarning("연결 재시도");
                        GameDataUpdate(selectedProbabilityFileId, inDate, maxRepeatCount - 1, param, onCompleted);
                        break;

                    case BackendState.Success:
                        Debug.Log("정보 수정 성공");
                        onCompleted?.Invoke(callback);
                        break;
                }

            });
        }


        /// <summary> 서버와 연결 상태를 체크하고 BackendState값을 반환하는 함수 </summary>
        public BackendState ErrorCheck(BackendReturnObject bro)
        {
            if (bro.IsSuccess())
            {
                Debug.Log("요청 성공!");
                return BackendState.Success;
            }
            else
            {
                if (bro.IsClientRequestFailError()) // 클라이언트의 일시적인 네트워크 끊김 시
                {
                    Debug.LogError("일시적인 네트워크 끊김");
                    return BackendState.Retry;
                }
                else if (bro.IsServerError()) // 서버의 이상 발생 시
                {
                    Debug.LogError("서버 이상 발생");
                    return BackendState.Retry;
                }
                else if (bro.IsMaintenanceError()) // 서버 상태가 '점검'일 시
                {
                    //점검 팝업창 + 로그인 화면으로 보내기
                    Debug.Log("게임 점검중");
                    return BackendState.Maintainance;
                }
                else if (bro.IsTooManyRequestError()) // 단기간에 많은 요청을 보낼 경우 발생하는 403 Forbbiden 발생 시
                {
                    //단기간에 많은 요청을 보내면 발생합니다. 5분동안 뒤끝의 함수 요청을 중지해야합니다.  
                    Debug.LogError("단기간에 많은 요청을 보냈습니다. 5분간 사용 불가");
                    return BackendState.Failure;
                }
                else if (bro.IsBadAccessTokenError())
                {
                    bool isRefreshSuccess = RefreshTheBackendToken(3); // 최대 3번 리프레시 실행

                    if (isRefreshSuccess)
                    {
                        Debug.LogError("토큰 발급 성공");
                        return BackendState.Retry;
                    }
                    else
                    {
                        Debug.LogError("토큰을 발급 받지 못했습니다.");
                        return BackendState.Failure;
                    }
                }

                //만약 기기에는 로그인 정보가 남아있는데 서버에 데이터가 없으면
                //기기에 저장된 로그인 정보를 삭제한다.
                else if (bro.GetMessage() == "bad customId, 잘못된 customId 입니다")
                    Backend.BMember.DeleteGuestInfo();

                else
                {
                    Debug.LogError(bro.GetErrorCode());
                }

                return BackendState.Retry;
            }
        }



        /// <summary> 뒤끝 토큰 재발급 함수 </summary>
        /// maxRepeatCount : 서버 연결 실패시 재 시도할 횟수
        public bool RefreshTheBackendToken(int maxRepeatCount)
        {
            if (maxRepeatCount <= 0)
            {
                Debug.Log("토큰 발급 실패");
                return false;
            }
            
            BackendReturnObject callback = Backend.BMember.RefreshTheBackendToken();

            if (callback.IsSuccess())
            {
                Debug.Log("토큰 발급 성공");
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

