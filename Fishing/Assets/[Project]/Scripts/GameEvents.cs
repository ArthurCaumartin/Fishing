
using System;

public static class GameEvents
{
    public delegate void GameEvent();

    public static GameEvent onDiveStart;
    public static GameEvent onDiveEnd;

    public static void RemoveAllListener()
    {
        onDiveStart = null;
        onDiveEnd = null;
    }
}