using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yuuki
{
    public class Chunk
    {
        public byte[] voxels;
        public ChunkDimension dims;
        public ChunkPosition position;

        public Chunk(byte[] voxels, ChunkDimension dims)
        {
            this.voxels = voxels;
            this.dims = dims;
            this.position = new ChunkPosition(0, 0, 0);
        }
    }

    public interface IGenerateVoxelChunk
    {
        Chunk Generate(VoxelPosition low, VoxelPosition high);
    }

    public class DefaultGenerateVoxelChunk : IGenerateVoxelChunk
    {
        public Chunk Generate(VoxelPosition low, VoxelPosition high)
        {
            return Voxel.Generate(low, high, new ValleyChunkGenerator());
        }
    }

}
