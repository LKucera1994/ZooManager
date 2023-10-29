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
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ZooManagerWithSql
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;
        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["ZooManagerWithSql.Properties.Settings.TESTDBConnectionString"].ConnectionString;

            sqlConnection = new SqlConnection(connectionString);

            ShowZoos();
            ShowAllAnimals();

            
        }
        private void ShowZoos()
        {
            try
            {
                string querry = "select * from Zoo";
                SqlDataAdapter adapter = new SqlDataAdapter(querry, sqlConnection);

                using (adapter)
                {
                    DataTable zooTable = new DataTable();
                    adapter.Fill(zooTable);
                    //Which information of the table in Datatable should be shown in our listbox? 
                    listZoos.DisplayMemberPath = "Location";

                    //Which value should be delivered, when an Item from our Listbox is selected?
                    listZoos.SelectedValuePath = "Id";

                    //the reference to the Data the Listbox should populate
                    listZoos.ItemsSource = zooTable.DefaultView;

                }

            }

            catch(Exception e)
            {
                //Show generic Error when something goes wrong
                MessageBox.Show(e.ToString());
            }



           

        }

        private void listZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowAssociatedAnimals();
            ShowSelectedZooInTextbox();

        }
        private void ShowAssociatedAnimals()
        {
            try
            {
                string querry = "select * from Animal a inner join ZooAnimal za on a.Id = za.AnimalId where za.ZooId = @ZooId";     
                SqlCommand sqlCommand = new SqlCommand(querry,sqlConnection);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

                using (adapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooID",listZoos.SelectedValue);

                    DataTable animalTable = new DataTable();

                    adapter.Fill(animalTable);

                    //Which information of the table in Datatable should be shown in our listbox? 
                    listAssociatedAnimals.DisplayMemberPath = "Name";

                    //Which value should be delivered, when an Item from our Listbox is selected?
                    listAssociatedAnimals.SelectedValuePath = "Id";

                    //the reference to the Data the Listbox should populate
                    listAssociatedAnimals.ItemsSource = animalTable.DefaultView;
                }

            }
            catch (Exception e)
            {
                //Show generic Error when something goes wrong
                MessageBox.Show(e.ToString());
            }

        }

        private void ShowAllAnimals()
        {
            try
            {
                string querry = "select * from Animal";
                SqlDataAdapter adapter = new SqlDataAdapter(querry,sqlConnection);
                using(adapter)
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    listAllAnimals.DisplayMemberPath = "Name";
                    listAllAnimals.SelectedValuePath="Id";
                    listAllAnimals.ItemsSource=dataTable.DefaultView;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }

        private void DeleteZoo_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string querry = "delete from Zoo where id = @ZooId";
                SqlCommand sqlcommand = new SqlCommand(querry, sqlConnection);
                sqlConnection.Open();
                sqlcommand.Parameters.AddWithValue("ZooId", listZoos.SelectedValue);

                sqlcommand.ExecuteScalar();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                

            }
            finally
            {
                sqlConnection.Close();

                ShowZoos();

            }


            

            
            

        }
        private void AddZoo_Click (object sender, RoutedEventArgs e)
        {
            try
            {
                string querry = "insert into Zoo values (@Location)";
                SqlCommand sqlcommand = new SqlCommand(querry, sqlConnection);
                sqlConnection.Open();

                sqlcommand.Parameters.AddWithValue("@Location", myTextBox.Text);
                sqlcommand.ExecuteScalar();


            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
            }
        }

        private void AddAnimalToZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string querry = "insert into ZooAnimal values (@ZooID, @AnimalId)";
                SqlCommand sqlcommand = new SqlCommand(querry, sqlConnection);
                sqlConnection.Open();
                sqlcommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlcommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);
                sqlcommand.ExecuteScalar();


            }

            catch (Exception ex)
            {
               // MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
                ShowAssociatedAnimals();
            }

        }

        private void RemoveAnimalFromZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string querry = "delete from ZooAnimal where id = @AnimalId";
                SqlCommand sqlcommand = new SqlCommand(querry, sqlConnection);
                sqlConnection.Open();
                sqlcommand.Parameters.AddWithValue("AnimalId", listAssociatedAnimals.SelectedValue);

                sqlcommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());


            }
            finally
            {
                sqlConnection.Close();

                ShowZoos();
                ShowAssociatedAnimals();

            }

        }

        private void DeleteAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string querry = "delete from Animal where id = @AnimalId";
                SqlCommand sqlcommand = new SqlCommand(querry, sqlConnection);
                sqlConnection.Open();
                sqlcommand.Parameters.AddWithValue("AnimalId", listAllAnimals.SelectedValue);

                sqlcommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());


            }
            finally
            {
                sqlConnection.Close();

                ShowZoos();
                ShowAllAnimals();
                ShowAssociatedAnimals();

            }

        }
        private void AddAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string querry = "insert into Animal values (@Name)";
                SqlCommand sqlcommand = new SqlCommand(querry, sqlConnection);
                sqlConnection.Open();

                sqlcommand.Parameters.AddWithValue("@Name", myTextBox.Text);
                sqlcommand.ExecuteScalar();


            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
                ShowAllAnimals();
            }
        }

        private void ShowSelectedZooInTextbox()
        {
            try
            {
                string querry = "select Location from Zoo where ID =@ZooId";
                SqlCommand sqlCommand = new SqlCommand(querry, sqlConnection);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

                using (adapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooID", listZoos.SelectedValue);

                    DataTable ZooDataTable = new DataTable();

                    adapter.Fill(ZooDataTable);

                    //ZooDatatable has one entry only 
                    myTextBox.Text = ZooDataTable.Rows[0]["Location"].ToString();
                }

            }
            catch (Exception ex)
            {
                //Show generic Error when something goes wrong
                MessageBox.Show(ex.ToString());
            }

        }
        private void ShowSelectedAnimalInTextbox()
        {
            try
            {
                string querry = "select Name from Animal where ID= @AnimalId";
                SqlCommand sqlCommand = new SqlCommand(querry, sqlConnection);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

                using (adapter)
                {
                    sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);

                    DataTable AnimalDataTable = new DataTable();

                    adapter.Fill(AnimalDataTable);

                    //ZooDatatable has one entry only 
                    myTextBox.Text = AnimalDataTable.Rows[0]["Name"].ToString();
                }

            }
            catch (Exception ex)
            {
                //Show generic Error when something goes wrong
                MessageBox.Show(ex.ToString());
            }

        }

        private void listAllAnimals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowSelectedAnimalInTextbox();
        }

        private void UpdateZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string querry = "update Zoo Set Location = @Location where Id= @ZooId";
                SqlCommand sqlcommand = new SqlCommand(querry, sqlConnection);
                sqlConnection.Open();
                sqlcommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlcommand.Parameters.AddWithValue("@Location", myTextBox.Text);
                sqlcommand.ExecuteScalar();


            }

            catch (Exception ex)
            {
                /// MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
                
            }

        }

        private void UpdateAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string querry = "update Animal Set Name = @Name where Id= @AnimalId";
                SqlCommand sqlcommand = new SqlCommand(querry, sqlConnection);
                sqlConnection.Open();
                sqlcommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);
                sqlcommand.Parameters.AddWithValue("@Name", myTextBox.Text);
                sqlcommand.ExecuteScalar();


            }

            catch (Exception ex)
            {
                /// MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAllAnimals();

            }

        }


    }
}
