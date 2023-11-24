using System;


namespace Muks.DataBind
{

    public class ObjectGetter
    {
        private string _dataID;
        private BindData<object> _data;

        public ObjectGetter(string dataID, ref object obj)
        {
            _dataID = dataID;
            _data = DataBind.GetObjectValue(_dataID);
            _data.CallBack += UpdateObject;
            obj = _data.Item;
        }

        private void UpdateObject(object obj)
        {
            _data.Item = obj;

        }
    }
}
