using System;
using System.Collections.Generic;

namespace Muks.DataBind 
{
    /// <summary> BindData Ŭ������ ����,�ҷ����⸦ �ϴ� Ŭ���� </summary>
    public class DataBindContainer<T>
    {

        private Dictionary<string, BindData<T>> _dataContainerDic = new Dictionary<string, BindData<T>>();


        /// <summary> �����͸� �����ϰ� ���� �����ϴ� �Լ� </summary>
        public void SetValue(string bindId, T data)
        {
            if (!_dataContainerDic.TryGetValue(bindId, out BindData<T> bindData))
            {
                bindData = new BindData<T>();
                _dataContainerDic.Add(bindId, bindData);
            }

            bindData.Item = data;
        }


        /// <summary> id�� �´� ���� �����ϴ� �Լ� </summary>
        public T GetValue(string bindId)
        {
            if (!_dataContainerDic.TryGetValue(bindId, out BindData<T> textData))
                throw new Exception("bindData not exist");

            return textData.Item;
        }


        /// <summary> �����͸� �������ִ� Ŭ������ �����ϴ� �Լ� </summary>
        public BindData<T> GetBindData(string bindId)
        {
            if (!_dataContainerDic.TryGetValue(bindId, out BindData<T> textData))
            {
                textData = new BindData<T>();
                _dataContainerDic.Add(bindId, textData);
                //UnityEngine.Debug.LogWarning("bindData not exist");
                return textData;
            }

            return textData;
        }
    }
}