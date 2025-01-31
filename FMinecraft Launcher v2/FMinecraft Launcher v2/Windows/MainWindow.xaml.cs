using FMinecraft_Launcher_v2.Pages;
using FMinecraft_Launcher_v2.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Path = System.IO.Path;
using System.IO.Compression;
using System.Net;
using System.Windows.Media.Imaging;
using System.Net.NetworkInformation;

namespace FMinecraft_Launcher_v2
{
    public partial class MainWindow : Window
    {
        // Define a new base path to the FMinecraft Launcher folder in AppData Roaming
        private static readonly string BasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".FMinecraftLauncher");

        #region Launcher Console Management

        #region Launcher Console Enums
        /// <summary>
        /// Represents the type of console output source. This helps to categorize log messages.
        /// </summary>
        public enum ConsoleType
        {
            /// <summary>
            /// Messages originating from the Minecraft game itself.
            /// </summary>
            Minecraft,
            /// <summary>
            /// Messages originating from the Launcher application.
            /// </summary>
            Launcher,
            /// <summary>
            /// Messages from other sources or uncategorized messages.
            /// </summary>
            Other
        }

        /// <summary>
        /// Represents the type of message being logged, indicating its severity or nature.
        /// </summary>
        public enum ConsoleMessageType
        {
            /// <summary>
            /// Default message type, indicating no specific severity.
            /// </summary>
            None,
            /// <summary>
            /// Indicates an error condition that needs attention.
            /// </summary>
            Error,
            /// <summary>
            /// Indicates a potential issue or something that deviates from the expected behavior but is not critical.
            /// </summary>
            Warning,
            /// <summary>
            /// General informational message about the launcher's operation.
            /// </summary>
            Info,
            /// <summary>
            /// Detailed messages intended for debugging purposes, typically not shown to end-users in release versions.
            /// </summary>
            Debug
        }
        #endregion

        #region Launcher Console Variables
        /// <summary>
        /// The console window instance, used to display log messages in a separate window.
        /// </summary>
        public Console_Window console_Window;

        /// <summary>
        /// A flag indicating whether the main window is closed. Used to prevent operations after window closure.
        /// </summary>
        public bool Closed = false;

        /// <summary>
        /// A StringBuilder to accumulate log messages before writing them to a file on application close.
        /// </summary>
        private readonly StringBuilder _log = new StringBuilder();
        #endregion

        #region Launcher Console Method
        /// <summary>
        /// Logs a message to the launcher console, debug output, and the in-memory log.
        /// This method handles formatting the log message with timestamp, source, and message type before outputting it.
        /// It also ensures that UI updates (displaying in the console window) are performed on the UI thread.
        /// </summary>
        /// <param name="consoleType">The source of the console message, categorized by <see cref="ConsoleType"/>.</param>
        /// <param name="messageType">The type of message, indicating severity or purpose, categorized by <see cref="ConsoleMessageType"/>.</param>
        /// <param name="text">The message text to be logged and displayed.</param>
        public void Launcher_Console(ConsoleType consoleType, ConsoleMessageType messageType, string text)
        {
            // Format the log message to include timestamp, console type, and message type.
            string logMessage = $"[{DateTime.Now:HH:mm:ss.fff} | {consoleType} | {messageType}] {text}\n";
            // Append the formatted message to the in-memory log.
            _log.Append(logMessage);

            // Check if the console window is initialized.
            if (console_Window != null)
            {
                // Use Dispatcher.Invoke to marshal the UI update back to the main UI thread.
                // This is crucial because console output updates the TextBox in the console window, which must be done on the UI thread.
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // Append the new log message to the output TextBox in the console window.
                    console_Window.outputTextBox.Text += logMessage;
                });
            }
            else
            {
                // If the console window is not initialized, output the log message to the debug output.
                // This ensures that logs are still captured even if the console window is not visible or initialized.
                Debug.WriteLine(logMessage);
            }
        }
        #endregion

        #endregion

        #region Main Window Initialization

        #region Constructor and Initial Setup
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// This constructor sets up the main application window, initializes components, checks for existing launcher processes,
        /// sets up the console window, and loads the initial page.
        /// </summary>
        public MainWindow()
        {
            Hide();

            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus != OperationalStatus.Up &&
                    ni.NetworkInterfaceType == NetworkInterfaceType.Loopback &&
                    ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel)
                {
                    ShowError("Please connect to the Internet first.", true);
                    Close();
                }
            }

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Starting FMinecraft Launcher v2 initialization.");

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Checking for existing launcher processes to prevent multiple instances.");
            // Check if another instance of the launcher is already running.
            if (CheckProcessesForCurrentExecutable(this))
            {
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Error, "Another instance of the launcher is already running. Terminating current instance.");
                ShowError("There are already other FMinecraft Launcher (v2) processes running and this process cannot be started.", true);
                Close(); // Terminate the current instance if another one is running.
                return; // Exit constructor to prevent further initialization.
            }
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "No other launcher processes found. Proceeding with initialization.");

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Calling InitializeComponent() to initialize WPF components.");
            InitializeComponent(); // Initialize WPF components defined in XAML.
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Component initialization completed.");

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Creating new Console_Window instance for launcher output.");
            console_Window = new Console_Window(this); // Create an instance of the console window, passing the main window as the owner.
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Console window initialized.");

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Setting HomePage as initial content for the main window.");
            Content = new HomePage(this); // Set the initial page content to the HomePage.
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Homepage loaded as initial content.");

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Calling Initialize() method for asynchronous initial setup.");
            Initialize(); // Call the asynchronous initialization method.
        }
        #endregion

        #region Asynchronous Initialization

        public bool isInitialized = false;
        /// <summary>
        /// Performs asynchronous initialization tasks for the launcher.
        /// This includes setting up directories, creating page instances, loading settings, and fetching version manifest.
        /// This method is called after the main window and basic components are initialized.
        /// </summary>
        private async void Initialize()
        {
            #region Loading Window Management
            LoadingWindow loadingWindow = new LoadingWindow(this);
            loadingWindow.Show();
            #endregion

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Starting asynchronous launcher initialization.");

            #region Directory Initialization
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Ensuring '.launcher' directory exists for launcher-specific files.");
            EnsureDirectoryExists(Path.Combine(BasePath, ".launcher")); // Ensure the .launcher directory exists.
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Ensuring '.minecraft' directory exists for Minecraft game files.");
            EnsureDirectoryExists(Path.Combine(BasePath, ".minecraft")); // Ensure the .minecraft directory exists.
            #endregion

            #region Launcher Profiles Setup
            string launcherProfilesPath = Path.Combine(BasePath, ".minecraft", "launcher_profiles.json");
            if (!File.Exists(launcherProfilesPath)) File.WriteAllText(launcherProfilesPath, $@"{{""clientToken"": ""{Guid.NewGuid().ToString("N")}"",""profiles"": {{}}}}");
            #endregion

            #region Page Instance Creation
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Creating HomePage instance for navigation.");
            homePage = new HomePage(this); // Create HomePage instance.
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Homepage instance created.");

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Creating SettingsPage instance for settings management.");
            settingsPage = new SettingsPage(this); // Create SettingsPage instance.
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Settings page instance created.");

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Creating AboutusPage instance for about information.");
            aboutPage = new AboutusPage(this); // Create AboutusPage instance.
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "About us page instance created.");
            #endregion

            #region Settings and Version Manifest Loading
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Loading launcher settings asynchronously from settings file.");
            await LoadLauncherSettingsAsync(); // Load launcher settings from file.
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Loading version manifest asynchronously from Mojang API.");
            await LoadVersionManifestAsync(); // Load Minecraft version manifest from Mojang.
            #endregion

            #region Launcher Cover Loading
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Loading launcher covers from disk.");
            await LoadLauncherCover();
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Launcher covers loaded.");
            #endregion

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Launcher initialization completed successfully.");

            #region Loading Window Completion and Main Window Display
            // Indicate to the loading window that initialization is finished (if needed for animation or finalization).
            loadingWindow.Finish = true;
            isInitialized = true;
            #endregion

        }
        #endregion

        #region Launcher Cover Management
        public async Task LoadLauncherCover()
        {
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Loading launcher covers.");
            //string coversDirectory = Path.Combine(BasePath, ".launcher", "Covers");
            //Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Checking for covers in directory: '{coversDirectory}'.");

            //settingsPage.CoverComboBox.Items.Clear();
            //Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Clearing existing items in Cover ComboBox.");

            //if (!Directory.Exists(coversDirectory))
            //{
            //Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Covers directory does not exist, creating: '{coversDirectory}'.");
            //Directory.CreateDirectory(coversDirectory);
            //}

            //string[] CoversPaths = Directory.GetFiles(coversDirectory);
            // Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Found {CoversPaths.Length} cover files.");

            //foreach (var cover in CoversPaths)
            //{
            //string coverFileName = Path.GetFileName(cover);
            //Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Adding cover to ComboBox: '{coverFileName}'.");
            //settingsPage.CoverComboBox.Items.Add(coverFileName);
            //}
            //Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Launcher covers loading complete.");

            //BitmapImage Cover = new BitmapImage(new Uri(Path.Combine(coversDirectory, settingsPage.CoverComboBox.Text), UriKind.RelativeOrAbsolute));
            //Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, Path.GetFullPath(Path.Combine(coversDirectory, settingsPage.CoverComboBox.Text)));
            //homePage.Cover.Source = Cover;
        }
        #endregion

        #region Directory Management
        /// <summary>
        /// Ensures that a directory exists at the specified path, creating it if it doesn't.
        /// Logs the action taken (creation or existence) using the launcher console.
        /// </summary>
        /// <param name="path">The path to the directory to ensure.</param>
        private void EnsureDirectoryExists(string path)
        {
            // Check if the directory already exists.
            if (!Directory.Exists(path))
            {
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Directory '{path}' does not exist. Creating it.");
                Directory.CreateDirectory(path); // Create the directory if it doesn't exist.
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, $"Created directory '{path}'.");
            }
            else
            {
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Directory '{path}' already exists.");
            }
        }
        #endregion

        #endregion

        #region Launcher Settings Management

        #region Settings Loading and Creation
        /// <summary>
        /// Loads launcher settings from the settings file asynchronously.
        /// If the settings file does not exist, it creates a default settings file.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task LoadLauncherSettingsAsync()
        {
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Loading launcher settings.");

            string settingsFilePath = Path.Combine(BasePath, ".launcher", "settings.json");
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Settings file path: '{settingsFilePath}'.");

            // Check if the settings file exists.
            if (!File.Exists(settingsFilePath))
            {
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Warning, "Settings file not found.");
                CreateDefaultSettingsFile(settingsFilePath); // Create a default settings file if it doesn't exist.
            }

            await ReadSettingsFile(settingsFilePath); // Read settings from the existing file.
        }

        /// <summary>
        /// Creates a default settings file if one does not exist.
        /// Populates the settings file with default values and saves it to disk.
        /// </summary>
        /// <param name="settingsFilePath">The path to the settings file to be created.</param>
        private async void CreateDefaultSettingsFile(string settingsFilePath)
        {
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Creating default settings file.");

            // Initialize a new Settings object with default values.
            var defaultSettings = new Settings
            {
                HideLauncher = true,
                ShowConsole = false,
                CloseLauncher = false,
                ShowReleases = true,
                ShowSnapshot = false,
                ShowAlpha = false,
                TopMost = false,
                AccessToken = "none",
                UserName = "Player"
            };
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Default settings object created.");

            // Serialize the default settings object to JSON format.
            string settingsJson = JsonConvert.SerializeObject(defaultSettings, Formatting.Indented);
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Default settings serialized to JSON.");

            // Ensure the directory for the settings file exists.
            Directory.CreateDirectory(Path.GetDirectoryName(settingsFilePath)!);
            // Write the JSON settings to the settings file asynchronously.
            await File.WriteAllTextAsync(settingsFilePath, settingsJson, Encoding.UTF8);

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Default settings file created successfully.");
        }

        /// <summary>
        /// Reads the settings file and applies the settings to the application.
        /// Deserializes the JSON settings file and calls ApplySettingsToUI to update UI elements.
        /// </summary>
        /// <param name="settingsFilePath">The path to the settings file to read.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task ReadSettingsFile(string settingsFilePath)
        {
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, $"Loading settings from '{settingsFilePath}'.");

            try
            {
                // Read all text from the settings file asynchronously.
                string settingsJson = await File.ReadAllTextAsync(settingsFilePath, Encoding.UTF8);
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Settings file read successfully.");
                // Deserialize the JSON string to a Settings object.
                var settings = JsonConvert.DeserializeObject<Settings>(settingsJson);
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Settings file deserialized successfully.");
                ApplySettingsToUI(settings); // Apply the loaded settings to the UI.
            }
            catch (Exception ex)
            {
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Error, $"Error loading launcher settings from '{settingsFilePath}': {ex.Message}");
            }
        }
        #endregion

        #region Settings Application and Saving
        /// <summary>
        /// Applies the loaded settings to the UI elements of the settings page.
        /// Updates the checkboxes and textboxes in the settings page based on the loaded settings.
        /// </summary>
        /// <param name="settings">The loaded settings object to apply.</param>
        public void ApplySettingsToUI(Settings settings)
        {
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Applying loaded settings to the UI.");
            // Update UI elements on the settings page with loaded settings.
            settingsPage.HideLauncher.IsChecked = settings.HideLauncher;
            settingsPage.ShowConsole.IsChecked = settings.ShowConsole;
            settingsPage.CloseLauncher.IsChecked = settings.CloseLauncher;
            settingsPage.releases.IsChecked = settings.ShowReleases;
            settingsPage.snapshot.IsChecked = settings.ShowSnapshot;
            settingsPage.Alpha.IsChecked = settings.ShowAlpha;
            settingsPage.TopMostLauncher.IsChecked = settings.TopMost;
            settingsPage.UserName.Text = settings.UserName;
            Topmost = settingsPage.TopMostLauncher.IsChecked.Value;
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Launcher settings applied to the settings page.");
        }

        /// <summary>
        /// Saves the current launcher settings asynchronously to the settings file.
        /// Serializes the provided settings object to JSON and writes it to the settings file.
        /// </summary>
        /// <param name="settings">The settings object to save.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task SaveLauncherSettingsAsync(Settings settings)
        {
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Saving launcher settings.");

            string settingsFilePath = Path.Combine(BasePath, ".launcher", "settings.json");
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Saving settings to '{settingsFilePath}'.");

            try
            {
                // Serialize the settings object to JSON format.
                string settingsJson = JsonConvert.SerializeObject(settings, Formatting.Indented);
                // Write the JSON settings to the settings file asynchronously.
                await File.WriteAllTextAsync(settingsFilePath, settingsJson, Encoding.UTF8);
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Launcher settings saved successfully.");
                ApplySettingsToUI(settings);
            }
            catch (Exception ex)
            {
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Error, $"Error saving launcher settings to '{settingsFilePath}': {ex.Message}");
            }
        }
        #endregion

        #region Settings Class Definition
        /// <summary>
        /// Represents the launcher settings.
        /// This class holds properties for various launcher settings, which are serialized and deserialized to/from the settings file.
        /// </summary>
        public class Settings
        {
            /// <summary>
            /// Gets or sets a value indicating whether to hide the launcher window after Minecraft is launched.
            /// </summary>
            public bool HideLauncher { get; set; }
            /// <summary>
            /// Gets or sets a value indicating whether to show the console window.
            /// </summary>
            public bool ShowConsole { get; set; }
            /// <summary>
            /// Gets or sets a value indicating whether to close the launcher after Minecraft is launched.
            /// </summary>
            public bool CloseLauncher { get; set; }
            /// <summary>
            /// Gets or sets a value indicating whether to show release versions in the version list.
            /// </summary>
            public bool ShowReleases { get; set; }
            /// <summary>
            /// Gets or sets a value indicating whether to show snapshot versions in the version list.
            /// </summary>
            public bool ShowSnapshot { get; set; }
            /// <summary>
            /// Gets or sets a value indicating whether to show alpha versions in the version list.
            /// </summary>
            public bool ShowAlpha { get; set; }
            /// <summary>
            /// Gets or sets a value indicating whether the launcher window should be always on top.
            /// </summary>
            public bool TopMost { get; set; }
            /// <summary>
            /// Gets or sets the username to be used for Minecraft login.
            /// </summary>
            public string UserName { get; set; }
            /// <summary>
            /// Gets or sets a value indicating whether a custom access token is used. (Currently not fully implemented in provided code)
            /// </summary>
            public bool CustomAccessToken { get; set; }
            /// <summary>
            /// Gets or sets the access token for Minecraft authentication.
            /// </summary>
            public string AccessToken { get; set; }
        }
        #endregion

        #endregion

        #region Process Management

        #region Check Existing Launcher Processes
        /// <summary>
        /// Checks for other running instances of the launcher executable to prevent multiple instances from running.
        /// Compares the file path of current process with other processes having the same name.
        /// </summary>
        /// <param name="mainWindow">The current main window instance, used for logging via Launcher_Console.</param>
        /// <returns><c>true</c> if another instance is found, otherwise <c>false</c>.</returns>
        private static bool CheckProcessesForCurrentExecutable(MainWindow mainWindow)
        {
            mainWindow.Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Checking for existing launcher processes.");

            string currentProcessPath = Process.GetCurrentProcess().MainModule!.FileName; // Get the file path of the current process.
            bool foundDuplicate = false;

            // Iterate through all processes with the same name as the current process.
            foreach (var process in Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName))
            {
                // Ignore the current process itself.
                if (process.Id != Process.GetCurrentProcess().Id)
                {
                    try
                    {
                        // Compare the file path of the process with the current process's file path.
                        if (string.Equals(process.MainModule?.FileName, currentProcessPath, StringComparison.OrdinalIgnoreCase))
                        {
                            mainWindow.Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Warning, $"Found another instance of the launcher (PID: {process.Id}).");
                            foundDuplicate = true; // Set flag to indicate a duplicate is found.
                            break; // Exit loop as a duplicate is found.
                        }
                    }
                    catch (Exception ex)
                    {
                        mainWindow.Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Warning, $"Error checking process '{process.Id}': {ex.Message}");
                    }
                }
            }

            mainWindow.Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Process check completed. Duplicate found: {foundDuplicate}");
            return foundDuplicate; // Return whether a duplicate instance was found.
        }
        #endregion

        #endregion

        #region Error Handling

        #region Show Error Window
        /// <summary>
        /// Shows an error message to the user using an ErrorWindow.
        /// Logs the error message to the launcher console and enables UI elements after the error window is closed.
        /// </summary>
        /// <param name="errorText">The error message to display in the error window.</param>
        public void ShowError(string errorText, bool isWarn = false)
        {
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Error, $"Showing error message: {errorText}");
            var errorWindow = new ErrorWindow(errorText); // Create a new ErrorWindow instance.
            errorWindow.ShowDialog(); // Show the error window as a modal dialog, blocking interaction with the main window until closed.
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Error window closed.");
            if (isWarn) return;
            Show(); // Ensure the main window is visible, in case it was hidden.
            if (homePage == null) return;
            homePage.LaunchButton.IsEnabled = true; // Re-enable the launch button.
            homePage.VerisonComboBox.IsEnabled = true; // Re-enable the version combobox.
        }
        #endregion

        #endregion

        #region Window Events

        #region Window Closing Event
        /// <summary>
        /// Handles the window's closing event.
        /// This method is called when the main window is about to close. It closes the console window, saves the launcher log, and sets the Closed flag.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (IsRunning && !settingsPage.CloseLauncher.IsChecked.Value) { e.Cancel = true; ShowError("Unable to close launcher while downloading or running.", true); }

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Window closing initiated.");
            Closed = true; // Set the Closed flag to indicate the window is closing.

            // Check if the console window is initialized.
            if (console_Window != null)
            {
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Closing console window.");
                console_Window.Close(); // Close the console window.
            }

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Saving launcher log to file.");
            string logFilePath = Path.Combine(BasePath, ".launcher", "Log", $"Log-{DateTime.Now:yyyy-MM-dd HH-mm-ss}.txt"); // Define log file path with timestamp.
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath)!); // Ensure the log directory exists.
            File.WriteAllText(logFilePath, _log.ToString()); // Write the accumulated log to the file.
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, $"Launcher log saved to '{logFilePath}'.");
        }
        #endregion

        #endregion

        #region Page Navigation

        #region Page Instances
        private HomePage homePage;
        private SettingsPage settingsPage;
        private AboutusPage aboutPage;
        #endregion

        #region Navigation Methods
        /// <summary>
        /// Navigates the main window content to the homepage.
        /// </summary>
        public void homepage()
        {
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Navigating to homepage.");
            Content = homePage; // Set the main window content to the HomePage.
        }

        /// <summary>
        /// Navigates the main window content to the settings page.
        /// </summary>
        public void settingspage()
        {
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Navigating to settings page.");
            Content = settingsPage; // Set the main window content to the SettingsPage.
        }

        /// <summary>
        /// Navigates the main window content to the about us page.
        /// </summary>
        public void aboutpage()
        {
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Navigating to about us page.");
            Content = aboutPage; // Set the main window content to the AboutusPage.
        }
        #endregion

        #endregion

        #region Minecraft Version Management

        #region Version Manifest Constants and Variables
        private const string VersionManifestUrl = "https://piston-meta.mojang.com/mc/game/version_manifest_v2.json";
        private JArray versions; // Stores the parsed version manifest as a JArray.
        #endregion

        #region Load Version Manifest
        /// <summary>
        /// Loads the Minecraft version manifest from the Mojang API asynchronously.
        /// Fetches the manifest, parses it, filters versions based on settings, and saves it to disk.
        /// Handles network and JSON parsing errors and updates the version combobox in the UI.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task LoadVersionManifestAsync()
        {
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Loading Minecraft version manifest.");

            try
            {
                using var client = new HttpClient();
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Clearing existing items in the version combobox to prepare for update.");
                homePage.VerisonComboBox.Items.Clear(); // Clear existing items in the version combobox.
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Fetching version manifest from '{VersionManifestUrl}'.");
                var response = await client.GetAsync(VersionManifestUrl); // Send GET request to fetch version manifest.
                response.EnsureSuccessStatusCode(); // Ensure HTTP response is successful.
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Version manifest download successful.");

                string jsonResponse = await response.Content.ReadAsStringAsync(); // Read response content as string.
                var manifest = JObject.Parse(jsonResponse); // Parse JSON response to JObject.
                versions = (JArray)manifest["versions"]!; // Extract the 'versions' array from the manifest.
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Version manifest parsed. Found {versions.Count} versions.");

                FilterAndAddVersionsToComboBox(); // Filter versions based on settings and add to combobox.
                SaveVersionManifestToDisk(manifest); // Save the downloaded manifest to disk.
            }
            catch (HttpRequestException httpEx)
            {
                ShowError($"Network Error: Unable to connect to the server or invalid response - {httpEx.Message}");
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Error, $"Error loading version manifest (Network): {httpEx.Message}");
            }
            catch (JsonException jsonEx)
            {
                ShowError($"JSON Parsing Error: Unable to parse server response - {jsonEx.Message}");
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Error, $"Error parsing version manifest JSON: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                ShowError($"An unexpected error occurred: {ex.Message}");
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Error, $"Unexpected error while loading version manifest: {ex.Message}");
            }
        }
        #endregion

        #region Filter and Add Versions to ComboBox
        /// <summary>
        /// Filters the available Minecraft versions based on the user's settings (releases, snapshots, alpha)
        /// and adds the filtered versions to the version combobox in the UI.
        /// If no versions match the filter, defaults to showing release versions.
        /// </summary>
        private void FilterAndAddVersionsToComboBox()
        {
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Filtering Minecraft versions based on settings.");

            // Filter versions based on settingsPage checkboxes (snapshot, releases, Alpha).
            var filteredVersions = versions.Where(v =>
                (settingsPage.snapshot.IsChecked == true && v["type"]?.ToString() == "snapshot") ||
                (settingsPage.releases.IsChecked == true && v["type"]?.ToString() == "release") ||
                (settingsPage.Alpha.IsChecked == true && v["type"]?.ToString() == "old_alpha")).ToList();

            // If no versions match the filter, default to showing release versions.
            if (!filteredVersions.Any())
            {
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Warning, "No versions match the current filter, defaulting to release versions.");
                filteredVersions = versions.Where(v => v["type"]?.ToString() == "release").ToList();
            }

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Adding filtered versions to the combobox. Count: {filteredVersions.Count}");
            // Add filtered version IDs to the version combobox.
            foreach (var version in filteredVersions)
            {
                homePage.VerisonComboBox.Items.Add(version["id"]?.ToString());
            }

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, $"{filteredVersions.Count} versions added to the version combobox.");
        }
        #endregion

        #region Save Version Manifest to Disk
        /// <summary>
        /// Saves the downloaded version manifest to disk as a JSON file.
        /// Creates the necessary directories if they do not exist.
        /// Navigates back to the homepage after saving the manifest.
        /// </summary>
        /// <param name="manifest">The version manifest JObject to save.</param>
        private void SaveVersionManifestToDisk(JObject manifest)
        {
            const string versionsDirName = "versions";
            string versionsConfigFolderPath = Path.Combine(BasePath, ".minecraft", versionsDirName); // Define path for versions config directory.
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Ensuring versions config directory exists at '{versionsConfigFolderPath}'.");
            Directory.CreateDirectory(versionsConfigFolderPath); // Ensure versions config directory exists.

            string versionsJson = JsonConvert.SerializeObject(manifest, Formatting.Indented); // Serialize manifest to JSON.
            string versionInfoPath = Path.Combine(versionsConfigFolderPath, "version_manifest_v2.json"); // Define path for version manifest file.
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Saving version manifest to '{versionInfoPath}'.");
            File.WriteAllText(versionInfoPath, versionsJson); // Write JSON manifest to file.

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Navigating back to the homepage after saving manifest.");
            homepage(); // Navigate back to the homepage after saving.
        }
        #endregion

        #region Get Version Info
        /// <summary>
        /// Retrieves detailed information for a specific Minecraft version from the Mojang API.
        /// Uses the version ID to find the version URL in the manifest and then fetches the detailed version JSON.
        /// Saves the version info to disk and handles network and JSON parsing errors.
        /// </summary>
        /// <param name="versionId">The ID of the Minecraft version to retrieve info for.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. Returns the version information as a <see cref="JObject"/> or <c>null</c> if an error occurs.</returns>
        private async Task<JObject?> GetVersionInfoAsync(string versionId)
        {
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, $"Getting version info for '{versionId}'.");

            // Find the version info in the loaded version manifest based on versionId.
            var versionInfo = versions.FirstOrDefault(v => v["id"]?.ToString() == versionId);
            if (versionInfo == null)
            {
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Error, $"Version info not found for '{versionId}'.");
                return null; // Return null if version info not found in manifest.
            }

            string? versionUrl = versionInfo["url"]?.ToString(); // Get the URL for version info from manifest.
            if (string.IsNullOrEmpty(versionUrl))
            {
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Error, $"Version URL not found for '{versionId}'.");
                return null; // Return null if version URL is missing.
            }

            try
            {
                using var client = new HttpClient();
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Fetching version info from '{versionUrl}'.");
                var response = await client.GetAsync(versionUrl); // Fetch detailed version info from URL.
                response.EnsureSuccessStatusCode(); // Ensure HTTP response is successful.
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Successfully fetched version info for '{versionId}'.");

                string versionJson = await response.Content.ReadAsStringAsync(); // Read response content as string.
                SaveVersionInfoToDisk(versionId, versionJson); // Save version info to disk.
                return JObject.Parse(versionJson); // Parse JSON and return as JObject.
            }
            catch (HttpRequestException ex)
            {
                ShowError($"Error getting version info for {versionId}: {ex.Message}");
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Error, $"Error getting version info for '{versionId}': {ex.Message}");
                return null; // Return null on network error.
            }
            catch (JsonException ex)
            {
                ShowError($"Error parsing version info for {versionId}: {ex.Message}");
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Error, $"Error parsing version info for '{versionId}': {ex.Message}");
                return null; // Return null on JSON parsing error.
            }
        }
        #endregion

        #region Save Version Info to Disk
        /// <summary>
        /// Saves the detailed information for a specific Minecraft version to disk as a JSON file.
        /// Creates the version-specific directory if it does not exist.
        /// </summary>
        /// <param name="versionId">The ID of the Minecraft version.</param>
        /// <param name="versionJson">The version information in JSON format.</param>
        private void SaveVersionInfoToDisk(string versionId, string versionJson)
        {
            string versionConfigFolderPath = Path.Combine(BasePath, ".minecraft", "versions", versionId); // Define path for version-specific config directory.
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Ensuring version config directory exists at '{versionConfigFolderPath}'.");
            Directory.CreateDirectory(versionConfigFolderPath); // Ensure version config directory exists.
            string versionInfoPath = Path.Combine(versionConfigFolderPath, $"{versionId}.json"); // Define path for version info file.
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Saving version info to '{versionInfoPath}'.");
            File.WriteAllText(versionInfoPath, versionJson); // Write JSON version info to file.
        }
        #endregion

        #endregion

        #region Minecraft Launching

        #region Launch Minecraft Method

        bool IsRunning = false;
        /// <summary>
        /// Launches the selected Minecraft version.
        /// This method orchestrates the entire Minecraft launch process, including fetching version info, downloading Java,
        /// downloading client JAR, assets, libraries, generating classpath, and starting the Minecraft process.
        /// Handles UI updates, console output redirection, and launcher window behavior based on settings.
        /// </summary>
        /// <param name="selectedVersion">The ID of the selected Minecraft version.</param>
        /// <param name="versionComboBox">The version selection combobox in the UI.</param>
        /// <param name="launchButton">The launch button in the UI.</param>
        /// <param name="progressBar">The progress bar in the UI to show download progress.</param>
        public async void Launch_Minecraft(string selectedVersion, ComboBox versionComboBox, Button launchButton, ProgressBar progressBar)
        {
            progressBar.Value = 0;
            IsRunning = true;

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, $"Attempting to launch Minecraft version '{selectedVersion}'.");
            versionComboBox.IsEnabled = false; // Disable version combobox during launch process.
            launchButton.IsEnabled = false; // Disable launch button during launch process.
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Disabled version combobox and launch button to prevent re-launch during process.");

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Fetching detailed version information for '{selectedVersion}'.");
            var versionJson = await GetVersionInfoAsync(selectedVersion); // Get detailed version info from API.
            if (versionJson == null)
            {
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Error, $"Failed to get version info for '{selectedVersion}', cannot launch.");
                versionComboBox.IsEnabled = true; // Re-enable version combobox on failure.
                launchButton.IsEnabled = true; // Re-enable launch button on failure.
                return; // Exit if version info fetch fails.
            }

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Downloading required Java runtime for this Minecraft version.");
            await DownloadJavaAsync(versionJson, progressBar); // Download required Java runtime.

            string mainClass = versionJson["mainClass"]!.ToString(); // Get main class for Minecraft launch.
            string minecraftVersion = versionJson["id"]!.ToString(); // Get Minecraft version ID.
            string versionFolderPath = Path.Combine(BasePath, ".minecraft", "versions", minecraftVersion); // Define version-specific folder path.
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Ensuring version folder exists at '{versionFolderPath}'.");
            Directory.CreateDirectory(versionFolderPath); // Ensure version folder exists.

            string clientUrl = versionJson["downloads"]!["client"]!["url"]!.ToString(); // Get client JAR download URL.
            string clientJarPath = Path.Combine(versionFolderPath, $"{minecraftVersion}.jar"); // Define client JAR file path.
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Downloading client JAR from '{clientUrl}' to '{clientJarPath}'.");
            await DownloadFileAsync(clientUrl, clientJarPath, progressBar, "client Jar file"); // Download client JAR file.

            string assetsIndex = versionJson["assetIndex"]!["id"]!.ToString(); // Get assets index ID.
            string assetsUrl = versionJson["assetIndex"]!["url"]!.ToString(); // Get assets index URL.
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Downloading assets index from '{assetsUrl}'.");
            await DownloadAssetsAsync(assetsUrl, assetsIndex, progressBar); // Download game assets.

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Downloading required libraries for Minecraft version.");
            List<string> classifiers = new List<string>();
            await DownloadLibrariesAsync(versionJson, progressBar, classifiers); // Download required libraries.
            foreach (var library in classifiers)
            {
                var DLLPath = Path.Combine(BasePath, ".minecraft", "versions", selectedVersion, "natives");
                Directory.CreateDirectory(DLLPath);
                await Task.Run(() => ZipFile.ExtractToDirectory(library, DLLPath, true));
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, library);
            }

            progressBar.Maximum = 1; progressBar.Value = 1;

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Generating classpath for Minecraft launch based on downloaded libraries and JARs.");
            string classpath = await Task.Run(() => GenerateClasspath(selectedVersion, versionJson)); // Generate classpath string.

            // Retrieve launcher settings for window behavior.
            bool showConsoleWindow = settingsPage.ShowConsole.IsChecked.Value;
            bool hideWindow = settingsPage.HideLauncher.IsChecked.Value;
            bool closeWindow = settingsPage.CloseLauncher.IsChecked.Value;
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Launcher settings - Show Console: {showConsoleWindow}, Hide Launcher: {hideWindow}, Close Launcher: {closeWindow}");

            if (showConsoleWindow)
            {
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Showing console window as per settings.");
                console_Window.Show(); // Show console window if setting is enabled.
            }
            if (hideWindow)
            {
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Hiding launcher window as per settings.");
                Hide(); // Hide launcher window if setting is enabled.
            }

            // Construct Minecraft launch arguments.
            string libraryPath = Path.GetFullPath(Path.Combine(BasePath, ".minecraft", "libraries"));
            string arguments = $"-Djava.library.path=\"{Path.Combine(BasePath, ".minecraft", "versions", selectedVersion, "natives")}\" -cp \"{classpath}\" {mainClass} --accessToken none --version {minecraftVersion} --username \"{settingsPage.UserName.Text}\" --assetsDir {Path.Combine(BasePath, ".minecraft", "assets")} --assetIndex {assetsIndex} --gameDir \"{Path.GetFullPath(Path.Combine(BasePath, ".minecraft"))}\"";

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Java Path for launching Minecraft: {Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Java", versionJson["javaVersion"]?["majorVersion"]?.ToString(), "bin", "java.exe")}");
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Minecraft Launch Arguments: {arguments}");

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Starting Minecraft process with generated arguments and Java path.");
            await Task.Run(() =>
            {
                try
                {
                    using var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = Path.Combine(BasePath, "Java", versionJson["javaVersion"]?["majorVersion"]?.ToString(), "bin", "javaw.exe"), // Java executable path.
                            Arguments = arguments, // Minecraft launch arguments.
                            UseShellExecute = false,
                            RedirectStandardOutput = true, // Redirect standard output for console logging.
                            RedirectStandardError = true, // Redirect standard error for console logging.
                            CreateNoWindow = true, // Do not create a new window for the process.
                            WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory // Set working directory.
                        }
                    };

                    // Event handlers for output and error data received from the Minecraft process.
                    process.OutputDataReceived += (s, e) => DispatchConsoleMessage(ConsoleMessageType.Info, e.Data);
                    process.ErrorDataReceived += (s, e) => DispatchConsoleMessage(ConsoleMessageType.Error, e.Data);

                    process.Start(); // Start the Minecraft process.
                    process.BeginOutputReadLine(); // Begin asynchronous read of standard output.
                    process.BeginErrorReadLine(); // Begin asynchronous read of standard error.
                    process.WaitForExit(); // Wait for the process to exit.
                    Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, $"Minecraft process exited with code {process.ExitCode}.");
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke(() => ShowError(ex.Message)); // Show error message in UI thread.
                    Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Error, $"Error launching Minecraft: {ex.Message}");
                }
            });

            // Local function to dispatch console messages to the launcher console.
            void DispatchConsoleMessage(ConsoleMessageType messageType, string? message)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    Application.Current.Dispatcher.Invoke(() =>
                        Launcher_Console(ConsoleType.Minecraft, messageType, message)); // Log Minecraft console output to launcher console.
                }
            }

            if (closeWindow)
            {
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Closing launcher window after Minecraft launch as per settings.");
                Close(); // Close launcher window if setting is enabled.
            }
            else
            {
                Show(); // Show launcher window if it was hidden but not closed.
                homePage.LaunchButton.IsEnabled = true; // Re-enable launch button after Minecraft process ends.
                homePage.VerisonComboBox.IsEnabled = true; // Re-enable version combobox after Minecraft process ends.
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, "Re-enabled version combobox and launch button after Minecraft launch process.");
            }

            IsRunning = false;
        }
        #endregion

        #region Generate Classpath
        /// <summary>
        /// Generates the classpath string for launching Minecraft.
        /// Includes the version JAR and all required libraries.
        /// </summary>
        /// <param name="version">The Minecraft version ID.</param>
        /// <param name="versionJson">The version information in JSON format.</param>
        /// <returns>The generated classpath string.</returns>
        private string GenerateClasspath(string version, JObject versionJson)
        {
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Generating classpath for version '{version}'.");
            var classpath = new List<string>
            {
                Path.Combine(BasePath, ".minecraft", "versions", version, $"{version}.jar") // Add version JAR to classpath.
            };

            var libraries = versionJson["libraries"]; // Get libraries from version JSON.
            // Iterate through libraries and add their paths to the classpath.
            foreach (var lib in libraries)
            {
                if (lib["downloads"]?["artifact"]?["path"] == null) continue; // Skip if artifact path is missing.
                string libPath = Path.Combine(BasePath, ".minecraft", "libraries", lib["downloads"]?["artifact"]?["path"]?.ToString()); // Define library path.
                if (File.Exists(libPath))
                {
                    classpath.Add(libPath); // Add library path to classpath if file exists.
                }
                else
                {
                    Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Warning, $"Library not found: '{libPath}'");
                }
            }

            string classpathString = string.Join(Path.PathSeparator.ToString(), classpath); // Join classpath entries with path separator.
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Generated classpath: '{classpathString}'");
            return classpathString; // Return the generated classpath string.
        }
        #endregion

        #region Download Libraries
        /// <summary>
        /// Downloads the required libraries for the selected Minecraft version.
        /// Calculates total download size, updates progress bar, and downloads each library file.
        /// Handles datafixerupper library specifically.
        /// </summary>
        /// <param name="versionJson">The version information in JSON format.</param>
        /// <param name="progressBar">The progress bar to update download progress.</param>
        /// <param name="classifiersList">List to store classifier paths for extraction after download</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task DownloadLibrariesAsync(JObject versionJson, ProgressBar progressBar, List<string> classifiersList)
        {
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Downloading Minecraft libraries.");
            var libraries = versionJson["libraries"]; // Get libraries from version JSON.

            // Calculate total size of libraries to download for progress bar.
            long totalSize = libraries
                    .Sum(lib =>
                    {
                        long artifactSize = (long?)lib["downloads"]?["artifact"]?["size"] ?? 0;
                        long classifierSize = lib["downloads"]?["classifiers"]?
                            .Children<JProperty>()
                            .Sum(classifier => (long?)classifier.Value["size"] ?? 0) ?? 0;
                        return artifactSize + classifierSize;
                    });

            progressBar.Value = 0; // Reset progress bar value.
            progressBar.Maximum = totalSize; // Set progress bar maximum to total download size.

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, $"Total size of libraries to download: {totalSize / 1024.0 / 1024.0:F2} MB.");

            long downloadedSize = 0; // Track downloaded size for progress bar updates.

            // Iterate through each library and download it.
            foreach (var lib in libraries)
            {
                var artifact = (JObject)lib["downloads"]?["artifact"]; // Get artifact info.
                string? libUrl = artifact?["url"]?.ToString(); // Get library download URL.
                if (!string.IsNullOrEmpty(libUrl))
                {
                    string? libName = lib["name"]?.ToString(); // Get library name for logging.
                    string libPath = Path.Combine(BasePath, ".minecraft", "libraries", artifact?["path"]?.ToString() ?? ""); // Define library file path.
                    long libSize = (long?)artifact?["size"] ?? 0; // Get library file size.

                    Directory.CreateDirectory(Path.GetDirectoryName(libPath)!); // Ensure library directory exists.

                    await DownloadFileAsync(libUrl, libPath, progressBar, libName, libSize); // Download library file.
                    downloadedSize += libSize; // Update downloaded size.
                    progressBar.Value = downloadedSize; // Update progress bar value.

                    if (libPath.Contains("datafixerupper"))
                    {
                        HandleDatafixerupperLibrary(libPath); // Handle datafixerupper library specifically.
                    }
                }
                else
                {
                    Launcher_Console(ConsoleType.Minecraft, ConsoleMessageType.Warning, $"Warning: No download URL for library: {lib["name"]}");
                }

                var classifiers = lib["downloads"]?["classifiers"]?["natives-windows"];
                if (classifiers != null)
                {
                    var classifiersPath = Path.Combine(BasePath, ".minecraft", "libraries", classifiers?["path"]?.ToString());
                    var classifiersSize = classifiers?["size"] ?? 0;
                    await DownloadFileAsync(classifiers?["url"]?.ToString(), classifiersPath, progressBar, Path.GetFileName(classifiersPath), (long)classifiersSize);
                    downloadedSize += (long)classifiersSize;
                    progressBar.Value = downloadedSize;

                    classifiersList.Add(classifiersPath);
                }
            }

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Libraries download completed.");
        }
        #endregion

        #region Handle Datafixerupper Library
        /// <summary>
        /// Handles specific logic for the datafixerupper library, currently just logging file existence.
        /// </summary>
        /// <param name="libPath">The path to the datafixerupper library.</param>
        private void HandleDatafixerupperLibrary(string libPath)
        {
            if (File.Exists(libPath))
            {
                Launcher_Console(ConsoleType.Minecraft, ConsoleMessageType.Info, $"Datafixerupper library file exists: {libPath}");
            }
            else
            {
                Launcher_Console(ConsoleType.Minecraft, ConsoleMessageType.Warning, $"Warning: Datafixerupper library file not found after download: {libPath}");
            }
        }
        #endregion

        #region Download Assets
        /// <summary>
        /// Downloads the game assets based on the assets index.
        /// Downloads the assets index file, parses it, calculates total size, and downloads each asset file.
        /// Updates the progress bar during download process.
        /// </summary>
        /// <param name="assetsUrl">The URL for the assets index JSON file.</param>
        /// <param name="assetsIndex">The index ID of the assets.</param>
        /// <param name="progressBar">The progress bar to update download progress.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task DownloadAssetsAsync(string assetsUrl, string assetsIndex, ProgressBar progressBar)
        {
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, $"Downloading assets index '{assetsIndex}'.");
            string assetsIndexPath = Path.Combine(BasePath, ".minecraft", "assets", "indexes", $"{assetsIndex}.json"); // Define assets index file path.
            await DownloadFileAsync(assetsUrl, assetsIndexPath, progressBar, $"{assetsIndex}.json"); // Download assets index file.

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Loading assets index from '{assetsIndexPath}'.");
            string assetsContent = await File.ReadAllTextAsync(assetsIndexPath); // Read assets index file content.
            JObject assetsJson = JObject.Parse(assetsContent); // Parse assets index JSON.
            var objects = assetsJson["objects"] as JObject; // Get objects from assets JSON.

            if (objects == null)
            {
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Warning, "No assets found in index.");
                return; // Exit if no assets found in index.
            }

            // Calculate total size of assets to download for progress bar.
            long totalSize = objects.Properties()
                .Sum(obj => obj.Value["size"]!.Value<long>());

            progressBar.Value = 0; // Reset progress bar value.
            progressBar.Maximum = totalSize; // Set progress bar maximum to total assets size.

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, $"Total size of assets to download: {totalSize / 1024.0 / 1024.0:F2} MB.");

            long downloadedSize = 0; // Track downloaded size for progress bar updates.
            // Iterate through each asset object and download it.
            foreach (var obj in objects)
            {
                string name = obj.Key; // Asset name.
                string hash = obj.Value["hash"]!.ToString(); // Asset hash.
                long size = obj.Value["size"]!.Value<long>(); // Asset size.
                string url = $"https://resources.download.minecraft.net/{hash.Substring(0, 2)}/{hash}"; // Construct asset download URL.
                string assetPath = Path.Combine(BasePath, ".minecraft", "assets", "objects", hash.Substring(0, 2), hash); // Define asset file path.

                await DownloadFileAsync(url, assetPath, progressBar, name, size); // Download asset file.
                downloadedSize += size; // Update downloaded size.
                progressBar.Value = downloadedSize; // Update progress bar value.
            }

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Assets download completed.");
        }
        #endregion

        #endregion

        #region Java Management

        #region Download Java Runtime
        /// <summary>
        /// Downloads the required Java runtime for the selected Minecraft version.
        /// Fetches Java manifest, parses it, and downloads necessary Java files.
        /// Updates progress bar during download process and handles errors.
        /// </summary>
        /// <param name="versionJson">The version information in JSON format.</param>
        /// <param name="progress">The progress bar to update download progress.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task DownloadJavaAsync(JObject versionJson, ProgressBar progress)
        {
            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Checking and downloading Java for version.");

            var javaVersion = versionJson["javaVersion"]; // Get Java version info from version JSON.
            if (javaVersion == null)
            {
                ShowError("Unable to get Java version information from version JSON.");
                return; // Exit if Java version info is missing.
            }

            string? component = javaVersion["component"]?.ToString(); // Get Java component.

            //Fix the crash problem of Java on Minecraft 1.14~1.18
            //Version MinecraftVersion = new Version(versionJson?["id"]?.ToString());
            //Version lowerBound = new Version("1.14.0");
            //Version upperBound = new Version("1.19.0");
            //if (MinecraftVersion >= lowerBound && MinecraftVersion < upperBound) component = "java-runtime-beta";

            string? majorVersion = javaVersion["majorVersion"]?.ToString(); // Get Java major version.

            if (string.IsNullOrEmpty(component) || string.IsNullOrEmpty(majorVersion))
            {
                ShowError("Component or Major version not found in the JSON");
                return; // Exit if component or major version is missing.
            }

            string javaFolderPath = Path.Combine(BasePath, "Java", majorVersion); // Define Java installation folder path.
            Directory.CreateDirectory(javaFolderPath); // Ensure Java folder exists.
            string manifestPath = Path.Combine(BasePath, "Java", "jre_manifest.json"); // Define path to JRE manifest file.

            EnsureJavaManifestExists(manifestPath); // Ensure JRE manifest file exists.

            JObject jreManifest;
            try
            {
                jreManifest = JObject.Parse(File.ReadAllText(manifestPath)); // Parse JRE manifest JSON.
            }
            catch (JsonException ex)
            {
                ShowError($"Error parsing jre_manifest.json: {ex.Message}");
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Error, $"Error parsing jre_manifest.json: {ex.Message}");
                return; // Exit on JSON parsing error.
            }

            string osArch = GetOSArchitecture(); // Get OS architecture string.
            var javaManifest = jreManifest["manifest"]?[osArch]?[component]?[0]?["manifest"]; // Get Java manifest for OS and component.
            string? manifestUrl = javaManifest?["url"]?.ToString(); // Get Java manifest download URL.

            if (string.IsNullOrEmpty(manifestUrl))
            {
                ShowError("Unable to find manifest URL for Java.");
                return; // Exit if Java manifest URL is missing.
            }

            string javaManifestPath = Path.Combine(javaFolderPath, "manifest.json"); // Define path for downloaded Java manifest.
            await DownloadFileAsync(manifestUrl, javaManifestPath, progress, "Java manifest file"); // Download Java manifest file.

            string jsonContent = await File.ReadAllTextAsync(javaManifestPath); // Read downloaded Java manifest content.
            var manifest = JObject.Parse(jsonContent); // Parse downloaded Java manifest JSON.
            var fileInfo = ParseJavaManifest(manifest); // Parse Java manifest to get file info.

            progress.Value = 0; // Reset progress bar value.
            progress.Maximum = fileInfo.Count; // Set progress bar maximum to number of Java files.

            // Iterate through Java files and download them.
            foreach (var fileItem in fileInfo)
            {
                string fullFilePath = Path.Combine(javaFolderPath, fileItem.Key.Replace("/", "\\")); // Define full file path for Java file.
                await DownloadFileAsync(fileItem.Value.url, fullFilePath, progress, fileItem.Key, fileItem.Value.size); // Download Java file.
                progress.Value++; // Update progress bar value.
            }

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, "Java download completed for version.");
        }
        #endregion

        #region Ensure Java Manifest Exists
        /// <summary>
        /// Ensures that the Java manifest file ("jre_manifest.json") exists in the Java directory.
        /// If it doesn't exist, it creates the file from embedded resources.
        /// </summary>
        /// <param name="manifestPath">The path to the Java manifest file.</param>
        private void EnsureJavaManifestExists(string manifestPath)
        {
            if (!File.Exists(manifestPath))
            {
                string javaInfoBytes = Properties.Resources.jre_manifest; // Get Java manifest from resources.
                File.WriteAllBytes(manifestPath, Encoding.UTF8.GetBytes(javaInfoBytes)); // Write manifest bytes to file.
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"jre_manifest.json written to '{manifestPath}'.");
            }
        }
        #endregion

        #region Download File Async
        /// <summary>
        /// Downloads a file from the specified URL to the specified path, updating the progress bar.
        /// Handles file existence check, download progress updates, and error handling.
        /// </summary>
        /// <param name="url">The URL of the file to download.</param>
        /// <param name="filePath">The path to save the downloaded file.</param>
        /// <param name="progress">The progress bar to update.</param>
        /// <param name="description">A description of the file being downloaded for logging.</param>
        /// <param name="expectedSize">The expected size of the file (optional), used for file integrity check.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task DownloadFileAsync(string url, string filePath, ProgressBar? progress = null, string? description = null, long? expectedSize = null)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!); // Ensure directory for file exists.
            // Skip download if file exists and size matches expected size (if provided).
            if (File.Exists(filePath) && expectedSize.HasValue && new FileInfo(filePath).Length == expectedSize)
            {
                //Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Skipping download for '{description}' as it already exists and size matches.");
                return; // Skip download if file already exists and size is correct.
            }

            Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, $"Downloading {description ?? Path.GetFileName(filePath)} from '{url}' to '{filePath}'.");

            try
            {
                using var client = new HttpClient();
                using var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead); // Send GET request to download file.
                response.EnsureSuccessStatusCode(); // Ensure HTTP response is successful.

                long? totalBytes = response.Content.Headers.ContentLength; // Get total file size from headers.
                // Check for file size mismatch if expected size is provided.
                if (expectedSize.HasValue && totalBytes != expectedSize)
                {
                    Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Warning, $"File size mismatch for {description ?? Path.GetFileName(filePath)}. Expected: {expectedSize}, Got: {totalBytes}.");
                }

                using var contentStream = await response.Content.ReadAsStreamAsync(); // Get download content stream.
                using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true); // Create file stream for saving.
                var buffer = new byte[8192]; // Buffer for reading and writing.
                long totalRead = 0; // Track total bytes read for progress updates.
                int read;

                // Read from content stream and write to file stream in chunks.
                while ((read = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await fileStream.WriteAsync(buffer, 0, read); // Write buffer to file stream.
                    totalRead += read; // Update total bytes read.
                }

                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Debug, $"Downloaded {totalRead} bytes for {description}.");
            }
            catch (HttpRequestException ex)
            {
                ShowError($"Error downloading {description}: {ex.Message}");
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Error, $"Error downloading {description}: {ex.Message}");
            }
            catch (IOException ex)
            {
                ShowError($"Error saving {description} to disk: {ex.Message}");
                Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Error, $"Error saving {description} to disk: {ex.Message}");
            }
        }
        #endregion

        #region Parse Java Manifest
        /// <summary>
        /// Parses the Java manifest file to get the URLs and sizes of individual Java files.
        /// Extracts file paths, download URLs, and file sizes from the manifest JSON.
        /// </summary>
        /// <param name="manifest">The Java manifest as a <see cref="JObject"/>.</param>
        /// <returns>A dictionary containing the file paths and their download information (URL and size).</returns>
        private Dictionary<string, (string url, long size)> ParseJavaManifest(JObject manifest)
        {
            var fileInfo = new Dictionary<string, (string url, long size)>(); // Dictionary to store file info.
            var files = manifest["files"] as JObject; // Get 'files' object from manifest.
            if (files != null)
            {
                // Iterate through each file entry in the manifest.
                foreach (var property in files.Properties())
                {
                    var file = property.Value as JObject; // Get file JObject.
                    var downloads = file?["downloads"] as JObject; // Get downloads JObject.
                    var raw = downloads?["raw"] as JObject; // Get raw download info.
                    string? url = raw?["url"]?.ToString(); // Get download URL.
                    long size = raw?["size"]?.Value<long>() ?? 0; // Get file size.
                    if (!string.IsNullOrEmpty(url))
                    {
                        fileInfo[property.Name] = (url, size); // Add file info to dictionary.
                    }
                }
            }
            return fileInfo; // Return dictionary of file info.
        }
        #endregion

        #region Get OS Architecture
        /// <summary>
        /// Gets the operating system architecture as a string, used for Java runtime selection.
        /// Currently supports Windows operating systems (x86, x64, arm64).
        /// Throws NotSupportedException for unsupported OS or architecture.
        /// </summary>
        /// <returns>A string representing the OS architecture (e.g., "windows-x64").</returns>
        private static string GetOSArchitecture()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Determine Windows OS architecture.
                return RuntimeInformation.OSArchitecture switch
                {
                    Architecture.Arm64 => "windows-arm64",
                    Architecture.X64 => "windows-x64",
                    Architecture.X86 => "windows-x86",
                    _ => throw new NotSupportedException("Unsupported operating system architecture."), // Throw exception for unsupported architecture.
                };
            }
            else
            {
                throw new NotSupportedException("Current examples only support Windows operating systems."); // Throw exception for non-Windows OS.
            }
        }
        #endregion

        #endregion
    }
}