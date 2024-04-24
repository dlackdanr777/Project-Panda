using UnityEngine;

public class Furniture : Item
{
    public string StoryId;
    public FurnitureType Type;
    public EFurnitureViewType ViewType;
    public int Layer;
    public bool IsMine;
    public Sprite RoomImage;

    public Furniture(string id, string name, string description, string storyId, int price, string rank, string map, int layer, FurnitureType type, Sprite image, Sprite roomImage) : base(id, name, description, price, rank, map, image) 
    {
        StoryId = storyId;
        Type = type;
        RoomImage = roomImage;
        Layer = layer;

        switch (type)
        {
            case FurnitureType.None:
                ViewType = EFurnitureViewType.None; break;
            case FurnitureType.WallPaper:
                ViewType = EFurnitureViewType.WallPaper; break;
            case FurnitureType.Floor:
                ViewType=EFurnitureViewType.Floor; break;
            case FurnitureType.LeftWall:
                ViewType = EFurnitureViewType.Wall; break;
            case FurnitureType.RightWall:
                ViewType = EFurnitureViewType.Wall; break;
            case FurnitureType.RightFurniture:
                ViewType = EFurnitureViewType.Furniture; break;
            case FurnitureType.LeftFurniture:
                ViewType = EFurnitureViewType.Furniture; break;
            default:
                ViewType = EFurnitureViewType.Prop; break;

        }
    }
}
