using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yuuki
{
    public class VoxelData
    {
        public byte[] voxels;
        public ChunkDimension dims;

        public VoxelData(byte[] voxels, ChunkDimension dims)
        {
            this.voxels = voxels;
            this.dims = dims;
        }
    }

    public class Voxel
    {
        public float Scale(int x, int fromLow, int fromHigh, int toLow, int toHigh)
        {
            return (x - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }

        // from https://github.com/mikolalysenko/mikolalysenko.github.com/blob/master/MinecraftMeshes2/js/testdata.js#L4
        public VoxelData Generate(VoxelPosition l, VoxelPosition h, IChunkGenerator f)
        {
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
            byte[] v = new byte[d.x*d.y*d.z];
            var n = 0;
            for(int k=l.z; k<h.z; ++k)
            {
                for (int j = l.y; j < h.y; ++j)
                {
                    for (int i = l.x; i < h.x; ++i, ++n) 
                    {
                        v[n] = f.Get(i,j,k);
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

            VoxelData data = new VoxelData(v, d);
            return data;
        }
    }
}
