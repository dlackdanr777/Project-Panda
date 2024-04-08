using System;

/// <summary>뒤끝 공지사항 데이터</summary>
public class Notice
{
    private string _title;
    public string Title => _title;

    private string _contents;
    public string Content => _contents;

    private DateTime _postingDate;
    public DateTime PostingDate => _postingDate;

    private string _imageKey;
    public string ImageKey => _imageKey;

    private string _inDate;
    public string InDate => _inDate;

    private string _uuid;
    public string UUID => _uuid;

    private string _linkUrl;
    public string LinkUrl => _linkUrl;

    private bool _isPublic;
    public bool IsPublic => _isPublic;

    private string _linkButtonName;
    public string LinkButtonName => _linkButtonName;

    private string _author;
    public string Author => _author;


    public Notice(string title, string contents, DateTime postingDate, string imageKey, string inDate, string uuid, string linkUrl, bool isPublic, string linkButtonName, string author)
    {
        _title = title;
        _contents = contents;
        _postingDate = postingDate;
        _imageKey = imageKey;
        _inDate = inDate;
        _uuid = uuid;
        _linkUrl = linkUrl;
        _isPublic = isPublic;
        _linkButtonName = linkButtonName;
        _author = author;
    }
}
