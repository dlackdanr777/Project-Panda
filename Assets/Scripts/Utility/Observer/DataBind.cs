using System;
using UnityEngine;
using UnityEngine.Events;

namespace Muks.DataBind
{
    /// <summary> �پ��� �����͸� id������ ���� �����͸� ����, �����ϴ� Ŭ���� </summary>
    public static class DataBind
    {
        private static DataBindContainer<string> _textBindDic = new DataBindContainer<string>();
        private static DataBindContainer<UnityAction> _unityActionBindDic = new DataBindContainer<UnityAction>();
        private static DataBindContainer<Sprite> _spriteBindDic = new DataBindContainer<Sprite>();


        /// <summary> �����͸� �����ϰ� ���� �����ϴ� �Լ� </summary>
        public static void SetTextValue(string bindId, string data)
        {
            _textBindDic.SetValue(bindId, data);
        }

        /// <summary> id�� �´� ���� �����ϴ� �Լ� </summary>
        public static string GetTextValue(string bindId)
        {
            return _textBindDic.GetValue(bindId);
        }

        /// <summary> �����͸� �������ִ� Ŭ������ �����ϴ� �Լ� </summary>
        public static BindData<string> GetTextBindData(string bindId)
        {
            return _textBindDic.GetBindData(bindId);
        }



        /// <summary> �����͸� �����ϰ� ���� �����ϴ� �Լ� </summary>
        public static void SetUnityActionValue(string bindId, UnityAction action)
        {
            _unityActionBindDic.SetValue(bindId, action);
        }

        /// <summary> id�� �´� ���� �����ϴ� �Լ� </summary>
        public static UnityAction GetUnityActionValue(string bindId)
        {
            return _unityActionBindDic.GetValue(bindId);
        }

        /// <summary> �����͸� �������ִ� Ŭ������ �����ϴ� �Լ� </summary>
        public static BindData<UnityAction> GetUnityActionBindData(string bindId)
        {
            return _unityActionBindDic.GetBindData(bindId);
        }



        /// <summary> �����͸� �����ϰ� ���� �����ϴ� �Լ� </summary>
        public static void SetSpriteValue(string bindId, Sprite sprite)
        {
            _spriteBindDic.SetValue(bindId, sprite);
        }

        /// <summary> id�� �´� ���� �����ϴ� �Լ� </summary>
        public static Sprite GetSpriteValue(string bindId)
        {
            return _spriteBindDic.GetValue(bindId);
        }

        /// <summary> �����͸� �������ִ� Ŭ������ �����ϴ� �Լ� </summary>
        public static BindData<Sprite> GetSpriteBindData(string bindId)
        {
            return _spriteBindDic.GetBindData(bindId);
        }
    }
}
