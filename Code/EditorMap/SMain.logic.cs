using System.Collections.Generic;
using EntryEngine;
using EntryEngine.UI;
using EntryEngine.Xna;
using Client.Game;

public partial class SMain : UIScene
{
    string mapFile;
    MAP map;
    MATRIX2x3 offset = MATRIX2x3.Identity;
    MATRIX2x3 offsetInvert;

    public SMain()
    {
        Initialize();
        XnaGate.Gate.DragFiles = DragMapFile;
    }
    
    private IEnumerable<ICoroutine> MyLoading()
    {
        ResetViewport();
        // 加载预设
        return null;
    }

    void DragMapFile(string[] files)
    {
        string file = files[0];
        if (file.EndsWith(MAP.FILE))
        {
            map = MAP.Load(_IO.ReadByte(file));
            mapFile = file;
            map.LoadTextureSync(Content);
        }
    }
    void ResetViewport()
    {
        offset.M31 = TBMap.Width * 0.5f;
        offset.M32 = TBMap.Height * 0.5f;
        MATRIX2x3.Invert(ref offset, out offsetInvert);
    }

    protected override void InternalEvent(Entry e)
    {
        base.InternalEvent(e);

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

        spriteBatch.Draw(TEXTURE.Pixel, new RECT(0, offset.M32, TBMap.Width, 1), COLOR.Lime);
        spriteBatch.Draw(TEXTURE.Pixel, new RECT(offset.M31, 0, 1, TBMap.Height), COLOR.Lime);

        spriteBatch.Begin(offset);


        
        spriteBatch.End();
    }
}
