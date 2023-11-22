public class UIInventory : UIView
{
    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    public override void Show()
    {
        gameObject.SetActive(true);
    }
}
