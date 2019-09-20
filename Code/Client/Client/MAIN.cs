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

        async = _IO.ReadAsync("Metadata.pcsv");
        if (!async.IsEnd) yield return async;
        PipelinePiece.GetDefaultPipeline().LoadMetadata(_IO.ReadPreambleText(async.Data));
        _LOG.Debug("加载纹理集表完毕");

        foreach (var item in _TABLE.LoadAsync("Tables\\"))
            if (!item.IsEnd)
                yield return item;
        _LOG.Debug("加载数据表完毕");

        _IO.IOEncoding = encoding;

        // 确保加载完了所有的基本资源后就可以启动第一个正式的菜单了
        //Entry.ShowMainScene<T>();
        //this.State = EState.Release;

        Content = Entry.NewContentManager();
        tileTexture = PATCH.GetNinePatch(new COLOR(255, 0, 0, 32), new COLOR(255, 0, 0, 196), 1);

        RECT rect = new RECT(0, 600, 100, 30);
        tile = new RECT[200];
        for (int i = 0; i < tile.Length; i++)
        {
            tile[i] = rect;
            rect.X += rect.Width;
            if (i == 6)
                rect.Y -= 60;
            if (i > 7)
            {
                rect.Y -= 9;
                rect.Width = 10;
            }
            mapWidth += rect.Width;
        }
        tile[tile.Length - 1] = new RECT(200, 400, 30, 120);

        charaTexture = PATCH.GetNinePatch(new COLOR(0, 255, 0, 32), new COLOR(0, 255, 0, 196), 1);
        chara = new RECT(200, 150, 50, 50);

        Entry.INPUT.Keyboard.AddMultipleClick((int)PCKeys.A, (int)PCKeys.D);

        anime = Content.Load<ANIMATION>("Actions\\蒲公英.mtseq");

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
    const float JUMP = -25;
    const float CLIMB_THRESHOLD = 10;
    bool running;
    ANIMATION anime;
    EFlip flip;
    bool CheckMoveX(float x)
    {
        RECT next = chara;
        next.X += x;
        return CheckMove(next);
    }
    bool CheckMoveY(float y)
    {
        RECT next = chara;
        next.Y += y;
        return CheckMove(next);
    }
    bool CheckMove(RECT next)
    {
        bool _left = next.X < chara.X;
        bool _right = next.X > chara.X;
        bool _up = next.Y < chara.Y;
        bool _down = next.Y > chara.Y;

        bool hitFlag = false;
        bool moved = true;
        float firstX = float.NaN;
        for (int i = 0; i < tile.Length; i++)
        {
            if (tile[i].Intersects(ref next))
            {
                hitFlag = true;

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
                    //next.Y -= intersect.Height;
                    next.Y = intersect.Y - next.Height;
                    if (next.Y > chara.Y)
                        continue;
                }
                if (_right)
                {
                    // 向右移动时向左挤出
                    //next.X -= intersect.Width;
                    next.X = intersect.X - next.Width;
                    if (next.X > chara.X)
                        continue;
                }
                if (_left)
                {
                    // 向左移动时向右挤出
                    //next.X += intersect.Width;
                    next.X = intersect.Right;
                    if (next.X < chara.X)
                        continue;
                }
                if (_up)
                {
                    // 向上移动时向下挤出
                    //next.Y += intersect.Height;
                    next.Y = intersect.Bottom;
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

        if (_down && (!moved || hitFlag))
        {
            jumpSpeed.X = 0;
            jumpSpeed.Y = 0;
        }

        return moved;
    }
    void Action(T动作.ET动作Action action)
    {
        anime.Play(action.ToString());
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
                flip = EFlip.FlipHorizontally;
                if (jumpSpeed.Y == 0)
                {
                    CheckMoveX(-SPEED * (running ? RUNNING : 1));
                    //Action(T动作.ET动作Action.蒲公英_移动);
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
                flip = EFlip.None;
                if (jumpSpeed.Y == 0)
                {
                    CheckMoveX(SPEED * (running ? RUNNING : 1));
                    //Action(T动作.ET动作Action.蒲公英_移动);
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
            Action(T动作.ET动作Action.蒲公英_起跳);
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
        {
            running = false;
            //if (jumpSpeed.Y == 0 && anime.Sequence.Name == T动作.ET动作Action.蒲公英_移动.ToString())
            //    Action(T动作.ET动作Action.蒲公英_待机);
        }
    }
    protected override void InternalUpdate(Entry e)
    {
        base.InternalUpdate(e);

        // 左右移动之后悬空时，坡度较小时直接下坡
        if (jumpSpeed.Y == 0)
        {
            jumpSpeed.Y = CLIMB_THRESHOLD;
            var temp = chara;
            bool moved = CheckMoveY(jumpSpeed.Y);
            if (!moved || jumpSpeed.Y != 0)
            {
                if (jumpSpeed.Y != 0)
                    Action(T动作.ET动作Action.蒲公英_下落);
                // 原本就站在地面上 || 空中下坠
                chara = temp;
                jumpSpeed.Y = 0;
            }
        }
        // 重力加速度
        float tempY = jumpSpeed.Y;
        jumpSpeed.Y += G;
        // 由上升变为降落
        if (tempY < 0 && jumpSpeed.Y > 0)
            Action(T动作.ET动作Action.蒲公英_下落);

        // 下落
        if (jumpSpeed.X != 0)
            CheckMoveX(jumpSpeed.X);
        CheckMoveY(jumpSpeed.Y);
        if (tempY != 0 && jumpSpeed.Y == 0)
            Action(T动作.ET动作Action.蒲公英_落地);
    }
    protected override void InternalDraw(GRAPHICS spriteBatch, Entry e)
    {
        base.InternalDraw(spriteBatch, e);

        spriteBatch.Begin(offset);

        for (int i = 0; i < tile.Length; i++)
        {
            spriteBatch.Draw(tileTexture, tile[i]);
        }

        spriteBatch.Draw(anime, chara.Location, 0, 0, 0, 1, 1, flip);
        spriteBatch.Draw(charaTexture, chara);

        spriteBatch.End();
    }
}
