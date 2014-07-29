using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yuuki
{
    /*
     * https://github.com/maxogden/voxel/blob/master/index.js#L83-L94
     */

    public class VoxelExampleFactory
    {
        public Chunk Sphere()
        {
            IChunkGenerator generator = new SphereChunkGenerator();
            return generator.Generate();
        }

        public Chunk Noise()
        {
            IChunkGenerator generator = new NoiseChunkGenerator();
            return generator.Generate();
        }

        public Chunk DenseNoise()
        {
            IChunkGenerator generator = new DenseNoiseChunkGenerator();
            return generator.Generate();
        }

        public Chunk Hill()
        {
            IChunkGenerator generator = new HillChunkGenerator();
            return generator.Generate();
        }
        public Chunk Valley()
        {
            IChunkGenerator generator = new ValleyChunkGenerator();
            return generator.Generate();
        }

        public Chunk HillyTerrain()
        {
            IChunkGenerator generator = new HillyTerrainChunkGenerator();
            return generator.Generate();
        }
        public Chunk Cube(int size)
        {
            IChunkGenerator generator = new CubeChunkGenerator(size);
            return generator.Generate();
        }
        public Chunk Cube1x1x1()
        {
            return Cube(1);
        }
        public Chunk Cube2x2x2()
        {
            return Cube(2);
        }
    }

    public class Voxel
    {
        public float Scale(int x, int fromLow, int fromHigh, int toLow, int toHigh)
        {
            return (x - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }
    }
}
