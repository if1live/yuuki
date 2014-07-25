﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Yuuki
{
    public struct Position
    {
        public float x;
        public float y;
        public float z;

        public Position(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public struct ChunkPosition
    {
        public int x;
        public int y;
        public int z;

        public ChunkPosition(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public string ToKey()
        {
            return String.Format("{0}|{1}|{2}", x, y, z);
        }
    }

    public struct ChunkerBound
    {
        public Position low;
        public Position high;

        public ChunkerBound(Position low, Position high)
        {
            this.low = low;
            this.high = high;
        }
    }

    public struct ChunkerParameter
    {
        public int distance;
        public int chunkSize;
        public int cubeSize;
    }

    public class Chunker
    {
        private int distance;
        private int chunkSize;
        private int cubeSize;

        private int chunkBits;
        private Dictionary<string, Chunker> chunks;

        public Chunker(ChunkerParameter param)
        {
            this.distance = param.distance;
            this.chunkSize = param.chunkSize;
            this.cubeSize = param.cubeSize;

            //this.generateVoxelChunk = opts.generateVoxelChunk
            this.chunks = new Dictionary<string, Chunker>();
            //this.meshes = {}

            if (MathHelper.IsPowOfTwo(this.chunkSize) == false)
            {
                throw new Exception("chunkSize must be a power of 2");
            }

            int bits = 0;
            for (var size = this.chunkSize; size > 0; size >>= 1)
            {
                bits++;
            }
            this.chunkBits = bits - 1;
        }

        public List<ChunkPosition> NearbyChunks(Position position)
        {
            //https://github.com/maxogden/voxel/blob/ff89d80f4dfa09dec38d72afb30e4107328ee9d4/chunker.js#L32
            return NearbyChunks(position, this.distance);
        }

        public List<ChunkPosition> NearbyChunks(Position position, int distance)
        {
            ChunkPosition current = ChunkAtPosition(position);
            int x = current.x;
            int y = current.y;
            int z = current.z;
            int dist = distance;
            List<ChunkPosition> nearby = new List<ChunkPosition>();
            for (int cx = (x - dist); cx != (x + dist); ++cx)
            {
                for (int cy = (y - dist); cy != (y + dist); ++cy)
                {
                    for (int cz = (z - dist); cz != (z + dist); ++cz)
                    {
                        nearby.Add(new ChunkPosition(cx, cy, cz));
                    }
                }
            }
            return nearby;
        }

        /*
        Chunker.prototype.requestMissingChunks = function(position) {
            var self = this
            this.nearbyChunks(position).map(function(chunk) {
                if (!self.chunks[chunk.join('|')]) {
                    self.emit('missingChunk', chunk)
                }
            })
        }
        */

        public ChunkerBound GetBounds(int x, int y, int z)
        {
            int bits = this.chunkBits;
            Position low = new Position(x << bits, y << bits, z << bits);
            Position high = new Position((x + 1) << bits, (y + 1) << bits, (z + 1) << bits);
            return new ChunkerBound(low, high);
        }

        /*
         * Chunker.prototype.generateChunk = function(x, y, z) {
          var self = this
          var bounds = this.getBounds(x, y, z)
          var chunk = this.generateVoxelChunk(bounds[0], bounds[1], x, y, z)
          var position = [x, y, z]
          chunk.position = position
          this.chunks[position.join('|')] = chunk
          return chunk
        }
         */


        public ChunkPosition ChunkAtCoordinates(int x, int y, int z)
        {
            int bits = this.chunkBits;
            int cx = x >> bits;
            int cy = y >> bits;
            int cz = z >> bits;
            ChunkPosition chunkPos = new ChunkPosition(cx, cy, cz);
            return chunkPos;
        }

        public ChunkPosition ChunkAtPosition(Position position)
        {
            int cubeSize = this.cubeSize;
            int x = (int)Math.Floor(position.x / cubeSize);
            int y = (int)Math.Floor(position.y / cubeSize);
            int z = (int)Math.Floor(position.z / cubeSize);
            ChunkPosition chunkPos = ChunkAtCoordinates(x, y, z);
            return chunkPos;
        }

        public int VoxelIndexFromCoordinates(int x, int y, int z)
        {
            int bits = this.chunkBits;
            int mask = (1 << bits) - 1;
            int vidx = (x & mask) + ((y & mask) << bits) + ((z & mask) << bits * 2);
            return vidx;
        }

        public int VoxelIndexFromPosition(Position pos)
        {
            Position v = this.VoxelVector(pos);
            return this.VoxelIndex(v);
        }


        public int VoxelAtCoordinates(int x, int y, int z)
        {
            string ckey = this.ChunkAtCoordinates(x, y, z).ToKey();
            Chunker chunk = this.chunks[ckey];
            if (chunk == null)
            {
                return -1;
            }
            int vidx = this.VoxelIndexFromCoordinates(x, y, z);
            //TODO
            //int v = chunk.voxels[vidx];
            int v = 1;
            return v;
        }

        public int VoxelAtCoordinates(int x, int y, int z, int val)
        {
            string ckey = this.ChunkAtCoordinates(x, y, z).ToKey();
            Chunker chunk = this.chunks[ckey];
            if (chunk == null)
            {
                return -1;
            }
            int vidx = this.VoxelIndexFromCoordinates(x, y, z);
            //TODO
            //int v = chunk.voxels[vidx];
            //chunk.voxels[vidx] = val;
            int v = 1;
            return v;
        }


        public int VoxelAtPosition(Position pos)
        {
            int cubeSize = this.cubeSize;
            int x = (int)Math.Floor(pos.x / cubeSize);
            int y = (int)Math.Floor(pos.y / cubeSize);
            int z = (int)Math.Floor(pos.z / cubeSize);
            int v = this.VoxelAtCoordinates(x, y, z);
            return v;
        }

        public int VoxelAtPosition(Position pos, int val)
        {
            int cubeSize = this.cubeSize;
            int x = (int)Math.Floor(pos.x / cubeSize);
            int y = (int)Math.Floor(pos.y / cubeSize);
            int z = (int)Math.Floor(pos.z / cubeSize);
            int v = this.VoxelAtCoordinates(x, y, z, val);
            return v;
        }

        // deprecated
        public int VoxelIndex(Position voxelVector)
        {
            return -1;
            //TODO
            //int vidx = VoxelIndexFromCoordinates(voxelVector.x, voxelVector.y, voxelVector.z);
            //return vidx;
        }

        // deprecated
        public Position VoxelVector(Position pos)
        {
            int cubeSize = this.cubeSize;
            int mask = (1 << this.chunkBits) - 1;
            int vx = (int)(Math.Floor(pos.x / cubeSize)) & mask;
            int vy = (int)(Math.Floor(pos.y / cubeSize)) & mask;
            int vz = (int)(Math.Floor(pos.z / cubeSize)) & mask;
            return new Position(vx, vy, vz);
        }
    }
}
