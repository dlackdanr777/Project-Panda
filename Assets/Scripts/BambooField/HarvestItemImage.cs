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
    [Tooltip("0단계 성장 이미지")]
    public Sprite ZeroStepImage;
    [Tooltip("1단계 성장 이미지")]
    public Sprite OneStepImage;
    [Tooltip("2단계 성장 이미지")]
    public Sprite TwoStepImage;
}
