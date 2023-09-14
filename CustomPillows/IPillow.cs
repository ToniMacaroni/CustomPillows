using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomPillows
{
    internal interface IPillow
    {
        void Init(PillowParams pillowParams);
        Transform CachedTransform { get; }
        GameObject CachedGameObject { get; }
    }
}
