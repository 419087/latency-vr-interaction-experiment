using UnityEngine;

[CreateAssetMenu(fileName = "EntryConfig", menuName = "Scriptable Objects/EntryConfig")]
public class EntryConfig : ScriptableObject
{
    public bool IsServer { get; private set; }
    public int PlayerId { get; private set; }
    public int ParticipantId { get; private set; }

    public void SetEntryConfigAsServer()
    {
        IsServer = true;
    }

    public void SetEntryConfigAsClient(int playerId, int participantId)
    {
        IsServer = false;
        PlayerId = playerId;
        ParticipantId = participantId;
    }
}
