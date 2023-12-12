using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HarvestItemImage", menuName = "Scriptable Object/HarvestImage", order = int.MaxValue)]
public class HarvestItemImage : ScriptableObject
{
    public GrowthStageImage[] GrowthStageImages;
}

[Serializable]
public class GrowthStageImage
{
    [Tooltip("0�ܰ� ���� �̹���")]
    public Sprite ZeroStepImage;
    [Tooltip("1�ܰ� ���� �̹���")]
    public Sprite OneStepImage;
    [Tooltip("2�ܰ� ���� �̹���")]
    public Sprite TwoStepImage;
}
