using Microsoft.Extensions.Logging;
using ModForge.Shared.Converter;
using ModForge.Shared.Factories;
using ModForge.Shared.Models.Abstractions;
using ModForge.Shared.Models.STORM;
using System.Runtime.CompilerServices;

namespace ModForge.Shared.Services
{
	public class StormService
	{
		private readonly ILogger<StormService> logger;
		private readonly UserConfigurationService userConfigurationService;
		private IDataPoint rootDataPoint;
		private Storm stormRoot;
		private List<StormDto> stormDtos = [];

		public StormService(ILogger<StormService> logger, UserConfigurationService userConfigurationService)
		{
			this.logger = logger;
			this.userConfigurationService = userConfigurationService;
			InitializeStormService();
		}

		public List<StormDto> GetStormDtos()
		{
			return stormDtos;
		}

		public StormDto GetStormDto(string id)
		{
			var stormDto = stormDtos.FirstOrDefault(x => x.Id == id);

			if (stormDto == null)
			{
				stormDto = new StormDto();
			}

			return stormDto.ReadStormFile();
		}

		private void InitializeStormService()
		{
			GetRootDataPoint();
			GetStormRoot();
			GetStormDataPoints();
		}

		private void GetStormDataPoints()
		{
			if (stormRoot == null)
			{
				return;
			}

			foreach (var source in stormRoot.Common.Sources)
			{
				stormDtos.Add(source.GetInitialStormDto(rootDataPoint.Path));
			}

			foreach (var task in stormRoot.Tasks)
			{
				foreach (var source in task.Sources)
				{
					stormDtos.Add(source.GetInitialStormDto(rootDataPoint.Path, task.Name));
				}
			}
		}

		private void GetStormRoot()
		{
			using var pakReader = new PakReader(rootDataPoint.Path);

			var rootPath = rootDataPoint.Endpoint.Replace('\\', '/');
			var rootDirectory = Path.GetDirectoryName(rootPath)?.Replace('\\', '/');

			stormRoot = pakReader.ReadStorm(rootPath);
		}

		private void GetRootDataPoint()
		{
			try
			{
				foreach (var endpoint in ToolResources.Keys.Endpoints().Where(x => x.Key == typeof(Storm)))
				{
					foreach (var kv in endpoint.Value)
					{
						var fullPath = Path.Combine(userConfigurationService.Current.GameDirectory, kv.Value);
						var dataPoint = DataPointFactory.CreateDataPoint(fullPath, kv.Key, endpoint.Key);
						if (dataPoint != null)
						{
							rootDataPoint = dataPoint;
						}
						else
						{
							logger.LogWarning("Failed to create data point for path: {Path}, key: {Key}, type: {Type}", fullPath, kv.Key, endpoint.Key);
						}
					}
				}
				logger.LogInformation("Data point collected: {Count}", rootDataPoint.Endpoint);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Failed to get data points.");
			}
		}

		//private IList<Storm> ImportStormFiles(StormDto stormDto)
		//{
		//	var stormFiles = new List<Storm>();
		//	var dataPoint = stormDto.DataPoint;

		//	if (dataPoint == null)
		//		return null;

		//	using var pakReader = new PakReader(dataPoint.Path);

		//	var rootPath = dataPoint.Endpoint.Replace('\\', '/');
		//	var rootDirectory = Path.GetDirectoryName(rootPath)?.Replace('\\', '/');

		//	var stormRoot = pakReader.ReadStorm(rootPath);

		//	if (stormRoot != null)
		//	{
		//		IEnumerable<string> ResolvePaths(IEnumerable<Source> sources)
		//		{
		//			foreach (var source in sources)
		//			{
		//				var combined = string.IsNullOrEmpty(rootDirectory)
		//					? source.Path.Replace('\\', '/')
		//					: $"{rootDirectory}/{source.Path.Replace('\\', '/')}";
		//				yield return combined;
		//			}
		//		}

		//		// Aus Common Sources
		//		foreach (var resolvedPath in ResolvePaths(stormRoot.Common.Sources))
		//		{
		//			var childStorm = pakReader.ReadStorm(resolvedPath);
		//			if (childStorm != null)
		//				stormFiles.Add(childStorm);
		//		}

		//		// Aus Tasks
		//		foreach (var task in stormRoot.Tasks)
		//		{
		//			foreach (var resolvedPath in ResolvePaths(task.Sources))
		//			{
		//				var childStorm = pakReader.ReadStorm(resolvedPath);
		//				if (childStorm != null)
		//					stormFiles.Add(childStorm);
		//			}
		//		}
		//	}
		//	else
		//	{
		//		logger.LogWarning("No storm file found for data point: {DataPoint}", dataPoint);
		//	}

		//	return stormFiles;
		//}
	}
}
