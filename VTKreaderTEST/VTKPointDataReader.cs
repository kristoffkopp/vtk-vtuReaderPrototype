using Kitware.VTK;
using System.Collections.Generic;

namespace VTKreaderTEST
{
	public class VTKPointDataReader
	{
		public List<double[]> readTranslation(vtkUnstructuredGrid unstructuredGrid)
		{
			return readTuple3NameSpecificPointDataArray(unstructuredGrid, "Translation", true);
		}
		public List<double[]> readRotationVectors(vtkUnstructuredGrid unstructuredGrid)
		{
			return readTuple3NameSpecificPointDataArray(unstructuredGrid, "RotationVector", false);
		}

		private List<double[]> readTuple3NameSpecificPointDataArray(vtkUnstructuredGrid unstructuredGrid, string dataArrayName, bool readExtremeValues)
		{
			var dataList = new List<double[]>();
			var pointData = unstructuredGrid.GetPointData();
			this.ExtremeDisplacement = new double[] { 0.0, 0.0, 0.0 };
			for (int i = 0; i < pointData.GetNumberOfArrays(); i++)
			{
				if (pointData.GetArrayName(i) != dataArrayName)
					continue;

				for (int j = 0; j < pointData.GetArray(i).GetNumberOfTuples(); j++)
				{
					var tuple = pointData.GetArray(i).GetTuple3(j);
					dataList.Add(tuple);
					if (!readExtremeValues)
						continue;

					for (int k = 0; k < 3; k++)
					{
						if (tuple[k] > this.ExtremeDisplacement[k])
							this.ExtremeDisplacement[k] = tuple[k];
					}
				}
			}
			return dataList;
		}

		public double[] ExtremeDisplacement { get; private set; }
	}
}