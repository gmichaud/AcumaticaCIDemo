using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.IO.Compression;
using System.ServiceModel;
using System.Xml;

namespace Velixo.Common.CustomizationPackageTools
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine(Environment.CommandLine);

            var rootCommand = new RootCommand();
            var buildCommand = new Command("build")
            {
                new Option<string>("--customizationpath", "The folder containing the customization source code (_project folder).") { IsRequired = true},
                new Option<string>("--packagefilename", "The name of the customization package.") { IsRequired = true},
                new Option<string>("--description", "The description of the customization project.") { IsRequired = true},
                new Option<int>("--level", "The number representing the level that is used to resolve conflicts that arise if multiple modifications of the same items of the website are merged. Defaults to 0."),
            };
            rootCommand.Add(buildCommand);

            var publishCommand = new Command("publish")
            {
                new Option<string>("--packagefilename", "The name of the customization package file.") { IsRequired = true},
                new Option<string>("--packagename", "The name of the customization.") { IsRequired = true},
                new Option<string>("--url", "The root URL of the Acumatica website where the customization should be published.") { IsRequired = true},
                new Option<string>("--username", "The username to connect.") { IsRequired = true},
                new Option<string>("--password", "The password to conect.") { IsRequired = true},

            };
            rootCommand.Add(publishCommand);

            buildCommand.Handler = CommandHandler.Create((string customizationPath, string packageFilename, string description, int level) =>
            {
                Console.WriteLine($"Generating customization package {packageFilename}...");
                BuildCustomizationPackage(customizationPath, packageFilename, description, level);
                Console.WriteLine("Done!");
            });

            publishCommand.Handler = CommandHandler.Create(async (string packageFilename, string packageName, string url, string username, string password) => 
            {
                Console.WriteLine($"Publishing customization package {packageFilename} to {url}...");
                await PublishCustomizationPackage(packageFilename, packageName, url, username, password);
                Console.WriteLine("Done!");

            });

            await rootCommand.InvokeAsync(args);
        }

        private static void BuildCustomizationPackage(string customizationPath, string packageFilename, string description, int level)
        {
            // Our poor man's version of PX.CommandLine.exe -- to keep things simple.
            var projectXml = new XmlDocument();
            var customizationNode = projectXml.CreateElement("Customization");

            customizationNode.SetAttribute("level", level.ToString());
            customizationNode.SetAttribute("description", description);
            customizationNode.SetAttribute("product-version", "20.202");

            // Append all .xml files to project.xml
            foreach (var file in Directory.GetFiles(Path.Combine(customizationPath, "_project"), "*.xml"))
            {
                if (file.EndsWith("ProjectMetadata.xml")) continue;

                Console.WriteLine($"Appending {Path.GetFileName(file)} to customization project.xml...");
                var currentFileXml = new XmlDocument();
                currentFileXml.Load(file);

                if(currentFileXml.DocumentElement == null) throw new Exception("project.xml empty");

                customizationNode.AppendChild(projectXml.ImportNode(currentFileXml.DocumentElement, true));
            }

            //Append other customization assets to zip file
            using (FileStream zipToOpen = new FileStream(packageFilename, FileMode.Create))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
                {
                    //Append every other files directly, flattening the file name in the process
                    foreach (var directory in Directory.GetDirectories(customizationPath))
                    {
                        if (directory.EndsWith(@"\_project")) continue;
                        AddAssetsToPackage(archive, directory, customizationPath, customizationNode);
                    }

                    projectXml.AppendChild(customizationNode);
                    ZipArchiveEntry projectFile = archive.CreateEntry("project.xml", CompressionLevel.Optimal);
                    using (StreamWriter writer = new StreamWriter(projectFile.Open()))
                    {
                        projectXml.Save(writer);
                    }
                }
            }
        }

        private static void AddAssetsToPackage(ZipArchive archive, string currentDirectory, string rootDirectory, XmlElement customizationElement)
        {
            Console.WriteLine($"Processing directory {currentDirectory}...");

            foreach (var file in Directory.GetFiles(currentDirectory))
            {
                string targetZipFileName = file.Substring(rootDirectory.Length);
                Console.WriteLine($"Adding {targetZipFileName} to customization project...");

                archive.CreateEntryFromFile(file, targetZipFileName, CompressionLevel.Optimal);

                //Add reference to customization project as well
                var fileElement = customizationElement.OwnerDocument.CreateElement("File");
                fileElement.SetAttribute("AppRelativePath", targetZipFileName);
                customizationElement.AppendChild(fileElement);
            }

            foreach (var directory in Directory.GetDirectories(currentDirectory))
            {
                AddAssetsToPackage(archive, directory, rootDirectory, customizationElement);
            }
        }

        private static async Task PublishCustomizationPackage(string packageFilename, string packageName, string url, string username, string password)
        {
            BasicHttpBinding binding = new BasicHttpBinding() {  AllowCookies = true };
            binding.Security.Mode = BasicHttpSecurityMode.Transport;
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            
            EndpointAddress address = new EndpointAddress(url + "/api/ServiceGate.asmx");
            
            var gate = new ServiceGate.ServiceGateSoapClient(binding, address);

            Console.WriteLine($"Logging in to {url}...");
            await gate.LoginAsync(username, password);
            Console.WriteLine($"Uploading package...");
            await gate.UploadPackageAsync(packageName, File.ReadAllBytes(packageFilename), true);
            Console.WriteLine($"Publishing customizations...");
            await gate.PublishPackagesAsync(new string[] { packageName }, true);
            Console.WriteLine($"Logging out...");
            await gate.LogoutAsync();
        }

    }
}
