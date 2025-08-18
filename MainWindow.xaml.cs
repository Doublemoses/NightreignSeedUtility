using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NightreignSeedUtility;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    SeedUtility seedUtility;

    public MainWindow()
    {
        InitializeComponent();
        seedUtility = new SeedUtility();
        InitUI();
    }

    private void InitUI()
    {
        listviewStoreItems.ItemsSource = seedUtility.storeItems;
        FilterStoreItems();
    }

    private void Button_GetStore(object sender, RoutedEventArgs e)
    {
        //MessageBox.Show(seedUtility.GetStoreFromSeed(0x150CB06E).ToString());

        if (textboxSeed.Text.Length == 0)
            return;

        uint storeId = 0;

        try
        {
            storeId = seedUtility.GetStoreFromSeed(Convert.ToUInt32(textboxSeed.Text, 16));
        }
        catch
        {
            MessageBox.Show("Invalid data in seed textbox.");
            return;
        }
        
        FilterStoreItems(storeId);

        textShopId.Text = "Store Id: " + storeId.ToString();
    }

    private void FilterStoreItems(uint storeId = 10000)
    {
        ICollectionView view = CollectionViewSource.GetDefaultView(seedUtility.storeItems);

        view.Filter = (entry) =>
        {
            StoreItem item = (StoreItem)entry;
            uint general = 0;

            switch (storeId - 90000)
            {
                case 1000:
                    break;
                case < 7:
                    general = 100;
                    break;
                case < 14:
                    general = 200;
                    break;
                case < 21:
                    general = 300;
                    break;
            }

            return item.Store == storeId || item.Store == general;
        };
    }

    private void TextBox_VerifySeed(object sender, TextCompositionEventArgs e)
    {
        foreach (char c in e.Text)
        {
            if (!char.IsAsciiHexDigit(c))
            {
                e.Handled = true;
                break;
            }
        }
    }
}