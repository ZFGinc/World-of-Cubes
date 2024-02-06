using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZFGinc.WorldOfCubes
{
    [Serializable]
    public class BlockInfo
    {
        public int IdBlock;
        public float posX;
        public float posY;
        public float posZ;
        public float rotX;
        public float rotY;
        public float rotZ;
        public float rotW;
        public EventAction eventAction;

        public BlockInfo(int idBlock, Vector3 position, Quaternion rotation, EventAction eventAction)
        {
            IdBlock = idBlock;

            posX = position.x;
            posY = position.y;
            posZ = position.z;

            rotX = rotation.x;
            rotY = rotation.y;
            rotZ = rotation.z;
            rotW = rotation.w;
            this.eventAction = eventAction;
        }
    }

    [Serializable]
    public class TriggerBlockInfo : BlockInfo
    {
        public bool State;
        public List<BlockInfo> Links;

        public TriggerBlockInfo(int idBlock, Vector3 position, Quaternion rotation, EventAction eventAction, bool state, List<BlockInfo> links) :
        base(idBlock, position, rotation, eventAction)
        {
            State = state;
            Links = links;
        }
    }

    [Serializable]
    public class MovementBlockInfo : BlockInfo
    {
        public float newposX;
        public float newposY;
        public float newposZ;
        public float Speed;

        public MovementBlockInfo(int idBlock, Vector3 position, Quaternion rotation, EventAction eventAction, Vector3 newPosition, float speed) :
        base(idBlock, position, rotation, eventAction)
        {
            newposX = newPosition.x;
            newposY = newPosition.y;
            newposZ = newPosition.z;
            Speed = speed;
        }
    }

    [Serializable]
    public class RotationBlockInfo : BlockInfo
    {
        public float newrotX;
        public float newrotY;
        public float newrotZ;
        public float newrotW;
        public float Speed;

        public RotationBlockInfo(int idBlock, Vector3 position, Quaternion rotation, EventAction eventAction, Quaternion newRotation, float speed) :
        base(idBlock, position, rotation, eventAction)
        {
            newrotX = newRotation.x;
            newrotY = newRotation.y;
            newrotZ = newRotation.z;
            newrotW = newRotation.w;

            Speed = speed;
        }
    }

    [Serializable]
    public class TriggerWithTimerBlockInfo : BlockInfo
    {
        public bool State;
        public float Timer;
        public List<BlockInfo> Links;

        public TriggerWithTimerBlockInfo(int idBlock, Vector3 position, Quaternion rotation, EventAction eventAction, bool state, float timer, List<BlockInfo> links) :
        base(idBlock, position, rotation, eventAction)
        {
            State = state;
            Timer = timer;
            Links = links;
        }
    }

    [Serializable]
    public class ValveBlockInfo : BlockInfo
    {
        public float TriggerRotation;
        public bool IsRemember;
        public List<BlockInfo> Links;

        public ValveBlockInfo(int idBlock, Vector3 position, Quaternion rotation, EventAction eventAction, float triggerRotation, bool isRemember, List<BlockInfo> links) :
        base(idBlock, position, rotation, eventAction)
        {
            TriggerRotation = triggerRotation;
            IsRemember = isRemember;
            Links = links;
        }
    }

}