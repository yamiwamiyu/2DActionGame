using System;
using System.Collections.Generic;
using System.Linq;
using EntryEngine;
using EntryEngine.Serialize;

[AReflexible]public partial class T地图
{
    public static bool __Load = true;
    /// <summary>同一ID的从属于一张地图</summary>
    public ushort ID;
    /// <summary>地形ID</summary>
    public ushort TID;
    /// <summary>地形放置的横坐标</summary>
    public int X;
    /// <summary>地形放置的纵坐标</summary>
    public int Y;
    /// <summary>锁屏模式，-1代表不能回头，1代表不能继续向前（例如打完某个小BOSS才能继续向前）</summary>
    public sbyte Lock;
    static T地图()
    {
        T地图 T地图 = new T地图();
        T地图.ID = default(ushort);
        T地图.TID = default(ushort);
        T地图.X = default(int);
        T地图.Y = default(int);
        T地图.Lock = default(sbyte);
    }
    
}
[AReflexible]public partial class T地形
{
    public static bool __Load = true;
    public ushort ID;
    /// <summary>图片路径</summary>
    public string Texture;
    /// <summary>碰撞范围矩形，不填则不碰撞</summary>
    public string Hit;
    /// <summary>碰撞范围矩形，不填则不碰撞</summary>
    [NonSerialized]
    public RECT[] _Hit;
    /// <summary>0.不翻转 / 1.水平翻转 / 2.垂直翻转 / 3.水平垂直翻转</summary>
    public EFlip Flip;
    static T地形()
    {
        T地形 T地形 = new T地形();
        T地形.ID = default(ushort);
        T地形.Texture = default(string);
        T地形.Hit = null;
        T地形.Flip = default(EFlip);
    }
    
}
public static partial class _TABLE
{
    public static string _path { get; private set; }
    public static T地图[] _T地图;
    public static Dictionary<ushort, T地图[]> _T地图ByID;
    public static T地形[] _T地形;
    public static Dictionary<ushort, T地形> _T地形ByID;
    
    public static event Action<T地图[]> OnLoadT地图;
    public static event Action<T地形[]> OnLoadT地形;
    
    public static void SetPath(string path)
    {
        if (_path != path)
        {
            _path = _IO.DirectoryWithEnding(path);
            _LOG.Info("Set table path: {0}", _path);
        }
    }
    public static void Load(string path)
    {
        SetPath(path);
        _LOG.Info("Load all tables", path);
        LoadT地图();
        LoadT地形();
    }
    public static IEnumerable<AsyncReadFile> LoadAsync(string path)
    {
        SetPath(path);
        _LOG.Info("LoadAsync all tables", path);
        foreach (var item in LoadT地图Async()) yield return item;
        foreach (var item in LoadT地形Async()) yield return item;
        yield break;
    }
    public static void Reload()
    {
        _LOG.Info("Reload all tables");
        Load(_path);
    }
    public static IEnumerable<AsyncReadFile> ReloadAsync()
    {
        _LOG.Info("ReloadAsync all tables");
        foreach (var item in LoadAsync(_path)) yield return item;
    }
    private static void __ParseT地图(T地图[] array)
    {
        Dictionary<ushort, List<T地图>> __group = new Dictionary<ushort, List<T地图>>();
        List<T地图> __list;
        foreach (var item in array)
        {
            if (!__group.TryGetValue(item.ID, out __list))
            {
                __list = new List<T地图>();
                __group.Add(item.ID, __list);
            }
            __list.Add(item);
        }
        _T地图ByID = new Dictionary<ushort, T地图[]>();
        foreach (var item in __group) _T地图ByID.Add(item.Key, item.Value.ToArray());
    }
    public static void LoadT地图()
    {
        if (!T地图.__Load) return;
        _LOG.Debug("loading table T地图");
        CSVReader _reader = new CSVReader(_IO.ReadText(_path + "T地图.csv"));
        T地图[] temp = _reader.ReadObject<T地图[]>();
        __ParseT地图(temp);
        if (OnLoadT地图 != null) OnLoadT地图(temp);
        _T地图 = temp;
    }
    public static IEnumerable<AsyncReadFile> LoadT地图Async()
    {
        if (!T地图.__Load) yield break;
        _LOG.Debug("loading async table T地图");
        var async = _IO.ReadAsync(_path + "T地图.csv");
        if (!async.IsEnd) yield return async;
        CSVReader _reader = new CSVReader(_IO.ReadPreambleText(async.Data));
        T地图[] temp = _reader.ReadObject<T地图[]>();
        __ParseT地图(temp);
        if (OnLoadT地图 != null) OnLoadT地图(temp);
        _T地图 = temp;
    }
    private static void __ParseT地形(T地形[] array)
    {
        _T地形ByID = array.ToDictionary(__a => __a.ID);
        int length = array.Length;
        for (int i = 0; i < length; i++)
        {
            var value = array[i];
            if (!string.IsNullOrEmpty(value.Hit))
            {
                string[] _array1 = value.Hit.Split('|');
                int length1 = _array1.Length;
                var result = new RECT[length1];
                for (int j = 0; j < length1; j++)
                {
                    string[] _array = _array1[j].Split(';');
                    result[j].X = float.Parse(_array[0]);
                    result[j].Y = float.Parse(_array[1]);
                    result[j].Width = float.Parse(_array[2]);
                    result[j].Height = float.Parse(_array[3]);
                }
                value._Hit = result;
            }
        }
    }
    public static void LoadT地形()
    {
        if (!T地形.__Load) return;
        _LOG.Debug("loading table T地形");
        CSVReader _reader = new CSVReader(_IO.ReadText(_path + "T地形.csv"));
        T地形[] temp = _reader.ReadObject<T地形[]>();
        __ParseT地形(temp);
        if (OnLoadT地形 != null) OnLoadT地形(temp);
        _T地形 = temp;
    }
    public static IEnumerable<AsyncReadFile> LoadT地形Async()
    {
        if (!T地形.__Load) yield break;
        _LOG.Debug("loading async table T地形");
        var async = _IO.ReadAsync(_path + "T地形.csv");
        if (!async.IsEnd) yield return async;
        CSVReader _reader = new CSVReader(_IO.ReadPreambleText(async.Data));
        T地形[] temp = _reader.ReadObject<T地形[]>();
        __ParseT地形(temp);
        if (OnLoadT地形 != null) OnLoadT地形(temp);
        _T地形 = temp;
    }
}
