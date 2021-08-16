using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RandomizerVY
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPage : ContentPage
    {
        public ListPage()
        {
            InitializeComponent();

            List<string> list = new List<string>();
            for (int i = 0; i < 10; i++)
                list.Add($"Элемент списка номер {i}");

            foreach (string str in list)
            {
                flexLayout
                    .Children
                    .Add(new Entry
                    {
                        Text = str,
                        HorizontalOptions = LayoutOptions.Center,
                        WidthRequest = 300,
                        HorizontalTextAlignment = TextAlignment.Center
                    });
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Random rnd = new Random();
            List<string> list = new List<string>();

            foreach (View child in flexLayout.Children)
                if (child is Entry)
                    list.Add((child as Entry).Text);

            string answer = list[rnd.Next(0, list.Count)];

            DisplayAlert("Результат", answer, "Ок");
        }
    }
}