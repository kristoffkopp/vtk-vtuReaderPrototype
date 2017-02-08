using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace VTKreadTEST
{
	public class Element
	{
		public Element(int cellType, List<Vector3D> vectorList)
		{
			CellType = cellType;
			VectorList = vectorList;
		}
		public int CellType { get; private set; }
		public List<Vector3D> VectorList { get; private set; }
	}
}