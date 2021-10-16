using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using SSDTScaffolderLib;

namespace SSDTScaffolderConsole
{
    class Program
    {
        static SSDTScaffolder sSDTScaffolder;
        static void Main(string[] args)
        {
            // Create a root command with some options
            var rootCommand = new RootCommand
            {
                new Option<DirectoryInfo>(
                    alias: "-i",
                    description: "SSDT Project Path" + Environment.NewLine +
                    "sqlproj must be presented"),
                new Option<DirectoryInfo>(
                    alias:"-o",
                    description: "Output Folder Path"),
                new Option<string>(
                    alias:"-cd",
                    description: "DbContext directory"),
                new Option<string>(
                    alias:"-md",
                    description: "POCO models directory"),
                new Option<string>(
                    alias:"-cn",
                    description: "DbContext class name"),
            };
            //throw new Exception("namespacy");
            rootCommand.Description = "SSDT project scaffolder App";

            // Note that the parameters of the handler method are matched according to the names of the options
            rootCommand.Handler = CommandHandler.Create<DirectoryInfo, DirectoryInfo, string, string, string>((i, o, cd, md, cn/*, fileOption*/) =>
            {
                if (!i.EnumerateFiles().Any(f => f.Name.ToLower().EndsWith("sqlproj")))
                    throw new ArgumentException(".sqlproj file not found!");
                if (!i.Exists || !o.Exists)
                    throw new ArgumentException("Path not found");
                if (string.IsNullOrEmpty(cn))
                    throw new ArgumentException("");

                string cdTmp = o.FullName;
                string mdTmp = o.FullName;
                string contextNamespace = "Titulky.DAL.EF_Models";
                string pocoNamespace = "Titulky.DAL.EF_Models";
                if (!string.IsNullOrEmpty(cd))
                    cdTmp = o.FullName + "\\" + cd;
                if (!string.IsNullOrEmpty(md))
                    mdTmp = o.FullName + "\\" + md;

                sSDTScaffolder = new SSDTScaffolder(i.FullName);
                sSDTScaffolder.ScaffoldPocoClasses(mdTmp, pocoNamespace);
                sSDTScaffolder.ScaffoldDbContext(cdTmp, contextNamespace, cn);
            });

            // Parse the incoming args and invoke the handler
            var parms = rootCommand.InvokeAsync(args).Result;

        }
    }
}
