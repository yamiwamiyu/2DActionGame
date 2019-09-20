using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntryEngine;

public partial class T地形
{
    [NonSerialized]
    public RECT BoundingBox;

    /// <summary>左上角为锚点的绘制，若要改为左下角，只需要y值加上BoundingBox.Height即可</summary>
    public void DrawHitArea(float x, float y, float scale)
    {
        if (_Hit == null) return;
        foreach (var area in _Hit)
        {
            __GRAPHICS.Draw(TEXTURE.Pixel,
                new RECT(x + (area.X - BoundingBox.X) * scale,
                    y + (area.Y - BoundingBox.Y) * scale,
                    area.Width * scale, area.Height * scale),
                new COLOR(255, 0, 0, 128));
        }
    }
}
public partial class T动作
{
    public PCKeys[][] Command { get; set; }

    public override string ToString()
    {
        return Action.ToString();
    }
}
public static partial class _TABLE
{
    static _TABLE()
    {
        OnLoadT地形 += new Action<T地形[]>(_TABLE_OnLoadT地形);
        OnLoadT动作 += new Action<T动作[]>(_TABLE_OnLoadT动作);
    }

    static void _TABLE_OnLoadT动作(T动作[] obj)
    {
        foreach (var item in obj)
        {
            if (item._Click != null)
            {
                item.Command = new PCKeys[item._Click.Length][];
                for (int i = 0; i < item._Click.Length; i++)
                {
                    item.Command[i] = new PCKeys[item._Click[i].Length];
                    for (int j = 0; j < item._Click[i].Length; j++)
                    {
                        item.Command[i][j] = (PCKeys)Enum.Parse(typeof(PCKeys), item._Click[i][j], true);
                    }
                }
            }
        }
    }
    static void _TABLE_OnLoadT地形(T地形[] obj)
    {
        foreach (var item in obj)
        {
            if (item._Hit == null) continue;

            // 计算包围盒
            RECT boundingBox = new RECT(float.MaxValue, float.MaxValue, float.MinValue, float.MinValue);
            foreach (var area in item._Hit)
            {
                if (area.X < boundingBox.X)
                    boundingBox.X = area.X;
                if (area.Y < boundingBox.Y)
                    boundingBox.Y = area.Y;
                if (area.Right > boundingBox.Width)
                    boundingBox.Width = area.Right;
                if (area.Bottom > boundingBox.Height)
                    boundingBox.Height = area.Bottom;
            }
            //boundingBox.Width -= boundingBox.X;
            //boundingBox.Height -= boundingBox.Y;
            item.BoundingBox = boundingBox;
            item.BoundingBox.Width -= boundingBox.X;
            item.BoundingBox.Height -= boundingBox.Y;

            if (item.Flip == EFlip.None) continue;
            // 翻转碰撞区域和包围盒
            if (item.Flip == EFlip.FlipHorizontally)
            {
                for (int i = 0; i < item._Hit.Length; i++)
                    item._Hit[i].X = boundingBox.X + boundingBox.Width - item._Hit[i].X - item._Hit[i].Width;
            }
            else if (item.Flip == EFlip.FlipVertically)
            {
                for (int i = 0; i < item._Hit.Length; i++)
                    item._Hit[i].Y = boundingBox.Y + boundingBox.Height - item._Hit[i].Y - item._Hit[i].Height;
            }
            else
            {
                for (int i = 0; i < item._Hit.Length; i++)
                {
                    item._Hit[i].X = boundingBox.X + boundingBox.Width - item._Hit[i].X - item._Hit[i].Width;
                    item._Hit[i].Y = boundingBox.Y + boundingBox.Height - item._Hit[i].Y - item._Hit[i].Height;
                }
            }
        }
    }
}
