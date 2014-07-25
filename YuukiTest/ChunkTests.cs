using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Yuuki;

/**
 * voxel.js 유닛테스트를 포팅
 * https://github.com/maxogden/voxel/blob/master/test.js
 */

namespace YuukiTest
{
    [TestFixture]
    [Category("Chunker")]
    internal class ChunkerTests
    {
        [Test]
        public void TestCreate()
        {
            ChunkerParameter param = new ChunkerParameter();
            param.distance = 2;
            param.chunkSize = 32;
            param.cubeSize = 1;
            Chunker chunker = new Chunker(param);
            Assert.IsNotNull(chunker);
        }

        [Test]
        public void TestChunkAtCoordinates()
        {
            ChunkerParameter param = new ChunkerParameter();
            param.cubeSize = 1;
            param.distance = 2;
            param.chunkSize = 32;
            Chunker chunker = new Chunker(param);


            Assert.AreEqual(chunker.ChunkAtCoordinates(0, 0, 0), new ChunkPosition(0, 0, 0));
            Assert.AreEqual(chunker.ChunkAtCoordinates(31, 0, 0), new ChunkPosition(0, 0, 0));
            Assert.AreEqual(chunker.ChunkAtCoordinates(32, 0, 0), new ChunkPosition(1, 0, 0));
            Assert.AreEqual(chunker.ChunkAtCoordinates(63, 0, 0), new ChunkPosition(1, 0, 0));
            Assert.AreEqual(chunker.ChunkAtCoordinates(-1, 0, 0), new ChunkPosition(-1, 0, 0));
            Assert.AreEqual(chunker.ChunkAtCoordinates(-32, 0, 0), new ChunkPosition(-1, 0, 0));
            Assert.AreEqual(chunker.ChunkAtCoordinates(-33, 0, 0), new ChunkPosition(-2, 0, 0));
            Assert.AreEqual(chunker.ChunkAtCoordinates(-64, 0, 0), new ChunkPosition(-2, 0, 0));
        }

        [Test]
        public void TestChunkAtPosition()
        {
            ChunkerParameter param = new ChunkerParameter();
            param.cubeSize = 1;
            param.distance = 2;
            param.chunkSize = 32;
            Chunker chunker = new Chunker(param);

            Assert.AreEqual(chunker.ChunkAtPosition(new Position(0, 0, 0)), new ChunkPosition(0, 0, 0));
            Assert.AreEqual(chunker.ChunkAtPosition(new Position(0.9999f, 0, 0)), new ChunkPosition(0, 0, 0));
            Assert.AreEqual(chunker.ChunkAtPosition(new Position(31.9999f, 0, 0)), new ChunkPosition(0, 0, 0));
            Assert.AreEqual(chunker.ChunkAtPosition(new Position(32, 0, 0)), new ChunkPosition(1, 0, 0));
            Assert.AreEqual(chunker.ChunkAtPosition(new Position(-0.0001f, 0, 0)), new ChunkPosition(-1, 0, 0));
            Assert.AreEqual(chunker.ChunkAtPosition(new Position(-32, 0, 0)), new ChunkPosition(-1, 0, 0));
            Assert.AreEqual(chunker.ChunkAtPosition(new Position(-32.0001f, 0, 0)), new ChunkPosition(-2, 0, 0));

        }

        [Test]
        public void TestVoxelIndexFromCoordinates()
        {
            ChunkerParameter param = new ChunkerParameter();
            param.distance = 2;
            param.chunkSize = 32;
            param.cubeSize = 1;
            Chunker chunker = new Chunker(param);

            Assert.AreEqual(chunker.VoxelIndexFromCoordinates(0, 0, 0), 0);
            Assert.AreEqual(chunker.VoxelIndexFromCoordinates(1, 0, 0), 1);
            Assert.AreEqual(chunker.VoxelIndexFromCoordinates(31, 0, 0), 31);
            Assert.AreEqual(chunker.VoxelIndexFromCoordinates(32, 0, 0), 0);
            Assert.AreEqual(chunker.VoxelIndexFromCoordinates(0, 1, 0), 32);
            Assert.AreEqual(chunker.VoxelIndexFromCoordinates(0, 31, 0), 32 * 31);
            Assert.AreEqual(chunker.VoxelIndexFromCoordinates(0, 32, 0), 0);
            Assert.AreEqual(chunker.VoxelIndexFromCoordinates(0, 0, 1), 32 * 32);
            Assert.AreEqual(chunker.VoxelIndexFromCoordinates(0, 0, 31), 32 * 32 * 31);
            Assert.AreEqual(chunker.VoxelIndexFromCoordinates(0, 0, 32), 0);
            Assert.AreEqual(chunker.VoxelIndexFromCoordinates(-1, 0, 0), 31);
            Assert.AreEqual(chunker.VoxelIndexFromCoordinates(-31, 0, 0), 1);
            Assert.AreEqual(chunker.VoxelIndexFromCoordinates(-32, 0, 0), 0);
        }

        /*
        [Test]
        public void TestVoxelIndexFromPosition()
        {
            ChunkerParameter param = new ChunkerParameter();
            param.distance = 2;
            param.chunkSize = 32;
            param.cubeSize = 1;
            Chunker chunker = new Chunker(param);

            Assert.AreEqual(chunker.VoxelIndexFromPosition(new Position(0, 0, 0)), 0);
            Assert.AreEqual(chunker.VoxelIndexFromPosition(new Position(0.9999f, 0, 0)), 0);
            Assert.AreEqual(chunker.VoxelIndexFromPosition(new Position(1.9999f, 0, 0)), 1);
            Assert.AreEqual(chunker.VoxelIndexFromPosition(new Position(-0.0001f, 0, 0)), 31);
            Assert.AreEqual(chunker.VoxelIndexFromPosition(new Position(-0.9999f, 0, 0)), 31);
            Assert.AreEqual(chunker.VoxelIndexFromPosition(new Position(-1, 0, 0)), 31);
        }
         */

        [Test]
        public void TestGetBounds()
        {
            ChunkerParameter param = new ChunkerParameter();
            param.distance = 2;
            param.chunkSize = 32;
            param.cubeSize = 1;
            Chunker chunker = new Chunker(param);

            ChunkerBound bound = chunker.GetBounds(0, 0, 0);
            Assert.AreEqual(bound.low, new Position(0, 0, 0));
            Assert.AreEqual(bound.high, new Position(32, 32, 32));

            bound = chunker.GetBounds(1, 0, 0);
            Assert.AreEqual(bound.low, new Position(32, 0, 0));
            Assert.AreEqual(bound.high, new Position(64, 32, 32));

            bound = chunker.GetBounds(-1, 0, 0);
            Assert.AreEqual(bound.low, new Position(-32, 0, 0));
            Assert.AreEqual(bound.high, new Position(0, 32, 32));
        }

        [Test]
        public void TestNearbyChunks()
        {
            ChunkerParameter param = new ChunkerParameter();
            param.distance = 2;
            param.chunkSize = 32;
            param.cubeSize = 1;
            Chunker chunker = new Chunker(param);

            var actual = chunker.NearbyChunks(new Position(0, 0, 0), 1);

            List<ChunkPosition> expected = new List<ChunkPosition>() 
            {
                new ChunkPosition(-1, -1, -1),
                new ChunkPosition(-1, -1, 0),
                new ChunkPosition(-1, 0, -1), 
                new ChunkPosition(-1, 0, 0), 
                new ChunkPosition(0, -1, -1), 
                new ChunkPosition(0, -1, 0), 
                new ChunkPosition(0, 0, -1), 
                new ChunkPosition(0, 0, 0)
            };
            Assert.AreEqual(actual, expected);
        }

        /*
        [Test]
        public void TestGenerateChunk()
        {
            ChunkerParameter param = new ChunkerParameter();
            param.distance = 2;
            param.chunkSize = 32;
            param.cubeSize = 1;
            Chunker chunker = new Chunker(param);

            chunker.GenerateChunk(0, 0, 0);
            Assert.AreEqual(!!chunker.chunks['0|0|0'], true);
            chunker.GenerateChunk(1, 0, 0);
            Assert.AreEqual(!!chunker.chunks['1|0|0'], true);
            chunker.GenerateChunk(-1, 0, 0);
            Assert.AreEqual(!!chunker.chunks['-1|0|0'], true);
        }
         * */

        /*
        [Test]
        public void TestRequestMissingChunks()
        {
            // note: chunkDistance
            ChunkerParameter param = new ChunkerParameter();
            param.distance = 1;
            param.chunkSize = 32;
            param.cubeSize = 1;
            Chunker chunker = new Chunker(param);

            chunker.GenerateChunk(0, 0, 0);
            chunker.GenerateChunk(1, 0, 0);
            chunker.GenerateChunk(-1, 0, 0);
            var missing = [];
            chunker.on('missingChunk', function (pos) {
                missing.push(pos)
            })
            chunker.RequestMissingChunks([0, 0, 0]);
            t.deepEqual(missing, [[-1, -1, -1], [-1, -1, 0], [-1, 0, -1], [0, -1, -1], [0, -1, 0], [0, 0, -1]])
        }
         */

        /*
        [Test]
        public void TestVoxelAtCoordinates()
        {
            ChunkerParameter param = new ChunkerParameter();
            param.distance = 2;
            param.chunkSize = 32;
            param.cubeSize = 1;
            Chunker chunker = new Chunker(param);

            //chunker.generateChunk(0, 0, 0)
            Assert.AreEqual(chunker.VoxelAtCoordinates(0, 16, 0), 0);
            Assert.AreEqual(chunker.VoxelAtCoordinates(0, 16, 0, 1), 0);
            Assert.AreEqual(chunker.VoxelAtCoordinates(0, 16, 0), 1);
            Assert.AreEqual(chunker.VoxelAtCoordinates(0, 16, 0, 0), 1);
            Assert.AreEqual(chunker.VoxelAtCoordinates(0, 16, 0), 0);
            Assert.AreEqual(chunker.VoxelAtCoordinates(-1, 0, 0), false);
            Assert.AreEqual(chunker.VoxelAtCoordinates(-1, 0, 0, 1), false);
            Assert.AreEqual(chunker.VoxelAtCoordinates(-1, 0, 0), false);
        }
         */

        /*
        [Test]
        public void TestVoxelAtPosition()
        {
            ChunkerParameter param = new ChunkerParameter();
            param.distance = 2;
            param.chunkSize = 32;
            param.cubeSize = 1;
            Chunker chunker = new Chunker(param);

            //chunker.generateChunk(0, 0, 0)
            Assert.AreEqual(chunker.VoxelAtPosition(new Position(0, 16, 0), 1), 0);
            Assert.AreEqual(chunker.VoxelAtPosition(new Position(0, 16.9999f, 0)), 1);
        }
         */
    }

}
