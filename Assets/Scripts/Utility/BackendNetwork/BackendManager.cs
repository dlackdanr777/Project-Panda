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


        /// <summary> 게임 정보 ID를 받아 JsonData를 넘겨주는 함수 </summary>
        public JsonData GetMyData(string selectedProbabilityFileId)
        {
            BackendReturnObject backendReturnObject = Backend.GameData.GetMyData(selectedProbabilityFileId, new Where());

            if (!backendReturnObject.IsSuccess())
            {
                Debug.Log("정보를 받아오지 못했습니다.");
                return null;
            }
                
            return backendReturnObject.FlattenRows();
        }


        /// <summary> 차트ID를 받아 JsonData를 넘겨주는 함수 </summary>
        public JsonData GetChartData(string selectedProbabilityFileId)
        {
            BackendReturnObject backendReturnObject = Backend.Chart.GetOneChartAndSave(selectedProbabilityFileId);

            if (!backendReturnObject.IsSuccess())
            {
                Debug.Log("정보를 받아오지 못했습니다.");
                return null;
            }

            return backendReturnObject.FlattenRows();
        }


    }
}

