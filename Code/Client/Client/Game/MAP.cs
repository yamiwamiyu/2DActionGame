using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntryEngine;
using EntryEngine.Serialize;
using System.IO;

namespace Client.Game
{
    public class MAP
    {
        public const string DIR = "Maps/";
        public const string FILE = ".map";
        public const int GRID = 50;
        private static readonly IEncrypt encrypt = new EncryptRandom();

        public List<TERRAIN> Terrains;

        public RECT BoundingBox
        {
            get
            {
                if (Terrains == null) return RECT.Empty;
                RECT rect = new RECT(float.MaxValue, float.MaxValue, float.MinValue, float.MinValue);
                foreach (var item in Terrains)
                {
                    //if (item.Data._Hit == null) continue;
                    if (item.Texture == null) continue;
                    float temp = item.X;
                    if (temp < rect.X) rect.X = temp;
                    temp += item.Texture.Width;
                    if (temp > rect.Width) rect.Width = temp;
                    temp = item.Y;
                    if (temp > rect.Height) rect.Height = temp;
                    temp -= item.Texture.Height;
                    if (temp < rect.Y) rect.Y = temp;
                    //foreach (var area in item.Data._Hit)
                    //{
                    //    float temp = item.X + area.X;
                    //    if (temp < rect.X)
                    //        rect.X = temp;
                    //    temp += area.Width;
                    //    if (temp > rect.Width)
                    //        rect.Width = temp;
                    //    temp = item.Y + area.Y;
                    //    if (temp < rect.Y)
                    //        rect.Y = temp;
                    //    temp += area.Height;
                    //    if (temp > rect.Height)
                    //        rect.Height = temp;
                    //}
                }
                rect.Width -= rect.X;
                rect.Height -= rect.Y;
                return rect;
            }
        }

        public void LoadTextureSync(ContentManager content)
        {
            foreach (var item in Terrains)
                if (item.Texture == null && !string.IsNullOrEmpty(item.Data.Texture))
                    item.Texture = content.Load<TEXTURE>(item.Data.Texture);
        }
        public IEnumerable<ICoroutine> LoadTexture(ContentManager content)
        {
            foreach (var item in Terrains)
            {
                TERRAIN terrain = item;
                if (terrain.Texture == null && !string.IsNullOrEmpty(item.Data.Texture))
                {
                    ICoroutine load = content.LoadAsync<TEXTURE>(item.Data.Texture, t => terrain.Texture = t);
                    if (!load.IsEnd)
                        yield return load;
                }
            }
        }

        public static void Save(string name, List<TERRAIN> terrains)
        {
            string __temp = JsonWriter.Serialize(terrains);
            byte[] bytes = Encoding.UTF8.GetBytes(__temp);
            encrypt.Encrypt(ref bytes);
            _IO.WriteByte(DIR + Path.ChangeExtension(name, FILE), bytes);
        }
        public static MAP Load(string name)
        {
            return Load(_IO.ReadByte(DIR + name + FILE));
        }
        public static MAP Load(byte[] data)
        {
            encrypt.Decrypt(ref data);
            string __temp = Encoding.UTF8.GetString(data);
            MAP map = new MAP();
            map.Terrains = JsonReader.Deserialize<List<TERRAIN>>(__temp);
            foreach (var item in map.Terrains)
            {
                item.SetData();
                item.SetHits();
            }
            return map;
        }
    }
    public class TERRAIN
    {
        public ushort ID;
        public int X;
        public int Y;
        public ELockMode Lock;

        public T地形 Data { get; set; }
        public RECT[] Hits { get; set; }
        public TEXTURE Texture { get; set; }
        public RECT Area
        {
            get
            {
                RECT area;
                area.X = X;
                if (Texture == null)
                {
                    area.Y = Y - Data.BoundingBox.Height;
                    area.Width = Data.BoundingBox.Width;
                    area.Height = Data.BoundingBox.Height;
                }
                else
                {
                    area.Y = Y - Texture.Height;
                    area.Width = Texture.Width;
                    area.Height = Texture.Height;
                }
                return area;
            }
        }

        public void SetData()
        {
            Data = _TABLE._T地形ByID[ID];
        }
        public void SetHits()
        {
            if (Data._Hit == null)
            {
                Hits = _SARRAY<RECT>.Empty;
                return;
            }
            Hits = new RECT[Data._Hit.Length];
            for (int i = 0; i < Data._Hit.Length; i++)
            {
                Hits[i] = Data._Hit[i];
                Hits[i].X += X;
                Hits[i].Y += Y;
            }
        }
        public void Draw()
        {
            if (Texture == null) return;
            __GRAPHICS.Draw(Texture, new VECTOR2(X, Y), 0, 0, 1, 1, 1, Data.Flip);
        }
        public void DrawHitArea()
        {
            Data.DrawHitArea(X, Y - Data.BoundingBox.Height, 1);
        }
    }
    public enum ELockMode
    {
        None,
        LockForward,
        LockBack,
    }
}
