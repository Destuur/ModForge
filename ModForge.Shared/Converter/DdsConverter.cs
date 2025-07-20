using Pfim;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Imaging;

namespace ModForge.Shared.Converter
{
	public sealed class DdsConverter : IDisposable
	{
		private readonly BlockingCollection<string> _images = new();
		private readonly Task _processTask;

		public DdsConverter() => _processTask = Task.Run(ProcessLoop);

		public void Add(string path) => _images.Add(path ?? throw new ArgumentNullException(nameof(path)));

		private void ProcessLoop()
		{
			List<Exception> conversionErrors = new();
			foreach (var path in _images.GetConsumingEnumerable())
			{
				try
				{
					Convert(path);
					Console.WriteLine("Converted {0}", path);
				}
				catch (Exception inner)
				{
					conversionErrors.Add(inner);
				}
			}

			if (conversionErrors.Any())
			{
				Console.WriteLine("Finished with errors");
				throw new AggregateException("Errors during bulk conversion", conversionErrors);
			}
			else
			{
				Console.WriteLine("Finished successfully");
			}
		}

		public static unsafe MemoryStream ConvertToPngStream(Stream inputStream)
		{
			using var image = Pfimage.FromStream(inputStream)
				?? throw new NotSupportedException("Unsupported DDS format");

			var format = image.Format switch
			{
				Pfim.ImageFormat.Rgba32 => PixelFormat.Format32bppArgb,
				_ => throw new NotImplementedException($"Format {image.Format} not supported"),
			};

			fixed (byte* ptr = image.Data)
			{
				using var bitmap = new Bitmap(image.Width, image.Height, image.Stride, format, (IntPtr)ptr);
				var ms = new MemoryStream();
				bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
				ms.Position = 0;
				return ms;
			}
		}

		public static unsafe void Convert(string path)
		{
			using var image = Pfimage.FromFile(path)
				?? throw new NotSupportedException($"Unsupported image format for {path}");

			var format = image.Format switch
			{
				Pfim.ImageFormat.Rgba32 => PixelFormat.Format32bppArgb,
				_ => throw new NotImplementedException(),// see the sample for more details
			};

			fixed (byte* ptr = image.Data)
			{
				using var bitmap = new Bitmap(image.Width, image.Height, image.Stride, format, (IntPtr)ptr);
				bitmap.Save(Path.ChangeExtension(path, ".png"), System.Drawing.Imaging.ImageFormat.Png);
			}
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				using (_images)
				using (_processTask)
				{
					_images.CompleteAdding();
					_processTask.Wait();
				}
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
