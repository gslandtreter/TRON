using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace TRON 
{
	/**
	 * <summary>
	 * A class containing all the necessary data for a mesh: Points, normal vectors, UV coordinates,
	 * and indices into each.
	 * Regardless of how the mesh file represents geometry, this is what we load it into,
	 * because this is most similar to how OpenGL represents geometry.
	 * We store data as arrays of vertices, UV coordinates and normals, and then a list of Triangle
	 * structures.  A Triangle is a struct which contains integer offsets into the vertex/normal/texcoord
	 * arrays to define a face.
	 * </summary>
	 */
	
	public class Mesh
    {
		public Vector3[] Vertices;
		public Vector2[] TexCoords;
		public Vector3[] Normals;
		public Tri[] Tris;

        uint dataBuffer;
        uint indexBuffer;
        int vertOffset, normOffset, texcoordOffset;

		/// <summary>
		///Creates a new Mesh object 
		/// </summary>
		/// <param name="vert">
		/// A <see cref="Vector3[]"/>
		/// </param>
		/// <param name="norm">
		/// A <see cref="Vector3[]"/>
		/// </param>
		/// <param name="tex">
		/// A <see cref="Vector2[]"/>
		/// </param>
		/// <param name="tri">
		/// A <see cref="Tri[]"/>
		/// </param>
		public Mesh(Vector3[] vert, Vector3[] norm, Vector2[] tex, Tri[] tri)
        {
			Vertices = vert;
			TexCoords = tex;
			Normals = norm;
			Tris = tri;
			
			Verify();
		}
		/// <summary>
		/// Returns an array containing the coordinates of all the <value>Vertices</value>.
	    /// So {<1,1,1>, <2,2,2>} will turn into {1,1,1,2,2,2}
		/// </summary>
		/// <returns>
		/// A <see cref="System.Double[]"/>
		/// </returns>
		private double[] VertexArray() 
        {
			double[] verts = new double[Vertices.Length*3];
			for(int i = 0; i < Vertices.Length; i++) 
            {
				verts[i*3] = Vertices[i].X;
				verts[i*3+1] = Vertices[i].Y;
				verts[i*3+2] = Vertices[i].Z;
			}
			
			return verts;
		}
		
		/// <summary>
		/// Returns an array containing the coordinates of the <value>Normals<,value>, similar to VertexArray.
		/// </summary>
		/// <returns>
		/// A <see cref="System.Double[]"/>
		/// </returns>
		private double[] NormalArray() 
        {
			double[] norms = new double[Normals.Length * 3];
			for(int i = 0; i < Normals.Length; i++) {
				norms[i * 3] = Normals[i].X;
				norms[i * 3 + 1] = Normals[i].Y;
				norms[i * 3 + 2] = Normals[i].Z;
			}
			
			return norms;
		}
		
		/// <summary>
		/// Returns an array containing the coordinates of the <value>TexCoords<value>, similar to VertexArray. 
		/// </summary>
		/// <returns>
		/// A <see cref="System.Double[]"/>
		/// </returns>
		private double[] TexcoordArray() 
        {
			double[] tcs = new double[TexCoords.Length*2];
			for(int i = 0; i < TexCoords.Length; i++) {
				tcs[i*3] = TexCoords[i].X;
				tcs[i*3+1] = TexCoords[i].Y;
			}
			
			return tcs;
		}

		/// <summary>
		///  Turns the Triangles into an array of Points.
		/// </summary>
		/// <returns>
		/// A <see cref="Point[]"/>
		/// </returns>
		protected Point[] Points() 
        {
			List<Point> points = new List<Point>();
			foreach(Tri t in Tris) {
				points.Add(t.P1);
				points.Add(t.P2);
				points.Add(t.P3);
			}
			return points.ToArray();
		}

		private void OpenGLArrays(out float[] verts, out float[] norms, out float[] texcoords, out uint[] indices) 
        {
			Point[] points = Points();
			verts = new float[points.Length * 3];
			norms = new float[points.Length * 3];
			texcoords = new float[points.Length * 2];
			indices = new uint[points.Length];
			
			for(uint i = 0; i < points.Length; i++) {
				Point p = points[i];
				verts[i*3] = (float) Vertices[p.Vertex].X;
				verts[i*3+1] = (float) Vertices[p.Vertex].Y;
				verts[i*3+2] = (float) Vertices[p.Vertex].Z;
				
				norms[i*3] = (float) Normals[p.Normal].X;
				norms[i*3+1] = (float) Normals[p.Normal].Y;
				norms[i*3+2] = (float) Normals[p.Normal].Z;
				
				texcoords[i*2] = (float) TexCoords[p.TexCoord].X;
				texcoords[i*2+1] = (float) TexCoords[p.TexCoord].Y;
				
				indices[i] = i;
			}
		}
		
		
		public override string ToString() 
        {
			StringBuilder s = new StringBuilder();
			s.AppendLine("Vertices:");
			foreach(Vector3 v in Vertices) 
            {
				s.AppendLine(v.ToString());
			}
			
			s.AppendLine("Normals:");
			foreach(Vector3 n in Normals) 
            {
				s.AppendLine(n.ToString());
			}
			s.AppendLine("TexCoords:");
			foreach(Vector2 t in TexCoords) 
            {
				s.AppendLine(t.ToString());
			}
			s.AppendLine("Tris:");
			foreach(Tri t in Tris) 
            {
				s.AppendLine(t.ToString());
			}
			return s.ToString();
		}
		
		
		/// <summary>
		/// Does some simple sanity checking to make sure that the offsets of the Triangles
	    /// actually refer to real points.  Throws an 
	    /// <exception cref="IndexOutOfRangeException">IndexOutOfRangeException</exception> if not.
		/// </summary>
		/// 
		private void Verify()
        {
			foreach(Tri t in Tris) 
            {
				foreach(Point p in t.Points()) 
                {
					if(p.Vertex >= Vertices.Length) 
                    {
						string message = String.Format("Vertex {0} >= length of vertices {1}", p.Vertex, Vertices.Length);
						throw new IndexOutOfRangeException(message);
					}
					if(p.Normal >= Normals.Length) 
                    {
						string message = String.Format("Normal {0} >= number of normals {1}", p.Normal, Normals.Length);
						throw new IndexOutOfRangeException(message);
					}
					if(p.TexCoord > TexCoords.Length) 
                    {
						string message = String.Format("TexCoord {0} > number of texcoords {1}", p.TexCoord, TexCoords.Length);
						throw new IndexOutOfRangeException(message);
					}
				}
			}
		}

        public void LoadBuffers()
        {
            float[] verts, norms, texcoords;
            uint[] indices;

            OpenGLArrays(out verts, out norms, out texcoords, out indices);
            GL.GenBuffers(1, out dataBuffer);
            GL.GenBuffers(1, out indexBuffer);

            // Set up data for VBO.
            // We're going to use one VBO for all geometry, and stick it in 
            // in (VVVVNNNNCCCC) order.  Non interleaved.
            int buffersize = (verts.Length + norms.Length + texcoords.Length);
            float[] bufferdata = new float[buffersize];
            vertOffset = 0;
            normOffset = verts.Length;
            texcoordOffset = (verts.Length + norms.Length);

            verts.CopyTo(bufferdata, vertOffset);
            norms.CopyTo(bufferdata, normOffset);
            texcoords.CopyTo(bufferdata, texcoordOffset);

            bool v = false;
            for (int i = texcoordOffset; i < bufferdata.Length; i++)
            {
                if (v)
                {
                    bufferdata[i] = 1 - bufferdata[i];
                    v = false;
                }
                else
                {
                    v = true;
                }
            }

            // Load geometry data
            GL.BindBuffer(BufferTarget.ArrayBuffer, dataBuffer);
            GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr)(buffersize * sizeof(float)), bufferdata,
                          BufferUsageHint.StaticDraw);

            // Load index data
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
            GL.BufferData<uint>(BufferTarget.ElementArrayBuffer,
                          (IntPtr)(indices.Length * sizeof(uint)), indices, BufferUsageHint.StaticDraw);
        }

        public uint LoadTexture(string file)
        {
            return Texture.LoadTex(file);
        }

        

        public void Render(uint textureID)
        {
            // Push current Array Buffer state so we can restore it later
            GL.PushClientAttrib(ClientAttribMask.ClientVertexArrayBit);

            GL.ClientActiveTexture(TextureUnit.Texture0);

            //Checa se a textura foi carregada
            if (textureID != 0)
                GL.BindTexture(TextureTarget.Texture2D, textureID);

            GL.BindBuffer(BufferTarget.ArrayBuffer, dataBuffer);
            // Normal buffer
            GL.NormalPointer(NormalPointerType.Float, 0, (IntPtr)(normOffset * sizeof(float)));

            // TexCoord buffer
            GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, (IntPtr)(texcoordOffset * sizeof(float)));

            // Vertex buffer
            GL.VertexPointer(3, VertexPointerType.Float, 0, (IntPtr)(vertOffset * sizeof(float)));

            // Index array
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
            GL.DrawElements(BeginMode.Triangles, Tris.Length * 3, DrawElementsType.UnsignedInt, IntPtr.Zero);

            // Restore the state
            GL.PopClientAttrib();
        }
	}
	
	public struct Vector2 
    {
		public double X;
		public double Y;
		public Vector2(double x, double y) 
        {
			X = x;
			Y = y;
		}
		
		public override string ToString() {return String.Format("<{0},{1}>", X, Y);}
	}
	
	public struct Vector3 
    {
		public double X;
		public double Y;
		public double Z;
		public Vector3(double x, double y, double z) {
			X = x;
			Y = y;
			Z = z;
		}
		
		public override string ToString() {return String.Format("<{0},{1},{2}>", X, Y, Z);}
	}
	
	public struct Point 
    {
		public int Vertex;
		public int Normal;
		public int TexCoord;
		
		public Point(int v, int n, int t) 
        {
			Vertex = v;
			Normal = n;
			TexCoord = t;
		}
		
		public override string ToString() {return String.Format("Point: {0},{1},{2}", Vertex, Normal, TexCoord);}
	}
	
	public class Tri 
    {
		public Point P1, P2, P3;
		public Tri() 
        {
			P1 = new Point();
			P2 = new Point();
			P3 = new Point();
		}
		public Tri(Point a, Point b, Point c) 
        {
			P1 = a;
			P2 = b;
			P3 = c;
		}
		
		public Point[] Points() 
        {
			return new Point[3]{P1, P2, P3};
		}
		
		public override string ToString() {return String.Format("Tri: {0}, {1}, {2}", P1, P2, P3);}
	}
}
