using UnityEngine;

[CreateAssetMenu(fileName = "PandaImage", menuName = "Scriptable Object/PandaImage", order = int.MaxValue)]
public class PandaImage : ScriptableObject
{
    public PandaStateImage[] PandaImages;
}
