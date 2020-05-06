# BurstLibNoise

![Burst LibNoise Comparison Image](http://i.imgur.com/6QdiZLq.png)

Added Unity Burst Job support for [LibNoise.Unity](https://github.com/ricardojmendez/LibNoise.Unity).

### Features
- ~3x speed boost on my computer using Burst parallel jobs
- Supports all LibNoise modules except for Cache, Curve, and Terrace
- Generate noise as texture for procedural generation heightmap

### Usage
Replace all "using LibNoise".\* with "using BurstLibNoise".\*  Remember to call Noise2D's Dispose method at the end. Modules descriptions can be found [here](http://libnoise.sourceforge.net/docs/group__modules.html).
