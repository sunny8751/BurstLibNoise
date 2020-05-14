using System;
using UnityEngine;
using BurstLibNoise.Generator;
using LibNoise;
using Unity.Collections;

namespace BurstLibNoise
{
    [CreateAssetMenu(fileName = "NoiseSettings", menuName = "NoiseSettings")]
    public class NoiseSettings : ScriptableObject {
        public ModuleData[] moduleData;

        public NoiseSettings() {}

        public NoiseSettings(ModuleData[] moduleData) {
            this.moduleData = moduleData;
        }
    }
}