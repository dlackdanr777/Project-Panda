using System.Collections.Generic;

public class Database_Ssun : SingletonHandler<Database_Ssun>
{
    public List<Dictionary<string, object>> DataSnack;

    void Awake()
    {
        base.Awake();
        DataSnack = CSVReader.Read("Snack");
    }
}
