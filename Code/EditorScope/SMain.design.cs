using System.Collections.Generic;
using EntryEngine;
using EntryEngine.UI;

public partial class SMain
{
    public EntryEngine.UI.TextureBox TB190712145935 = new EntryEngine.UI.TextureBox();
    public EntryEngine.UI.Label[] L190712150313 = new EntryEngine.UI.Label[4]
    {
        new EntryEngine.UI.Label(),
        new EntryEngine.UI.Label(),
        new EntryEngine.UI.Label(),
        new EntryEngine.UI.Label(),
    };
    public EntryEngine.UI.TextBox TBResult = new EntryEngine.UI.TextBox();
    public EntryEngine.UI.Button BSave = new EntryEngine.UI.Button();
    public EntryEngine.UI.TextureBox TBCanvas = new EntryEngine.UI.TextureBox();
    public EntryEngine.UI.DropDown DDAction = new EntryEngine.UI.DropDown();
    
    
    private void Initialize()
    {
        this.Name = "SMain";
        EntryEngine.RECT this_Clip = new EntryEngine.RECT();
        this_Clip.X = 0f;
        this_Clip.Y = 0f;
        this_Clip.Width = 1280f;
        this_Clip.Height = 720f;
        this.Clip = this_Clip;
        
        TB190712145935.Name = "TB190712145935";
        EntryEngine.RECT TB190712145935_Clip = new EntryEngine.RECT();
        TB190712145935_Clip.X = 800f;
        TB190712145935_Clip.Y = 0f;
        TB190712145935_Clip.Width = 480f;
        TB190712145935_Clip.Height = 720f;
        TB190712145935.Clip = TB190712145935_Clip;
        
        TB190712145935.Color = new COLOR()
        {
            B = 128,
            G = 128,
            R = 128,
            A = 255,
        };
        this.Add(TB190712145935);
        L190712150313[0].Name = "L190712150313";
        EntryEngine.RECT L190712150313_0__Clip = new EntryEngine.RECT();
        L190712150313_0__Clip.X = 18f;
        L190712150313_0__Clip.Y = 623.375f;
        L190712150313_0__Clip.Width = 446f;
        L190712150313_0__Clip.Height = 0;
        L190712150313[0].Clip = L190712150313_0__Clip;
        
        L190712150313[0].UIText = new EntryEngine.UI.UIText();
        L190712150313[0].UIText.Text = "左键拖拽或创建矩形";
        L190712150313[0].UIText.FontColor = new COLOR()
        {
            B = 255,
            G = 255,
            R = 255,
            A = 255,
        };
        L190712150313[0].UIText.TextAlignment = (EPivot)0;
        L190712150313[0].UIText.TextShader = null;
        L190712150313[0].UIText.Padding.X = 0f;
        L190712150313[0].UIText.Padding.Y = 0f;
        L190712150313[0].UIText.FontSize = 16f;
        L190712150313[0].Text = "左键拖拽或创建矩形";
        TB190712145935.Add(L190712150313[0]);
        L190712150313[1].Name = "L190712150313";
        EntryEngine.RECT L190712150313_1__Clip = new EntryEngine.RECT();
        L190712150313_1__Clip.X = 17f;
        L190712150313_1__Clip.Y = 641.625f;
        L190712150313_1__Clip.Width = 446f;
        L190712150313_1__Clip.Height = 0;
        L190712150313[1].Clip = L190712150313_1__Clip;
        
        L190712150313[1].UIText = new EntryEngine.UI.UIText();
        L190712150313[1].UIText.Text = "右键拖拽画布或删除矩形";
        L190712150313[1].UIText.FontColor = new COLOR()
        {
            B = 255,
            G = 255,
            R = 255,
            A = 255,
        };
        L190712150313[1].UIText.TextAlignment = (EPivot)0;
        L190712150313[1].UIText.TextShader = null;
        L190712150313[1].UIText.Padding.X = 0f;
        L190712150313[1].UIText.Padding.Y = 0f;
        L190712150313[1].UIText.FontSize = 16f;
        L190712150313[1].Text = "右键拖拽画布或删除矩形";
        TB190712145935.Add(L190712150313[1]);
        L190712150313[2].Name = "L190712150313";
        EntryEngine.RECT L190712150313_2__Clip = new EntryEngine.RECT();
        L190712150313_2__Clip.X = 17f;
        L190712150313_2__Clip.Y = 659.875f;
        L190712150313_2__Clip.Width = 446f;
        L190712150313_2__Clip.Height = 0;
        L190712150313[2].Clip = L190712150313_2__Clip;
        
        L190712150313[2].UIText = new EntryEngine.UI.UIText();
        L190712150313[2].UIText.Text = "空格重置视口";
        L190712150313[2].UIText.FontColor = new COLOR()
        {
            B = 255,
            G = 255,
            R = 255,
            A = 255,
        };
        L190712150313[2].UIText.TextAlignment = (EPivot)0;
        L190712150313[2].UIText.TextShader = null;
        L190712150313[2].UIText.Padding.X = 0f;
        L190712150313[2].UIText.Padding.Y = 0f;
        L190712150313[2].UIText.FontSize = 16f;
        L190712150313[2].Text = "空格重置视口";
        TB190712145935.Add(L190712150313[2]);
        L190712150313[3].Name = "L190712150313";
        EntryEngine.RECT L190712150313_3__Clip = new EntryEngine.RECT();
        L190712150313_3__Clip.X = 17f;
        L190712150313_3__Clip.Y = 678.125f;
        L190712150313_3__Clip.Width = 446f;
        L190712150313_3__Clip.Height = 0;
        L190712150313[3].Clip = L190712150313_3__Clip;
        
        L190712150313[3].UIText = new EntryEngine.UI.UIText();
        L190712150313[3].UIText.Text = "拖拽动作文件到视口加载动作";
        L190712150313[3].UIText.FontColor = new COLOR()
        {
            B = 255,
            G = 255,
            R = 255,
            A = 255,
        };
        L190712150313[3].UIText.TextAlignment = (EPivot)0;
        L190712150313[3].UIText.TextShader = null;
        L190712150313[3].UIText.Padding.X = 0f;
        L190712150313[3].UIText.Padding.Y = 0f;
        L190712150313[3].UIText.FontSize = 16f;
        L190712150313[3].Text = "拖拽动作文件到视口加载动作";
        TB190712145935.Add(L190712150313[3]);
        TBResult.Name = "TBResult";
        EntryEngine.RECT TBResult_Clip = new EntryEngine.RECT();
        TBResult_Clip.X = 18f;
        TBResult_Clip.Y = 18.375f;
        TBResult_Clip.Width = 446f;
        TBResult_Clip.Height = 545f;
        TBResult.Clip = TBResult_Clip;
        
        TBResult.DefaultText = new EntryEngine.UI.UIText();
        TBResult.DefaultText.Text = "";
        TBResult.DefaultText.FontColor = new COLOR()
        {
            B = 211,
            G = 211,
            R = 211,
            A = 255,
        };
        TBResult.DefaultText.TextAlignment = (EPivot)0;
        TBResult.DefaultText.TextShader = null;
        TBResult.DefaultText.Padding.X = 0f;
        TBResult.DefaultText.Padding.Y = 0f;
        TBResult.DefaultText.FontSize = 16f;
        TBResult.UIText = new EntryEngine.UI.UIText();
        TBResult.UIText.Text = "";
        TBResult.UIText.FontColor = new COLOR()
        {
            B = 0,
            G = 0,
            R = 0,
            A = 255,
        };
        TBResult.UIText.TextAlignment = (EPivot)0;
        TBResult.UIText.TextShader = null;
        TBResult.UIText.Padding.X = 20f;
        TBResult.UIText.Padding.Y = 20f;
        TBResult.UIText.FontSize = 16f;
        TBResult.BreakLine = true;
        TB190712145935.Add(TBResult);
        BSave.Name = "BSave";
        EntryEngine.RECT BSave_Clip = new EntryEngine.RECT();
        BSave_Clip.X = 191f;
        BSave_Clip.Y = 567.375f;
        BSave_Clip.Width = 100f;
        BSave_Clip.Height = 44f;
        BSave.Clip = BSave_Clip;
        
        BSave.Color = new COLOR()
        {
            B = 255,
            G = 255,
            R = 255,
            A = 55,
        };
        BSave.UIText = new EntryEngine.UI.UIText();
        BSave.UIText.Text = "复制";
        BSave.UIText.FontColor = new COLOR()
        {
            B = 255,
            G = 255,
            R = 255,
            A = 255,
        };
        BSave.UIText.TextAlignment = (EPivot)17;
        BSave.UIText.TextShader = null;
        BSave.UIText.Padding.X = 0f;
        BSave.UIText.Padding.Y = 0f;
        BSave.UIText.FontSize = 16f;
        BSave.Text = "复制";
        TB190712145935.Add(BSave);
        TBCanvas.Name = "TBCanvas";
        EntryEngine.RECT TBCanvas_Clip = new EntryEngine.RECT();
        TBCanvas_Clip.X = 0f;
        TBCanvas_Clip.Y = 0f;
        TBCanvas_Clip.Width = 800f;
        TBCanvas_Clip.Height = 720f;
        TBCanvas.Clip = TBCanvas_Clip;
        
        this.Add(TBCanvas);
        DDAction.Name = "DDAction";
        EntryEngine.RECT DDAction_Clip = new EntryEngine.RECT();
        DDAction_Clip.X = 640f;
        DDAction_Clip.Y = 18.375f;
        DDAction_Clip.Width = 150f;
        DDAction_Clip.Height = 31f;
        DDAction.Clip = DDAction_Clip;
        
        DDAction.Color = new COLOR()
        {
            B = 255,
            G = 255,
            R = 255,
            A = 128,
        };
        DDAction.UIText = new EntryEngine.UI.UIText();
        DDAction.UIText.Text = "";
        DDAction.UIText.FontColor = new COLOR()
        {
            B = 255,
            G = 255,
            R = 255,
            A = 255,
        };
        DDAction.UIText.TextAlignment = (EPivot)17;
        DDAction.UIText.TextShader = null;
        DDAction.UIText.Padding.X = 0f;
        DDAction.UIText.Padding.Y = 0f;
        DDAction.UIText.FontSize = 16f;
        TBCanvas.Add(DDAction);
        
        this.PhaseShowing += Show;
    }
    protected override IEnumerable<ICoroutine> Loading()
    {
        ICoroutine async;
        ICoroutine ___async;
        TB190712145935.Texture = PATCH.GetNinePatch(PATCH.NullColor, new COLOR(192, 192, 192, 255), 2);
        
        
        
        
        TBResult.SourceNormal = PATCH.GetNinePatch(PATCH.NullColor, new COLOR(0, 0, 0, 255), 1);
        
        
        BSave.SourceNormal = PATCH.GetNinePatch(PATCH.NullColor, new COLOR(0, 0, 0, 255), 2);
        
        DDAction.SourceNormal = PATCH.GetNinePatch(PATCH.NullColor, new COLOR(255, 255, 0, 255), 2);
        
        
        var __loading = MyLoading();
        if (__loading != null)
        foreach (var item in __loading)
        yield return item;
    }
    private void Show(EntryEngine.UI.UIScene __scene)
    {
        
    }
}
