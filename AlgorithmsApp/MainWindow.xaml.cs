using Algorithms;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
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

namespace AlgorithmsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Solve_Click(object sender, RoutedEventArgs e)
        {
            var pso = new PSO(numParticles: int.Parse(numParticlestxt.Text),
                                maxIterations: int.Parse(maxIterationstxt.Text),
                                inertia: double.Parse(inertiatxt.Text), 
                                cognitive: double.Parse(cognitivetxt.Text),
                                social: double.Parse(socialtxt.Text));

            var x = int.Parse(Xtxt.Text);

            var numDimension = int.Parse(numDimensionstxt.Text);

            var xmin = new double[numDimension];
            
            var xmax = new double[numDimension];

            for (int i = 0; i < numDimension; i++)
            {
                xmin[i] = -x;
                xmax[i] = +x;
            }

            var result = pso.Solve(fitnessFunction: x => x.Sum(xi => xi * xi), numDimensions: numDimension, minX: xmin, maxX: xmax);

            Console.WriteLine($"Best solution found: ({string.Join(", ", result.Select(x => x.ToString()))})");

            //var plotModel = new PlotModel();
            //plotModel.Series.Add(new LineSeries
            //{
            //    Title = "Best Positions",
            //    ItemsSource = ys.Select((y, i) => new DataPoint(xs[i], y))
            //});

            //plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X" });
            //plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y" });

        }
    }
}