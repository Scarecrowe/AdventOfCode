namespace AdventOfCode.Runner.The_Head_Elfs_Office
{
    using System.Diagnostics;
    using System.Runtime;
    using AdventOfCode.Core;
    using AdventOfCode.Core.ConsoleMenu;
    using AdventOfCode.Runner.North_Pole_Operations;

    public class TheHeadElfsOfficeMenu : ConsoleMenu, IConsoleMenu
    {
        public TheHeadElfsOfficeMenu()
            : base("The Head Elf's Office")
        {
            this.Items.Add(
                TheHeadElfsOfficeMenuType.ViewMachineSpecs,
                "View Machine Specs",
                "Show CPU, memory and runtime details");

            this.AddBackMenuItem("Return to North Pole Operations");
            this.AddExitMenuItem();
        }

        public async Task<IConsoleMenu> Execute()
        {
            this.Reset();

            IConsoleMenuItem? item = await this.WriteMenu();

            switch (item?.Key)
            {
                case TheHeadElfsOfficeMenuType.ViewMachineSpecs:
                    this.Reset();
                    WriteMachineSpecs();
                    this.WaitForUser();
                    break;

                case GenericMenu.Back:
                case GenericMenu.MainMenu:
                    return new NorthPoleOperationsMenu();

                case GenericMenu.Exit:
                    return await new ExitMenu().Execute();
            }

            return new TheHeadElfsOfficeMenu();
        }

        private static void WriteMachineSpecs()
        {
            Process process = Process.GetCurrentProcess();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            PuzzleConsole.WriteLine("Machine Specifications");
            PuzzleConsole.WriteLine("--------------------------------------------------------------------------------");

            PuzzleConsole.WriteLine($"Machine Name           : {Environment.MachineName}");
            PuzzleConsole.WriteLine($"OS Version             : {Environment.OSVersion}");

            if (OperatingSystem.IsWindows())
            {
                PuzzleConsole.WriteLine($"CPU Model              : {GetCpuName()}");
                PuzzleConsole.WriteLine($"CPU Max Speed          : {GetCpuSpeed()}");
                PuzzleConsole.WriteLine($"Installed RAM          : {GetInstalledRam()}");
            }

            PuzzleConsole.WriteLine($"Processor Count        : {Environment.ProcessorCount}");
            PuzzleConsole.WriteLine($"64-bit OS              : {Environment.Is64BitOperatingSystem}");
            PuzzleConsole.WriteLine($"64-bit Process         : {Environment.Is64BitProcess}");

            PuzzleConsole.WriteLine();

            PuzzleConsole.WriteLine("Process Information");
            PuzzleConsole.WriteLine("--------------------------------------------------------------------------------");

            PuzzleConsole.WriteLine($"Working Set            : {FormatBytes(Environment.WorkingSet)}");
            PuzzleConsole.WriteLine($"Process Memory         : {FormatBytes(process.WorkingSet64)}");
            PuzzleConsole.WriteLine($"Private Memory         : {FormatBytes(process.PrivateMemorySize64)}");
            PuzzleConsole.WriteLine($"GC Total Memory        : {FormatBytes(GC.GetTotalMemory(false))}");

            PuzzleConsole.WriteLine();

            PuzzleConsole.WriteLine("Benchmark Settings");
            PuzzleConsole.WriteLine("--------------------------------------------------------------------------------");

            PuzzleConsole.WriteLine($"Server GC              : {GCSettings.IsServerGC}");
            PuzzleConsole.WriteLine($"GC Latency Mode        : {GCSettings.LatencyMode}");
            PuzzleConsole.WriteLine($"Process Affinity       : {process.ProcessorAffinity}");
            PuzzleConsole.WriteLine($"Process Priority       : {process.PriorityClass}");
            PuzzleConsole.WriteLine($"Thread Priority        : {Thread.CurrentThread.Priority}");

#if DEBUG
            PuzzleConsole.WriteLine("Build Configuration    : Debug");
#else
            PuzzleConsole.WriteLine("Build Configuration    : Release");
#endif

            PuzzleConsole.WriteLine();

            PuzzleConsole.WriteLine("Runtime");
            PuzzleConsole.WriteLine("--------------------------------------------------------------------------------");

            PuzzleConsole.WriteLine($"Runtime Version        : {Environment.Version}");
            PuzzleConsole.WriteLine($"Framework              : {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}");
            PuzzleConsole.WriteLine($"Architecture           : {System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture}");
            PuzzleConsole.WriteLine($"Current Directory      : {Environment.CurrentDirectory}");
            PuzzleConsole.WriteLine($"Base Directory         : {AppContext.BaseDirectory}");

            PuzzleConsole.WriteLine("--------------------------------------------------------------------------------");
            PuzzleConsole.Flush();
        }

        private static string GetCpuName()
        {
            return RunPowerShell("(Get-CimInstance Win32_Processor).Name");
        }

        private static string GetCpuSpeed()
        {
            string result = RunPowerShell("(Get-CimInstance Win32_Processor).MaxClockSpeed");

            return string.IsNullOrWhiteSpace(result)
                ? "Unknown"
                : $"{result} MHz";
        }

        private static string GetInstalledRam()
        {
            string result = RunPowerShell("[math]::Round((Get-CimInstance Win32_ComputerSystem).TotalPhysicalMemory / 1GB, 2)");

            return string.IsNullOrWhiteSpace(result)
                ? "Unknown"
                : $"{result} GB";
        }

        private static string RunPowerShell(string command)
        {
            try
            {
                ProcessStartInfo startInfo = new()
                {
                    FileName = "powershell",
                    Arguments = $"-Command \"{command}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                using Process process = Process.Start(startInfo)!;

                string output = process.StandardOutput.ReadToEnd().Trim();

                process.WaitForExit();

                return output;
            }
            catch
            {
                return "Unknown";
            }
        }

        private static string FormatBytes(long bytes)
        {
            string[] sizes =
            [
                "B",
                "KB",
                "MB",
                "GB",
                "TB",
            ];

            double value = bytes;
            int order = 0;

            while (value >= 1024 && order < sizes.Length - 1)
            {
                order++;
                value /= 1024;
            }

            return $"{value:0.##} {sizes[order]}";
        }
    }
}