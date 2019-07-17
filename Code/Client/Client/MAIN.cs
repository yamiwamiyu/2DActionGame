using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntryEngine;
using EntryEngine.UI;

public class MAIN : UIScene
{
    protected override IEnumerable<ICoroutine> Loading()
    {
        // 你可以自定义你的第一个初始菜单来替换MAIN

        AsyncReadFile async;

        // 你还可以加载其它一些常量表

        //async = _IO.ReadAsync("Tables\\CC.xml");
        //if (!async.IsEnd) yield return async;
        //_CC.Load(_IO.ReadPreambleText(async.Data));
        //_LOG.Debug("加载客户端常量表完毕");

        //async = _IO.ReadAsync("Tables\\C.xml");
        //if (!async.IsEnd) yield return async;
        //_C.Load(_IO.ReadPreambleText(async.Data));
        //_LOG.Debug("加载常量表完毕");

        Encoding encoding = _IO.IOEncoding;
        _IO.IOEncoding = Encoding.Default;

        //async = _IO.ReadAsync("Tables\\LANGUAGE.csv");
        //if (!async.IsEnd) yield return async;
        //_LANGUAGE.Load(_IO.ReadPreambleText(async.Data), "");
        //_LOG.Debug("加载语言表完毕");

        //foreach (var item in _TABLE.LoadAsync("Tables\\"))
        //    if (!item.IsEnd)
        //        yield return item;
        //_LOG.Debug("加载数据表完毕");

        _IO.IOEncoding = encoding;

        // 确保加载完了所有的基本资源后就可以启动第一个正式的菜单了
        //Entry.ShowMainScene<T>();
        //this.State = EState.Release;

        tileTexture = PATCH.GetNinePatch(new COLOR(255, 0, 0, 32), new COLOR(255, 0, 0, 196), 1);

        RECT rect = new RECT(0, 600, 100, 30);
        tile = new RECT[50];
        for (int i = 0; i < tile.Length; i++)
        {
            tile[i] = rect;
            rect.X += rect.Width;
            if (i == 6)
                rect.Y -= 60;
            if (i > 7)
            {
                rect.Y -= 9;
                //rect.Width = 5;
            }
            mapWidth += rect.Width;
        }
        tile[tile.Length - 1] = new RECT(200, 400, 30, 120);

        charaTexture = PATCH.GetNinePatch(new COLOR(0, 255, 0, 32), new COLOR(0, 255, 0, 196), 1);
        chara = new RECT(200, 150, 50, 50);

        Entry.INPUT.Keyboard.AddMultipleClick((int)PCKeys.A, (int)PCKeys.D);

        yield break;
    }

    float mapWidth;
    RECT[] tile;
    TEXTURE tileTexture;
    MATRIX2x3 offset = MATRIX2x3.Identity;
    RECT chara;
    TEXTURE charaTexture;
    VECTOR2 jumpSpeed;
    const float SPEED = 3.2f;
    const float RUNNING = 3f;
    const float G = 0.98f;
    const float FLY = 0.2f;
    const float JUMP = -15;
    const float CLIMB_THRESHOLD = 10;
    bool running;
    void CheckCollisionX(float x)
    {
        RECT next = chara;
        next.X += x;
        CheckCollision(next);
    }
    void CheckCollisionY(float y)
    {
        RECT next = chara;
        next.Y += y;
        CheckCollision(next);
    }
    void CheckCollision(RECT next)
    {
        bool _left = next.X < chara.X;
        bool _right = next.X > chara.X;
        bool _up = next.Y < chara.Y;
        bool _down = next.Y > chara.Y;

        bool moved = true;
        float firstX = float.NaN;
        for (int i = 0; i < tile.Length; i++)
        {
            if (tile[i].Intersects(ref next))
            {
                RECT intersect;
                RECT.Intersect(ref tile[i], ref next, out intersect);
                // 稍微凸起的地面
                if (intersect.Bottom == next.Bottom && next.Y <= chara.Y)
                {
                    if (intersect.Height < CLIMB_THRESHOLD)
                    {
                        // 防止大坡度时走得太快
                        if (float.IsNaN(firstX))
                        {
                            // 上楼梯
                            next.Y -= intersect.Height;

                            firstX = next.X - intersect.Width;
                            i = -1;
                            continue;
                        }
                        else
                        {
                            next.X = firstX;
                            break;
                        }
                    }
                }
                if (_down)
                {
                    // 向下移动时向上挤出
                    next.Y -= intersect.Height;
                    if (next.Y > chara.Y)
                        continue;
                }
                if (_right)
                {
                    // 向右移动时向左挤出
                    next.X -= intersect.Width;
                    if (next.X > chara.X)
                        continue;
                }
                if (_left)
                {
                    // 向左移动时向右挤出
                    next.X += intersect.Width;
                    if (next.X < chara.X)
                        continue;
                }
                if (_up)
                {
                    // 向上移动时向下挤出
                    next.Y += intersect.Height;
                    if (next.Y < chara.Y)
                        continue;
                }

                moved = false;
                break;
            }
        }

        if (moved)
        {
            if (next.X <= 0) next.X = 0;
            if (next.X >= mapWidth - chara.Width) next.X = mapWidth - chara.Width;

            chara = next;

            // 地图跟随移动
            var gsize = Entry.GRAPHICS.GraphicsSize.X;
            var gsizeHalf = Entry.GRAPHICS.GraphicsSize.X * 0.5f;
            if (next.X <= gsizeHalf)
                offset.M31 = 0;
            else if (next.X >= mapWidth - gsizeHalf)
                offset.M31 = gsize - mapWidth;
            else
                offset.M31 = gsizeHalf - next.X;
        }
        else
        {
            if (_down)
            {
                jumpSpeed.X = 0;
                jumpSpeed.Y = 0;
            }
        }
    }
    protected override void InternalEvent(Entry e)
    {
        base.InternalEvent(e);
        
        // 双击冲刺
        ComboClick combo = e.INPUT.Keyboard.GetComboClick((int)PCKeys.A);
        if (combo != null && combo.IsDoubleClick)
        {
            running = true;
        }
        else
        {
            combo = e.INPUT.Keyboard.GetComboClick((int)PCKeys.D);
            if (combo != null && combo.IsDoubleClick)
            {
                running = true;
            }
        }
        // 按照最后按下的方向键移动
        int[] keys = e.INPUT.Keyboard.Current.GetPressedKey();
        bool doMove = false;
        EDirection4 direction = EDirection4.Up;
        for (int i = keys.Length - 1; i >= 0; i--)
        {
            if (keys[i] == (int)PCKeys.A)
            {
                if (jumpSpeed.Y == 0)
                {
                    CheckCollisionX(-SPEED * (running ? RUNNING : 1));
                }
                else
                {
                    jumpSpeed.X -= FLY;
                }
                doMove = true;
                direction = EDirection4.Left;
                break;
            }
            else if (keys[i] == (int)PCKeys.D)
            {
                if (jumpSpeed.Y == 0)
                {
                    CheckCollisionX(SPEED * (running ? RUNNING : 1));
                }
                else
                {
                    jumpSpeed.X += FLY;
                }
                doMove = true;
                direction = EDirection4.Right;
                break;
            }
        }

        // 单击跳跃
        if (jumpSpeed.Y == 0 && e.INPUT.Keyboard.IsPressed(PCKeys.W))
        {
            jumpSpeed.Y = JUMP;
            if (doMove)
            {
                jumpSpeed.X = SPEED;
                if (running) jumpSpeed.X *= RUNNING;
                if (direction == EDirection4.Left)
                    jumpSpeed.X = -jumpSpeed.X;
            }
            else
                jumpSpeed.X = 0;
        }
        if (!doMove)
            running = false;
    }
    protected override void InternalUpdate(Entry e)
    {
        base.InternalUpdate(e);

        // 重力加速度
        jumpSpeed.Y += G;

        // 下落
        CheckCollisionX(jumpSpeed.X);
        CheckCollisionY(jumpSpeed.Y);
    }
    protected override void InternalDraw(GRAPHICS spriteBatch, Entry e)
    {
        base.InternalDraw(spriteBatch, e);

        spriteBatch.Begin(offset);

        for (int i = 0; i < tile.Length; i++)
        {
            spriteBatch.Draw(tileTexture, tile[i]);
        }

        spriteBatch.Draw(charaTexture, chara);

        spriteBatch.End();
    }
}
