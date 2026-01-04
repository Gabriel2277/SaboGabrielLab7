using SaboGabrielLab7.Models;
namespace SaboGabrielLab7;

public partial class ListPage : ContentPage
{
	public ListPage()
	{
		InitializeComponent();
	}
    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        slist.Date = DateTime.UtcNow;
        await App.Database.SaveShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        await App.Database.DeleteShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductPage((ShopList)this.BindingContext)
        {
            BindingContext = new Product()
        });
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var shopl = (ShopList)BindingContext;

        // Asigura-te c? shopl.ID nu este 0 (vezi sectiunea de mai jos)
        if (shopl.ID != 0)
        {
            listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
        }
    }
    async void OnDeleteItemButtonClicked(object sender, EventArgs e)
    {
        var selectedProduct = listView.SelectedItem as Product;

        if (selectedProduct != null)
        {
            var shopl = (ShopList)BindingContext;

            await App.Database.DeleteListProductAsync(
                shopl.ID,
                selectedProduct.ID
            );

            listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
            listView.SelectedItem = null;
        }
        else
        {
            await DisplayAlert(
                "Selection Required",
                "Please select a product from the list to delete it.",
                "OK"
            );
        }
    }

}
