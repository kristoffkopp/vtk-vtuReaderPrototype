using Kitware.VTK;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace VTKreaderTEST
{
	public class VTKCellDataReader
	{
		public List<double> readAllForcesBeam(vtkUnstructuredGrid unstructuredGrid) {
			return readUnknownTuplesizeNameSpecificCellDataArray(unstructuredGrid, "AllForcesBeam", true);
		}
		public List<double> readAllForcesShell(vtkUnstructuredGrid unstructuredGrid)
		{
			return readUnknownTuplesizeNameSpecificCellDataArray(unstructuredGrid, "AllForcesShell", false);
		}
		private List<double> readUnknownTuplesizeNameSpecificCellDataArray(vtkUnstructuredGrid unstructuredGrid, string arrayName, bool readMinAndMaxValues)
		{
			var dataArray = new List<double>();
			var cellData = unstructuredGrid.GetCellData();
			MaxForcesBeam = new double[6];
			MinForcesBeam = new double[6];
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

					if (!readMinAndMaxValues)
						continue;

					for (int k = 0; k < managedArray.Length; k++)
					{
						if (managedArray[k] > MaxForcesBeam[k % 6]) //Modelo 6 will give position 0-5 in MaxForcesBeam two times reading through a array of 12 elements
							MaxForcesBeam[k % 6] = managedArray[k];

						if (managedArray[k] < MinForcesBeam[k % 6] || MinForcesBeam[k % 6] == 0) //Modelo 6 will give position 0-5 in MaxForcesBeam two times reading through a array of 12 elements
							MinForcesBeam[k % 6] = managedArray[k];
					}
				}
			}
			return dataArray;
		}
		public double[] MaxForcesBeam { get; private set; }
		public double[] MinForcesBeam { get; private set; }
	}
}