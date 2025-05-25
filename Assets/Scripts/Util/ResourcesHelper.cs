using UnityEngine;

public static class ResourcesHelper
{
    public static GameEvent GetGameEvent(string name) => Resources.Load(@"Events\" + name) as GameEvent;
}
