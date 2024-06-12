using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;

using System.Runtime.InteropServices;
using System.Security.AccessControl;

using System.Text;
using System.Threading;
using System.Windows.Forms;
using OptimaGL.lib;
using Microsoft.Win32;

namespace OptimaGL
{
    public sealed partial class MainForm : Form
    {
        private const string LogFileName = "OptimaGL.log";
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        private const int CS_DROPSHADOW = 0x00020000;
        private readonly string _systemPath = Path.GetPathRoot(Environment.SystemDirectory);
        private bool _destroyFlag;

        private List<string> _errorsList = new List<string>();
        private int _fatalErrors;
        private string _system32Location;
        private bool _win10 = true;

        public MainForm(string[] args)
        {

            InitializeComponent();
            DoubleBuffered = true;
            // Re create log file
            RecreateLogFile(LogFileName);
            // Check windows version

            _SetShellSys32Path();


            StealthMode(args); //check args
            
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            /*
             * Check windows system restore Enabled status
             */
            if (WindowsUtil.SystemRestore_Status() == 0) _OutPut("Windows Restore DISABLE", LogLevel.Warning);
        }


        private void StealthMode(IEnumerable<string> args)
        {
            var currentArgs = args as string[] ?? args.ToArray();
            foreach (var currentArg in currentArgs)
            {
                if (currentArg.IndexOf("/deleteapp=", StringComparison.Ordinal) > -1)
                {
                    DeleteWindows10MetroApp(currentArg.Replace("/deleteapp=", null));
                }
                if (currentArg.IndexOf("/destroy", StringComparison.Ordinal) > -1)
                {
                    WindowState = FormWindowState.Minimized;
                    ShowInTaskbar = false;
                    _destroyFlag = true;
                    //Windows 10
                    if (_win10) DestroyWindowsSpyingMainThread();
                    
                }
                if (currentArg.IndexOf("/deleteonedrive", StringComparison.Ordinal) > -1)
                {
                    DeleteOneDrive();
                }
            }
            if (currentArgs.Any())
            Process.GetCurrentProcess().Kill();
        }

        private void _SetShellSys32Path()
        {
            if (File.Exists(_systemPath + @"Windows\Sysnative\cmd.exe"))
            {
                _system32Location = _systemPath + @"Windows\Sysnative\";
            }
            else
            {
                _system32Location = _systemPath + @"Windows\System32\";
            }
        }

        private void btnDestroyWindowsSpying_Click(object sender, EventArgs e)
        {
            StartDestroyWindowsSpying();
        }

        private void Progressbaradd(int numberadd)
        {
            try
            {
                Invoke(new MethodInvoker(delegate
                {
                    try
                    {
                        ProgressBarStatus.Value += numberadd;
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }));
            }
            catch (Exception)
            {
                try
                {
                    ProgressBarStatus.Value += numberadd;
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private void Progressbaradd1(int numberadd)
        {
            try
            {
                Invoke(new MethodInvoker(delegate
                {
                    try
                    {
                        if (numberadd == 0)
                        {
                            ProgressBarStatus1.Value = 0;
                        }
                        else
                        {
                            ProgressBarStatus1.Value += numberadd;
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }));
            }
            catch (Exception)
            {
                try
                {
                    if (numberadd == 0)
                    {
                        ProgressBarStatus1.Value = 0;
                    }
                    else
                    {
                        ProgressBarStatus1.Value += numberadd;
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private void EnableOrDisableTab(bool enableordisable)
        {
            try
            {
                Invoke(new MethodInvoker(delegate
                {
                    ControlBox = enableordisable;
                   
                    btnDestroyWindowsSpying.Enabled = enableordisable;
                    tabPageSettings.Enabled = enableordisable;
                    
                }));
            }
            catch (Exception)
            {
                ControlBox = enableordisable;
                tabPageMain.Enabled = enableordisable;
                tabPageSettings.Enabled = enableordisable;
                
            }
        }

        public void RecreateLogFile(string logfilename)
        {
            try
            {
                if (!File.Exists(logfilename))
                {
                    File.Create(logfilename).Close();
                }
                else
                {
                    File.Delete(logfilename);
                    File.Create(logfilename).Close();
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                _OutPut(ex.Message, LogLevel.Debug);
#endif
            }
        }

        public void DeleteFile(string filepath)
        {
            RunCmd($"/c del /F /Q \"{filepath}\"");
        }

        public void RunCmd(string args)
        {
            ProcStartargs(Paths.ShellCmdLocation, args);
        }

        private void ProcStartargs(string name, string args)
        {
            try
            {
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = name,
                        Arguments = args,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true,
                        StandardOutputEncoding = Encoding.GetEncoding(866)
                    }
                };
                proc.Start();
                // ReSharper disable once NotAccessedVariable
                string line = null;
                while (!proc.StandardOutput.EndOfStream)
                {
                    line += Environment.NewLine + proc.StandardOutput.ReadLine();
                }
                proc.WaitForExit();
#if DEBUG
                _OutPut($"Start: {name} {args}{Environment.NewLine}Output: {line}", LogLevel.Debug);
#endif
            }
                // ReSharper disable once UnusedVariable
            catch (Exception ex)
            {
                _OutPut($"Error start prog {name} {args}", LogLevel.Error);
#if DEBUG
                _OutPut(ex.Message, LogLevel.Debug);
#endif
                _fatalErrors++;
                _errorsList.Add($"Error start prog {name} {args}");
            }
        }

        private static void CreateRestorePoint(string description)
        {
            var oScope = new ManagementScope("\\\\localhost\\root\\default");
            var oPath = new ManagementPath("SystemRestore");
            var oGetOp = new ObjectGetOptions();
            var oProcess = new ManagementClass(oScope, oPath, oGetOp);

            var oInParams =
                oProcess.GetMethodParameters("CreateRestorePoint");
            oInParams["Description"] = description;
            oInParams["RestorePointType"] = 12; // MODIFY_SETTINGS
            oInParams["EventType"] = 100;

            oProcess.InvokeMethod("CreateRestorePoint", oInParams, null);
        }


        private void DeleteWindows10MetroApp(string appname)
        {
            ProcStartargs("powershell",
                $"-command \"Get-AppxPackage *{appname}* | Remove-AppxPackage\"");
        }

        private void StartDestroyWindowsSpying()
        {
            _errorsList.Clear();
            _fatalErrors = 0;
            
            SetCompleteText(true);
            _OutPut($"Starting: {DateTime.Now}.");
            _OutPutSplit();
            ProgressBarStatus.Value = 0;
            new Thread(DestroyWindowsSpyingMainThread).Start();
        }

        private void DestroyWindowsSpyingMainThread()
        {
            if (checkBoxCreateSystemRestorePoint.Checked)
            {
                try
                {
                    var restorepointName = $"DestroyWindowsSpying {DateTime.Now}";
                    _OutPut($"Creating restore point {restorepointName}...");
                    CreateRestorePoint(restorepointName);
                    _OutPut($"Restore point {restorepointName} created.");
                }
                    // ReSharper disable once UnusedVariable
                catch (Exception ex)
                {
                    _OutPut("Error creating restore point.");
#if DEBUG
                    _OutPut(ex.Message, LogLevel.Debug);
#endif
                }
            }
            Progressbaradd(10);
            if (checkBoxKeyLoggerAndTelemetry.Checked)
            {
                // DISABLE TELEMETRY
                _OutPut("Disable telemetry...");
                DigTrackFullRemove();
                // DELETE KEYLOGGER
                _OutPut("Delete keylogger...");
                RunCmd(
                    "/c reg add \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows\\Windows Search\" /v \"AllowCortana\" /t REG_DWORD /d 0 /f ");
                    // disable Cortana;
                _OutPut("Cortana disable #1");
            }
            Progressbaradd(15); //25
            if (checkBoxAddToHosts.Checked)
            {
                AddToHostsAndFirewall();
            }
            Progressbaradd(20); //45
            if (checkBoxDisablePrivateSettings.Checked)
            {
                string[] regkeyvalandother =
                {
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\DeviceAccess\Global\{21157C1F-2651-4CC1-90CA-1F28B02263F6}",
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\DeviceAccess\Global\{2EEF81BE-33FA-4800-9670-1CD474972C3F}",
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\DeviceAccess\Global\{7D7E8402-7C54-4821-A34E-AEEFD62DED93}",
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\DeviceAccess\Global\{992AFA70-6F47-4148-B3E9-3003349C1548}",
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\DeviceAccess\Global\{9D9E0118-1807-4F2E-96E4-2CE57142E196}",
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\DeviceAccess\Global\{A8804298-2D5F-42E3-9531-9C8C39EB29CE}",
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\DeviceAccess\Global\{B19F89AF-E3EB-444B-8DEA-202575A71599}",
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\DeviceAccess\Global\{BFA794E4-F964-4FDB-90F6-51056BFE4B44}",
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\DeviceAccess\Global\{C1D23ACC-752B-43E5-8448-8D0E519CD6D6}",
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\DeviceAccess\Global\{D89823BA-7180-4B81-B50C-7E471E6121A3}",
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\DeviceAccess\Global\{E5323777-F976-4f5b-9B55-B94699C46E44}",
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\DeviceAccess\Global\{E6AD100E-5F4E-44CD-BE0F-2265D88D14F5}",
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\DeviceAccess\Global\{E83AF229-8640-4D18-A213-E22675EBB2C3}",
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\DeviceAccess\Global\LooselyCoupled"
                };
                foreach (var currentRegKey in regkeyvalandother)
                {
                    SetRegValueHkcu(currentRegKey, "Value", "Deny", RegistryValueKind.String);
                }
                SetRegValueHkcu(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Search", "CortanaEnabled", "0",
                    RegistryValueKind.DWord);
                SetRegValueHkcu(@"SOFTWARE\Microsoft\InputPersonalization", "RestrictImplicitInkCollection", "1",
                    RegistryValueKind.DWord);
                SetRegValueHklm(@"SOFTWARE\Policies\Microsoft\Windows\Windows Search", "DisableWebSearch", "1",
                    RegistryValueKind.DWord);
                SetRegValueHklm(@"SOFTWARE\Policies\Microsoft\Windows\Windows Search", "ConnectedSearchUseWeb", "0",
                    RegistryValueKind.DWord);
                SetRegValueHklm(@"SOFTWARE\Policies\Microsoft\Windows\LocationAndSensors", "DisableLocation", "1",
                    RegistryValueKind.DWord);
                SetRegValueHklm(@"SOFTWARE\Policies\Microsoft\Windows\LocationAndSensors",
                    "DisableWindowsLocationProvider", "1", RegistryValueKind.DWord);
                SetRegValueHklm(@"SOFTWARE\Policies\Microsoft\Windows\LocationAndSensors", "DisableLocationScripting",
                    "1", RegistryValueKind.DWord);
                SetRegValueHklm(@"SOFTWARE\Policies\Microsoft\Windows\LocationAndSensors", "DisableSensors", "1",
                    RegistryValueKind.DWord);
                SetRegValueHklm(@"SYSTEM\CurrentControlSet\Services\lfsvc\Service\Configuration", "Status", "0",
                    RegistryValueKind.DWord);
                SetRegValueHklm(
                    @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Sensor\Overrides\{BFA794E4-F964-4FDB-90F6-51056BFE4B44}",
                    "SensorPermissionState", "0", RegistryValueKind.DWord);
                SetRegValueHkcu(@"SOFTWARE\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod", "0", RegistryValueKind.DWord);
                SetRegValueHkcu(@"SOFTWARE\Microsoft\Siuf\Rules", "PeriodInNanoSeconds", "0", RegistryValueKind.DWord);
                SetRegValueHkcu(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Search", "BingSearchEnabled", "0",
                    RegistryValueKind.DWord);
                SetRegValueHklm(@"SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingDataSharing", "1",
                    RegistryValueKind.DWord);
                SetRegValueHklm(@"SOFTWARE\Policies\Microsoft\Windows\HandwritingErrorReports",
                    "PreventHandwritingErrorReports", "1", RegistryValueKind.DWord);
                SetRegValueHklm(@"SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory", "1",
                    RegistryValueKind.DWord);
                SetRegValueHklm(@"SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenCamera", "1",
                    RegistryValueKind.DWord);
                SetRegValueHklm(@"SOFTWARE\Microsoft\Windows\CurrentVersion\AdvertisingInfo", "Enabled", "0",
                    RegistryValueKind.DWord);
                SetRegValueHkcu(@"SOFTWARE\Microsoft\Windows\CurrentVersion\AdvertisingInfo", "Enabled", "0",
                    RegistryValueKind.DWord);
                SetRegValueHkcu(@"SOFTWARE\Microsoft\Input\TIPC", "Enabled", "0", RegistryValueKind.DWord);
                SetRegValueHklm(@"SOFTWARE\Policies\Microsoft\Biometrics", "Enabled", "0", RegistryValueKind.DWord);
                SetRegValueHklm(@"SOFTWARE\Policies\Microsoft\Windows\CredUI", "DisablePasswordReveal", "1",
                    RegistryValueKind.DWord);
                SetRegValueHkcu(@"SOFTWARE\Microsoft\Windows\CurrentVersion\SettingSync", "SyncPolicy", "5",
                    RegistryValueKind.DWord);
                SetRegValueHkcu(@"SOFTWARE\Microsoft\Windows\CurrentVersion\SettingSync\Groups\Personalization",
                    "Enabled", "0", RegistryValueKind.DWord);
                SetRegValueHkcu(@"SOFTWARE\Microsoft\Windows\CurrentVersion\SettingSync\Groups\BrowserSettings",
                    "Enabled", "0", RegistryValueKind.DWord);
                SetRegValueHkcu(@"SOFTWARE\Microsoft\Windows\CurrentVersion\SettingSync\Groups\Credentials", "Enabled",
                    "0", RegistryValueKind.DWord);
                SetRegValueHkcu(@"SOFTWARE\Microsoft\Windows\CurrentVersion\SettingSync\Groups\Language", "Enabled", "0",
                    RegistryValueKind.DWord);
                SetRegValueHkcu(@"SOFTWARE\Microsoft\Windows\CurrentVersion\SettingSync\Groups\Accessibility", "Enabled",
                    "0", RegistryValueKind.DWord);
                SetRegValueHkcu(@"SOFTWARE\Microsoft\Windows\CurrentVersion\SettingSync\Groups\Windows", "Enabled", "0",
                    RegistryValueKind.DWord);
                _OutPut("Disable private settings");
            }
            Progressbaradd(10); //55
            if (checkBoxDisableWindowsDefender.Checked)
            {
                try
                {
                    SetRegValueHklm(@"SOFTWARE\Policies\Microsoft\Windows Defender", "DisableAntiSpyware", "1",
                        RegistryValueKind.DWord);
                    SetRegValueHklm(@"SOFTWARE\Policies\Microsoft\Windows Defender\Spynet", "SpyNetReporting", "0",
                        RegistryValueKind.DWord);
                    SetRegValueHklm(@"SOFTWARE\Policies\Microsoft\Windows Defender\Spynet", "SubmitSamplesConsent", "2",
                        RegistryValueKind.DWord);
                    SetRegValueHklm(@"SOFTWARE\Policies\Microsoft\MRT", "DontReportInfectionInformation", "1",
                        RegistryValueKind.DWord);
                    _OutPut("Disable Windows Defender.");
                    SetRegValueHklm(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer", "SmartScreenEnabled", "Off",
                        RegistryValueKind.String);
                    _OutPut("Disable smart screen.");
                }
                catch (Exception ex)
                {
                    _OutPut("Error disable windows Defender or Smart Screen", LogLevel.Error);
#if DEBUG
                    _OutPut(ex.Message, LogLevel.Debug);
#endif
                    _fatalErrors++;
                    _errorsList.Add($"Error disable Windows Defender or Smart Screen. Message: {ex.Message}");
                }
            }
            Progressbaradd(5); //60
            Progressbaradd(10); //70
            if (checkBoxSPYTasks.Checked)
            {
                DisableSpyingTasks();
            }
            Progressbaradd(10); //80
            if (checkBoxDeleteWindows10Apps.Checked)
            {
                RemoveWindows10Apps();
            }
            Progressbaradd(20); //100
            EnableOrDisableTab(true);
            try
            {
                Invoke(new MethodInvoker(delegate { SetCompleteText(); }));
            }
            catch (Exception)
            {
                try
                {
                    SetCompleteText();
                }
                    // ReSharper disable once UnusedVariable
                    // ReSharper disable once EmptyGeneralCatchClause
                catch (Exception ex)
                {
#if DEBUG
                    _OutPut(ex.Message, LogLevel.Debug);
#endif
                }
            }
        }

        private void DigTrackFullRemove()
        {
            RunCmd("/c net stop DiagTrack ");
            RunCmd("/c net stop diagnosticshub.standardcollector.service ");
            RunCmd("/c net stop dmwappushservice ");
            RunCmd("/c net stop WMPNetworkSvc ");
            RunCmd("/c sc config DiagTrack start=disabled ");
            RunCmd("/c sc config diagnosticshub.standardcollector.service start=disabled ");
            RunCmd("/c sc config dmwappushservice start=disabled ");
            RunCmd("/c sc config WMPNetworkSvc start=disabled ");
            RunCmd(
                "/c REG ADD HKLM\\SYSTEM\\ControlSet001\\Control\\WMI\\AutoLogger\\AutoLogger-Diagtrack-Listener /v Start /t REG_DWORD /d 0 /f");
            RunCmd("/c sc delete dmwappushsvc");
            RunCmd("/c sc delete \"Diagnostics Tracking Service\"");
            RunCmd("/c sc delete diagtrack");
            RunCmd("/c reg add \"HKLM\\SYSTEM\\CurrentControlSet\\Services\\SharedAccess\\Parameters\\FirewallPolicy\\FirewallRules\"  /v \"{60E6D465-398E-4850-BE86-7EF7620A2377}\" /t REG_SZ /d  \"v2.24|Action=Block|Active=TRUE|Dir=Out|App=C:\\windows\\system32\\svchost.exe|Svc=DiagTrack|Name=Windows  Telemetry|\" /f");
            RunCmd("/c reg add \"HKLM\\SYSTEM\\CurrentControlSet\\Services\\SharedAccess\\Parameters\\FirewallPolicy\\FirewallRules\"  /v \"{2765E0F4-2918-4A46-B9C9-43CDD8FCBA2B}\" /t REG_SZ /d  \"v2.24|Action=Block|Active=TRUE|Dir=Out|App=C:\\windows\\systemapps\\microsoft.windows.cortana_cw5n1h2txyewy\\searchui.exe|Name=Search  and Cortana  application|AppPkgId=S-1-15-2-1861897761-1695161497-2927542615-642690995-327840285-2659745135-2630312742|\"  /f");
            RunCmd(
                "/c reg add \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Device Metadata\" /v \"PreventDeviceMetadataFromNetwork\" /t REG_DWORD /d 1 /f ");
            RunCmd(
                "/c reg add \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\DataCollection\" /v \"AllowTelemetry\" /t REG_DWORD /d 0 /f ");
            RunCmd(
                "/c reg add \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\MRT\" /v \"DontOfferThroughWUAU\" /t REG_DWORD /d 1 /f ");
            RunCmd(
                "/c reg add \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\SQMClient\\Windows\" /v \"CEIPEnable\" /t REG_DWORD /d 0 /f ");
            RunCmd(
                "/c reg add \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows\\AppCompat\" /v \"AITEnable\" /t REG_DWORD /d 0 /f ");
            RunCmd(
                "/c reg add \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows\\AppCompat\" /v \"DisableUAR\" /t REG_DWORD /d 1 /f ");
            RunCmd(
                "/c reg add \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows\\DataCollection\" /v \"AllowTelemetry\" /t REG_DWORD /d 0 /f ");
            RunCmd(
                "/c reg add \"HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\WMI\\AutoLogger\\AutoLogger-Diagtrack-Listener\" /v \"Start\" /t REG_DWORD /d 0 /f ");
            RunCmd(
                "/c reg add \"HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\WMI\\AutoLogger\\SQMLogger\" /v \"Start\" /t REG_DWORD /d 0 /f ");
            RunCmd(
                "/c reg add \"HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Siuf\\Rules\" /v \"NumberOfSIUFInPeriod\" /t REG_DWORD /d 0 /f ");
            RunCmd(
                "/c reg delete \"HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Siuf\\Rules\" /v \"PeriodInNanoSeconds\" /f ");
        }

        private void SetCompleteText(bool start = false)
        {
            if (_destroyFlag)
                return;
            if (start)
            {
                StatusCommandsLable.Text = @"Оптимизация Windows 10 - Ждите ";
                StatusCommandsLable.ForeColor = Color.Black;
            }
            else
            {
                if (_fatalErrors == 0)
                {
                    StatusCommandsLable.Text = $"Оптимизация Windows 10 - {"Успешно"}!";
                    StatusCommandsLable.ForeColor = Color.DarkGreen;
                    if (MessageBox.Show("CompleteMSG", "Info",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                    Process.Start("shutdown.exe", "-r -t 0");
                    Process.GetCurrentProcess().Kill();
                }
                else
                {
                    StatusCommandsLable.Text = $"Оптимизация 10 Spying - ошибка: {_fatalErrors}";
                    StatusCommandsLable.ForeColor = Color.Red;
                    try
                    {
                        var errorsMsg = _errorsList.Aggregate<string, string>(null,
                            (current, errorMsg) => current + (errorMsg + "\r\n"));
                        var errorFilePath = Path.GetTempPath() + @"\errors.log";
                        File.Create(errorFilePath).Close();
                        File.WriteAllText(errorFilePath, errorsMsg);
                        Process.Start(errorFilePath);
                    }
                    catch
                    {
                        // ignored
                    }
                    if (MessageBox.Show(string.Format("ErrorMSG", _fatalErrors),
                        "Info",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                    Process.Start("shutdown.exe", "-r -t 0");
                    Process.GetCurrentProcess().Kill();
                }
            }
        }

        private void DisableSpyingTasks()
        {
            string[] disabletaskslist =
            {
                @"Microsoft\Office\Office 15 Subscription Heartbeat",
                @"Microsoft\Office\Office ClickToRun Service Monitor",
                @"Microsoft\Office\OfficeTelemetry\AgentFallBack2016",
                @"Microsoft\Office\OfficeTelemetry\OfficeTelemetryAgentLogOn2016",
                @"Microsoft\Office\OfficeTelemetryAgentFallBack",
                @"Microsoft\Office\OfficeTelemetryAgentFallBack2016",
                @"Microsoft\Office\OfficeTelemetryAgentLogOn",
                @"Microsoft\Office\OfficeTelemetryAgentLogOn2016",
                @"Microsoft\Windows\AppID\SmartScreenSpecific",
                @"Microsoft\Windows\Application Experience\AitAgent",
                @"Microsoft\Windows\Application Experience\Microsoft Compatibility Appraiser",
                @"Microsoft\Windows\Application Experience\ProgramDataUpdater",
                @"Microsoft\Windows\Application Experience\StartupAppTask",
                @"Microsoft\Windows\Autochk\Proxy",
                @"Microsoft\Windows\CloudExperienceHost\CreateObjectTask",
                @"Microsoft\Windows\Customer Experience Improvement Program\BthSQM",
                @"Microsoft\Windows\Customer Experience Improvement Program\Consolidator",
                @"Microsoft\Windows\Customer Experience Improvement Program\KernelCeipTask",
                @"Microsoft\Windows\Customer Experience Improvement Program\Uploader",
                @"Microsoft\Windows\Customer Experience Improvement Program\UsbCeip",
                @"Microsoft\Windows\DiskDiagnostic\Microsoft-Windows-DiskDiagnosticDataCollector",
                @"Microsoft\Windows\DiskFootprint\Diagnostics",
                @"Microsoft\Windows\FileHistory\File History (maintenance mode)",
                @"Microsoft\Windows\Maintenance\WinSAT",
                @"Microsoft\Windows\Media Center\ActivateWindowsSearch",
                @"Microsoft\Windows\Media Center\ConfigureInternetTimeService",
                @"Microsoft\Windows\Media Center\DispatchRecoveryTasks",
                @"Microsoft\Windows\Media Center\ehDRMInit",
                @"Microsoft\Windows\Media Center\InstallPlayReady",
                @"Microsoft\Windows\Media Center\mcupdate",
                @"Microsoft\Windows\Media Center\MediaCenterRecoveryTask",
                @"Microsoft\Windows\Media Center\ObjectStoreRecoveryTask",
                @"Microsoft\Windows\Media Center\OCURActivate",
                @"Microsoft\Windows\Media Center\OCURDiscovery",
                @"Microsoft\Windows\Media Center\PBDADiscovery",
                @"Microsoft\Windows\Media Center\PBDADiscoveryW1",
                @"Microsoft\Windows\Media Center\PBDADiscoveryW2",
                @"Microsoft\Windows\Media Center\PvrRecoveryTask",
                @"Microsoft\Windows\Media Center\PvrScheduleTask",
                @"Microsoft\Windows\Media Center\RegisterSearch",
                @"Microsoft\Windows\Media Center\ReindexSearchRoot",
                @"Microsoft\Windows\Media Center\SqlLiteRecoveryTask",
                @"Microsoft\Windows\Media Center\UpdateRecordPath",
                @"Microsoft\Windows\PI\Sqm-Tasks",
                @"Microsoft\Windows\Power Efficiency Diagnostics\AnalyzeSystem",
                @"Microsoft\Windows\Shell\FamilySafetyMonitor",
                @"Microsoft\Windows\Shell\FamilySafetyRefresh",
                @"Microsoft\Windows\Shell\FamilySafetyUpload",
                @"Microsoft\Windows\Windows Error Reporting\QueueReporting"
            };
            foreach (var currentTask in disabletaskslist)
            {
                ProcStartargs("SCHTASKS", $"/Change /TN \"{currentTask}\" /disable");
                _OutPut("Disabled task: " + currentTask);
            }
        }

        private void AddToHostsAndFirewall()
        {
            try
            {
                string[] hostsdomains =
                {
                    "a.ads1.msn.com",
                    "a.ads2.msads.net",
                    "a.ads2.msn.com",
                    "a.rad.msn.com",
                    "a-0001.a-msedge.net",
                    "a-0002.a-msedge.net",
                    "a-0003.a-msedge.net",
                    "a-0004.a-msedge.net",
                    "a-0005.a-msedge.net",
                    "a-0006.a-msedge.net",
                    "a-0007.a-msedge.net",
                    "a-0008.a-msedge.net",
                    "a-0009.a-msedge.net",
                    "ac3.msn.com",
                    "ad.doubleclick.net",
                    "adnexus.net",
                    "adnxs.com",
                    "ads.msn.com",
                    "ads1.msads.net",
                    "ads1.msn.com",
                    "aidps.atdmt.com",
                    "aka-cdn-ns.adtech.de",
                    "a-msedge.net",
                    "apps.skype.com",
                    "az361816.vo.msecnd.net",
                    "az512334.vo.msecnd.net",
                    "b.ads1.msn.com",
                    "b.ads2.msads.net",
                    "b.rad.msn.com",
                    "bs.serving-sys.com",
                    "c.atdmt.com",
                    "c.msn.com",
                    "ca.telemetry.microsoft.com",
                    "cache.datamart.windows.com",
                    "cdn.atdmt.com",
                    "cds26.ams9.msecn.net",
                    "choice.microsoft.com",
                    "choice.microsoft.com.nsatc.net",
                    "compatexchange.cloudapp.net",
                    "corp.sts.microsoft.com",
                    "corpext.msitadfs.glbdns2.microsoft.com",
                    "cs1.wpc.v0cdn.net",
                    "db3aqu.atdmt.com",
                    "db3wns2011111.wns.windows.com",
                    "df.telemetry.microsoft.com",
                    "diagnostics.support.microsoft.com",
                    "ec.atdmt.com",
                    "fe2.update.microsoft.com.akadns.net",
                    "fe3.delivery.dsp.mp.microsoft.com.nsatc.net",
                    "feedback.microsoft-hohm.com",
                    "feedback.search.microsoft.com",
                    "feedback.windows.com",
                    "flex.msn.com",
                    "g.msn.com",
                    "h1.msn.com",
                    "i1.services.social.microsoft.com",
                    "i1.services.social.microsoft.com.nsatc.net",
                    "lb1.www.ms.akadns.net",
                    "live.rads.msn.com",
                    "m.adnxs.com",
                    "m.hotmail.com",
                    "msedge.net",
                    "msftncsi.com",
                    "msnbot-207-46-194-33.search.msn.com",
                    "msnbot-65-55-108-23.search.msn.com",
                    "msntest.serving-sys.com",
                    "oca.telemetry.microsoft.com",
                    "oca.telemetry.microsoft.com.nsatc.net",
                    "pre.footprintpredict.com",
                    "preview.msn.com",
                    "pricelist.skype.com",
                    "rad.live.com",
                    "rad.msn.com",
                    "redir.metaservices.microsoft.com",
                    "reports.wes.df.telemetry.microsoft.com",
                    "s.gateway.messenger.live.com",
                    "s0.2mdn.net",
                    "schemas.microsoft.akadns.net ",
                    "secure.adnxs.com",
                    "secure.flashtalking.com",
                    "services.wes.df.telemetry.microsoft.com",
                    "settings.data.microsoft.com",
                    "settings-sandbox.data.microsoft.com",
                    "settings-win.data.microsoft.com",
                    "sls.update.microsoft.com.akadns.net",
                    "sO.2mdn.net",
                    "spynet2.microsoft.com",
                    "spynetalt.microsoft.com",
                    "sqm.df.telemetry.microsoft.com",
                    "sqm.telemetry.microsoft.com",
                    "sqm.telemetry.microsoft.com.nsatc.net",
                    "ssw.live.com",
                    "static.2mdn.net",
                    "statsfe1.ws.microsoft.com",
                    "statsfe2.update.microsoft.com.akadns.net",
                    "statsfe2.ws.microsoft.com",
                    "survey.watson.microsoft.com",
                    "telecommand.telemetry.microsoft.com",
                    "telecommand.telemetry.microsoft.com.nsatc.net",
                    "telecommand.telemetry.microsoft.com.nsat­c.net",
                    "telemetry.appex.bing.net",
                    "telemetry.appex.bing.net:443",
                    "telemetry.microsoft.com",
                    "telemetry.urs.microsoft.com",
                    "ui.skype.com",
                    "v10.vortex-win.data.microsoft.com",
                    "view.atdmt.com",
                    "vortex.data.microsoft.com",
                    "vortex-bn2.metron.live.com.nsatc.net",
                    "vortex-cy2.metron.live.com.nsatc.net",
                    "vortex-sandbox.data.microsoft.com",
                    "vortex-win.data.microsoft.com",
                    "watson.live.com",
                    "watson.microsoft.com",
                    "watson.ppe.telemetry.microsoft.com",
                    "watson.telemetry.microsoft.com",
                    "watson.telemetry.microsoft.com.nsatc.net",
                    "wes.df.telemetry.microsoft.com",
                    "win10.ipv6.microsoft.com",
                    "www.msftncsi.com",
                };
                var hostslocation = _system32Location + @"drivers\etc\hosts";
                string hosts = null;
                if (File.Exists(hostslocation))
                {
                    hosts = File.ReadAllText(hostslocation);
                    File.SetAttributes(hostslocation, FileAttributes.Normal);
                    DeleteFile(hostslocation);
                }
                File.Create(hostslocation).Close();
                File.WriteAllText(hostslocation, hosts + Environment.NewLine);
                foreach (
                    var currentHostsDomain in
                        hostsdomains.Where(
                            currentHostsDomain =>
                                hosts != null && hosts.IndexOf(currentHostsDomain, StringComparison.Ordinal) == -1))
                {
                    RunCmd($"/c echo 0.0.0.0 {currentHostsDomain} >> \"{hostslocation}\"");
                    _OutPut($"Add to hosts - {currentHostsDomain}");
                }
            }
            catch (Exception ex)
            {
                _errorsList.Add($"Error add to hosts. Message: {ex.Message}");
                _fatalErrors++;
                _OutPut("Error add HOSTS", LogLevel.Error);
#if DEBUG
                _OutPut(ex.Message, LogLevel.Debug);
#endif
            }
            RunCmd("/c ipconfig /flushdns");

            _OutPut("Add hosts MS complete.");
            BlockIpAddr();
        }

        private void RemoveWindows10Apps()
        {
            if (checkBoxDeleteApp3d.Checked)
            {
                DeleteWindows10MetroApp("3d");
                _OutPut("Delete builder 3D");
            }

            if (checkBoxDeleteMailCalendarMaps.Checked)
            {
                DeleteWindows10MetroApp("communi");
                DeleteWindows10MetroApp("maps");
                _OutPut("Delete Mail, Calendar, Maps");
            }
            if (checkBoxDeleteAppBing.Checked)
            {
                DeleteWindows10MetroApp("bing");
                _OutPut("Delete Money, Sports, News and Weather");
            }
            if (checkBoxDeleteAppZune.Checked)
            {
                DeleteWindows10MetroApp("zune");
                _OutPut("Delete Groove Music and Film TV");
            }
            if (checkBoxDeleteAppPeopleOneNote.Checked)
            {
                DeleteWindows10MetroApp("people");
                DeleteWindows10MetroApp("note");
                _OutPut("Delete People and OneNote");
            }
            if (checkBoxDeleteAppPhone.Checked)
            {
                DeleteWindows10MetroApp("phone");
                _OutPut("Delete Phone Companion");
            }

            if (checkBoxDeleteAppSolit.Checked)
            {
                DeleteWindows10MetroApp("solit");
                _OutPut("Delete Solitaire Collection");
            }

            if (checkBoxDeleteAppXBOX.Checked)
            {
                DeleteWindows10MetroApp("xbox");
                _OutPut("Delete XBOX");
            }
        }

        private void checkBoxDeleteWindows10Apps_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxDeleteApp3d.Enabled = checkBoxDeleteWindows10Apps.Checked;
            checkBoxDeleteAppBing.Enabled = checkBoxDeleteWindows10Apps.Checked;
            checkBoxDeleteAppPeopleOneNote.Enabled = checkBoxDeleteWindows10Apps.Checked;
            checkBoxDeleteAppPhone.Enabled = checkBoxDeleteWindows10Apps.Checked;
            checkBoxDeleteAppSolit.Enabled = checkBoxDeleteWindows10Apps.Checked;
            checkBoxDeleteAppXBOX.Enabled = checkBoxDeleteWindows10Apps.Checked;
            checkBoxDeleteAppZune.Enabled = checkBoxDeleteWindows10Apps.Checked;
            checkBoxDeleteMailCalendarMaps.Enabled = checkBoxDeleteWindows10Apps.Checked;
        }

        private void btnDeleteAllWindows10Apps_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("Really", "Question", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) ==
                DialogResult.Yes)
            {
                EnableOrDisableTab(false);
                MessageBox.Show("PressOkAndWait15");
                new Thread(() =>
                {
                    DeleteWindows10MetroApp(null);
                    Invoke(new MethodInvoker(delegate
                    {
                        EnableOrDisableTab(true);
                        MessageBox.Show("Complete", "Info", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }));
                }).Start();
            }
            else
            {
                MessageBox.Show(@"=(", @"%(");
            }
        }

        private void btnRestoreSystem_Click(object sender, EventArgs e)
        {
            Process.Start(_system32Location + "rstrui.exe");
        }

        private void btnEnableUac_Click(object sender, EventArgs e)
        {
            SetRegValueHklm(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\", "EnableLUA", "1",
                RegistryValueKind.DWord);
            _OutPut("Enable UAC");

            if (
                MessageBox.Show("CompleteMSG", "Info", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Process.Start("shutdown.exe", "-r -t 0");
            }
        }

        private void btnDisableUac_Click(object sender, EventArgs e)
        {
            SetRegValueHklm(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\", "EnableLUA", "0",
                RegistryValueKind.DWord);
            _OutPut("Disable UAC");
            if (
                MessageBox.Show("CompleteMSG", "Info", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Process.Start("shutdown.exe", "-r -t 0");
            }
        }

        private void btnEnableWindowsUpdate_Click(object sender, EventArgs e)
        {
            ProgressBarStatus1.Value = 0;
            ProgressBarStatus1.Value = 10;
            ProcStartargs("powershell", "-command \"Set-Service -Name wuauserv -StartupType Automatic\"");
            ProgressBarStatus1.Value = 25;
            RunCmd("/c net start wuauserv");
            ProgressBarStatus1.Value = 60;
            RunCmd("/c netsh advfirewall firewall delete rule name=\"WindowsUpdateBlock\"");
            ProgressBarStatus1.Value = 100;
            _OutPut("Windows Update enabled");
            MessageBox.Show("Система обновлений Windows 10 успешно включена!", "Информация", MessageBoxButtons.OK,
            MessageBoxIcon.Information);
        }

        private void btnDisableWindowsUpdate_Click(object sender, EventArgs e)
        {
            ProgressBarStatus1.Value = 0;
            ProgressBarStatus1.Value = 10;
            RunCmd("/c net stop wuauserv");
            ProgressBarStatus1.Value = 25;
            ProcStartargs("powershell", "-command \"Set-Service -Name wuauserv -StartupType Disabled\"");
            ProgressBarStatus1.Value = 45;
            RunCmd("/c netsh advfirewall firewall delete rule name=\"WindowsUpdateBlock\"");
            ProgressBarStatus1.Value = 70;

            RunCmd(
                "/c netsh advfirewall firewall add rule name=\"WindowsUpdateBlock\" dir=out interface=any action=block service=wuauserv");
            ProgressBarStatus1.Value = 100;

            _OutPut("Windows Update disabled");
            MessageBox.Show("Система обновлений Windows 10 успешно отключена!", "Информация", MessageBoxButtons.OK,
            MessageBoxIcon.Information);
        }


        private void DestroyWindowsSpyingMainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void DestroyWindowsSpyingMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }


        private void btnDeleteOneDrive_Click(object sender, EventArgs e)
        {
            // Показываем сообщение пользователю с вопросом о желании создать компактную ОС
            DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить Microsoft OneDrive?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ProgressBarStatus1.Value = 0;
                ProgressBarStatus1.Value = 35;
                DeleteOneDrive();
                ProgressBarStatus1.Value = 100;
            }
        }

        void DeleteOneDrive()
        {

            try
            {
                RunCmd("/c taskkill /f /im OneDrive.exe > NUL 2>&1");
                RunCmd("/c ping 127.0.0.1 -n 5 > NUL 2>&1");
                if (File.Exists(_systemPath + @"Windows\System32\OneDriveSetup.exe"))
                    ProcStartargs(_systemPath + @"Windows\System32\OneDriveSetup.exe", "/uninstall");
                if (File.Exists(_systemPath + @"Windows\SysWOW64\OneDriveSetup.exe"))
                    ProcStartargs(_systemPath + @"Windows\SysWOW64\OneDriveSetup.exe", "/uninstall");
                RunCmd("/c ping 127.0.0.1 -n 5 > NUL 2>&1");
                RunCmd("/c rd \"%USERPROFILE%\\OneDrive\" /Q /S > NUL 2>&1");
                RunCmd("/c rd \"C:\\OneDriveTemp\" /Q /S > NUL 2>&1");
                RunCmd("/c rd \"%LOCALAPPDATA%\\Microsoft\\OneDrive\" /Q /S > NUL 2>&1");
                RunCmd("/c rd \"%PROGRAMDATA%\\Microsoft OneDrive\" /Q /S > NUL 2>&1");
                RunCmd(
                    "/c REG DELETE \"HKEY_CLASSES_ROOT\\CLSID\\{018D5C66-4533-4307-9B53-224DE2ED1FE6}\" /f > NUL 2>&1");
                RunCmd(
                    "/c REG DELETE \"HKEY_CLASSES_ROOT\\Wow6432Node\\CLSID\\{018D5C66-4533-4307-9B53-224DE2ED1FE6}\" /f > NUL 2>&1");

                SetRegValueHklm(@"SOFTWARE\Policies\Microsoft\Windows\OneDrive", "DisableFileSyncNGSC", "1",
                    RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                Invoke(new MethodInvoker(delegate
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }));
#if DEBUG
                    _OutPut(ex.Message, LogLevel.Debug);
#endif
            };
        }



        private void btnRemoveOldFirewallRules_Click(object sender, EventArgs e)
        {
            string[] rulename =
            {
                "MS Spynet block 1",
                "MS Spynet block 2",
                "MS telemetry block 1",
                "MS telemetry block 2",
                "185.13.160.61_Block",
                "184.86.56.12_Block",
                "204.79.197.200_Block" // bing.com
            };
            foreach (var hostname in rulename)
            {
                RunCmd($"/c netsh advfirewall firewall delete rule name=\"{hostname}\"");
            }

            MessageBox.Show("Complete", "Info");
        }



        private void btnFixRotateScreen_Click(object sender, EventArgs e)
        {
            SetRegValueHklm(@"SOFTWARE\Policies\Microsoft\Windows\LocationAndSensors", "DisableSensors", "0", RegistryValueKind.DWord);
            
        }





        private readonly string[] _updatesnumberlist =
        {
            "2952664",
            "2976978",
            "2990214",
            "3021917",
            "3035583",
            "3042058",
            "3044374",
            "3050265",
            "3065987",
            "3065988",
            "3068708",
            "3075249",
            "3075851",
            "3075853",
            "3080149",
            "3083324",
            "3083325",
            "3083710",
            "3083711",
            "3088195",
            "3093513",
            "3093983",
            "3102810",
            "3112336",
            "971033",
            "976932"
            // THX rgadguard
        };



        private void BlockIpAddr()
        {
            string[] ipAddr =
            {
                "104.96.147.3",
                "111.221.29.177",
                "111.221.29.253",
                "111.221.64.0-111.221.127.255", // singapure
                "131.253.40.37",
                "134.170.115.60",
                "134.170.165.248",
                "134.170.165.253",
                "134.170.185.70",
                "134.170.30.202",
                "137.116.81.24",
                "137.117.235.16",
                "157.55.129.21",
                "157.55.130.0-157.55.130.255",
                "157.55.133.204",
                "157.55.235.0-157.55.235.255",
                "157.55.236.0-157.55.236.255", // NEW TH2 SPY IP
                "157.55.240.220",
                "157.55.52.0-157.55.52.255",
                "157.55.56.0-157.55.56.255",
                "157.56.106.189",
                "157.56.121.89",
                "157.56.124.87", // NEW TH2 Spy IP
                "157.56.91.77",
                "157.56.96.54",
                "168.63.108.233",
                "191.232.139.2",
                "191.232.139.254",
                "191.232.80.58",
                "191.232.80.62",
                "191.237.208.126",
                "195.138.255.0-195.138.255.255",
                "2.22.61.43",
                "2.22.61.66",
                "204.79.197.200",
                "207.46.101.29",
                "207.46.114.58",
                "207.46.223.94",
                "207.68.166.254",
                "212.30.134.204",
                "212.30.134.205",
                "213.199.179.0-213.199.179.255", // Ireland
                "23.102.21.4",
                "23.218.212.69",
                "23.223.20.82", // cache.datamart.windows.com
                "23.57.101.163",
                "23.57.107.163",
                "23.57.107.27",
                "23.99.10.11",
                "64.4.23.0-64.4.23.255",
                "64.4.54.22",
                "64.4.54.32",
                "64.4.6.100",
                "65.39.117.230",
                "65.39.117.230",
                "65.52.100.11",
                "65.52.100.7",
                "65.52.100.9",
                "65.52.100.91",
                "65.52.100.92",
                "65.52.100.93",
                "65.52.100.94",
                "65.52.108.29",
                "65.52.108.33",
                "65.55.108.23",
                "65.55.138.114",
                "65.55.138.126",
                "65.55.138.186",
                "65.55.223.0-65.55.223.255",
                "65.55.252.63",
                "65.55.252.71",
                "65.55.252.92",
                "65.55.252.93",
                "65.55.29.238",
                "65.55.39.10",
                "77.67.29.176" // NEW TH2 Spy IP
            };
            foreach (var currentIpAddr in ipAddr)
            {
                RunCmd($"/c route -p ADD {currentIpAddr} MASK 255.255.255.255 0.0.0.0");
                RunCmd($"/c route -p change {currentIpAddr} MASK 255.255.255.255 0.0.0.0 if 1");
                RunCmd($"/c netsh advfirewall firewall delete rule name=\"{currentIpAddr}_Block\"");
                RunCmd(
                    string.Format(
                        "/c netsh advfirewall firewall add rule name=\"{0}_Block\" dir=out interface=any action=block remoteip={0}",
                        currentIpAddr));
                _OutPut($"Add Windows Firewall rule: \"{currentIpAddr}_Block\"");
            }
            RunCmd("/c netsh advfirewall firewall delete rule name=\"Explorer.EXE_BLOCK\"");
            RunCmd(
                $"/c netsh advfirewall firewall add rule name=\"Explorer.EXE_BLOCK\" dir=out interface=any action=block program=\"{_systemPath}Windows\\explorer.exe\"");
            RunCmd("/c netsh advfirewall firewall delete rule name=\"WSearch_Block\"");
            RunCmd(
                "/c netsh advfirewall firewall add rule name=\"WSearch_Block\" dir=out interface=any action=block service=WSearch");
            _OutPut("Add Windows Firewall rule: \"WSearch_Block\"");
            _OutPut("Ip list blocked");
        }


        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x%2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        public static string ByteArrayToString(byte[] ba)
        {
            var hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }

        private void CaptionWindow_Click(object sender, EventArgs e) { }
        private void checkBoxDeleteAppXBOX_CheckedChanged(object sender, EventArgs e) { }
        private void StatusCommandsLable_Click(object sender, EventArgs e) { }
        private void groupBox3_Enter(object sender, EventArgs e) { }
        private void buttonEnableWindowsUpdate_Click(object sender, EventArgs e) { }
        private void ProgressBarStatus_Click(object sender, EventArgs e) { }
        private void CloseButton_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void MinimizeButton_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void CaptionWindow_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        [DllImport("user32.dll")]
        // ReSharper disable once InconsistentNaming
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void CaptionWindow_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillEllipse(new SolidBrush(Color.WhiteSmoke), 3,3,23,23);
            e.Graphics.DrawImage(Icon.ToBitmap(), 6, 5, 19, 19);
        }

        public static Color Rainbow(float progress)
        {
            var div = (Math.Abs(progress%1)*6);
            var ascending = (int) ((div%1)*255);
            var descending = 255 - ascending;

            switch ((int) div)
            {
                case 0:
                    return Color.FromArgb(255, 255, ascending, 0);
                case 1:
                    return Color.FromArgb(255, descending, 255, 0);
                case 2:
                    return Color.FromArgb(255, 0, 255, ascending);
                case 3:
                    return Color.FromArgb(255, 0, descending, 255);
                case 4:
                    return Color.FromArgb(255, ascending, 0, 255);
                default: // case 5:
                    return Color.FromArgb(255, 255, 0, descending);
            }
        }

        // Функция для дефрагментации всех дисков
        public static void DefragmentAllDrives()
        {
            try
            {
                foreach (DriveInfo drive in DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed))
                {
                    string driveLetter = drive.Name;
                    ExecuteCommand($"defrag {driveLetter} /O"); // Оптимизация и дефрагментация
                    Console.WriteLine($"{driveLetter} drive defragmented.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during defragmentation: " + ex.Message);
            }
        }

        // Функция для очистки всех дисков
        public static void CleanupAllDrives()
        {
            try
            {
                foreach (DriveInfo drive in DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed))
                {
                    string driveLetter = drive.Name.TrimEnd('\\');
                    ExecuteCommand($"cleanmgr /d {driveLetter} /sagerun:1");
                    Console.WriteLine($"{driveLetter} drive cleaned.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during disk cleanup: " + ex.Message);
            }
        }

        // Функция для очистки всех папок Temp
        public static void ClearAllTempFolders()
        {
            try
            {
                string userTempPath = Path.GetTempPath();
                DeleteFilesInDirectory(userTempPath);

                string systemTempPath = Environment.GetEnvironmentVariable("TEMP", EnvironmentVariableTarget.Machine);
                if (!string.IsNullOrEmpty(systemTempPath))
                {
                    DeleteFilesInDirectory(systemTempPath);
                }

                Console.WriteLine("Temp folders cleared.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during temp folder cleanup: " + ex.Message);
            }
        }

        // Функция для очистки корзины
        public static void EmptyRecycleBin()
        {
            try
            {
                SHEmptyRecycleBin(IntPtr.Zero, null, RecycleFlags.SHRB_NOCONFIRMATION | RecycleFlags.SHRB_NOPROGRESSUI | RecycleFlags.SHRB_NOSOUND);
                Console.WriteLine("Recycle bin emptied.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error emptying recycle bin: " + ex.Message);
            }
        }

        // Вспомогательная функция для удаления файлов в директории
        private static void DeleteFilesInDirectory(string path)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }

                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting files in directory: " + ex.Message);
            }
        }



        // Импортируем функцию SHEmptyRecycleBin из shell32.dll
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern int SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlags dwFlags);

        // Флаги для SHEmptyRecycleBin
        [Flags]
        private enum RecycleFlags : uint
        {
            SHRB_NOCONFIRMATION = 0x00000001,  // Не показывать подтверждающее диалоговое окно
            SHRB_NOPROGRESSUI = 0x00000002,    // Не показывать индикатор выполнения
            SHRB_NOSOUND = 0x00000004          // Не проигрывать звук после очистки корзины
        }

        // Функция для открытия окна автозагрузки приложений
        public static void OpenStartupAppsSettings()
        {
            try
            {
                Process.Start("ms-settings:startupapps");
                Console.WriteLine("Startup apps settings opened.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error opening startup apps settings: " + ex.Message);
            }
        }

        // Функция для создания компактной операционной системы
        public static void CreateCompactOS()
        {
            try
            {   

                // Отключение гибернации
                ExecuteCommand("powercfg /h off");

                // Очистка временных файлов
                ExecuteCommand("cleanmgr /sagerun:1");

                // Сжатие системных файлов
                ExecuteCommand("compact.exe /CompactOS:always");

                MessageBox.Show("Compact OS создана успешно.", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании компактной ОС: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Функция для удаления файла гибернации и отключения гибернации
        public static void DisableHibernate()
        {
            try
            {
                // Отключаем гибернацию
                ExecuteCommand("powercfg /h off");

                // Удаляем файл гибернации
                DeleteHiberfil();
                
                Console.WriteLine("Hibernate disabled successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error disabling hibernate: " + ex.Message);
            }
        }

        // Вспомогательная функция для удаления файла гибернации
        private static void DeleteHiberfil()
        {
            try
            {
                // Получаем имя файла гибернации
                string hiberfilPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\hiberfil.sys";

                // Удаляем файл гибернации
                if (File.Exists(hiberfilPath))
                {
                    File.Delete(hiberfilPath);
                    Console.WriteLine("Hiberfil.sys deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Hiberfil.sys not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting hiberfil.sys: " + ex.Message);
            }
        }



        static void KillEdgeProcesses()
        {
            ExecuteCommand("taskkill /F /IM msedge.exe");
        }

        static void RemoveEdgeDirectories()
        {
            string[] directories = {
                @"C:\Windows\SystemApps\Microsoft.MicrosoftEdge_8wekyb3d8bbwe",
                @"C:\Program Files (x86)\Microsoft\Edge",
                @"C:\Program Files (x86)\Microsoft\EdgeUpdate",
                @"C:\Program Files (x86)\Microsoft\EdgeCore",
                @"C:\Program Files (x86)\Microsoft\EdgeWebView"
            };

            foreach (var dir in directories)
            {
                KillDir(dir);
            }
        }

        static void KillDir(string targetDir)
        {
            if (Directory.Exists(targetDir))
            {
                ExecuteCommand($"takeown /a /r /d Y /f \"{targetDir}\"");
                ExecuteCommand($"icacls \"{targetDir}\" /grant administrators:f /t");
                Directory.Delete(targetDir, true);
                Console.WriteLine($"Removed dir {targetDir}");
            }
            else
            {
                Console.WriteLine($"{targetDir} does not exist.");
            }
        }

        static void ModifyRegistry()
        {
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\EdgeUpdate"))
            {
                key.SetValue("DoNotUpdateToEdgeWithChromium", 1, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Active Setup\Installed Components", true))
            {
                key.DeleteSubKey("{9459C573-B17A-45AE-9F64-1857B5D58CEE}", false);
            }

            Console.WriteLine("Registry modified.");
        }

        static void RemoveEdgeShortcuts()
        {
            string[] shortcuts = {
                @"C:\Users\Public\Desktop\Microsoft Edge.lnk",
                @"%ProgramData%\Microsoft\Windows\Start Menu\Programs\Microsoft Edge.lnk",
                @"%APPDATA%\Microsoft\Internet Explorer\Quick Launch\User Pinned\TaskBar\Microsoft Edge.lnk"
            };

            foreach (var shortcut in shortcuts)
            {
                DelShortcut(Environment.ExpandEnvironmentVariables(shortcut));
            }

            foreach (var user in Directory.GetDirectories(@"C:\Users"))
            {
                DelShortcut(Path.Combine(user, "Desktop", "edge.lnk"));
                DelShortcut(Path.Combine(user, "Desktop", "Microsoft Edge.lnk"));
            }
        }

        static void DelShortcut(string shortcutPath)
        {
            if (File.Exists(shortcutPath))
            {
                File.Delete(shortcutPath);
                Console.WriteLine($"Removed shortcut {shortcutPath}");
            }
            else
            {
                Console.WriteLine($"{shortcutPath} does not exist.");
            }
        }


        static void DisableBackgroundApps()
        {
            try
            {
                
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications", true))
                {
                    if (key != null)
                    {
                        key.SetValue("GlobalUserDisabled", 1, RegistryValueKind.DWord);
                        Console.WriteLine("Background apps disabled.");
                    }
                    else
                    {
                        Console.WriteLine("BackgroundAccessApplications registry key not found.");
                    }
                }

                using (RegistryKey keyLocalMachine = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", true))
                {
                    if (keyLocalMachine == null)
                    {
                        using (RegistryKey newKeyLocalMachine = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"))
                        {
                            newKeyLocalMachine.SetValue("LetAppsRunInBackground", 2, RegistryValueKind.DWord);
                            Console.WriteLine("App background running policy set.");
                        }
                    }
                    else
                    {
                        keyLocalMachine.SetValue("LetAppsRunInBackground", 2, RegistryValueKind.DWord);
                        Console.WriteLine("App background running policy set.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error disabling background apps: {ex.Message}");
            }
        }


        // Вспомогательная функция для выполнения командной строки
        private static void ExecuteCommand(string command)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C " + command;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrEmpty(output))
            {
                Console.WriteLine(output);
            }

            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine("ERROR: " + error);
            }
        }
        #region Output

        private void LogOutputTextBox_TextChanged(object sender, EventArgs e)
        {
            LogOutputTextBox.SelectionStart = LogOutputTextBox.Text.Length;
            LogOutputTextBox.ScrollToCaret();
        }

        private void _OutPutSplit()
        {
            try
            {
                Invoke(new MethodInvoker(OutPutSplitInvoke));
            }
            catch
            {
                try
                {
                    OutPutSplitInvoke();
                }
                catch (Exception ex)
                {
                    _fatalErrors++;
                    _errorsList.Add($"Error in outputsplit. Message: {ex.Message}");
                }
            }
        }

        private void _OutPut(string str, LogLevel logLevel = LogLevel.Info)
        {
            try
            {
                Invoke(new MethodInvoker(delegate { _OutPutInvoke(str, logLevel); }));
            }
            catch
            {
                try
                {
                    _OutPutInvoke(str, logLevel);
                }
                catch (Exception ex)
                {
                    _fatalErrors++;
                    _errorsList.Add($"Error in output. Message: {ex.Message}");
                }
            }
        }

        private enum LogLevel // thx TRoskop
        {
            Info,
            Warning,
            Error,
            FatalError,
            Debug
        };

        private void _OutPutInvoke(string str, LogLevel logLevel)
        {
            if (logLevel == LogLevel.Debug && string.IsNullOrEmpty(str))
                return;

            switch (logLevel)
            {
                case LogLevel.Info:
                    str = "[INFO] " + str;
                    break;
                case LogLevel.Warning:
                    str = "[WARNING] " + str;
                    break;
                case LogLevel.Error:
                    str = "[ERROR] " + str;
                    break;
                case LogLevel.FatalError:
                    str = "[!! FATAL ERROR !!] " + str;
                    break;
                case LogLevel.Debug:
                    str = "[DEBUG] " + str;
                    break;
            }
            File.WriteAllText(LogFileName, File.ReadAllText(LogFileName) + str + Environment.NewLine);
            Console.WriteLine(str);
            LogOutputTextBox.Text += str + Environment.NewLine;
        }

        private void OutPutSplitInvoke()
        {
            var splittext = $"=========================={Environment.NewLine}";
            File.WriteAllText(LogFileName, File.ReadAllText(LogFileName) + splittext);
            LogOutputTextBox.Text += splittext;
        }

        #endregion

        #region registry

        private void SetRegValueHkcu(string regkeyfolder, string paramname, string paramvalue, RegistryValueKind keytype)
        {
            var registryKey = Registry.CurrentUser.CreateSubKey(regkeyfolder);
            registryKey?.Close();
            var myKey = Registry.CurrentUser.OpenSubKey(regkeyfolder, RegistryKeyPermissionCheck.ReadWriteSubTree,
                RegistryRights.FullControl);
            try
            {
                myKey?.SetValue(paramname, paramvalue, keytype);
            }
            catch (Exception ex)
            {
                _fatalErrors++;
                _errorsList.Add($"Error SetRegValueHkcu. Message: {ex.Message}");
                
            }

            myKey?.Close();
        }

        private void SetRegValueHklm(string regkeyfolder, string paramname, string paramvalue, RegistryValueKind keytype)
        {
            var registryKey = Registry.LocalMachine.CreateSubKey(regkeyfolder);
            registryKey?.Close();
            var myKey = Registry.LocalMachine.OpenSubKey(regkeyfolder,
                RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl);
            try
            {
                myKey?.SetValue(paramname, paramvalue, keytype);
            }
            catch (Exception ex)
            {
                _fatalErrors++;
                _errorsList.Add($"Error SetRegValueHklm. Message: {ex.Message}");
                
            }
            myKey?.Close();
        }


        #endregion




        private void delete_trash_button_Click(object sender, EventArgs e)
        {
            // Показываем сообщение пользователю с вопросом о желании создать компактную ОС
            DialogResult result = MessageBox.Show("Будет проведена дефрагментация дисков,\n" +
                                                  "очистка дисков от кэша, очистка корзины\n" +
                                                  "и очистка папки TEMP. Подтвердите, что \n" +
                                                  "вы согласны на очистку от мусора.", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ProgressBarStatus1.Value = 0;
                ProgressBarStatus1.Value = 10;
                DefragmentAllDrives();
                ProgressBarStatus1.Value = 25;
                CleanupAllDrives();
                ProgressBarStatus1.Value = 50;
                ClearAllTempFolders();
                ProgressBarStatus1.Value = 75;
                EmptyRecycleBin();
                ProgressBarStatus1.Value = 100;
            }
        }

        private void startup_button_Click(object sender, EventArgs e)
        {
            OpenStartupAppsSettings();
        }

        private void create_compactos_button_Click(object sender, EventArgs e)
        {
            // Показываем сообщение пользователю с вопросом о желании создать компактную ОС
            DialogResult result = MessageBox.Show("Вы уверены, что хотите создать компактную версию операционной системы?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ProgressBarStatus1.Value = 0;
                ProgressBarStatus1.Value = 40;
                CreateCompactOS();
                ProgressBarStatus1.Value = 100;
            }
        }

        private void delete_hiber_button_Click(object sender, EventArgs e)
        {
            // Показываем сообщение пользователю с вопросом о желании создать компактную ОС
            DialogResult result = MessageBox.Show("Гибернация - это функция, которая сохраняет\n" +
                                                  "текущее состояние операционной системы и\n" +
                                                  "отключает компьютер или ноутбук так, чтобы\n" +
                                                  "при последующем включении можно было быстро\n" +
                                                  "вернуться к тому же состоянию. Однако файл\n" +
                                                  "гибернации (hiberfil.sys) может занимать\n" +
                                                  "значительное количество места на диске, особенно\n" +
                                                  "на ноутбуках с ограниченным объемом памяти.\n\n" +
                                                  "Подтвердите, что вы хотите удалить файл гибернации. ", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ProgressBarStatus1.Value = 0;
                ProgressBarStatus1.Value = 60;
                DisableHibernate();
                ProgressBarStatus1.Value = 100;

            }
            }

        private void delete_edge_button_Click(object sender, EventArgs e)
        {
            // Показываем сообщение пользователю с вопросом о желании создать компактную ОС
            DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить Microsoft Edge?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ProgressBarStatus1.Value = 0;
                ProgressBarStatus1.Value = 15;
                KillEdgeProcesses();
                ProgressBarStatus1.Value = 35;
                RemoveEdgeDirectories();
                ProgressBarStatus1.Value = 50;
                ModifyRegistry();
                ProgressBarStatus1.Value = 75;
                RemoveEdgeShortcuts();
                ProgressBarStatus1.Value = 100;
            }
        }





        private void disable_fonapps_button_Click(object sender, EventArgs e)
        {
            // Показываем сообщение пользователю с вопросом о желании создать компактную ОС
            DialogResult result = MessageBox.Show("Вы уверены, что хотите отключить фоновые приложения?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ProgressBarStatus1.Value = 0;
                ProgressBarStatus1.Value = 35;
                DisableBackgroundApps();
                ProgressBarStatus1.Value = 100;
            }
        }

        private void disable_hyperv_button_Click(object sender, EventArgs e)
        {
            // Показываем сообщение пользователю с вопросом о желании создать компактную ОС
            DialogResult result = MessageBox.Show("Вы уверены, что хотите отключить  процессы HyperV?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                ProgressBarStatus1.Value = 0;
                ProgressBarStatus1.Value = 50;
                ExecuteCommand("bcdedit /set hypervisorlaunchtype off");
                ProgressBarStatus1.Value = 100;
                
            }
        }

        private void enable_hyperv_button_Click(object sender, EventArgs e)
        {
            // Показываем сообщение пользователю с вопросом о желании создать компактную ОС
            DialogResult result = MessageBox.Show("Вы уверены, что хотите включить  процессы HyperV?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                ProgressBarStatus1.Value = 0;
                ProgressBarStatus1.Value = 50;
                // Вызываем функцию для включения гипервизора
                ExecuteCommand("bcdedit /set hypervisorlaunchtype auto");
                
                ProgressBarStatus1.Value = 100;
            }
        }

        private void delete_hyperv_button_Click(object sender, EventArgs e)
        {
            // Показываем сообщение пользователю с вопросом о желании создать компактную ОС
            DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить HyperV?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                ProgressBarStatus1.Value = 0;
                ProgressBarStatus1.Value = 50;
                ExecuteCommand("DISM /Online /Disable-Feature:Microsoft-Hyper-V");
                ProgressBarStatus1.Value = 100;
                

            }
        }
    }
}