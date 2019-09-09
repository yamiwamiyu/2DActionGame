using System.Collections.Generic;
using EntryEngine;
using EntryEngine.UI;
using EntryEngine.Xna;
using Client.Game;
using System.Linq;
using System.IO;

public partial class SMain : UIScene
{
    string mapFile;
    MAP map;
    MATRIX2x3 offset = MATRIX2x3.Identity;
    MATRIX2x3 offsetInvert;
    TERRAIN painting = new TERRAIN();
    bool display = true;

    internal float Scale { get { return offset.M11; } set { offset.M11 = value; } }

    public SMain()
    {
        Initialize();
        XnaGate.Gate.DragFiles = DragMapFile;

        TBThumnail.DrawAfterEnd = DrawThumbnail;
    }
    
    private IEnumerable<ICoroutine> MyLoading()
    {
        NewMap();

        _TABLE.SetPath("Tables/");
        foreach (var item in _TABLE.LoadT地形Async())
            yield return item;

        ResetViewport();
        // 预设地形分类
        Dictionary<string, List<T地形>> dics = new Dictionary<string,List<T地形>>();
        foreach (var item in _TABLE._T地形)
        {
            string key;
            if (string.IsNullOrEmpty(item.Texture))
                key = "碰撞区域";
            else
                key = item.Texture.Split('\\')[1];

            List<T地形> list;
            if (!dics.TryGetValue(key, out list))
            {
                list = new List<T地形>();
                dics.Add(key, list);
            }

            list.Add(item);
        }
        // 加载预设
        foreach (var type in dics)
        {
            var cb = ___CBTypes();
            cb.Name = type.Key;
            cb.Text = type.Key;
            cb.X = PTypes.ChildCount * cb.Width + 5;
            cb.OnChecked += (sender1, e1) =>
            {
                PPre.Clear();

                List<T地形> list = dics[sender1.Name];
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];

                    TextureBox container = new TextureBox();
                    container.Width = 100;
                    container.Height = 100;
                    container.X = 10 + (i % 5) * (container.Width + 5);
                    container.Y = 5 + (i / 5) * (container.Height + 5);
                    container.DisplayMode = EViewport.Strength;
                    container.Texture = PATCH.GetNinePatch(COLOR.TransparentBlack, COLOR.Black, 1);

                    TextureBox tb = new TextureBox();
                    tb.Tag = item;
                    tb.Width = container.Width;
                    tb.Height = container.Height;
                    tb.DisplayMode = EViewport.Adapt;
                    tb.Flip = item.Flip;
                    if (string.IsNullOrEmpty(item.Texture))
                    {
                        // 没有图像时绘制碰撞区域
                        tb.DrawAfterEnd = (sender, sb, e) =>
                        {
                            float scale;
                            VECTOR2 offset;
                            __GRAPHICS.ViewAdapt(item.BoundingBox.Size, tb.Size, out scale, out offset);
                            RECT view = sender.ViewClip;
                            //foreach (var area in item._Hit)
                            //{
                            //    sb.Draw(TEXTURE.Pixel,
                            //        new RECT(view.X + offset.X + (area.X - item.BoundingBox.X) * scale,
                            //            view.Y + offset.Y + (area.Y - item.BoundingBox.Y) * scale,
                            //            area.Width * scale, area.Height * scale),
                            //        new COLOR(255, 0, 0, 128));
                            //}
                            item.DrawHitArea(view.X + offset.X, view.Y + offset.Y, scale);
                        };
                    }
                    else
                    {
                        tb.Texture = Content.Load<TEXTURE>(item.Texture);
                    }
                    tb.Clicked += (sender, e) =>
                    {
                        // 单击选中地形
                        painting.ID = item.ID;
                        painting.Data = item;
                        painting.Texture = tb.Texture;
                    };

                    container.Add(tb);
                    PPre.Add(container);
                }
            };
            PTypes.Add(cb);
        }
        ((CheckBox)PTypes[0]).DoCheck();

        font = new UIText();
        //font.FontSize = 12;
        font.FontColor = COLOR.White;
        font.TextShader = new TextShader()
        {
            Color = COLOR.Silver,
        };
        font.TextAlignment = EPivot.BottomLeft;
    }

    void DragMapFile(string[] files)
    {
        string file = files[0];
        if (file.EndsWith(MAP.FILE))
        {
            map = MAP.Load(_IO.ReadByte(file));
            mapFile = Path.GetFileNameWithoutExtension(file);
            map.LoadTextureSync(Content);
        }
    }
    void ResetViewport()
    {
        offset.M11 = 1;
        offset.M22 = 1;
        offset.M31 = TBMap.Width * 0.5f;
        offset.M32 = TBMap.Height * 0.5f;
        MATRIX2x3.Invert(ref offset, out offsetInvert);
    }
    void SortTerrains()
    {
        // 保持图层顺序关系，并且从左往右，从上往下排列
    }
    VECTOR2 GetCurrentPosition()
    {
        var position = Entry.INPUT.Pointer.Position;
        VECTOR2.Transform(ref position, ref offsetInvert);
        position.X = (int)position.X / MAP.GRID * MAP.GRID - (position.X < 0 ? MAP.GRID : 0);
        position.Y = (int)position.Y / MAP.GRID * MAP.GRID + (position.Y > 0 ? MAP.GRID : 0);
        return position;
    }
    void NewMap()
    {
        mapFile = null;
        map = new MAP();
        map.Terrains = new List<TERRAIN>();
        ResetViewport();
    }

    protected override void InternalEvent(Entry e)
    {
        base.InternalEvent(e);

        // 空格重置视口
        if (e.INPUT.Keyboard.IsClick(PCKeys.Space))
            ResetViewport();
        // 滑轮缩放地图
        if (e.INPUT.Mouse.ScrollWheelValue != 0)
        {
            float scale = offset.M11;
            scale = _MATH.Clamp(scale + e.INPUT.Mouse.ScrollWheelValue * 0.1f, 0.2f, 1);
            offset.M11 = scale;
            offset.M22 = scale;
            MATRIX2x3.Invert(ref offset, out offsetInvert);
        }
        if (e.INPUT.Pointer.IsPressed(1))
        {
            // 拖拽地图
            var delta = e.INPUT.Pointer.DeltaPosition;
            offset.M31 += delta.X;
            offset.M32 += delta.Y;
            MATRIX2x3.Invert(ref offset, out offsetInvert);
        }
        if (e.INPUT.Keyboard.Ctrl && e.INPUT.Keyboard.IsClick(PCKeys.A))
            NewMap();

        VECTOR2 position = GetCurrentPosition();
        painting.X = (int)position.X;
        painting.Y = (int)position.Y;
        int index = -1;
        for (int i = 0; i < map.Terrains.Count; i++)
        {
            if (map.Terrains[i].X == position.X && map.Terrains[i].Y == position.Y)
            {
                index = i;
                break;
            }
        }
        if (e.INPUT.Pointer.IsTap(1))
        {
            // 右键删除地形
            if (index != -1)
                map.Terrains.RemoveAt(index);
            // 右键取消绘制
            else
                painting.ID = 0;
        }
        // 左键刷地形
        if (e.INPUT.Pointer.IsClick(0))
        {
            if (painting.ID == 0)
            {
                // 拿起之前绘制的地形变成笔刷
                if (index != -1)
                {
                    painting.ID = map.Terrains[index].ID;
                    painting.Data = map.Terrains[index].Data;
                    painting.Texture = map.Terrains[index].Texture;
                }
            }
        }
        if (e.INPUT.Pointer.IsPressed(0))
        {
            if (e.INPUT.Keyboard.Alt)
            {
                // Alt + 左键删除地形
                if (index != -1)
                    map.Terrains.RemoveAt(index);
            }
            else if (painting.ID != 0 && TBMap.IsHover)
            {
                RECT rect = painting.Area;
                if (!map.Terrains.Any(t => t.Area.Intersects(rect)))
                {
                    // 刷地形
                    TERRAIN terrain = new TERRAIN();
                    terrain.ID = painting.ID;
                    terrain.X = painting.X;
                    terrain.Y = painting.Y;
                    terrain.Data = painting.Data;
                    terrain.Texture = painting.Texture;
                    map.Terrains.Add(terrain);
                }
            }
        }
        if (e.INPUT.Keyboard.IsClick(PCKeys.D1))
        {
            // 1修改锁定信息
            if (index != -1)
            {
                int value = (int)map.Terrains[index].Lock + 1;
                if (value > 2)
                    value = 0;
                map.Terrains[index].Lock = (ELockMode)value;
            }
        }
        else if (e.INPUT.Keyboard.IsClick(PCKeys.D2))
        {
        }
        else if (e.INPUT.Keyboard.IsClick(PCKeys.D3))
        {
            // 3修改显示额外信息
            display = !display;
        }
        // 保存地图
        if (e.INPUT.Keyboard.Ctrl && e.INPUT.Keyboard.IsClick(PCKeys.S))
        {
            if (e.INPUT.Keyboard.Shift || string.IsNullOrEmpty(mapFile))
            {
                _IO._iO.FileBrowserSave("NewMap", new string[] { MAP.FILE.Substring(1) },
                    file =>
                    {
                        mapFile = Path.GetFileNameWithoutExtension(file.File);
                        MAP.Save(mapFile, map.Terrains);
                    });
            }
            else
                MAP.Save(mapFile, map.Terrains);
        }
    }
    PATCH patchSelectedGrid = PATCH.GetNinePatch(new COLOR(255, 0, 0, 64), new COLOR(255, 0, 0, 128), 1);
    PATCH patchViewport = PATCH.GetNinePatch(COLOR.TransparentBlack, COLOR.Lime, 1);
    UIText font;
    protected override void InternalDraw(GRAPHICS spriteBatch, Entry e)
    {
        base.InternalDraw(spriteBatch, e);

        #region 绘制网格
        if (display)
        {
            int size = (int)(MAP.GRID * offset.M11);

            int col = (int)TBMap.Width / size / 2 + 1;
            int row = (int)TBMap.Height / size / 2 + 1;
            RECT rect;
            rect.X = 0;
            rect.Width = TBMap.Width;
            rect.Height = 1;
            for (int i = -row; i <= row; i++)
            {
                rect.Y = TBMap.Height * 0.5f + offset.M32 % size - TBMap.Height * 0.5f % size + i * size;
                spriteBatch.Draw(TEXTURE.Pixel, rect, COLOR.Lime, 0, 0, 0.5f, EFlip.None);
            }
            rect.Y = 0;
            rect.Width = 1;
            rect.Height = TBMap.Height;
            for (int i = -col; i <= col; i++)
            {
                rect.X = TBMap.Width * 0.5f + offset.M31 % size - TBMap.Width * 0.5f % size + i * size;
                spriteBatch.Draw(TEXTURE.Pixel, rect, COLOR.Lime, 0, 0.5f, 0, EFlip.None);
            }
        }
        #endregion

        spriteBatch.Begin(offset);

        #region 绘制地图
        foreach (var item in map.Terrains)
            item.Draw();
        foreach (var item in map.Terrains)
            if (item.Texture == null)
                item.DrawHitArea();
        if (display)
        {
            foreach (var item in map.Terrains)
                if (item.Texture != null)
                    item.Draw();
            // 绘制地形的格子
            foreach (var item in map.Terrains)
                spriteBatch.Draw(TEXTURE.Pixel, new RECT(item.X, item.Y, MAP.GRID, MAP.GRID), new COLOR(255, 255, 0, 32), 0, 0, 1, EFlip.None);
            // 绘制地形碰撞信息

            // 绘制地形锁定信息
            foreach (var item in map.Terrains)
                if (item.Lock == ELockMode.LockBack)
                {
                    font.Text = "←";
                    font.Draw(spriteBatch, new RECT(item.X, item.Y - MAP.GRID, MAP.GRID, MAP.GRID));
                }
                else if (item.Lock == ELockMode.LockForward)
                {
                    font.Text = "→";
                    font.Draw(spriteBatch, new RECT(item.X, item.Y - MAP.GRID, MAP.GRID, MAP.GRID));
                }
        }
        #endregion

        spriteBatch.Draw(TEXTURE.Pixel, VECTOR2.Zero, COLOR.Yellow, 0, 0.5f, 0.5f, 5, 5);

        VECTOR2 position = GetCurrentPosition();
        #region 绘制当前绘制项

        // 绘制当前选中的格子
        spriteBatch.Draw(patchSelectedGrid, new RECT(position.X, position.Y, MAP.GRID, MAP.GRID), 0, 0, 1);

        if (painting.ID != 0)
        {
            if (painting.Texture == null)
                painting.Data.DrawHitArea(position.X, position.Y - painting.Data.BoundingBox.Height, 1);
            else
                spriteBatch.Draw(painting.Texture, new VECTOR2(painting.X, painting.Y), new COLOR(255, 255, 255, 160), 0, 0, 1, 1, 1, painting.Data.Flip);
        }

        #endregion

        spriteBatch.End();
    }
    void DrawThumbnail(UIElement sender, GRAPHICS sb, Entry e)
    {
        var box = map.BoundingBox;

        VECTOR2 offset;
        float scale;
        __GRAPHICS.ViewAdapt(box.Size, TBThumnail.Size, out scale, out offset);

        sb.Begin(MATRIX2x3.CreateTransform(0, box.X, box.Y, scale, scale, TBThumnail.X + offset.X, TBThumnail.Y + offset.Y));

        foreach (var item in map.Terrains)
            item.Draw();

        //VECTOR2 p1 = VECTOR2.Transform(VECTOR2.Zero, offsetInvert);
        //VECTOR2 p2 = VECTOR2.Transform(TBMap.Size, offsetInvert);
        //RECT rect = RECT.CreateRectangle(p1, p2);
        //__GRAPHICS.Draw(patchViewport, rect);

        sb.End();
    }
}
