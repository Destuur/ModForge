using KCD2.ModForge.Shared.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.ModForge.Shared.Factories
{
	public static class DataPointFactory
	{
		public static IDataPoint CreateDataPoint(string path, string endpoint, Type type)
		{
			return new DataPoint(path, endpoint, type);
		}
	}
}
