﻿#pragma checksum "E:\kodisoft\IRTDocs2\IRTDocs2\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "41DDE27D6CF7B200D50774EAAFEF16B3"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IRTDocs2
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.BackgroundGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 2:
                {
                    this.AddImage1 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 16 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.AddImage1).Click += this.AddImage1_Click;
                    #line default
                }
                break;
            case 3:
                {
                    this.AddImage2 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 21 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.AddImage2).Click += this.AddImage2_Click;
                    #line default
                }
                break;
            case 4:
                {
                    this.AddImage3 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 26 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.AddImage3).Click += this.AddImage3_Click;
                    #line default
                }
                break;
            case 5:
                {
                    this.Qr = (global::Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl)(target);
                    #line 33 "..\..\..\MainPage.xaml"
                    ((global::Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl)this.Qr).Draw += this.Qr_OnDraw;
                    #line 34 "..\..\..\MainPage.xaml"
                    ((global::Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl)this.Qr).CreateResources += this.Qr_OnCreateResources;
                    #line 35 "..\..\..\MainPage.xaml"
                    ((global::Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl)this.Qr).PointerPressed += this.Qr_OnPointerPressed;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

