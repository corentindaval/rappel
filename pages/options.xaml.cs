using Microsoft.Maui.Platform;
using rappel.models;
using System.Collections.ObjectModel;
#if ANDROID
using Android.Content;
#endif


namespace rappel.pages;

public partial class options : ContentPage
{
    public bool affcback = false;
    public bool affctxt = false;
   

    public options()
	{
		InitializeComponent();
       if (((App)Application.Current).datas != null)
        {
            actdes.IsToggled=((App)Application.Current).datas.activer;
            btnctxt.BackgroundColor = ((App)Application.Current).datas.ctxt;
            btncback.BackgroundColor = ((App)Application.Current).datas.cback;
            entrerintervalle.Placeholder= ((App)Application.Current).datas.delai.ToString();
            entrerttxt.Placeholder= ((App)Application.Current).datas.ttxt.ToString();
        }
    }

    private void majact(object sender, ToggledEventArgs e)
    {
        ((App)Application.Current).datas.activer = e.Value;
        Preferences.Set("active", e.Value.ToString());
        if (((App)Application.Current).datas.activer == true)
        {
            ((App)Application.Current).OnStartServiceClicked();
            ((App)Application.Current).OnSendToServiceClicked(2);
        }
        else
        {
            ((App)Application.Current).StopService();
        }



    }

    private void majintervalle(object sender, FocusEventArgs e)
    {
        if (sender is Entry entry)
        {
            string valeur = entry.Text?.Trim() ?? string.Empty;
            if (int.TryParse(valeur, out int result))
            {
                ((App)Application.Current).datas.delai = result;
                Preferences.Set("delai", result.ToString());
                if (((App)Application.Current).datas.activer == true)
                {
                 ((App)Application.Current).OnSendToServiceClicked(2);
                 
                }
            }
            else
            {
                DisplayAlert("erreur", "enter non valide", "OK");
            }
        }
        else
        {
            DisplayAlert("erreur", "enter non valide", "OK");
        }
    }

    public void affcachcolor(int cat)
    {
        List<Color> listcouleur = new List<Color>();
        listcouleur = [Colors.AliceBlue, Colors.AntiqueWhite, Colors.Aqua, Colors.Aquamarine, Colors.Azure, Colors.Beige, Colors.Bisque, Colors.Black, Colors.BlanchedAlmond, Colors.Blue, Colors.BlueViolet, Colors.Brown, Colors.BurlyWood, Colors.CadetBlue, Colors.Chartreuse, Colors.Chocolate, Colors.Coral, Colors.CornflowerBlue, Colors.Cornsilk, Colors.Crimson, Colors.Cyan, Colors.DarkBlue, Colors.DarkCyan, Colors.DarkGoldenrod, Colors.DarkGray, Colors.DarkGreen, Colors.DarkGrey, Colors.DarkKhaki, Colors.DarkMagenta, Colors.DarkOliveGreen, Colors.DarkOrange, Colors.DarkOrchid, Colors.DarkRed, Colors.DarkSalmon, Colors.DarkSeaGreen, Colors.DarkSlateBlue, Colors.DarkSlateGray, Colors.DarkSlateGrey, Colors.DarkTurquoise, Colors.DarkViolet, Colors.DeepPink, Colors.DeepSkyBlue, Colors.DimGray, Colors.DimGrey, Colors.DodgerBlue, Colors.Firebrick, Colors.FloralWhite, Colors.ForestGreen, Colors.Fuchsia, Colors.Gainsboro, Colors.GhostWhite, Colors.Gold, Colors.Goldenrod, Colors.Gray, Colors.Green, Colors.GreenYellow, Colors.Grey, Colors.Honeydew, Colors.HotPink, Colors.IndianRed, Colors.Indigo, Colors.Ivory, Colors.Khaki, Colors.Lavender, Colors.LavenderBlush, Colors.LawnGreen, Colors.LemonChiffon, Colors.LightBlue, Colors.LightCoral, Colors.LightCyan, Colors.LightGoldenrodYellow, Colors.LightGray, Colors.LightGreen, Colors.LightGrey, Colors.LightPink, Colors.LightSalmon, Colors.LightSeaGreen, Colors.LightSkyBlue, Colors.LightSlateGray, Colors.LightSlateGrey, Colors.LightSteelBlue, Colors.LightYellow, Colors.Lime, Colors.LimeGreen, Colors.Linen, Colors.Magenta, Colors.Maroon, Colors.MediumAquamarine, Colors.MediumBlue, Colors.MediumOrchid, Colors.MediumPurple, Colors.MediumSeaGreen, Colors.MediumSlateBlue, Colors.MediumSpringGreen, Colors.MediumTurquoise, Colors.MediumVioletRed, Colors.MidnightBlue, Colors.MintCream, Colors.MistyRose, Colors.Moccasin, Colors.NavajoWhite, Colors.Navy, Colors.OldLace, Colors.Olive, Colors.OliveDrab, Colors.Orange, Colors.OrangeRed, Colors.Orchid, Colors.PaleGoldenrod, Colors.PaleGreen, Colors.PaleTurquoise, Colors.PaleVioletRed, Colors.PapayaWhip, Colors.PeachPuff, Colors.Peru, Colors.Pink, Colors.Plum, Colors.PowderBlue, Colors.Purple, Colors.Red, Colors.RosyBrown, Colors.RoyalBlue, Colors.SaddleBrown, Colors.Salmon, Colors.SandyBrown, Colors.SeaGreen, Colors.SeaShell, Colors.Sienna, Colors.Silver, Colors.SkyBlue, Colors.SlateBlue, Colors.SlateGray, Colors.SlateGrey, Colors.Snow, Colors.SpringGreen, Colors.SteelBlue, Colors.Tan, Colors.Teal, Colors.Thistle, Colors.Tomato, Colors.Transparent, Colors.Turquoise, Colors.Violet, Colors.Wheat, Colors.White, Colors.WhiteSmoke, Colors.Yellow, Colors.YellowGreen];
      //  List<Color> listcouleur2 = new List<Color>();
      //  listcouleur2 = [Colors.Black,Colors.DarkRed,Colors.Crimson,Colors.Red,Colors.OrangeRed,Colors.DarkOrange,Colors.Orange,Colors.Yellow, Colors.LightGoldenrodYellow, Colors.LightYellow,Colors.YellowGreen,Colors.GreenYellow,Colors.DarkGreen,Colors.Green,Colors.LightGreen,Colors.LightSeaGreen,Colors.DarkTurquoise,Colors.MediumTurquoise,Colors.Turquoise,Colors.DarkCyan,Colors.Cyan,Colors.LightCyan,Colors.Aqua,Colors.MediumAquamarine,Colors.Aquamarine,Colors.DarkBlue,Colors.Blue,Colors.AliceBlue,Colors.CadetBlue,Colors.CornflowerBlue,Colors.DarkSlateBlue,Colors.DeepSkyBlue,Colors.LightSteelBlue];
        //cat=0 pour cback cat=1 pour ctxt
        if (cat == 0)
        {
            if (affcback == false)
            {
                var row = 0;
                var col = 0;
                foreach (var couleur in listcouleur)
                {
                    var nvbtn = new Button
                    {
                        WidthRequest=20,
                        HeightRequest=20,
                        BackgroundColor=couleur,
                        BorderColor=Colors.Black,
                        BorderWidth=1
                    };
                    nvbtn.Clicked += (s, e) =>
                    {
                        majcback(couleur);
                        colorback.Clear();
                        affcback = false;
                    };
                    colorback.AddWithSpan(nvbtn, row, col);
                    if (col == 10)
                    {
                        col = 0;
                        row += 1;
                    }
                    else
                    {
                        col += 1;
                    }
                }
                affcback = true;
            }
            else
            {
                colorback.Clear();
               affcback = false;
            }
               

            }else if (cat == 1)
        {
            if (affctxt == false)
            {
                var row = 0;
                var col = 0;
                foreach (var couleur in listcouleur)
                {
                    var nvbtn = new Button
                    {
                        WidthRequest = 20,
                        HeightRequest = 20,
                        BackgroundColor = couleur,
                        BorderColor = Colors.Black,
                        BorderWidth = 1
                    };
                    nvbtn.Clicked += (s, e) =>
                    {
                        majctxt(couleur);
                        colortxt.Clear();
                        affctxt = false;
                    };
                    colortxt.AddWithSpan(nvbtn, row, col);
                    if (col == 10)
                    {
                        col = 0;
                        row += 1;
                    }
                    else
                    {
                        col += 1;
                    }
                }
                affctxt = true;
            }
            else
            {
                colortxt.Clear();
                affctxt = false;
            }
        }
    }
    
    private void majcback(Color couleur)
    {
        ((App)Application.Current).datas.cback = couleur;
        Preferences.Set("cback", ((App)Application.Current).datas.cback.ToArgbHex());
        btncback.BackgroundColor = couleur;
    }

    private void majctxt(Color couleur)
    {
        ((App)Application.Current).datas.ctxt = couleur;
        Preferences.Set("ctxt", ((App)Application.Current).datas.ctxt.ToArgbHex());
        btnctxt.BackgroundColor = couleur;
    }

    private void Button_ctxt(object sender, EventArgs e)
    {
        affcachcolor(1);
    }
    private void Button_cback(object sender, EventArgs e)
    {
        affcachcolor(0);
    }
    private void majttxt(object sender, FocusEventArgs e)
    {
        if (sender is Entry entry)
        {
            string valeur = entry.Text?.Trim() ?? string.Empty;
            if (int.TryParse(valeur, out int result))
            {
                ((App)Application.Current).datas.ttxt = result;
                Preferences.Set("ttxt", result.ToString());
            }
            else
            {
                DisplayAlert("erreur", "enter non valide", "OK");
            }
        }
        else
        {
            DisplayAlert("erreur", "enter non valide", "OK");
        }
    }
}