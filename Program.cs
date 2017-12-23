using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
	class Program
	{
		static void Main(string[] args)
		{
			TimerElapsed();
		}

		/// <summary>
		/// The scheduled timer elapsed, see if there is any work to do
		/// </summary>
		/// <remarks>Called on a ThreadPool thread</remarks>
		private static void TimerElapsed()
		{
			try
			{

				// find all files not modified in the specified interval
				var dir = new DirectoryInfo(@"C:\Users\brobichaud\Desktop\New folder");
				var age = DateTime.UtcNow.Subtract(TimeSpan.FromHours(22));
				var files = dir.EnumerateFiles().Where(file => file.LastWriteTimeUtc < age);
				var count = files.Count();

				Console.WriteLine("Older than: " + age);

				foreach (var file in files)
				{
					Console.WriteLine("Age: " + file.LastWriteTimeUtc);
					MoveFile(file, @"C:\Users\brobichaud\Desktop\New folder\pipeline");
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
