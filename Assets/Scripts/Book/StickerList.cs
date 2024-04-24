using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickerList 
{
    public int MaxStickerCount { get; private set; } = 100; //최대 저장 개수
    public int Count => _stickerList.Count;

    private List<Sticker> _stickerList = new List<Sticker>();

    public List<Sticker> GetStickerList()
    {
        return _stickerList;
    }

    public void Add(Sticker sticker)
    {
        if(_stickerList.Count < MaxStickerCount)
        {
            for(int i=0;i<Count;i++)
            {
                if (_stickerList[i].Id.Equals(sticker.Id))
                {
                    return;
                }
            }
        }
        _stickerList.Add(sticker);
    }

    public void AddById(string id, Sprite image)
    {
        Add(new Sticker(id, image));
    }
}
