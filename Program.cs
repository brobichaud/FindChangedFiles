using System;
using System.IO;
using System.Linq;

namespace ConsoleApplication1
{
	class Program
	{
		static void Main(string[] args)
		{
			TimerElapsed();
		}

		private static void TimerElapsed()
		{
			try
			{
				// find all files not modified in the specified interval
				var dir = new DirectoryInfo(@"C:\New-folder");
				var age = DateTime.UtcNow.Subtract(TimeSpan.FromHours(22));
				var files = dir.EnumerateFiles().Where(file => file.LastWriteTimeUtc < age);
				var count = files.Count();

				Console.WriteLine("Older than: " + age);

				foreach (var file in files)
				{
					Console.WriteLine("Age: " + file.LastWriteTimeUtc);
					MoveFile(file, @"C:\New-folder\pipeline");
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Telemetry pipeline processing failed: " + e);
			}
			finally
			{
			}
		}

		private static void MoveFile(FileInfo source, string dest)
		{
			try
			{
				var srcFileName = source.Name;
				var destPath = Path.Combine(dest, srcFileName);
				Console.WriteLine("Dest: " + destPath);
				File.Move(source.FullName, destPath);
			}
			catch { }  // we'll try again next interval
		}
	}
}
