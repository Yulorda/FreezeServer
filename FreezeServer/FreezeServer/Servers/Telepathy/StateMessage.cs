// incoming message queue of <connectionId, message>
// (not a HashSet because one connection can have multiple new messages)
// -> a struct to minimize GC

public struct StateMessage
{
    public readonly int connectionId;
    public readonly EventType eventType;

    public StateMessage(int connectionId, EventType eventType)
    {
        this.connectionId = connectionId;
        this.eventType = eventType;
    }
}

public struct NetworkPackageData
{
    public readonly int connectionId;
    public readonly byte[] data;

    public NetworkPackageData(int connectionId, byte[] data)
    {
        this.connectionId = connectionId;
        this.data = data;
    }
}