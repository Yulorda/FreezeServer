using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

[Serializable]
public enum UnitState
{
    Stay,
    Run
}

[Serializable]
public class UnitMoveState : ISerializable
{
    public int id;

    [NonSerialized]
    public Position position;

    public UnitState state;

    public UnitMoveState()
    {
    }

    private UnitMoveState(SerializationInfo info, StreamingContext context)
    {
        position = new Position((int)info.GetValue("x", typeof(int)), (int)info.GetValue("y", typeof(int)));
        state = (UnitState)info.GetValue("state", typeof(UnitState));
        id = info.GetInt32("id");
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("x", position.X);
        info.AddValue("y", position.Y);
        info.AddValue("state", state);
        info.AddValue("id", id);
    }

    public override bool Equals(object obj)
    {
        if (obj is UnitMoveState otherPoint)
        {
            return id == otherPoint.id;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return new { id }.GetHashCode();
    }
}

[Serializable]
public enum UnitLifeStatus
{
    Create,
    Delete
}

[Serializable]
public class UnitLifeState
{
    public int id;
    public UnitLifeStatus lifeStatus;
    public UnitMoveState unitState;
}

[Serializable]
public class BoardProperty
{
    public int rank;
}

[Serializable]
public class MoveTask
{
    public List<UnitMoveState> unitMoveStates = new List<UnitMoveState>();
}
