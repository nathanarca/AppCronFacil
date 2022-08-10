using Plugin.VersionTracking;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCronometroFacil.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageSobre : ContentPage
	{
		public PageSobre ()
		{
			InitializeComponent ();

            labelVersao.Text = $"Versão: {CrossVersionTracking.Current.CurrentVersion}";
        }
	}
}