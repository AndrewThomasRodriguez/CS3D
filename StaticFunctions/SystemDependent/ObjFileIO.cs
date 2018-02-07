using CS3D.CustomDataTypes;
using CS3D.DataTypes.FundamentalGraphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CS3D.StaticFunctions.SystemDependent
{
    static class ObjFileIO
    {
        public static BaseTriangle[] LoadObj(string filename)
        {
            //buffers used to make triangles
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<VerticiesUVIndex> indexes = new List<VerticiesUVIndex>();

            //loading file line by line and parsing
            using (StreamReader sr = new StreamReader(filename))
            {
                string line = String.Empty; //line buffer

                while ((line = sr.ReadLine()) != null) //read until end of file
                {
                    //loading line
                    var values = Regex.Split(line, " ");
                    var type = values[0];

                    //Parser line type
                    switch (type)
                    {
                        case "vt": //uv
                            uvs.Add(CreateUV(values[1], values[2]));
                            break;

                        case "v": //Vertex
                            vertices.Add(CreateVertex(values[1], values[2], values[3]));
                            break;

                        case "f": //face
                            indexes.Add(CreateIndexes(values[1], values[2], values[3]));
                            break;
                    }
                }
            }

            //make triangle list
            BaseTriangle[] returnList = new BaseTriangle[indexes.Count];

            //for (int i = 0; i < indexes.Count; i++)
            Parallel.For(0, indexes.Count, i =>
            {
                returnList[i] = new BaseTriangle(
                    new TriangleData
                    {
                        Vertex1 = vertices[indexes[i].vertexIndex1 - 1],
                        Vertex2 = vertices[indexes[i].vertexIndex2 - 1],
                        Vertex3 = vertices[indexes[i].vertexIndex3 - 1],
                        Uv1 = uvs[indexes[i].uvIndex1 - 1],
                        Uv2 = uvs[indexes[i].uvIndex2 - 1],
                        Uv3 = uvs[indexes[i].uvIndex3 - 1],
                    });
            });

            return returnList;
        }

        private struct VerticiesUVIndex
        {
            public int vertexIndex1, uvIndex1;
            public int vertexIndex2, uvIndex2;
            public int vertexIndex3, uvIndex3;
        }

        private static VerticiesUVIndex CreateIndexes(string index1, string index2, string index3)
        {
            string[] values1 = index1.Split('/');
            string[] values2 = index2.Split('/');
            string[] values3 = index3.Split('/');

            return new VerticiesUVIndex
            {
                vertexIndex1 = int.Parse(values1[0]),
                uvIndex1 = int.Parse(values1[1]),

                vertexIndex2 = int.Parse(values2[0]),
                uvIndex2 = int.Parse(values2[1]),

                vertexIndex3 = int.Parse(values3[0]),
                uvIndex3 = int.Parse(values3[1])
            };
        }

        private static Vector2 CreateUV(string u, string v)
        {
            return new Vector2() { u = float.Parse(u), v = float.Parse(v) };
        }

        private static Vector3 CreateVertex(string x, string y, string z)
        {
            return new Vector3() { x = float.Parse(x), y = float.Parse(y), z = float.Parse(z) };
        }
    }
}
