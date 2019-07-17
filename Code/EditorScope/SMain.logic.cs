using System.Collections.Generic;
using EntryEngine;
using EntryEngine.UI;
using System;
using System.Text;

public partial class SMain : UIScene
{
    public static Action<string[]> DragFile;

    MATRIX2x3 offset;
    MATRIX2x3 offsetInvert;
    TEXTURE animation;
    List<RECT> rects = new List<RECT>();
    VECTOR2 start = new VECTOR2(float.NaN, 0);
    VECTOR2 dragOffset = new VECTOR2(float.NaN, 0);
    int dragIndex = -1;
    TEXTURE area;

    bool Creating { get { return !float.IsNaN(start.X); } }

    public SMain()
    {
        Initialize();
        TBResult.TextEditOver += new Action<Label>(TBResult_TextEditOver);
        BSave.Clicked += new DUpdate<UIElement>(BSave_Clicked);
        DragFile = DragLoad;
        DDAction.Visible = false;
    }

    void DragLoad(string[] files)
    {
        string file = files[0];
        if (animation != null) Content.Dispose(animation.Key);
        try
        {
            animation = Content.Load<TEXTURE>("TEXTURE", file);
        }
        catch (Exception)
        {
        }
    }
    void Copy()
    {
        Entry.INPUT.InputDevice.Copy(TBResult.Text);
    }
    void BSave_Clicked(UIElement sender, Entry e)
    {
        Copy();
    }
    void TBResult_TextEditOver(Label obj)
    {
        string origin = TBResult.Text;
        if (string.IsNullOrEmpty(origin))
        {
            rects.Clear();
            return;
        }

        try
        {
            List<RECT> temp = new List<RECT>();
            string[] datas = origin.Split('|');
            for (int i = 0; i < datas.Length; i++)
            {
                string[] param = datas[i].Split(';');
                temp.Add(new RECT(int.Parse(param[0]), int.Parse(param[1]), int.Parse(param[2]), int.Parse(param[3])));
            }
            this.rects = temp;
        }
        catch (Exception)
        {
            Save();
        }
    }
    void Save()
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0, n = rects.Count - 1; i <= n; i++)
        {
            var rect = rects[i];
            builder.AppendFormat("{0};{1};{2};{3}", (int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
            if (i != n)
                builder.Append("|");
        }
        TBResult.Text = builder.ToString();
    }
    
    private IEnumerable<ICoroutine> MyLoading()
    {
        area = PATCH.GetNinePatch(new COLOR(255, 0, 0, 64), new COLOR(255, 0, 0, 222), 1);
        ResetViewport();
        return null;
    }

    void ResetViewport()
    {
        offset = MATRIX2x3.Identity;
        offset.M31 = 400;
        offset.M32 = 400;
        MATRIX2x3.Invert(ref offset, out offsetInvert);
    }
    VECTOR2 GetCurrentPosition()
    {
        var position = Entry.INPUT.Pointer.Position;
        VECTOR2.Transform(ref position, ref offsetInvert);
        return position;
    }
    int ClickIndex()
    {
        VECTOR2 p;
        return ClickIndex(out p);
    }
    int ClickIndex(out VECTOR2 position)
    {
        position = GetCurrentPosition();
        for (int i = rects.Count - 1; i >= 0; i--)
            if (rects[i].Contains(position))
                return i;
        return -1;
    }

    protected override void InternalUpdate(Entry e)
    {
        base.InternalUpdate(e);
        if (!TBResult.Checked)
            Save();
    }
    protected override void InternalEvent(Entry e)
    {
        base.InternalEvent(e);
        // 空格重置视口
        if (e.INPUT.Keyboard.IsClick(PCKeys.Space))
            ResetViewport();
        // 保存数据到剪切板
        if (e.INPUT.Keyboard.Ctrl && e.INPUT.Keyboard.IsClick(PCKeys.S))
            Copy();
        // 创建或选中时上下左右微调
        if (dragIndex != -1)
        {
            if (e.INPUT.Keyboard.IsInputKeyPressed(PCKeys.A) || e.INPUT.Keyboard.IsInputKeyPressed(PCKeys.Left))
                dragOffset.X--;
            if (e.INPUT.Keyboard.IsInputKeyPressed(PCKeys.D) || e.INPUT.Keyboard.IsInputKeyPressed(PCKeys.Right))
                dragOffset.X++;
            if (e.INPUT.Keyboard.IsInputKeyPressed(PCKeys.S) || e.INPUT.Keyboard.IsInputKeyPressed(PCKeys.Down))
                dragOffset.Y++;
            if (e.INPUT.Keyboard.IsInputKeyPressed(PCKeys.W) || e.INPUT.Keyboard.IsInputKeyPressed(PCKeys.Up))
                dragOffset.Y--;
        }
        //else if (Creating)
        //{
        //    if (e.INPUT.Keyboard.IsInputKeyPressed(PCKeys.A) || e.INPUT.Keyboard.IsInputKeyPressed(PCKeys.Left))
        //        start.X--;
        //    if (e.INPUT.Keyboard.IsInputKeyPressed(PCKeys.D) || e.INPUT.Keyboard.IsInputKeyPressed(PCKeys.Right))
        //        start.X++;
        //    if (e.INPUT.Keyboard.IsInputKeyPressed(PCKeys.S) || e.INPUT.Keyboard.IsInputKeyPressed(PCKeys.Down))
        //        start.Y++;
        //    if (e.INPUT.Keyboard.IsInputKeyPressed(PCKeys.W) || e.INPUT.Keyboard.IsInputKeyPressed(PCKeys.Up))
        //        start.Y--;
        //}
        // 左键拖拽/生成矩形
        if (TBCanvas.IsHover && e.INPUT.Pointer.IsClick(0))
        {
            VECTOR2 position;
            int index = ClickIndex(out position);
            if (index == -1)
            {
                // 创建新矩形
                start = position;
            }
            else
            {
                // 拖拽旧矩形
                dragIndex = index;
                dragOffset = rects[dragIndex].Location;
            }
        }

        if (dragIndex != -1 && e.INPUT.Pointer.IsPressed(0))
        {
            // 拖拽
            rects[dragIndex] = new RECT(e.INPUT.Pointer.Position - e.INPUT.Pointer.ClickPosition + dragOffset, rects[dragIndex].Size);
        }

        if (e.INPUT.Pointer.IsRelease(0))
        {
            if (dragIndex != -1)
            {
                // 确认拖拽
                dragIndex = -1;
            }
            else if (Creating)
            {
                // 确认创建
                rects.Add(RECT.CreateRectangle(start, GetCurrentPosition()));
                start.X = float.NaN;
            }
        }

        if (e.INPUT.Pointer.IsClick(1))
        {
            if (dragIndex != -1)
            {
                // 取消拖拽
                rects[dragIndex] = new RECT(e.INPUT.Pointer.ClickPosition - dragOffset, rects[dragIndex].Size);
                dragIndex = -1;
            }
            else if (Creating)
            {
                // 取消创建新矩形
                start.X = float.NaN;
            }
            else
            {
                // 删除矩形
                int index = ClickIndex();
                if (index != -1)
                    rects.RemoveAt(index);
            }
        }

        if (e.INPUT.Pointer.IsPressed(1))
        {
            // 拖拽地图
            var delta = e.INPUT.Pointer.DeltaPosition;
            offset.M31 += delta.X;
            offset.M32 += delta.Y;
            MATRIX2x3.Invert(ref offset, out offsetInvert);
        }
    }
    protected override void InternalDraw(GRAPHICS spriteBatch, Entry e)
    {
        base.InternalDraw(spriteBatch, e);

        spriteBatch.Draw(TEXTURE.Pixel, new RECT(0, offset.M32, 800, 1), COLOR.Lime);
        spriteBatch.Draw(TEXTURE.Pixel, new RECT(offset.M31, 0, 1, 800), COLOR.Lime);

        spriteBatch.Begin(offset);

        if (animation != null)
            spriteBatch.Draw(animation, VECTOR2.Zero, 0, 0.5f, 0.5f);

        for (int i = 0; i < rects.Count; i++)
            spriteBatch.Draw(area, rects[i]);
        if (Creating)
            spriteBatch.Draw(area, RECT.CreateRectangle(start, GetCurrentPosition()));

        spriteBatch.End();
        
    }
}
