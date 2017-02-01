using Kitware.VTK;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Documents;

namespace VTKreaderTEST
{
	public class VTKCellDataReader
	{
		public List<double> readAllForcesBeam(vtkUnstructuredGrid unstructuredGrid) {
			return readUnknownTuplesizeNameSpecificCellDataArray(unstructuredGrid, "AllForcesBeam");
		}
		public List<double> readAllForcesShell(vtkUnstructuredGrid unstructuredGrid)
		{
			return readUnknownTuplesizeNameSpecificCellDataArray(unstructuredGrid, "AllForcesShell");
		}
		private List<double> readUnknownTuplesizeNameSpecificCellDataArray(vtkUnstructuredGrid unstructuredGrid, string arrayName)
		{
			var dataArray = new List<double>();
			var cellData = unstructuredGrid.GetCellData();
			for (int i = 0; i < cellData.GetNumberOfArrays(); i++)
			{
				if (cellData.GetArrayName(i) != arrayName)
					continue;

				var numbComp = cellData.GetArray(i).GetNumberOfComponents();
				for (int j = 0; j < cellData.GetArray(i).GetNumberOfTuples(); j++)
				{
					var tempTuple = cellData.GetArray(i).GetTuple(j);
					double[] managedArray = new double[numbComp];
					Marshal.Copy(tempTuple, managedArray, 0, numbComp);
					dataArray.AddRange(managedArray);
				}
			}
			return dataArray;
		}
	}
}