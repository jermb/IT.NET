﻿#pragma checksum "C:\Users\jermb\Source\Repos\IT.NET\TextEditor\IntroUWP\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2C954F87C890883A2311B57DC652C50A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TextEditor
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.18362.1")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // MainPage.xaml line 29
                {
                    this.FileName = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 3: // MainPage.xaml line 30
                {
                    this.InputBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.InputBox).Paste += this.InputBox_ClipboardAction;
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.InputBox).CuttingToClipboard += this.InputBox_ClipboardAction;
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.InputBox).KeyDown += this.TabHandling;
                }
                break;
            case 4: // MainPage.xaml line 24
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element4 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element4).Click += this.About;
                }
                break;
            case 5: // MainPage.xaml line 16
                {
                    this.NewButton = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)this.NewButton).Click += this.New;
                }
                break;
            case 6: // MainPage.xaml line 17
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element6 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element6).Click += this.Open;
                }
                break;
            case 7: // MainPage.xaml line 18
                {
                    this.SaveButton = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)this.SaveButton).Click += this.Save;
                }
                break;
            case 8: // MainPage.xaml line 19
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element8 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element8).Click += this.SaveAs;
                }
                break;
            case 9: // MainPage.xaml line 21
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element9 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element9).Click += this.Exit;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.18362.1")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

