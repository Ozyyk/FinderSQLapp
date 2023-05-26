using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;

namespace Vyhledavac01
{
	public partial class MainWindow : Window
	{
		private const string ConnectionString = "Data Source=LAPTOP-TKUMLAS3\\SQLEXPRESS;Initial Catalog=StwPh01_12345678_2023;User ID=LAPTOP-TKUMLAS3\\beloh;Integrated Security=True";

		public MainWindow()
		{
			InitializeComponent();
			LoadData();
			textbox.TextChanged += Textbox_TextChanged; 
		}

		private void LoadData()
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(ConnectionString))
				{
					string searchTerm = textbox.Text;
					string query = GetSqlQuery();

					using (SqlCommand command = new SqlCommand(query, connection))
					{
						command.Parameters.AddWithValue("@hledanyVyraz", searchTerm);

						connection.Open();

						SqlDataAdapter adapter = new SqlDataAdapter(command);
						DataTable dataTable = new DataTable();
						adapter.Fill(dataTable);

						datagrid.ItemsSource = dataTable.DefaultView;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Chyba při načítání dat: " + ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private string GetSqlQuery()
		{
			return @"
                select 
                DB_NAME() as 'účetní jednotka',
                fapol.stext,
                ISNULL(fapol.pozn,'-') as 'Položka pozn',
                ISNULL(fapol.VCislo,'-') as 'Výrobní číslo',

                case 
                WHEN fa.RelTpFak = 11 THEN 'Faktury přijaté'
                WHEN fa.RelTpFak = 1 THEN 'Faktury vydané'
                ELSE 'nevím' END as 'Typ dokladu',

                fa.cislo,
                ISNULL(fa.Firma,'-'),
                fa.Datum,
                ISNULL(fa.Pozn,'-') as 'FA pozn',
                ISNULL(fa.pozn2,'-') as 'FA pozn 2'

                from fapol
                left join fa on fapol.refag = fa.id

                WHERE FA.RelTpFak IN (1,11)
                AND
                (fapol.stext like '%' + @hledanyVyraz + '%'
                OR fapol.pozn like '%' + @hledanyVyraz + '%'
                or fapol.VCislo like '%' + @hledanyVyraz + '%'
                or fa.Pozn like '%' + @hledanyVyraz + '%'
                or fa.pozn2 like '%' + @hledanyVyraz + '%') ";

		}

		private void Textbox_TextChanged(object sender, TextChangedEventArgs e)
		{
			LoadData();
		}

        bool rezim = true;

        string colorss1 = "#505050";
        string colorss2 = "#8F8F8F";


        private void dark_light_BUTTON_Click(object sender, RoutedEventArgs e)
        {
			if (rezim) //dark
			{
				Color dark__gray = (Color)ColorConverter.ConvertFromString(colorss1);
				Color dark__gray2 = (Color)ColorConverter.ConvertFromString(colorss2);



				dark_light_BUTTON.Background = new SolidColorBrush(Colors.White);
				dark_light_BUTTON.Foreground = new SolidColorBrush(dark__gray);
				main_widow_colorssss.Background = new SolidColorBrush(dark__gray2);
				Nazev_programu.Foreground = new SolidColorBrush(Colors.White);
				textbox.Background = new SolidColorBrush(Colors.White);
				textbox.Foreground = new SolidColorBrush(Colors.Black);
				textbox.BorderBrush = new SolidColorBrush(Colors.Black);
				datagrid.Background = new SolidColorBrush(dark__gray);
				datagrid.RowBackground = new SolidColorBrush(Colors.White);
				datagrid.AlternatingRowBackground = new SolidColorBrush(Colors.DarkGray);
				datagrid.Foreground = new SolidColorBrush(Colors.Black);
				dark_light_BUTTON.Content = "Light";
				rezim = false;
			}
			else //light
			{
				dark_light_BUTTON.Background = new SolidColorBrush(Colors.White);
				dark_light_BUTTON.Foreground = new SolidColorBrush(Colors.Black);
				main_widow_colorssss.Background = new SolidColorBrush(Colors.White);
				Nazev_programu.Foreground = new SolidColorBrush(Colors.Black);
				textbox.Background = new SolidColorBrush(Colors.White);
				textbox.Foreground = new SolidColorBrush(Colors.Black);
				textbox.BorderBrush = new SolidColorBrush(Colors.DarkGray);
				datagrid.Background = new SolidColorBrush(Colors.White);
				datagrid.RowBackground = new SolidColorBrush(Colors.White);
				datagrid.AlternatingRowBackground = new SolidColorBrush(Colors.DarkGray);
				datagrid.Foreground = new SolidColorBrush(Colors.Black);
				dark_light_BUTTON.Content = "Dark";
				rezim = true;
			}



		}
    }
}