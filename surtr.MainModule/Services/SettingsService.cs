namespace Surtr.MainModule.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// The settings service. Very basic version. Find a cleaner way to do it.
    /// </summary>
    public class SettingsService : ISettingsService
    {
        /// <summary>
        /// The settings file
        /// </summary>
        private readonly string surtrFile;

        /// <summary>
        /// The local library folder
        /// </summary>
        private string localLibraryFolder;

        /// <summary>
        /// The is loaded flag.
        /// </summary>
        private bool isLoaded;

        /// <summary>
        /// The remote library folder
        /// </summary>
        private string remoteLibraryFolder;

        /// <summary>
        /// The maximum remote size
        /// </summary>
        private string maximumRemoteSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsService"/> class.
        /// </summary>
        public SettingsService()
        {
            // The folder for the current user 
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the base folder with your specific folder....
            var surtrFolder = Path.Combine(appDataFolder, "surtr");
            this.surtrFile = Path.Combine(surtrFolder, "surtr.config");

            // Check if folder exists and if not, create it
            if (!Directory.Exists(surtrFolder))
            {
                Directory.CreateDirectory(surtrFolder);
            }

            this.Load();
        }

        /// <summary>
        /// Gets or sets the local library folder.
        /// </summary>
        /// <value>
        /// The local library folder.
        /// </value>
        public string LocalLibraryFolder
        {
            get
            {
                if (!this.isLoaded)
                {
                    this.Load();
                }

                return this.localLibraryFolder;
            }

            set
            {
                this.localLibraryFolder = value;
                this.Save();
            }
        }

        /// <summary>
        /// Gets or sets the remote library folder.
        /// </summary>
        public string RemoteLibraryFolder
        {
            get
            {
                if (!this.isLoaded)
                {
                    this.Load();
                }

                return this.remoteLibraryFolder;
            }

            set
            {
                this.remoteLibraryFolder = value;
                this.Save();
            }
        }

        /// <summary>
        /// Gets or sets the maximum remote size.
        /// </summary>
        public string MaximumRemoteSize
        {
            get
            {
                if (!this.isLoaded)
                {
                    this.Load();
                }

                return this.maximumRemoteSize;
            }

            set
            {
                this.maximumRemoteSize = value;
                this.Save();
            }
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        private void Load()
        {
            this.remoteLibraryFolder = string.Empty;
            this.localLibraryFolder = string.Empty;
            this.maximumRemoteSize = string.Empty;
            
            // TODO Do the actual stuff
            if (File.Exists(this.surtrFile))
            {
                var lines = File.ReadLines(this.surtrFile);
                foreach (var line in lines)
                {
                    this.Set(line);
                }
            }

            this.isLoaded = true;
        }

        /// <summary>
        /// Sets the specified setting line.
        /// </summary>
        /// <param name="settingLine">
        /// The setting line.
        /// </param>
        private void Set(string settingLine)
        {
            var args = settingLine.Split(':');
            if (args.Length < 2)
            {
                return;
            }

            if (args.Length > 2)
            {
                for (int i = 2; i < args.Length; i++)
                {
                    args[1] += ":" + args[i];
                }
            }

            switch (args[0])
            {
                case "LocalLibraryFolder":
                    this.localLibraryFolder = args[1];
                    break;
                case "RemoteLibraryFolder":
                    this.remoteLibraryFolder = args[1];
                    break;
                case "MaximumRemoteSize":
                    this.maximumRemoteSize = args[1];
                    break;
            }
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        private void Save()
        {
            var lines = new List<string>();
            lines.Add(string.Format("LocalLibraryFolder:{0}", this.localLibraryFolder));
            lines.Add(string.Format("RemoteLibraryFolder:{0}", this.remoteLibraryFolder));
            lines.Add(string.Format("MaximumRemoteSize:{0}", this.maximumRemoteSize));
            File.WriteAllLines(this.surtrFile, lines);
        }
    }
}