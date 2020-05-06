using System;
using System.Xml.Serialization;
using UnityEngine;
using Debug = System.Diagnostics.Debug;
using BurstLibNoise;
using Unity.Collections;
using LibNoise;

namespace BurstLibNoise
{

    /// <summary>
    /// Base class for burst noise modules. Must be implemented with LibNoise.ModuleBase to work.
    /// </summary>
    public interface BurstModuleBase
    {
        ModuleData GetData(int[] sources);

        BurstModuleBase Source(int i);
    }
}