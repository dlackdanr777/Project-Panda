using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

[CreateAssetMenu(fileName = "Message Database", menuName = "Message Database")]
public class MessageDatabase : ScriptableObject
{
    public Message[] Messages;
}
