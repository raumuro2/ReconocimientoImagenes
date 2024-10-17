using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using ReconocimientoImagenes;

namespace ReconocimientoImagenes
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string imagePath = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private static ReconocimientoModel.ModelOutput getPrediccion()
        {
            var imageBytes = File.ReadAllBytes(imagePath);
            ReconocimientoModel.ModelInput sampleData = new ReconocimientoModel.ModelInput()
            {
                ImageSource = imageBytes,
            };

            var result = ReconocimientoModel.Predict(sampleData);

            return result;
        }
        private void btn_flecha_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (imagePath != "")
            {
                var prediccion = getPrediccion();
                int score = 0;
                string result = "gato";
                if (prediccion.Score[1] > 0.5) score = 1;
                if (prediccion.PredictedLabel == "dogs") result = "perro";
                txt_result.Text = "Es un " + result + " al " + prediccion.Score[score]*100 + "%";
            }
            else
            {
                MessageBox.Show("Debes de seleccionar una imagen");
            }
        }

        private void btn_subirImagen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp|Todos los archivos|*.*";

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                imagePath = openFileDialog.FileName;

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagePath);
                bitmap.EndInit();

                imageControl.Source = bitmap;
                txt_result.Text = "";
            }
        }
    }
}
