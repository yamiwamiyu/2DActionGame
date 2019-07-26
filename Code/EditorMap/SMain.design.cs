using System.Collections.Generic;
using EntryEngine;
using EntryEngine.UI;

public partial class SMain
{
    public EntryEngine.UI.TextureBox TBMap = new EntryEngine.UI.TextureBox();
    public EntryEngine.UI.TextureBox TB190718101113 = new EntryEngine.UI.TextureBox();
    public EntryEngine.UI.Panel PPre = new EntryEngine.UI.Panel();
    public EntryEngine.UI.Label L190726102325 = new EntryEngine.UI.Label();
    public EntryEngine.UI.TextureBox TBThumnail = new EntryEngine.UI.TextureBox();
    public EntryEngine.UI.Label L190726103109 = new EntryEngine.UI.Label();
    
    
    private void Initialize()
    {
        this.Name = "SMain";
        EntryEngine.RECT this_Clip = new EntryEngine.RECT();
        this_Clip.X = 0f;
        this_Clip.Y = 0f;
        this_Clip.Width = 1280f;
        this_Clip.Height = 720f;
        this.Clip = this_Clip;
        
        TBMap.Name = "TBMap";
        EntryEngine.RECT TBMap_Clip = new EntryEngine.RECT();
        TBMap_Clip.X = 0f;
        TBMap_Clip.Y = 0f;
        TBMap_Clip.Width = 720f;
        TBMap_Clip.Height = 720f;
        TBMap.Clip = TBMap_Clip;
        
        this.Add(TBMap);
        TB190718101113.Name = "TB190718101113";
        EntryEngine.RECT TB190718101113_Clip = new EntryEngine.RECT();
        TB190718101113_Clip.X = 720f;
        TB190718101113_Clip.Y = 0f;
        TB190718101113_Clip.Width = 560f;
        TB190718101113_Clip.Height = 470f;
        TB190718101113.Clip = TB190718101113_Clip;
        
        this.Add(TB190718101113);
        PPre.Name = "PPre";
        EntryEngine.RECT PPre_Clip = new EntryEngine.RECT();
        PPre_Clip.X = 0f;
        PPre_Clip.Y = 0f;
        PPre_Clip.Width = 560f;
        PPre_Clip.Height = 376.375f;
        PPre.Clip = PPre_Clip;
        
        PPre.DragMode = (EDragMode)1;
        PPre.DragInertia = 0.5f;
        TB190718101113.Add(PPre);
        L190726102325.Name = "L190726102325";
        EntryEngine.RECT L190726102325_Clip = new EntryEngine.RECT();
        L190726102325_Clip.X = 35f;
        L190726102325_Clip.Y = 393.1875f;
        L190726102325_Clip.Width = 490f;
        L190726102325_Clip.Height = 62f;
        L190726102325.Clip = L190726102325_Clip;
        
        L190726102325.UIText = new EntryEngine.UI.UIText();
        L190726102325.UIText.Text = "左键拖拽地形 / Alt + 左键删除地形 / 右键拖拽地图 / Ctrl + S保存地图\n / Ctrl + Shift + S另存为地图 / 拖拽地图文件到窗口以加载地图 / \n1,2,3分别设置地形不能回头，默认，不能向前 / Ctrl + A新建地图";
        L190726102325.UIText.FontColor = new COLOR()
        {
            B = 0,
            G = 0,
            R = 0,
            A = 255,
        };
        L190726102325.UIText.TextAlignment = (EPivot)0;
        L190726102325.UIText.TextShader = null;
        L190726102325.UIText.Padding.X = 0f;
        L190726102325.UIText.Padding.Y = 0f;
        L190726102325.UIText.FontSize = 16f;
        L190726102325.Text = "左键拖拽地形 / Alt + 左键删除地形 / 右键拖拽地图 / Ctrl + S保存地图 / Ctrl + Shift + S另存为地图 / 拖拽地图文件到窗口以加载地图 / 1,2,3分别设置地形不能回头，默认，不能向前 / Ctrl + A新建地图";
        L190726102325.BreakLine = true;
        TB190718101113.Add(L190726102325);
        TBThumnail.Name = "TBThumnail";
        EntryEngine.RECT TBThumnail_Clip = new EntryEngine.RECT();
        TBThumnail_Clip.X = 720f;
        TBThumnail_Clip.Y = 470f;
        TBThumnail_Clip.Width = 560f;
        TBThumnail_Clip.Height = 250f;
        TBThumnail.Clip = TBThumnail_Clip;
        
        TBThumnail.Color = new COLOR()
        {
            B = 255,
            G = 255,
            R = 255,
            A = 0,
        };
        this.Add(TBThumnail);
        L190726103109.Name = "L190726103109";
        EntryEngine.RECT L190726103109_Clip = new EntryEngine.RECT();
        L190726103109_Clip.X = 10f;
        L190726103109_Clip.Y = 6.375f;
        L190726103109_Clip.Width = 0;
        L190726103109_Clip.Height = 0;
        L190726103109.Clip = L190726103109_Clip;
        
        L190726103109.UIText = new EntryEngine.UI.UIText();
        L190726103109.UIText.Text = "缩略图";
        L190726103109.UIText.FontColor = new COLOR()
        {
            B = 0,
            G = 255,
            R = 0,
            A = 255,
        };
        L190726103109.UIText.TextAlignment = (EPivot)0;
        L190726103109.UIText.TextShader = null;
        L190726103109.UIText.Padding.X = 0f;
        L190726103109.UIText.Padding.Y = 0f;
        L190726103109.UIText.FontSize = 16f;
        L190726103109.Text = "缩略图";
        TBThumnail.Add(L190726103109);
        
        this.PhaseShowing += Show;
    }
    protected override IEnumerable<ICoroutine> Loading()
    {
        ICoroutine async;
        ICoroutine ___async;
        TB190718101113.Texture = PATCH.GetNinePatch(PATCH.NullColor, new COLOR(0, 0, 0, 255), 1);
        
        TBThumnail.Texture = PATCH.GetNinePatch(PATCH.NullColor, new COLOR(0, 255, 0, 255), 1);
        
        
        var __loading = MyLoading();
        if (__loading != null)
        foreach (var item in __loading)
        yield return item;
    }
    private void Show(EntryEngine.UI.UIScene __scene)
    {
        
    }
}
