using System.Collections.Generic;
using UnityEngine;

public class MouseEventListener : MonoBehaviour, IEventListener
{
    bool UIMode = false;
    List<MouseEvent> MouseEvents;
    List<Texture2D> Sprites;
    int CurrentSprite = 0;
    float FramesPerSecond;
    float Counter = 0;

    public void OnEventRaised(IEvent gameEvent, Component sender, object data)
    {
        if (UIMode)
            if (((MouseEvent)gameEvent).IsUI)
                ConfigureCursor(data);
            else
                return;
        ConfigureCursor(data);
    }

    private void Update()
    {
        Counter = -Time.deltaTime;
        if (Counter < 0)
        {
            NextSprite();
            Counter = FramesPerSecond;
        }
    }

    private void OnEnable()
    {
        var defaultCursor = MouseEventHelper.GetMouseEvent("Default");
        ConfigureCursor(defaultCursor.Textures);

        MouseEvents = MouseEventHelper.GetMouseEvents();
        foreach (var mouseEvent in MouseEvents)
            mouseEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        foreach (var mouseEvent in MouseEvents)
            mouseEvent.UnregisterListener(this);
    }

    public void SetUIMode(bool enabled) => UIMode = enabled;

    private void ConfigureCursor(object data)
    {
        Sprites = (List<Texture2D>)data;
        FramesPerSecond = 1 / Sprites.Count;
        Counter = FramesPerSecond;
        CurrentSprite = 0;
    }

    private void NextSprite()
    {
        Cursor.SetCursor(Sprites[CurrentSprite], Vector2.zero, CursorMode.Auto);
        CurrentSprite++;
        if (CurrentSprite == Sprites.Count)
            CurrentSprite = 0;
    }
}
