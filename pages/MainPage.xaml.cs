using rappel.models;
using Application = Microsoft.Maui.Controls.Application;
using Communication = Microsoft.Maui.ApplicationModel.Communication;


#if ANDROID
using rappel.Platforms.Android;
using Android.Content;
using System.Runtime.CompilerServices;
#endif

namespace rappel
{
    public partial class MainPage : ContentPage
    {
        public string datatest;
        private static readonly string[] ImageExtensions ={ ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
        private static readonly string[] VideoExtensions ={ ".mp4", ".avi", ".mov", ".mkv", ".wmv", ".flv", ".webm" };
        public MainPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<App>(this, "majnvmsg", (sender) =>
            {
                    affmessages(((App)Application.Current).datas.listmsg, ((App)Application.Current).datas.listmms);
            });
           
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (((App)Application.Current).datas.listmsg != null)
            {
                affmessages(((App)Application.Current).datas.listmsg, ((App)Application.Current).datas.listmms);
            }
            else
            {
                    Label pasmsg = new Label
                    {
                        Text = "pas de nouveau message",
                        TextColor = ((App)Application.Current).datas.ctxt,
                        FontSize = ((App)Application.Current).datas.ttxt,
                        Padding = 40
                    };
                    affmsg.Clear();
                    affmsg.Children.Add(pasmsg);
            }
        }

        public async void Repondre(string txtreponse, string numeroexp)
        {
            try
            {
                var message = new SmsMessage(txtreponse, new[] { numeroexp });
                await Sms.ComposeAsync(message);
                await DisplayAlert("reussite", "message envoyer", "OK");
            }
            catch (FeatureNotSupportedException)
            {
                await DisplayAlert("Erreur", "L'envoi de SMS n'est pas supporté sur cet appareil.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", $"Impossible d'envoyer le SMS : {ex.Message}", "OK");
            }
        }

        public async void affmessages(List<messages> listmsg,List<MmsMessage> listmms)
        {
            affmsg.Children.Clear();

#if ANDROID
            if (listmms != null)
            {
                foreach(var mms in listmms)
                {
                    var formmsg = new VerticalStackLayout
                    {
                        Spacing = 5
                    };
                    var txtnom = "";
                    if (((App)Application.Current).datas.permcontact.Equals(PermissionStatus.Granted))
                    {
                        var contact = await Communication.Contacts.GetAllAsync();
                        if (contact != null) {
                            txtnom = reccont(contact, mms.Sender);
                        }
                    }
                    else
                    {
                        txtnom = mms.Sender;
                        DisplayAlert("erreur", "l'application n'a pas les authorisation necessaire !", "OK");
                    }
                    var labelnom = new Label
                    {
                        Text = "de : " + txtnom,
                        TextColor = ((App)Application.Current).datas.ctxt,
                        FontSize = ((App)Application.Current).datas.ttxt
                    };
                    formmsg.Children.Add(labelnom);
                    string txtmsg = mms.Text;
                    char sep = ' ';
                    string[] lcont = txtmsg.Split(sep);
                    List<Frame> listweb = new List<Frame>();
                    var nvtxt = "";
                    foreach (var part in lcont)
                    {
                        if (part.Length > 5)
                        {
                            if (part[0] == 'h' & part[1] == 't' & part[2] == 't' & part[3] == 'p')
                            {
                                //ajout verif url
                                if (checkurl(part) == true)
                                {
                                    var nvweb = new WebView
                                    {
                                        Source = part,
                                        WidthRequest = 400,
                                        HeightRequest = 400,
                                    };
                                    var borderweb = new Frame
                                    {
                                        CornerRadius = 20,
                                    };
                                    borderweb.Content = nvweb;
                                    listweb.Add(borderweb);
                                }
                                else
                                {
                                    //function a ajouter dans la version 2
                                }
                       
                            }
                            else
                            {
                                nvtxt += part;
                            }
                        }
                        else
                        {
                            nvtxt += part;
                        }

                    }
                    if (nvtxt != "")
                    {
                        var txt = new Label
                        {
                            Text = mms.Text,
                            TextColor = ((App)Application.Current).datas.ctxt,
                            FontSize = ((App)Application.Current).datas.ttxt,
                            Padding = 10
                        };
                        formmsg.Children.Add(txt);
                        if (listweb != null)
                        {
                            foreach (var wp in listweb)
                            {
                                formmsg.Children.Add(wp);
                            }
                        }
                    }
                    else
                    {
                        foreach (var wp in listweb)
                        {
                            formmsg.Children.Add(wp);
                        }
                    }
                    if(mms.MediaPaths != null)
                    {
                        foreach (var med in mms.MediaPaths) { 
                        var extmed= Path.GetExtension(med)?.ToLowerInvariant();
                          if(Array.Exists(ImageExtensions, e => e == extmed) == true)
                            {
                                var nvimg = new Image
                                {
                                    Source =med,
                                    WidthRequest = 400,
                                    HeightRequest = 400,
                                };
                                formmsg.Children.Add(nvimg);
                            }
                          if(Array.Exists(VideoExtensions, e => e == extmed) == true)
                            {
                                var nvwebview = new WebView {
                                    WidthRequest = 400,
                                    HeightRequest = 400,
                                };
                                var nvhtml = new HtmlWebViewSource
                                {
                                    Html = "<video src='"+med+"' controls autoplay></video>"
                                };
                                nvwebview.Source = nvhtml;
                                formmsg.Children.Add(nvwebview);
                            }

                        }
                    }
                    var mepreponse=new HorizontalStackLayout{
                    
                    };
                    var repondre=new Entry{
                    Placeholder="reponse",

                    };
                    var envoiereponse=new Button{
                    Text=">",
                     WidthRequest = 40,
                     HeightRequest = 40,
                    };
                     envoiereponse.Clicked += (s, e) => {
                     Repondre(repondre.Text,mms.Sender);
                    };
                    mepreponse.Children.Add(repondre);
                    mepreponse.Children.Add(envoiereponse);
                    formmsg.Children.Add(mepreponse);
                    var btnsuprmsg = new Button
                    {
                        Text = "x",
                        WidthRequest = 40,
                        HeightRequest = 40
                    };
                    btnsuprmsg.Clicked += (s, e) =>
                    {
                        suprmms(mms.Id);
                    };

                    formmsg.Children.Add(btnsuprmsg);

                    var bordermsg = new Frame
                    {
                        BackgroundColor = ((App)Application.Current).datas.cback,
                        HasShadow = true
                    };
                    bordermsg.Content = formmsg;
                    affmsg.Children.Add(bordermsg);
                }
            }
            foreach (var item in listmsg)
            {
                var formmsg = new VerticalStackLayout
                {
                 Spacing=5
                };
                var txtnom = "";
                if (((App)Application.Current).datas.permcontact.Equals(PermissionStatus.Granted))
                {
                    var contact = await Communication.Contacts.GetAllAsync();
                    if (contact != null) {
                        txtnom = reccont(contact, item.numero);
                    }
                   
                }
                else
                {
                    txtnom = item.numero;
                    DisplayAlert("erreur", "l'application n'a pas les authorisation necessaire !", "OK");
                }
                    var labelnom = new Label
                    {
                        Text ="de : "+ txtnom,
                        TextColor = ((App)Application.Current).datas.ctxt,
                        FontSize= ((App)Application.Current).datas.ttxt
                    };
                    formmsg.Children.Add(labelnom);
                    string txtmsg=item.msg;
                    char sep=' ';
                    string[] lcont=txtmsg.Split(sep);
                    List<Frame> listweb = new List<Frame>();
                    var nvtxt="";
                    foreach(var part in lcont){
                    if(part.Length>5){
                    if(part[0]=='h' & part[1]=='t' & part[2]=='t' & part[3]=='p'){
                      var nvweb= new WebView{
                      Source=part,
                      WidthRequest=400,
                      HeightRequest=400,
                      };
                      var borderweb=new Frame{
                          CornerRadius = 20,
                      };
                      borderweb.Content=nvweb;
                      listweb.Add(borderweb);
                     }else{
                     nvtxt+=part;
                     }
                    }else{
                     nvtxt+=part;
                    }
                     
                    }
                    if(nvtxt!=""){
                      var txt = new Label {
                        Text=item.msg,
                        TextColor = ((App)Application.Current).datas.ctxt,
                        FontSize = ((App)Application.Current).datas.ttxt,
                        Padding =10
                       };
                     formmsg.Children.Add(txt);
                     if(listweb !=null){
                     foreach(var wp in listweb){
                     formmsg.Children.Add(wp);
                     }
                     }
                    }else{
                     foreach(var wp in listweb){
                     formmsg.Children.Add(wp);
                     }
                    }
                      var mepreponse=new HorizontalStackLayout{
                     
                  
                    };
                    var repondre=new Entry{
                    Placeholder="reponse",
                    WidthRequest = 300,
                    };
                    var envoiereponse=new Button{
                    Text=">",
                     WidthRequest = 40,
                     HeightRequest = 40,
                    };
                    envoiereponse.Clicked += (s, e) => {
                      Repondre(repondre.Text,item.numero);
                    };
                    mepreponse.Children.Add(repondre);
                    mepreponse.Children.Add(envoiereponse);
                    formmsg.Children.Add(mepreponse);

                var btnsuprmsg = new Button {
                Text="x",
                WidthRequest=40,
                HeightRequest=40
                };
                btnsuprmsg.Clicked += (s, e) =>
                {
                    suprmsg(item.msgid);
                };

              formmsg.Children.Add(btnsuprmsg);
             
                var bordermsg=new Frame{
                 BackgroundColor = ((App)Application.Current).datas.cback,
                 HasShadow = true
                };
                bordermsg.Content=formmsg;
                affmsg.Children.Add(bordermsg);
            }
#endif
        }

        //fonction qui verifie que l'url est fiable(pour version 1 verif limiter au lien de youtube
        public bool checkurl(string url)
        {
           bool urlsafe = false;
            string[] parturl = url.Split("//");
            string deburl=parturl[0];
            string contenturl=parturl[1];
            if (parturl.Length > 2)
            {
                contenturl = "";
                for (int i = 1; i <= parturl.Length - 1; i++)
                {
                    contenturl += parturl[i];
                }
            }
            string[] partcontent = contenturl.Split("/");
            string site=partcontent[0];
            if (site == "www.youtube.com")
            {
                urlsafe = true;
            }
            return urlsafe;
        }

        public string reccont(IEnumerable<Contact> contact,string expediteur)
        {
            var rep = "";
            var present = false;
            foreach (var cont in contact)
            {
                foreach (var num in cont.Phones)
                {
                    var nvnum = num.ToString();
                    var premchar = nvnum[0];
                    if (premchar != 0)
                    {
                        var npremchar = premchar;
                        var pindex = 0;
                        //trouver index debut puis recup num a partir de l'index
                        for (int i = 0; i <= nvnum.Length - 1; i++)
                        {
                            if (nvnum[i].ToString() == "0")
                            {
                                pindex = i;
                                break;
                            }
                        }
                        var majnum = "";
                        for (int i = pindex; i <= nvnum.Length - 1; i++)
                        {
                            majnum += nvnum[i];
                        }
                        if (majnum.ToString() == expediteur)
                        {
                            rep = cont.GivenName + " " + cont.FamilyName;
                            present = true;
                        }
                    }
                    else
                    {
                        if (num.ToString() == expediteur)
                        {
                            rep = cont.GivenName + " " + cont.FamilyName;
                            present = true;
                        }
                    }
                }
            }
            if (present == false)
            {
                rep = expediteur;
            }
            return rep;
        }
        public void suprmsg(int id)
        {
            foreach (var item in ((App)Application.Current).datas.listmsg)
            {
                if(item.msgid == id)
                {
                    ((App)Application.Current).datas.listmsg.Remove(item);
                    break;
                }
            }
            savemsg();
            affmessages(((App)Application.Current).datas.listmsg, ((App)Application.Current).datas.listmms);
        }

        public void suprmms(string id)
        {
            foreach(var item in ((App)Application.Current).datas.listmms)
            {
                if (item.Id == id)
                {
                    ((App)Application.Current).datas.listmms.Remove(item);
                    break;
                }
            }
            ((App)Application.Current).savemms();
            affmessages(((App)Application.Current).datas.listmsg, ((App)Application.Current).datas.listmms);
        }

        public void savemsg()
        {
            if (((App)Application.Current).datas.listmsg != null)
            {
                var stocktxt = "";
                var sepdom = " /sd ";
                var sepel = " /sl ";
                var num = 0;
                foreach (var dom in ((App)Application.Current).datas.listmsg)
                {
                    if (num > 0)
                    {
                        stocktxt += sepdom;
                    }
                    else
                    {
                        num += 1;
                    }
                    string msgid = dom.msgid.ToString();
                    string tid = dom.tid.ToString();
                    string nume = dom.numero.ToString();
                    string name = dom.name;
                    string date = dom.date.ToString();
                    string msg = dom.msg;
                    string type = dom.type;
                    stocktxt += msgid + sepel + tid + sepel + nume + sepel + name + sepel + date + sepel + msg + sepel + type;
                }
                Preferences.Set("listmsg", stocktxt);
            }

        }
    }

}
