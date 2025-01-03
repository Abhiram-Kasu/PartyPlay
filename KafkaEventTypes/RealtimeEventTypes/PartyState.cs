using System.Collections.Frozen;
using System.Collections.Immutable;


namespace KafkaEventTypes.RealtimeEventTypes;


public readonly record struct CurrentSongState(int SongId, TimeSpan SongTime, int NetVotes);
public readonly record struct QueueSongState(int SongId, int UserId, int NetVotes);

public sealed record PartyState(int PartyId, 
    int OwnerId,
    FrozenSet<int> PartyUserIds,
    CurrentSongState CurrentSongState,
    ImmutableList<QueueSongState> QueueSongStates);
    