using System;
using System.Collections.Generic;
using UnityEngine;

namespace Input
{
    public interface IPlayerInput
    {
        public event Action<CrabDirection> Move;
        public event Action<CrabClawPose> ChangeClawPose;
        public event Action LungeLeft;
        public event Action LungeRight;
    }
}