using System;


namespace Muks.DataBind
{

    public class ActionGetter
    {
        private string _dataID;
        private BindData<Action> _data;

        public ActionGetter(string dataID, ref Action action)
        {
            _dataID = dataID;
            _data = DataBind.GetActionValue(_dataID);
            _data.CallBack += UpdateAction;
            action = _data.Item;
        }

        private void UpdateAction(Action action)
        {
            _data.Item = action;
        }
    }
}
