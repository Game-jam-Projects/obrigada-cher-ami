using UnityEngine;

public interface IEventListener
{
    public void OnEventRaised(IEvent gameEvent, Component sender, object data);
}
