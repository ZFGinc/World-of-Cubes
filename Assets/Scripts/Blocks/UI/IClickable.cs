using System.Collections.Generic;

public enum UIComponents
{
    Link = 0,
    State,
    Timer,
    Speed,
    OldPosition,
    NewPosition,
    OldRotation,
    NewRotation,
    TriggerRotation,
    IsRemember,
}

public interface IClickable
{
    public List<UIComponents> GetUI();
}

