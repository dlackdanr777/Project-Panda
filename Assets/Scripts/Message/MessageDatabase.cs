using UnityEngine;

[CreateAssetMenu(fileName = "Message Database", menuName = "Message Database")]
public class MessageDatabase : ScriptableObject
{
    public Message[] Messages;
}
