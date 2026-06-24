using Microsoft.Win32;
using NAudio.Wave;
using System.Diagnostics;
using System.Media;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

class Program
{

    static string version = "1.1.1";
    static async Task Main(string[] args)
    {
        try
        {
            Console.SetWindowSize(1, 1);
            Console.SetBufferSize(90, 40);
            Console.SetWindowSize(90, 40);
        }
        catch { }
        Task.Run(() => audio());
        menu();
    }

    static void audio() {
        try {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("WinFuck_UnInstaller.menu.mp3")) {
                if (stream == null) return;
                using (var mp3Reader = new Mp3FileReader(stream))
                using (var volumeProvider = new WaveChannel32(mp3Reader))
                using (var outputDevice = new WaveOutEvent()) {
                    volumeProvider.Volume = 0.06f;
                    outputDevice.Init(volumeProvider);
                    outputDevice.Play();
                    while (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        Thread.Sleep(500);
                    }
                    audio();
                }
            }
        }
        catch { }
    }

    static void menu()
    {
        
        Console.Clear();
        Console.Title = $"WinFuck UnInstaller {version} (Dev build)*";
        Console.WriteLine();
        Console.WriteLine();
        Thread.Sleep(290);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("""   .::    .   .::':::'::.     ::; ::::::;'.      :::  .,-:::::  :::   ..""");
        Console.WriteLine("""   ';;,  ;;  ;;;' ;;; ;;;;,  `;;; ;;;'''';;      ;;;,;;;'````'  ;;; .;;'""");
        Console.WriteLine("""    '[[, [[, [['  [[[  [[[[[. '[[ [[[,== [['     [[[[[[         [[[[[/'  """);
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("""      Y$c$$$c$P   $$$  $$$ "Y$c$$ $$$"`` $$      $$$$$$        _$$$$,  """);
        Console.WriteLine("""       "88"888    888  888    Y88 888    88    .d888`88bo,__,o,"888"88o, """);
        Console.WriteLine("""        "M "M"    MMM  MMM     YM "MM,    "YmmMMMM""  "YUMMMMMP"MMM "MMP""");
        Console.WriteLine();
        Console.WriteLine($"       Uninstaller v{version}");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine();
        Console.WriteLine("   1. Restore Notification");
        Console.WriteLine("   2. Restore Filters");
        Console.WriteLine("   3. Restore WinUpdates");
        Console.WriteLine("   4. Restore Recovery");
        Console.WriteLine("   5. Restore Reset tools");
        Console.WriteLine("   6. Disable Block AV sites");
        Console.WriteLine("   7. Kill ResetSurvival");
        Console.WriteLine("   8. About");
        Console.WriteLine("   9. Exit");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Gray;
        string key = "";
        Console.WriteLine($"  ┌──WinFuck@UnInstaller)~[v{version}]");
        Console.CursorVisible = true;
        Console.Write("  └─$ "); key = Console.ReadLine()?.Trim();

        string methodName = "option_" + key;
        MethodInfo method = typeof(Program).GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

        if (method != null)
        {
            method.Invoke(null, null);
        }
        else
        {
            Console.CursorVisible = false;
            Console.WriteLine();
            Console.WriteLine("      Choice something just, bruh");
            Thread.Sleep(1400);
            menu();
        }
    }

    static void option_1()
    {
        try
        {
            Console.WriteLine("[!] Начало востановление уведомлений");
            using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender Security Center", true))
            {
                using (var key1 = key.CreateSubKey("Notifications", true))
                {
                    key1.DeleteValue("DisableNotifications", false);
                    key1.DeleteValue("DisableEnhancedNotifications", false);
                }
                using (var key1 = key.CreateSubKey("Systray", true))
                {
                    key1.SetValue("HideSystray", 0, RegistryValueKind.DWord);
                }
            }
            using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows Defender Security Center\Virus and threat protection", true))
            {
                key.DeleteValue("SummaryNotificationDisabled", false);
            }
            using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard", true))
            {
                using (var key1 = key.CreateSubKey("ASR", true))
                {
                    key1.DeleteValue("HideMitigationUserNotifications", false);
                }
                using (var key1 = key.CreateSubKey("Network Protection", true))
                {
                    key1.DeleteValue("HideNetworkProtectionUserNotifications", false);
                }
            }
            using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows Defender Security Center\Notifications", true))
            {
                key.DeleteValue("DisableNotifications", false);
                key.DeleteValue("DisableEnhancedNotifications", false);
                key.DeleteValue("NoActionNotificationDisabled", false);
                key.DeleteValue("FilesBlockNotificationDisabled", false);
            }
            Console.WriteLine("[OK] Уведомления Microsoft Defender восстановлены.");
            using (var key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run", true))
            {
                key.DeleteValue("SecurityHealth", false);
            }
            using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
            {
                key.SetValue("SecurityHealth", @"%SystemRoot%\System32\SecurityHealthSystray.exe", RegistryValueKind.String);
            }
            Console.WriteLine("[OK] Центр SecurityHealth восстановлен.");
            string[] firewallProfiles = { "StandardProfile", "DomainProfile" };
            foreach (var profile in firewallProfiles)
            {
                using (var key = Registry.LocalMachine.CreateSubKey($@"SOFTWARE\Policies\Microsoft\WindowsFirewall\{profile}", true))
                {
                    key.DeleteValue("DisableNotifications", false);
                }
            }
            string[] firewallProfiles1 = { "StandardProfile", "PublicProfile", "DomainProfile" };
            foreach (var profile in firewallProfiles1)
            {
                using (var key = Registry.LocalMachine.CreateSubKey($@"SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\{profile}", true))
                {
                    key.DeleteValue("DisableNotifications", false);
                }
            }
            Console.WriteLine("[OK] Уведомления брандмауэра восстановлены.");
            using (var key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.SecurityAndMaintenance", true))
            {
                key.SetValue("Enabled", 1, RegistryValueKind.DWord);
            }
            Console.WriteLine("[OK] Центр отчетов о защите восстановлен.");
            using (var key = Registry.LocalMachine.CreateSubKey(@"Software\Policies\Microsoft\Windows\Explorer", true))
            {
                key.DeleteValue("DisableNotificationCenter", false);
            }
            using (var key = Registry.CurrentUser.CreateSubKey(@"Software\Policies\Microsoft\Windows\Explorer", true))
            {
                key.DeleteValue("DisableNotificationCenter", false);
            }
            Console.WriteLine("[OK] Центр уведомлений восстановлен.");
            using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", true))
            {
                key.SetValue("ConsentPromptBehaviorAdmin", 5, RegistryValueKind.DWord);
            }
            using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender\UX Configuration", true))
            {
                key.SetValue("Notification_Style", 0, RegistryValueKind.DWord);
            }
            Console.WriteLine("[OK] Уведомления отключения UAC восстановлены.");
        }
        catch { }
        Console.WriteLine("[*] Готово");
        Thread.Sleep(1800);
        menu();
    }

    static void option_2()
    {
        try
        {
            using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", true))
            {
                key.SetValue("EnableLUA", 1, RegistryValueKind.DWord);
                key.SetValue("ConsentPromptBehaviorAdmin", 5, RegistryValueKind.DWord);
                key.SetValue("ConsentPromptBehaviorUser", 3, RegistryValueKind.DWord);
                key.SetValue("PromptOnSecureDesktop", 1, RegistryValueKind.DWord);
                key.SetValue("FilterAdministratorToken", 1, RegistryValueKind.DWord);
                key.SetValue("EnableVirtualization", 1, RegistryValueKind.DWord);
                key.SetValue("EnableInstallerDetection", 1, RegistryValueKind.DWord);
            }
            Console.WriteLine("[OK] UAC восстановлен.");
            using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows", true))
            {
                using (var key1 = key.CreateSubKey("System", true))
                {
                    key1.SetValue("EnableSmartScreen", 1, RegistryValueKind.DWord);
                }
                using (var key1 = key.CreateSubKey("AppPrivacy", true))
                {
                    key1.SetValue("DisableStoreSmartScreen", 0, RegistryValueKind.DWord);
                }
            }
            using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer", true))
            {
                key.SetValue("SmartScreenEnabled", "on", RegistryValueKind.String);
            }
            using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Edge", true))
            {
                key.SetValue("SmartScreenEnabled", 1, RegistryValueKind.DWord);
            }
            using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender\SmartScreen", true))
            {
                key.SetValue("ConfigureAppInstallControlEnabled", 1, RegistryValueKind.DWord);
            }
            Console.WriteLine("[OK] SmartScreen восстановлен.");
            using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Attachments", true))
            {
                key.SetValue("SaveZoneInformation", 2, RegistryValueKind.DWord);
            }
            using (var key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\AppHost", true))
            {
                key.SetValue("EnableWebContentEvaluation", 1, RegistryValueKind.DWord);
            }
            using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\DeviceGuard", true))
            {
                key.SetValue("EnableVirtualizationBasedSecurity", 1, RegistryValueKind.DWord);
                key.SetValue("LsaCfgFlags", 1, RegistryValueKind.DWord);
                key.SetValue("RequirePlatformSecurityFeatures", 1, RegistryValueKind.DWord);
            }
            Console.WriteLine("[OK] Device Guard восстановлен");
            using (var key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control", true))
            {
                using (var key1 = key.CreateSubKey("Lsa", true))
                {
                    key1.SetValue("RunAsPPL", 1, RegistryValueKind.DWord);
                }
                using (var key1 = key.CreateSubKey("DeviceGuard\\Scenarios\\HypervisorEnforcedCodeIntegrity", true))
                {
                    key1.SetValue("Enabled", 1, RegistryValueKind.DWord);
                }
            }
            using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender", true))
            {
                key.SetValue("PUAProtection", 1, RegistryValueKind.DWord);
            }
            Console.WriteLine("[OK] PUA восстановлен.");
            RunShort("schtasks.exe", "/change /tn \"\\Microsoft\\Windows\\RemovalTools\\MRT_HB\" /enable");
            Console.WriteLine("[OK] Плановая проверка MRT восстановлена.");
        }
        catch { }
        Console.WriteLine("[*] Готово");
        Thread.Sleep(1800);
        menu();
    }
    static void option_3()
    {
        try
        {
            Console.WriteLine("[!] Начало востановления Windows Update");
            using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", true))
            {
                key.DeleteValue("DoNotConnectToWindowsUpdateInternetLocations", false);
                key.DeleteValue("DisableWindowsUpdateAccess", false);

                using (var key1 = key.CreateSubKey("AU", true))
                {
                    key1.DeleteValue("DontOfferThroughWUAU", false);
                    key1.DeleteValue("NoAutoUpdate", false);
                    key1.DeleteValue("AUOptions", false);
                }
            }
            Console.WriteLine("[OK] Блокировка Windows Update удаленна");
            using (var key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\WaaSMedicSvc", true))
            {
                key.SetValue("Start", 3, RegistryValueKind.DWord);
            }
            RunShort("sc.exe", "config wuauserv start=auto");
            RunShort("sc.exe", "config UsoSvc start=auto");
            RunShort("sc.exe", "config dosvc start=auto");
            Console.WriteLine("[OK] Тип запуска служб установлен на \"Авто\".");
            RunShort("net.exe", "start wuauserv /y");
            RunShort("net.exe", "start UsoSvc /y");
            RunShort("net.exe", "start dosvc /y");
            Console.WriteLine("[OK] Службы обновлений Windows запущены.");
        }
        catch { }
        Console.WriteLine("[*] Готово");
        Thread.Sleep(1800);
        menu();
    }
    static void option_4()
    {
        try
        {
            using (var key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\mpssvc", true)) { key.SetValue("Start", 2, RegistryValueKind.DWord); }
            Console.WriteLine("[OK] Служба брандмауэра запущена.");
            RunShort("netsh.exe", "advfirewall set allprofiles state on");
            Console.WriteLine("[OK] Брандмауэр Windows включен.");
            RunShort("reagentc", "/enable");
            Console.WriteLine("[OK] Среда восстановления включена.");
            RunShort("bcdedit", "/set {current} recoveryenabled yes");
            Console.WriteLine("[OK] Автоматическое восстановления при сбое включенно.");
            RunShort("bcdedit", "/set {current} bootstatuspolicy displayallfailures");
            Console.WriteLine("[OK] Политика статуса загрузки сброшена.");
            RunShort("schtasks.exe", "/change /tn \"\\Microsoft\\Windows\\Servicing\\ProactiveScan\" /enable");
            Console.WriteLine("[OK] Задача ProactiveScan активирована.");
            RunShort("powershell.exe", "-ExecutionPolicy Bypass -Command \"Get-CimInstance Win32_LogicalDisk -Filter 'DriveType=3' | ForEach-Object { Enable-ComputerRestore -Drive ($_.DeviceID + '\\') }\"");
            Console.WriteLine("[OK] Точки восстановление системы включены для всех дисков.");
            using (var key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\CrashControl", true)) { key.SetValue("AutoReboot", 1, RegistryValueKind.DWord); }
            Console.WriteLine("[OK] Автоперезагрузка при BSOD включена.");
            using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", true)) { key.DeleteValue("SettingsPageVisibility", false); }
            using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\RecoveryEnvironment", true)) { key.DeleteValue("DisableOSReset", false); }
            using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", true)) { key.DeleteValue("DisableOSReset", false); }
            using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", true)) { key.DeleteValue("DisableSR", false); key.DeleteValue("DisableConfig", false); }
            using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", true)) { key.DeleteValue("DisableSR", false); key.DeleteValue("DisableConfig", false); }
            using (var key = Registry.CurrentUser.CreateSubKey(@"Software\Policies\Microsoft\Windows\System", true)) { key.DeleteValue("DisableCMD", false); }
            using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Settings", true)) { key.DeleteValue("DisableDriverRollback", false); }
            using (var key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\SafeBoot\Option", true)) { key.SetValue("OptionValue", 0, RegistryValueKind.DWord); }
            Console.WriteLine("[OK] Конфигурация SafeMode восстановлена.");
            using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options", true))
            {
                using (var k = key.CreateSubKey("bootim.exe", true)) { k.DeleteValue("Debugger", false); }
                using (var k = key.CreateSubKey("msconfig.exe", true)) { k.DeleteValue("Debugger", false); }
            }
            Console.WriteLine("[OK] Ограничения сняты (CMD, драйверы, системные утилиты).");
        }
        catch (Exception ex) { }
        Console.WriteLine("[*] Готово");
        Thread.Sleep(1800);
        menu();
    }

    static void option_5()
    {
        Console.WriteLine("[?] Удаление политик ограничений (Safer, IFEO)...");
        using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Safer\CodeIdentifiers", true))
        {
            key.DeleteValue("DefaultLevel", false);
            key.DeleteValue("TransparentEnabled", false);
            key.DeleteValue("PolicyScope", false);
            key.DeleteSubKeyTree(@"0\Paths", false);

        }
        Console.WriteLine("[OK] Ограничения Safer сняты");
        using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options", true))
        {
            using (var k = key.CreateSubKey("sfc.exe", true)) { k.DeleteValue("Debugger", false); }
            using (var k = key.CreateSubKey("reagentc.exe", true)) { k.DeleteValue("Debugger", false); }
            using (var k = key.CreateSubKey("rstrui.exe", true)) { k.DeleteValue("Debugger", false); }
            using (var k = key.CreateSubKey("sdclt.exe", true)) { k.DeleteValue("Debugger", false); }
            using (var k = key.CreateSubKey("recdisc.exe", true)) { k.DeleteValue("Debugger", false); }
            using (var k = key.CreateSubKey("systemreset.exe", true)) { k.DeleteValue("Debugger", false); }
            using (var k = key.CreateSubKey("RecoveryDrive.exe", true)) { k.DeleteValue("Debugger", false); }
            using (var k = key.CreateSubKey("mrt.exe", true)) { k.DeleteValue("Debugger", false); }
            using (var k = key.CreateSubKey("MRT.exe", true)) { k.DeleteValue("MitigationOptions", false); }
            using (var k = key.CreateSubKey("TiWorker.exe", true)) { k.DeleteValue("Debugger", false); }
        }
        Console.WriteLine("[OK] Ограничения IFEO сняты");
        Console.WriteLine("[*] Готово");
        Thread.Sleep(1800);
        menu();
    }

    static void option_6()
    {
        Console.WriteLine("[!] Удаление блокировки антивирусных сайтов");
        string hostsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"drivers\etc\hosts");
        string hostsContent = """
        127.0.0.1 localhost
        ::1 localhost
        """;

        try
        {
            GrantFullControlToFile(hostsPath);
            File.WriteAllText(hostsPath, hostsContent);
            Console.WriteLine("[OK] Удаленна блокировка антивирусных сайтов");
            RunShort("ipconfig.exe", "/flushdns");
            Console.WriteLine("[OK] Выполнено обновление dns для применения изменений");
        }
        catch (UnauthorizedAccessException)
        {
            Debug.WriteLine("");
        }
        Console.WriteLine("[*] Готово");
        Thread.Sleep(1800);
        menu();
    }

    static void option_7()
    {
        try
        {
            Console.WriteLine("[!] Начало уничтожения ResetSurvival");
            string system32 = Environment.GetFolderPath(Environment.SpecialFolder.System);
            string xml_file = Path.Combine(system32, @"Recovery\ReAgent.xml");
            if (!File.Exists(xml_file))
            {
                Console.WriteLine("[!Warning] ReAgent.xml НЕ НАЙДЕН!");
                Console.WriteLine("[!Warning] Востановление системы до заводских вероятно сломано!");
                Thread.Sleep(1800);
                return;
            }
            RunShort("powershell.exe", $"$text = [System.IO.File]::ReadAllText('{xml_file}'); $regex = '(?s)\\s*<PackageConfiguration>.*?</PackageConfiguration>'; $text -replace $regex, '' | Set-Content '{xml_file}'");
            Console.WriteLine("[OK] ResetSurvival уничтожен");
        }
        catch { }

        Console.WriteLine("[*] Готово");
        Thread.Sleep(1800);
        menu();
    }

    static void option_8()
    {
        Console.CursorVisible = false;
        Console.Clear();
        Console.SetCursorPosition(25, 11);
        Console.WriteLine($"@_WinFuck Uninstaller {version} (Dev build)");
        Console.SetCursorPosition(30, 12);
        Console.Write("# By Xeroxx1337  &  SideSquad");
        Thread.Sleep(1000);
        Console.SetCursorPosition(33, 14);
        Console.Write("Press enter to back menu");
        Console.ReadKey(true);
        menu();
    }

    static void option_9()
    {
        Thread.Sleep(120);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Thread.Sleep(170);
        Console.ForegroundColor = ConsoleColor.Black;
        Thread.Sleep(200);
        return;
    }

    static void GrantFullControlToFile(string filePath)
    {

        try
        {
            IdentityReference administratorsGroup = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);
            FileInfo fileInfo = new FileInfo(filePath);

            FileSecurity fileSecurity = fileInfo.GetAccessControl();

            fileSecurity.SetOwner(administratorsGroup);
            fileInfo.SetAccessControl(fileSecurity);

            fileSecurity = fileInfo.GetAccessControl();

            FileSystemAccessRule fullControlRule = new FileSystemAccessRule(
                administratorsGroup,
                FileSystemRights.FullControl,
                AccessControlType.Allow
            );

            fileSecurity.AddAccessRule(fullControlRule);
            fileInfo.SetAccessControl(fileSecurity);
        }
        catch { }
    }
    static void RunShort(string fileName, string arguments)
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            using (Process? proc = Process.Start(psi))
            {
                proc?.WaitForExit(5000);
            }
        }
        catch { }
    }

}