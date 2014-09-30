using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
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
using System.IO;
using MapExport;
using System.Diagnostics;

namespace MCMapExporter
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const string MapImportDirectory = @"A:\Code\MCMaps";
		private const string MapExportDirectory = @"A:\Code\MapsExport";
		private readonly BackgroundWorker worker = new BackgroundWorker();
		private readonly Stopwatch exportTimer = new Stopwatch();
		private string map = "";

		public MainWindow()
		{
			InitializeComponent();
			FillMapList();
			worker.DoWork += worker_DoWork;
			worker.RunWorkerCompleted += worker_RunWorkerCompleted; 
		}

		private void ExportButton_Click(object sender, RoutedEventArgs e)
		{
			if (MapsList.SelectedItem != null)
			{
				map = MapsList.SelectedItem.ToString();
				exportTimer.Reset();
				exportTimer.Start();
				worker.RunWorkerAsync(map);
				StatusBox.Text += "Exporting : " + map + "...\n";
			}
		}

		private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			exportTimer.Stop();
			StatusBox.Text += "Export of " + map + " finished after " + exportTimer.ElapsedMilliseconds / 1000 + " seconds.\n";
		}

		private void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			exportMap(e.Argument.ToString());
		}

		private void FillMapList()
		{
			var maps = MapExportUtils.GetExportableMaps(MapImportDirectory);
			maps.ForEach(m => MapsList.Items.Add(m));
		}

		private void exportMap(string map)
		{
				//StatusBox.Text += "Exporting " + map;
				MapExportUtils.ExportMap(map, MapImportDirectory, MapExportDirectory);
		}
	}
}
