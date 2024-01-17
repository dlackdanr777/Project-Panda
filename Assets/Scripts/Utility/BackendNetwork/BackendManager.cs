using BackEnd;
using UnityEngine;


namespace Muks.BackEnd
{
    public class BackendManager : SingletonHandler<BackendManager>
    {

        public override void Awake()
        {
            base.Awake();
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
    }
}

