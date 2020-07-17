using System;

namespace GameDevStack.CommonEnums
{
    [Serializable] public enum SpaceType
    {
        Local,
        World
    }

    [Serializable] public enum StartType
    {
        FromValue,
        ToValue
    }

    [Serializable] public enum DebugType
    {
        None,
        Log,
        LogWarning,
        LogError,
    }
}