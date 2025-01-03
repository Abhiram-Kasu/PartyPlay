namespace KafkaEventTypes.RealtimeEventTypes;


public enum EventType
{
    //Server Induced
    SongQueueRemoved,
    //Client Induced
    SongQueueAdded,
    SongVoteChange,
    SongPlaybackChange,
    PartyUserChange,
    
}

public abstract record EventBase(EventType Type, PartyState NewState);
//Pretty sure we need the enum to keep the inheritance through serialization
public sealed record SongQueueRemovedEvent(PartyState NewState) : EventBase(EventType.SongQueueRemoved, NewState);
public sealed record SongQueueAddedEvent(PartyState NewState) : EventBase(EventType.SongQueueAdded, NewState);
public sealed record SongVoteChangeEvent(PartyState NewState) : EventBase(EventType.SongVoteChange, NewState);
public sealed record SongPlaybackChangeEvent(PartyState NewState) : EventBase(EventType.SongPlaybackChange, NewState);
public sealed record PartyUserChangeEvent(PartyState NewState) : EventBase(EventType.PartyUserChange, NewState);





