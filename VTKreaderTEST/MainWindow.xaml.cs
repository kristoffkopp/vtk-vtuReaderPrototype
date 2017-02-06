using System;
using System.Windows;
using System.Windows.Media.Media3D;

namespace VTKreaderTEST
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			try { 
				RedirectVTKOutput errorObserver = new RedirectVTKOutput();

				VTKreader reader = new VTKreader(@"C:\Users\Kristoffer\Dropbox\Dokumenter\Masteroppgave - Spring 2017\TDD vtk-files\TESTTESTTESTTEST.vtu");
				var unstructuredGrid = reader.readFile();

				VTKPointReader pointReader = new VTKPointReader();
				var vectorPoints = pointReader.pointReader(unstructuredGrid);

				VTKPointDataReader pointDataReader = new VTKPointDataReader();
				//translation and rotationVector can be written as Vector3D - since cfemWrapper is using UVector3D to read translation AND rotation. Is now a double[,]
				var translation = pointDataReader.readTranslation(unstructuredGrid);
				var rotationVector = pointDataReader.readRotationVectors(unstructuredGrid);
				var extremeDisplacement = pointDataReader.ExtremeDisplacement;

				VTKCellReader cellReader = new VTKCellReader();
				var vtkCells = cellReader.readCells(unstructuredGrid);

				VTKCellDataReader cellDataReader = new VTKCellDataReader();
				var allForcesBeams = cellDataReader.readAllForcesBeam(unstructuredGrid);
				var allFourcesShell = cellDataReader.readAllForcesShell(unstructuredGrid);
				var maxForcesBeam = cellDataReader.MaxForcesBeam;
				var minForcesBeam = cellDataReader.MinForcesBeam;
				var maxForceShell = cellDataReader.MaxForcesShell;
				var minForcesShell = cellDataReader.MinForcesShell;

				VTKFieldDataReader fieldDataReader = new VTKFieldDataReader();
				fieldDataReader.readFieldData(unstructuredGrid);

				ElementReader elementReader = new ElementReader();
				var elements = elementReader.readCellsAndPoints(vtkCells, vectorPoints);

				foreach (Element element in elements)
					printToTextBox(element);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		
		private void printToTextBox(Element element)
		{
			richTextBox.AppendText("Element type: " + element.CellType.ToString());
			foreach(Vector3D vector in element.VectorList)
			{
				richTextBox.AppendText(System.Environment.NewLine);
				richTextBox.AppendText("Coordinates: " + vector.ToString());
			}
			richTextBox.AppendText(System.Environment.NewLine);
			richTextBox.AppendText(System.Environment.NewLine);
		}
	}
}