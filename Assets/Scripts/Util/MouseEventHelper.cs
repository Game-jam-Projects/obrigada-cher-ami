using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class MouseEventHelper
{
    public static MouseEvent GetMouseEvent(string name) => Resources.Load(name) as MouseEvent;
    public static List<MouseEvent> GetMouseEvents() => Resources.LoadAll("", typeof(MouseEvent)).Cast<MouseEvent>().ToList();

    public static void Default() => GetMouseEvent("Default").Broadcast();
    public static void Clickable() => GetMouseEvent("Clickable").Broadcast();
    public static void Walkable() => GetMouseEvent("Walkable").Broadcast();
    public static void Pressed() => GetMouseEvent("Pressed").Broadcast();
    public static void Diario() => GetMouseEvent("Diario").Broadcast();

    public static void UIDefault() => GetMouseEvent("UIDefault").Broadcast();
    public static void UIClickable() => GetMouseEvent("UIClickable").Broadcast();
    public static void UIPressed() => GetMouseEvent("UIPressed").Broadcast();

}
