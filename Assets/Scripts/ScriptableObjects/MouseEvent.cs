using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Mouse")]
public class MouseEvent : ScriptableObject, IEvent
{
    List<IEventListener> Listeners;
    public List<Texture2D> Textures;
    public bool IsUI = false;

    [HideInInspector] public string Name { get => name; }

    private void OnEnable() => Listeners = new List<IEventListener>();

    public void Broadcast()
        => BroadcastEvent(null, Textures);

    private void BroadcastEvent(Component sender, object data)
    {
        for (int i = Listeners.Count - 1; i >= 0; i--)
            Listeners[i].OnEventRaised(this, sender, data);
    }

    public void RegisterListener(IEventListener listener)
    {
        if (!Listeners.Contains(listener))
            Listeners.Add(listener);
    }

    public void UnregisterListener(IEventListener listener)
    {
        if (Listeners.Contains(listener))
            Listeners.Remove(listener);
    }
}
