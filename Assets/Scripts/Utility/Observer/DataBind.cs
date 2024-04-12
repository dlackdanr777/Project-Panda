using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Muks.DataBind
{
    public class DataBind : MonoBehaviour
    {
        private static Dictionary<string, BindData<string>> _dataBindingText;

        private static Dictionary<string, BindData<UnityAction>> _dataBindingUnityAction;

        private static Dictionary<string, BindData<Action>> _dataBindingAction; //사용하지 않음

        private static Dictionary<string, BindData<Sprite>> _dataBindingSprite;

        private static Dictionary<string, BindData<object>> _dataBindingObject;

        /// <summary>
        /// 텍스트 데이터의 연결장소와 보낼 값을 지정해두는 함수(data값이 변할 경우 GetTextValue로 연결된 곳의 값도 변함)
        /// </summary>
        public static void SetTextValue(string dataID, string data)
        {
            if (_dataBindingText == null)
                _dataBindingText = new Dictionary<string, BindData<string>>();

            if (!_dataBindingText.TryGetValue(dataID, out BindData<string> textData))
            {
                textData = new BindData<string>();
                _dataBindingText.Add(dataID, textData);
            }
            textData.Item = data;
        }


        /// <summary>
        /// 저장된 텍스트 데이터의 연결장소를 불러와 연결하는 함수
        /// </summary>
        public static BindData<string> GetTextBindData(string dataID)
        {
            if (_dataBindingText == null)
                _dataBindingText = new Dictionary<string, BindData<string>>();

            if (!_dataBindingText.TryGetValue(dataID, out BindData<string> textData))
            {
                textData = new BindData<string>();
                _dataBindingText.Add(dataID, textData);
            }
            return textData;
        }


        /// <summary>
        /// 버튼 데이터의 연결장소와 보낼값을 지정해두는 함수(action값이 변할 경우 GetButtonValue로 연결된 곳의 값도 변함)
        /// </summary>
        public static void SetUnityActionValue(string dataID, UnityAction action)
        {
            if (_dataBindingUnityAction == null)
                _dataBindingUnityAction = new Dictionary<string, BindData<UnityAction>>();

            if (!_dataBindingUnityAction.TryGetValue(dataID, out BindData<UnityAction> buttonData))
            {
                buttonData = new BindData<UnityAction>();
                _dataBindingUnityAction.Add(dataID, buttonData);
            }

            buttonData.Item = action;
        }


        /// <summary>
        /// 버튼 텍스트 데이터의 연결장소를 불러오는 함수
        /// </summary>
        public static BindData<UnityAction> GetUnityActionBindData(string dataID)
        {
            if (_dataBindingUnityAction == null)
                _dataBindingUnityAction = new Dictionary<string, BindData<UnityAction>>();

            if (!_dataBindingUnityAction.TryGetValue(dataID, out BindData<UnityAction> buttonData))
            {
                buttonData = new BindData<UnityAction>();
                _dataBindingUnityAction.Add(dataID, buttonData);
            }
            return buttonData;
        }


        /// <summary>
        /// 버튼 데이터의 연결장소와 보낼값을 지정해두는 함수(action값이 변할 경우 GetButtonValue로 연결된 곳의 값도 변함)
        /// </summary>
        public static void SetSpriteValue(string dataID, Sprite sprite)
        {
            if (_dataBindingSprite == null)
                _dataBindingSprite = new Dictionary<string, BindData<Sprite>>();

            if (!_dataBindingSprite.TryGetValue(dataID, out BindData<Sprite> imageData))
            {
                imageData = new BindData<Sprite>();
                _dataBindingSprite.Add(dataID, imageData);
            }
            imageData.Item = sprite;
        }


        /// <summary>dataID를 통해 이미지 데이터의 연결 장소를 불러오는 함수 </summary>
        public static BindData<Sprite> GetSpriteBindData(string dataID)
        {
            if (_dataBindingSprite == null)
                _dataBindingSprite = new Dictionary<string, BindData<Sprite>>();

            if (!_dataBindingSprite.TryGetValue(dataID, out BindData<Sprite> imageData))
            {
                imageData = new BindData<Sprite>();
                _dataBindingSprite.Add(dataID, imageData);
            }
            return imageData;
        }


        /// <summary>dataID를 통해 UnityAction 함수를 반환하는 함수 </summary>
        public static UnityAction GetUnityActionValue(string dataID)
        {
            if (_dataBindingUnityAction == null)
                _dataBindingUnityAction = new Dictionary<string, BindData<UnityAction>>();

            if (!_dataBindingUnityAction.TryGetValue(dataID, out BindData<UnityAction> actionData))
            {
                actionData = new BindData<UnityAction>();
                _dataBindingUnityAction.Add(dataID, actionData);
            }
            return actionData.Item;
        }


        /// <summary>dataID를 통해 오브젝트 자료형 데이터를 연결하는 함수 </summary>
        public static void SetObjectValue(string dataID, object obj)
        {
            if (_dataBindingObject == null)
                _dataBindingObject = new Dictionary<string, BindData<object>>();

            if (!_dataBindingObject.TryGetValue(dataID, out BindData<object> objectData))
            {
                objectData = new BindData<object>();
                _dataBindingObject.Add(dataID, objectData);
            }
            objectData.Item = obj;
        }



        /// <summary>dataID를 통해 연결된 오브젝트 자료형 데이터를 반환하는 함수 </summary>
        public static object GetObjectValue(string dataID)
        {
            if (_dataBindingObject == null)
                _dataBindingObject = new Dictionary<string, BindData<object>>();

            if (!_dataBindingObject.TryGetValue(dataID, out BindData<object> objectData))
            {
                objectData = new BindData<object>();
                _dataBindingObject.Add(dataID, objectData);
            }

            return objectData.Item;
        }
    }
}
