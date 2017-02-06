using Kitware.VTK;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace VTKreaderTEST
{
	public class VTKCellDataReader
	{
		public List<double> readAllForcesBeam(vtkUnstructuredGrid unstructuredGrid) {
			MaxForcesBeam = new double[6];
			MinForcesBeam = new double[6];
			return readUnknownTuplesizeNameSpecificCellDataArray(unstructuredGrid, "AllForcesBeam");
		}
		public List<double> readAllForcesShell(vtkUnstructuredGrid unstructuredGrid)
		{
			MaxForcesShell = new double[8];
			MinForcesShell = new double[8];
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

					if (arrayName == "AllForcesBeam")
					{
						for (int k = 0; k < managedArray.Length; k++)
						{
							if (managedArray[k] > MaxForcesBeam[k % 6]) //Modelo 6 iterates from 0-5 in all cell_vertexes to find the max and min forces (X,Y,Z,Mx,My,Mz)
								MaxForcesBeam[k % 6] = managedArray[k];

							if (managedArray[k] < MinForcesBeam[k % 6] || MinForcesBeam[k % 6] == 0) 
								MinForcesBeam[k % 6] = managedArray[k];
						}
					}
					if (arrayName == "AllForcesShell")
					{
						for (int k = 0; k < managedArray.Length; k++)
						{
							if (managedArray[k] > MaxForcesShell[k % 8]) //Modelo 8 iterates from 0-7 in all cell_vertexes to find the max and min value (Nx,Ny,Nz,Mx,My,Mz,Vzx,Vzy)
								MaxForcesShell[k % 8] = managedArray[k];

							if (managedArray[k] < MinForcesShell[k % 8] || MinForcesShell[k % 8] == 0) 
								MinForcesShell[k % 8] = managedArray[k];
						}
					}
				}
			}
			return dataArray;
		}
		public double[] MaxForcesBeam { get; private set; }
		public double[] MinForcesBeam { get; private set; }
		public double[] MaxForcesShell { get; private set; }
		public double[] MinForcesShell { get; private set; }
	}
}