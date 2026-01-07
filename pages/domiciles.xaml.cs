using Microsoft.Maui.Controls.Compatibility;
using rappel.models;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Grid = Microsoft.Maui.Controls.Grid;


namespace rappel.pages;

public partial class domiciles : ContentPage
{
    List<domicile> listdom = new List<domicile>();
    public domiciles()
	{
		InitializeComponent();
        if (((App)Application.Current).datas.listdoms != null)
        {
            listdom = ((App)Application.Current).datas.listdoms;
            affichagedom(listdom);
        }


    }

    public async void nvdomicile(string nomdom) {
        if (((App)Application.Current).datas.permgpsperm.Equals(PermissionStatus.Granted) || ((App)Application.Current).datas.permgpstemp.Equals(PermissionStatus.Granted))
        {
            var nvpos = await Geolocation.GetLocationAsync();
            double valeurArrondiex = Math.Round(nvpos.Longitude, 4); // Arrondi à 2 décimales
            double valeurArrondiey = Math.Round(nvpos.Latitude, 4); // Arrondi à 2 décimales
            var nvdom = new domicile();
            nvdom.nom = nomdom;
            nvdom.longitude = valeurArrondiex;
            nvdom.latitude = valeurArrondiey;
            nvdom.iddomicile = listdom.Count;
            listdom.Add(nvdom);
            ((App)Application.Current).datas.listdoms = listdom;
            affichagedom(listdom);
            savedom();
            creanvdom.Children.Clear();
            if (((App)Application.Current).datas.activer == true)
            {
                ((App)Application.Current).OnStartServiceClicked();
                ((App)Application.Current).OnSendToServiceClicked(1);
            }
        }
        else
        {
            DisplayAlert("erreur", "l'application n'a pas les authorisation necessaire !", "OK");
        }



    }

	public void suprdomicile(int id)
	{
        listdom.Remove(listdom[id]);
        foreach (var item in listdom)
        {
           if (item.iddomicile > id)
            {
                item.iddomicile -= 1;
            }
        }
        ((App)Application.Current).datas.listdoms = listdom;
        savedom();
        affichagedom(listdom);
    }

    public void savedom()
    {
        if (((App)Application.Current).datas.listdoms != null)
        {
            var stocktxt = "";
            var sepdom = " /sd ";
            var sepel = " /sl ";
            var num = 0;
            foreach (var dom in ((App)Application.Current).datas.listdoms)
            {
                if (num > 0)
                {
                    stocktxt += sepdom;
                }
                else
                {
                    num += 1;
                }
                stocktxt += dom.nom + sepel + dom.longitude.ToString() + sepel + dom.latitude.ToString() + sepel + dom.iddomicile.ToString();
            }
            Preferences.Set("listdom", stocktxt);
        }
      


    }


    public void affichagedom(List<domicile> listedomicile)
	{
        affdom.Children.Clear();
        var grid = new Grid
        {
            RowSpacing = 10,
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto }, // Ligne 0 : taille automatique
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }, // Ligne 1 : occupe l'espace restant
                new RowDefinition { Height = GridLength.Auto } // Ligne 2 : taille automatique
            },
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }, // Colonne 0 : occupe l'espace restant
                new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },  // Colonne 1 : deux fois plus large
                new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) }  // Colonne 1 : deux fois plus large
            }
        };

        var row = 0;
        foreach (var item in listedomicile)
        {
			var labelnom = new Label
			{
				Text= item.nom,
            };
            grid.AddWithSpan(labelnom, row, 0);
			var labelx = new Label {
			 Text="x : "+item.latitude.ToString(),
            };
            grid.AddWithSpan(labelx, row, 1);
            var labely = new Label {
                Text ="y : "+item.longitude.ToString(),
            };
            grid.AddWithSpan(labely, row, 2);
            var bsupr = new Button { 
			Text="x",
            HeightRequest=40,
            WidthRequest=40,
            CornerRadius=45,
            TranslationY=-10,
			};
            bsupr.Clicked += (s, e) =>
            {
				suprdomicile(item.iddomicile);
            };
            grid.AddWithSpan(bsupr, row, 3);
            row += 1;
        }
        affdom.Children.Add(grid);
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        var buttontest = new Button
        {
            Text = "+",
			CornerRadius=90,
        };
	
            var entrernom = new Entry
		{
			Placeholder="nom du domicile",
            MaxLength=30
		};

        buttontest.Clicked += (s, e) =>
        {
            var nomdom = entrernom.Text;
            nvdomicile(nomdom);
        };

        var formulaire = new HorizontalStackLayout
		{
			Spacing=40,
		};
		formulaire.Children.Add(entrernom);
		formulaire.Children.Add(buttontest);


		if (creanvdom.Children.Count == 0)
		{
            creanvdom.Children.Add(formulaire);
		}
		else
		{
			creanvdom.Children.Clear();
            creanvdom.Children.Add(formulaire);
        }

		
    }

}