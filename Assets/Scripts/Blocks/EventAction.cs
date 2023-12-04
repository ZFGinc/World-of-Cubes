using System;

namespace ZFGinc.Assets.WorldOfCubes
{
    [Serializable]
    public enum EventAction
    {
        None = 0,
        Destroy,
        SetStateTrue,
        SetStateFalse
    }
}
