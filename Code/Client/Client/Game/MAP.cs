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
        private const string DIR = "Maps/";
        public const string FILE = ".map";
        private static readonly IEncrypt encrypt = new EncryptRandom();

        //public int Width
        //{
        //    get
        //    {
        //        if (Terrains == null) return 0;
        //        int max = 0;
        //        for (int i = 0; i < Terrains.Length; i++)
        //            foreach (var item in Terrains[i].Data._Hit)
        //            {
        //                int right = Terrains[i].X + (int)item.Right;
        //                if (right > max)
        //                    max = right;
        //            }
        //        return max;
        //    }
        //}
        //public int Height
        //{
        //    get
        //    {
        //        if (Terrains == null) return 0;
        //        int max = 0;
        //        for (int i = 0; i < Terrains.Length; i++)
        //            foreach (var item in Terrains[i].Data._Hit)
        //            {
        //                int top = Terrains[i].Y + (int)item.Y;
        //                if (top > max)
        //                    max = top;
        //            }
        //        return max;
        //    }
        //}
        public TERRAIN[] Terrains;

        public void LoadTextureSync(ContentManager content)
        {
            foreach (var item in Terrains)
                if (item.Texture == null)
                    item.Texture = content.Load<TEXTURE>(item.Data.Texture);
        }
        public IEnumerable<ICoroutine> LoadTexture(ContentManager content)
        {
            foreach (var item in Terrains)
            {
                TERRAIN terrain = item;
                if (terrain.Texture == null)
                {
                    ICoroutine load = content.LoadAsync<TEXTURE>(item.Data.Texture, t => terrain.Texture = t);
                    if (!load.IsEnd)
                        yield return load;
                }
            }
        }

        public static void Save(string name, TERRAIN[] terrains)
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
            map.Terrains = JsonReader.Deserialize<TERRAIN[]>(__temp);
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

        public void SetData()
        {
            Data = _TABLE._T地形ByID[ID];
        }
        public void SetHits()
        {
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
            __GRAPHICS.Draw(Texture, new VECTOR2(X, Y));
        }
    }
    public enum ELockMode
    {
        None,
        LockForward,
        LockBack,
    }
}
