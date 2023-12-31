﻿using System.Collections.Generic;

namespace ZFGinc.Assets.WorldOfCubes
{

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
        EventAction
    }

    public interface IClickable
    {
        public List<UIComponents> GetUI();
    }

}