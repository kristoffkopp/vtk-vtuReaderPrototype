﻿using Kitware.VTK;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace VTKreaderTEST
{
	public class VTKCellDataReader
	{
		public List<double> readAllForcesBeam(vtkUnstructuredGrid unstructuredGrid) {
			CalculateMaxForcesBeam = new double[6];
			CalculateMinForcesBeam = new double[6];
			return readUnknownTuplesizeNameSpecificCellDataArray(unstructuredGrid, "AllForcesBeam");
		}
		public List<double> readAllForcesShell(vtkUnstructuredGrid unstructuredGrid)
		{
			CalculateMaxForcesShell = new double[8];
			CalculateMinForcesShell = new double[8];
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

					if (arrayName == "AllForcesBeam") //Modelo 6 iterates from 0-5 in all cell_vertexes to find the max and min forces (X,Y,Z,Mx,My,Mz)
						calculateExtremeForces(managedArray, 6, CalculateMinForcesBeam, CalculateMaxForcesBeam);

					if (arrayName == "AllForcesShell") //Modelo 8 iterates from 0-7 in all cell_vertexes to find the max and min value (Nx,Ny,Nz,Mx,My,Mz,Vzx,Vzy)
						calculateExtremeForces(managedArray, 8, CalculateMinForcesShell, CalculateMaxForcesShell);
				}
			}
			return dataArray;
		}

		private void calculateExtremeForces(double[] managedArray, int numberOfForces, double[] calculateMin, double[] calculateMax)
		{
			for (int k = 0; k < managedArray.Length; k++)
			{
				if (managedArray[k] > calculateMax[k % numberOfForces])
					calculateMax[k % numberOfForces] = managedArray[k];

				if (managedArray[k] < calculateMin[k % numberOfForces] || calculateMin[k % numberOfForces] == 0)
					calculateMin[k % numberOfForces] = managedArray[k];
			}
		}

		public double[] CalculateMaxForcesBeam { get; private set; }
		public double[] CalculateMinForcesBeam { get; private set; }
		public double[] CalculateMaxForcesShell { get; private set; }
		public double[] CalculateMinForcesShell { get; private set; }
	}
}