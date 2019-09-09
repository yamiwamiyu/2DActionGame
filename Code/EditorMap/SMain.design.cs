using System.Collections.Generic;
using EntryEngine;
using EntryEngine.UI;

public partial class SMain
{
    public EntryEngine.UI.TextureBox TB190718101113 = new EntryEngine.UI.TextureBox();
    public EntryEngine.UI.Label L190726102325 = new EntryEngine.UI.Label();
    public EntryEngine.UI.Panel PPre = new EntryEngine.UI.Panel();
    public EntryEngine.UI.Panel PTypes = new EntryEngine.UI.Panel();
    public EntryEngine.UI.CheckBox   CBTypes = new EntryEngine.UI.CheckBox();
    public EntryEngine.UI.TextureBox TBThumnail = new EntryEngine.UI.TextureBox();
    public EntryEngine.UI.Label L190726103109 = new EntryEngine.UI.Label();
    public EntryEngine.UI.TextureBox TBMap = new EntryEngine.UI.TextureBox();
    private EntryEngine.UI.CheckBox ___CBTypes()
    {
        var CBTypes = new EntryEngine.UI.CheckBox();
        CBTypes.Name = "  CBTypes";
        EntryEngine.RECT   CBTypes_Clip = new EntryEngine.RECT();
        CBTypes_Clip.X = 2f;
        CBTypes_Clip.Y = 2f;
        CBTypes_Clip.Width = 80f;
        CBTypes_Clip.Height = 24f;
        CBTypes.Clip =   CBTypes_Clip;
        
        CBTypes.SourceClicked = PATCH.GetNinePatch(PATCH.NullColor, new COLOR(0, 0, 0, 255), 1);
        CBTypes.UIText = new EntryEngine.UI.UIText();
        CBTypes.UIText.Text = "分类分类分类";
        CBTypes.UIText.FontColor = new COLOR()
        {
            B = 51,
            G = 51,
            R = 51,
            A = 255,
        };
        CBTypes.UIText.TextAlignment = (EPivot)17;
        CBTypes.UIText.TextShader = null;
        CBTypes.UIText.Padding.X = 0f;
        CBTypes.UIText.Padding.Y = 0f;
        CBTypes.UIText.FontSize = 12f;
        
        CBTypes.Text = "分类分类分类";
        CBTypes.IsRadioButton = true;
        CBTypes.CheckedOverlayNormal = true;
        
        return CBTypes;
    }
    
    
    private void Initialize()
    {
        this.Name = "SMain";
        EntryEngine.RECT this_Clip = new EntryEngine.RECT();
        this_Clip.X = 0f;
        this_Clip.Y = 0f;
        this_Clip.Width = 1280f;
        this_Clip.Height = 720f;
        this.Clip = this_Clip;
        
        TB190718101113.Name = "TB190718101113";
        EntryEngine.RECT TB190718101113_Clip = new EntryEngine.RECT();
        TB190718101113_Clip.X = 720f;
        TB190718101113_Clip.Y = 0f;
        TB190718101113_Clip.Width = 560f;
        TB190718101113_Clip.Height = 470f;
        TB190718101113.Clip = TB190718101113_Clip;
        
        this.Add(TB190718101113);
        L190726102325.Name = "L190726102325";
        EntryEngine.RECT L190726102325_Clip = new EntryEngine.RECT();
        L190726102325_Clip.X = 11f;
        L190726102325_Clip.Y = 397.375f;
        L190726102325_Clip.Width = 538f;
        L190726102325_Clip.Height = 61f;
        L190726102325.Clip = L190726102325_Clip;
        
        L190726102325.UIText = new EntryEngine.UI.UIText();
        L190726102325.UIText.Text = "左键刷地形 / 右键或Alt+左键删除地形 / 右键拖拽地图 / Ctrl + S保存地\n图 / Ctrl + Shift + S另存为地图 / 拖拽地图文件到窗口以加载地图 / 1\n键设置地形锁 / 3键隐藏显示额外信息 / Ctrl + A新建地图";
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
        L190726102325.Text = "左键刷地形 / 右键或Alt+左键删除地形 / 右键拖拽地图 / Ctrl + S保存地图 / Ctrl + Shift + S另存为地图 / 拖拽地图文件到窗口以加载地图 / 1键设置地形锁 / 3键隐藏显示额外信息 / Ctrl + A新建地图";
        L190726102325.BreakLine = true;
        TB190718101113.Add(L190726102325);
        PPre.Name = "PPre";
        EntryEngine.RECT PPre_Clip = new EntryEngine.RECT();
        PPre_Clip.X = 0f;
        PPre_Clip.Y = 27f;
        PPre_Clip.Width = 560f;
        PPre_Clip.Height = 360f;
        PPre.Clip = PPre_Clip;
        
        PPre.DragMode = (EDragMode)1;
        PPre.DragInertia = 0.5f;
        TB190718101113.Add(PPre);
        PTypes.Name = "PTypes";
        EntryEngine.RECT PTypes_Clip = new EntryEngine.RECT();
        PTypes_Clip.X = 0f;
        PTypes_Clip.Y = 0f;
        PTypes_Clip.Width = 560f;
        PTypes_Clip.Height = 27f;
        PTypes.Clip = PTypes_Clip;
        
        PTypes.DragMode = (EDragMode)1;
        TB190718101113.Add(PTypes);
        TBThumnail.Name = "TBThumnail";
        EntryEngine.RECT TBThumnail_Clip = new EntryEngine.RECT();
        TBThumnail_Clip.X = 720f;
        TBThumnail_Clip.Y = 470f;
        TBThumnail_Clip.Width = 560f;
        TBThumnail_Clip.Height = 250f;
        TBThumnail.Clip = TBThumnail_Clip;
        
        TBThumnail.Color = new COLOR()
        {
            B = 51,
            G = 51,
            R = 51,
            A = 255,
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
        TBMap.Name = "TBMap";
        EntryEngine.RECT TBMap_Clip = new EntryEngine.RECT();
        TBMap_Clip.X = 0f;
        TBMap_Clip.Y = 0f;
        TBMap_Clip.Width = 720f;
        TBMap_Clip.Height = 720f;
        TBMap.Clip = TBMap_Clip;
        
        this.Add(TBMap);
        
        this.PhaseShowing += Show;
    }
    protected override IEnumerable<ICoroutine> Loading()
    {
        ICoroutine async;
        ICoroutine ___async;
        TB190718101113.Texture = PATCH.GetNinePatch(PATCH.NullColor, new COLOR(0, 0, 0, 255), 1);
        
        CBTypes.SourceClicked = PATCH.GetNinePatch(PATCH.NullColor, new COLOR(0, 0, 0, 255), 1);
        
        TBThumnail.Texture = PATCH.GetNinePatch(PATCH.NullColor, new COLOR(153, 153, 153, 255), 1);
        
        
        var __loading = MyLoading();
        if (__loading != null)
        foreach (var item in __loading)
        yield return item;
    }
    private void Show(EntryEngine.UI.UIScene __scene)
    {
        
    }
}
