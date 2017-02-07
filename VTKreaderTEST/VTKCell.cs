using System.Collections.Generic;

namespace WriteReaderTEST
{
	public class VTKCell
	{
		public VTKCell(int cellType, List<int> vtkPointsIds)
		{
			CellType = cellType;
			VtkPointIds = vtkPointsIds;
		}

		public int CellType { get; private set; }
		public List<int> VtkPointIds { get; private set; }
	}
}