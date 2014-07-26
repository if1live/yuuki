using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yuuki
{
    public interface IChunkGenerator
    {
        byte Get(int i, int j, int k);
    }

    // shape and terrain generator functions
    class SphereChunkGenerator : IChunkGenerator
    {
        public byte Get(int i, int j, int k)
        {
            var retval = i*i+j*j+k*k <= 16*16 ? 1 : 0;
            return (byte)retval;
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
    }

    /*
    class CheckerChunkGenerator : IChunkGenerator
    {
        public byte Get(int i, int j, int k)
        {
            return !!((i+j+k)&1) ? (((i^j^k)&2) ? 1 : 0xffffff) : 0;
        }
    }
     * */

    class HillChunkGenerator : IChunkGenerator
    {
        public byte Get(int i, int j, int k)
        {
            var retval = j <= 16 * Math.Exp(-(i*i + k*k) / 64) ? 1 : 0;
            return (byte)retval;
        }

    }

    class ValleyChunkGenerator : IChunkGenerator
    {
        public byte Get(int i, int j, int k)
        {
            var retval = j <= (i*i + k*k) * 31 / (32*32*2) + 1 ? 1 : 0;
            return (byte)retval;
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
    }
  
}


