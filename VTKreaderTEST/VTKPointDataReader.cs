using Kitware.VTK;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace VTKreaderTEST
{
	public class VTKPointDataReader
	{
		public List<double> readDisplacements(vtkUnstructuredGrid unstructuredGrid)
		{
			return readTuple3NameSpecificPointDataArray(unstructuredGrid, "Displacement");
		}
		public List<double> readTranslation(vtkUnstructuredGrid unstructuredGrid)
		{
			return readTuple3NameSpecificPointDataArray(unstructuredGrid, "Translation");
		}
		public List<double> readRotationVectors(vtkUnstructuredGrid unstructuredGrid)
		{
			return readTuple3NameSpecificPointDataArray(unstructuredGrid, "RotationVector");
		}
		public List<double> readExtremeDisplacement(vtkUnstructuredGrid unstructuredGrid)
		{
			vtkFieldData fieldData = new vtkFieldData();
			vtkDoubleArray array = new vtkDoubleArray();
			array.SetNumberOfComponents(3);
			var a= array.InsertNextValue(3.1);
			var aa = array.InsertNextValue(4.2);
			var aaa = array.InsertNextValue(5.3);
			array.SetName("ExtremeDisplacement");
			fieldData.AddArray(array);
			unstructuredGrid.SetFieldData(fieldData);
			vtkXMLUnstructuredGridWriter writer = vtkXMLUnstructuredGridWriter.New();
			writer.SetFileName(@"C:\Users\Kristoffer\Dropbox\Dokumenter\Masteroppgave - Spring 2017\TDD vtk-files\TESTTESTTESTTEST.vtu");
			writer.SetInput(unstructuredGrid);
			writer.SetDataModeToAscii();
			writer.Write();
			return readTuple3NameSpecificPointDataArray(unstructuredGrid, "ExtremeDisplacement");
		}

		private List<double> readTuple3NameSpecificPointDataArray(vtkUnstructuredGrid unstructuredGrid, string dataArrayName)
		{
			var dataList = new List<double>();
			var pointData = unstructuredGrid.GetPointData();
			for (int i = 0; i < pointData.GetNumberOfArrays(); i++)
			{
				if (pointData.GetArrayName(i) != dataArrayName)
					continue;

				for (int j = 0; j < pointData.GetArray(i).GetNumberOfTuples(); j++)
					dataList.AddRange(pointData.GetArray(i).GetTuple3(j));
			}
			return dataList;
		}
	}
}