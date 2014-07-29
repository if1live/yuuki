using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yuuki
{
    public class ChunkGeneratorHelper
    {
        IChunkGenerator generator;
        public ChunkGeneratorHelper(IChunkGenerator generator)
        {
            this.generator = generator;
        }
        public Chunk Generate()
        {
            return Generate(generator.Low, generator.High);
        }
        public Chunk Generate(VoxelPosition l, VoxelPosition h)
        {
            // from https://github.com/mikolalysenko/mikolalysenko.github.com/blob/master/MinecraftMeshes2/js/testdata.js#L4
            /*
             * js상의 함수 인자는 4개이다.
             * https://github.com/maxogden/voxel/blob/master/index.js#L24
             * function generate(l, h, f, game) {
             * 
             * 하지만 voxel.js의 샘플을 보면 3개만 쓴다
             * https://github.com/maxogden/voxel
             * voxel.generate([0,0,0], [32,32,32], voxel.generator['Hilly Terrain'])
             * 
             * 마지막 인자는 레거시의 흔적으로 추정되서 지웠다.
             */
            ChunkDimension d = new ChunkDimension(h.x - l.x, h.y - l.y, h.z - l.z);
            byte[] v = new byte[d.x * d.y * d.z];
            var n = 0;
            for (int k = l.z; k < h.z; ++k)
            {
                for (int j = l.y; j < h.y; ++j)
                {
                    for (int i = l.x; i < h.x; ++i, ++n)
                    {
                        v[n] = generator.Get(i, j, k);
                        /*
                         * js 원본의 코드는 f에 인자가 5개 넘어간다. 
                         * https://github.com/maxogden/voxel/blob/master/index.js#L31
                         * v[n] = f(i,j,k,n,game)
                         * 
                         * 하지만 chunk generator은 인자 3개만 쓰니 나머지는 버림
                         * 지금 당장은 있어봐야 의미가 없다
                         */
                    }
                }
            }

            Chunk data = new Chunk(v, d);
            return data;
        }

    }

    public interface IChunkGenerator
    {
        byte Get(int i, int j, int k);
        VoxelPosition Low { get; }
        VoxelPosition High { get; }
        Chunk Generate();
    }

    // shape and terrain generator functions
    class SphereChunkGenerator : IChunkGenerator
    {
        public byte Get(int i, int j, int k)
        {
            var retval = i*i+j*j+k*k <= 16*16 ? 1 : 0;
            return (byte)retval;
        }
        public VoxelPosition Low { get { return new VoxelPosition(-16, -16, -16); } }
        public VoxelPosition High { get { return new VoxelPosition(16, 16, 16); } }

        public Chunk Generate()
        {
            return new ChunkGeneratorHelper(this).Generate();
        }
    }

    class NoiseChunkGenerator : IChunkGenerator
    {
        Random random;

        public NoiseChunkGenerator()
        {
            random = new Random();
        }

        public byte Get(int i, int j, int k)
        {
            // return Math.random() < 0.1 ? Math.random() * 0xffffff : 0;
            if(random.NextDouble() < 0.1) {
                return (byte)(random.NextDouble() * (double)(0xffffff));
            } else {
                return 0;
            }
        }

        public VoxelPosition Low { get { return new VoxelPosition(0, 0, 0); } }
        public VoxelPosition High { get { return new VoxelPosition(16, 16, 16); } }

        public Chunk Generate()
        {
            return new ChunkGeneratorHelper(this).Generate();
        }
    }

    class DenseNoiseChunkGenerator : IChunkGenerator
    {
        Random random;
        
        public DenseNoiseChunkGenerator()
        {
            random = new Random();
        }

        public byte Get(int i, int j, int k)
        {
            // return Math.round(Math.random() * 0xffffff);
            double flag = 0xffffff;
            double rand = random.NextDouble();
            double val = random.NextDouble() * flag;
            return (byte)Math.Round(val);
        }

        public VoxelPosition Low { get { return new VoxelPosition(0, 0, 0); } }
        public VoxelPosition High { get { return new VoxelPosition(16, 16, 16); } }

        public Chunk Generate()
        {
            return new ChunkGeneratorHelper(this).Generate();
        }
    }

    /*
    class CheckerChunkGenerator : IChunkGenerator
    {
        public byte Get(int i, int j, int k)
        {
            return !!((i+j+k)&1) ? (((i^j^k)&2) ? 1 : 0xffffff) : 0;
        }
     * VoxelPosition low = new VoxelPosition(0,0,0);
            VoxelPosition high = new VoxelPosition(8, 8, 8);
    }
     * */

    class HillChunkGenerator : IChunkGenerator
    {
        public byte Get(int i, int j, int k)
        {
            var retval = j <= 16 * Math.Exp(-(i*i + k*k) / 64) ? 1 : 0;
            return (byte)retval;
        }

        public VoxelPosition Low { get { return new VoxelPosition(-16, 0, -16); } }
        public VoxelPosition High { get { return new VoxelPosition(16, 16, 16); } }

        public Chunk Generate()
        {
            return new ChunkGeneratorHelper(this).Generate();
        }
    }

    class ValleyChunkGenerator : IChunkGenerator
    {
        public byte Get(int i, int j, int k)
        {
            var retval = j <= (i*i + k*k) * 31 / (32*32*2) + 1 ? 1 : 0;
            return (byte)retval;
        }
        public VoxelPosition Low { get { return new VoxelPosition(0, 0, 0); } }
        public VoxelPosition High { get { return new VoxelPosition(32, 32, 32); } }

        public Chunk Generate()
        {
            return new ChunkGeneratorHelper(this).Generate();
        }
    }

    class HillyTerrainChunkGenerator : IChunkGenerator
    {
        Random random;
        
        public HillyTerrainChunkGenerator()
        {
            random = new Random();
        }

        public byte Get(int i, int j, int k)
        {
            var h0 = 3.0 * Math.Sin(Math.PI * i / 12.0 - Math.PI * k * 0.1) + 27;    
            if(j > h0+1) {
                return 0;
            }
            if(h0 <= j) {
                return 1;
            }
            var h1 = 2.0 * Math.Sin(Math.PI * i * 0.25 - Math.PI * k * 0.3) + 20;
            if(h1 <= j) {
                return 2;
            }
            if(2 < j) {
                var retval = random.NextDouble() < 0.1 ? 0x222222 : 0xaaaaaa;
                return (byte)retval;
            }
            return 3;
        }

        public VoxelPosition Low { get { return new VoxelPosition(0, 0, 0); } }
        public VoxelPosition High { get { return new VoxelPosition(32, 32, 32); } }

        public Chunk Generate()
        {
            return new ChunkGeneratorHelper(this).Generate();
        }
    }

    /*
     * https://github.com/mikolalysenko/mikolalysenko.github.com/blob/master/MinecraftMeshes/js/testdata.js
     * 에서 긁어온 샘플 몇개
     */
    class CubeChunkGenerator : IChunkGenerator
    {
        int size;

        public CubeChunkGenerator(int size)
        {
            this.size = size;
        }

        public byte Get(int i, int j, int k)
        {
            return 1;
        }

        public VoxelPosition Low { get { return new VoxelPosition(0, 0, 0); } }
        public VoxelPosition High { get { return new VoxelPosition(size, size, size); } }

        public Chunk Generate()
        {
            return new ChunkGeneratorHelper(this).Generate();
        }
    }

    class HoleChunkGenerator : IChunkGenerator
    {
        public VoxelPosition Low { get { return new VoxelPosition(0, 0, 0); } }
        public VoxelPosition High { get { return new VoxelPosition(16, 16, 1); } }

        public byte Get(int i, int j, int k)
        {
            if(Math.Abs(i-7) > 3 || Math.Abs(j-7) > 3)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public Chunk Generate()
        {
            return new ChunkGeneratorHelper(this).Generate();
        }
    }
}


