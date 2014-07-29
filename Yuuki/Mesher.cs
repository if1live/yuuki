using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yuuki
{
    public struct QuadVertex
    {
        public int x;
        public int y;
        public int z;
        public QuadVertex(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
    public class Quad
    {
        public QuadVertex v1;
        public QuadVertex v2;
        public QuadVertex v3;
        public QuadVertex v4;

        public Quad(QuadVertex v1, QuadVertex v2, QuadVertex v3, QuadVertex v4)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.v4 = v4;
        }
    }

    public interface IMesher
    {
        List<Quad> Build(byte[] volume, ChunkDimension dims);
    }

    /*
     * http://mikolalysenko.github.io/MinecraftMeshes/js/stupid.js
     */
    public class StupidMesher : IMesher
    {
        public List<Quad> Build(byte[] volume, ChunkDimension dims)
        {
            List<Quad> quads = new List<Quad>();
            int[] x = new int[3]{0,0,0};
            var n = 0;
            for(x[2]=0; x[2]<dims.z; ++x[2])
            {
                for(x[1]=0; x[1]<dims.y; ++x[1])
                {
                    for(x[0]=0; x[0]<dims.x; ++x[0])
                    {
                        if(volume[n++] > 0) {
                            for(var d=0; d<3; ++d) {
                                int[] t = new int[3]{x[0], x[1], x[2]};
                                int[] u = new int[3]{0,0,0};
                                int[] v = new int[3]{0,0,0};
                                u[(d+1)%3] = 1;
                                v[(d+2)%3] = 1;
                                for(var s=0; s<2; ++s) {
                                    t[d] = x[d] + s;

                                    Quad quad = new Quad(
                                        new QuadVertex(t[0],           t[1],           t[2]          ),
                                        new QuadVertex(t[0]+u[0],      t[1]+u[1],      t[2]+u[2]     ),
                                        new QuadVertex(t[0]+u[0]+v[0], t[1]+u[1]+v[1], t[2]+u[2]+v[2]),
                                        new QuadVertex(t[0]     +v[0], t[1]     +v[1], t[2]     +v[2])
                                    );
                                    quads.Add(quad);
                                }
                            }
                        }
                    }
                }
            }
            return quads;
        }
    }
}


