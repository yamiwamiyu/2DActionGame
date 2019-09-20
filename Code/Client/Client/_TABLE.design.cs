using System;
using System.Collections.Generic;
using System.Linq;
using EntryEngine;
using EntryEngine.Serialize;

[AReflexible]public partial class T动作
{
    public static bool __Load = true;
    /// <summary>动作</summary>
    public enum ET动作Action
    {
        蒲公英_向右移动,
        蒲公英_向右跑动,
        蒲公英_向左移动,
        蒲公英_向左跑动,
        蒲公英_起跳,
        蒲公英_上升,
        蒲公英_下落,
        蒲公英_落地,
        蒲公英_地面普攻1,
        蒲公英_地面普攻2,
        蒲公英_地面普攻3,
        蒲公英_地面普攻4,
    }
    public ET动作Action Action;
    /// <summary>触发动作需要在浮空上升时</summary>
    public bool FloatUp;
    /// <summary>触发动作需要在浮空下落时</summary>
    public bool FloatDown;
    /// <summary>触发动作需要的状态</summary>
    public int StatusNeed;
    /// <summary>按住按键，放开时会结束动作；有按下按键时，按住按键仅作为结束动作的一个条件</summary>
    public string Pressed;
    /// <summary>按住按键，放开时会结束动作；有按下按键时，按住按键仅作为结束动作的一个条件</summary>
    [NonSerialized]
    public string[] _Pressed;
    /// <summary>按下按键;逗号隔开是连续指令，分号隔开是并行指令</summary>
    public string Click;
    /// <summary>按下按键;逗号隔开是连续指令，分号隔开是并行指令</summary>
    [NonSerialized]
    public string[][] _Click;
    /// <summary>要求前置动作</summary>
    public string ActionNeed;
    /// <summary>要求前置动作</summary>
    [NonSerialized]
    public string[] _ActionNeed;
    /// <summary>开始作用时间，单位秒，触发时为0</summary>
    public float TimeStart;
    /// <summary>作用结束时间，单位秒；若不填则为无限时间（动作动画为无限循环播放时）或动作动画播放完成后；若此时间小于等于开始作用时间，则仅触发一次；若此时间大于开始作用时间，则在这个时间后，动作将视为结束</summary>
    public float TimeEnd;
    /// <summary>作用间隔时间，单位秒，若不填则每帧都起作用</summary>
    public float TimeInterval;
    /// <summary>设置角色状态，除了</summary>
    public int SetStatus;
    /// <summary>角色动作</summary>
    public string DoAction;
    /// <summary>角色朝向变化</summary>
    public ETowards Towards;
    /// <summary>设定初始速度，不填则继承原始速度</summary>
    public float SpeedX;
    /// <summary>设定加速度</summary>
    public float AccelerationX;
    /// <summary>设定初始速度，不填则继承原始速度；正数向下，负数向上</summary>
    public float SpeedY;
    /// <summary>设定加速度；正数向下，负数向上</summary>
    public float AccelerationY;
    static T动作()
    {
        T动作 T动作 = new T动作();
        T动作.Action = default(ET动作Action);
        T动作.FloatUp = default(bool);
        T动作.FloatDown = default(bool);
        T动作.StatusNeed = default(int);
        T动作.Pressed = null;
        T动作.Click = null;
        T动作.ActionNeed = null;
        T动作.TimeStart = default(float);
        T动作.TimeEnd = default(float);
        T动作.TimeInterval = default(float);
        T动作.SetStatus = default(int);
        T动作.DoAction = default(string);
        T动作.Towards = default(ETowards);
        T动作.SpeedX = default(float);
        T动作.AccelerationX = default(float);
        T动作.SpeedY = default(float);
        T动作.AccelerationY = default(float);
    }
    
}
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
public enum ETowards
{
    不变=0,
    向左=1,
    向右=2,
    相反=3,
}
public static partial class _TABLE
{
    public static string _path { get; private set; }
    public static T动作[] _T动作;
    public static T地图[] _T地图;
    public static Dictionary<ushort, T地图[]> _T地图ByID;
    public static T地形[] _T地形;
    public static Dictionary<ushort, T地形> _T地形ByID;
    
    public static event Action<T动作[]> OnLoadT动作;
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
        LoadT动作();
        LoadT地图();
        LoadT地形();
    }
    public static IEnumerable<AsyncReadFile> LoadAsync(string path)
    {
        SetPath(path);
        _LOG.Info("LoadAsync all tables", path);
        foreach (var item in LoadT动作Async()) yield return item;
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
    private static void __ParseT动作(T动作[] array)
    {
        int length = array.Length;
        for (int i = 0; i < length; i++)
        {
            var value = array[i];
            if (!string.IsNullOrEmpty(value.Pressed))
            {
                string[] _array1 = value.Pressed.Split(',');
                int length1 = _array1.Length;
                var result = new string[length1];
                for (int j = 0; j < length1; j++)
                {
                    result[j] = _array1[j];
                }
                value._Pressed = result;
            }
            if (!string.IsNullOrEmpty(value.Click))
            {
                string[] _array2 = value.Click.Split(';');
                int length2 = _array2.Length;
                var result1 = new string[length2][];
                for (int k = 0; k < length2; k++)
                {
                    string[] _array1 = _array2[k].Split(',');
                    int length1 = _array1.Length;
                    var result = new string[length1];
                    for (int j = 0; j < length1; j++)
                    {
                        result[j] = _array1[j];
                    }
                    result1[k] = result;
                }
                value._Click = result1;
            }
            if (!string.IsNullOrEmpty(value.ActionNeed))
            {
                string[] _array1 = value.ActionNeed.Split(',');
                int length1 = _array1.Length;
                var result = new string[length1];
                for (int j = 0; j < length1; j++)
                {
                    result[j] = _array1[j];
                }
                value._ActionNeed = result;
            }
        }
    }
    public static void LoadT动作()
    {
        if (!T动作.__Load) return;
        _LOG.Debug("loading table T动作");
        CSVReader _reader = new CSVReader(_IO.ReadText(_path + "T动作.csv"));
        T动作[] temp = _reader.ReadObject<T动作[]>();
        __ParseT动作(temp);
        if (OnLoadT动作 != null) OnLoadT动作(temp);
        _T动作 = temp;
    }
    public static IEnumerable<AsyncReadFile> LoadT动作Async()
    {
        if (!T动作.__Load) yield break;
        _LOG.Debug("loading async table T动作");
        var async = _IO.ReadAsync(_path + "T动作.csv");
        if (!async.IsEnd) yield return async;
        CSVReader _reader = new CSVReader(_IO.ReadPreambleText(async.Data));
        T动作[] temp = _reader.ReadObject<T动作[]>();
        __ParseT动作(temp);
        if (OnLoadT动作 != null) OnLoadT动作(temp);
        _T动作 = temp;
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
