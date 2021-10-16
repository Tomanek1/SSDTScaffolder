using SSDTScaffolderLib.Generators;
using SSDTScaffolderLib.Helpers;
using SSDTScaffolderLib.Interface;
using SSDTScaffolderLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ssdt = Microsoft.SqlServer.Dac.Model;


namespace SSDTScaffolderLib.Scaffolders
{
    internal class DbContextScaffolder : IScaffolder
    {
        private DbContextGenerator dbContextGenerator;
        private IEnumerable<Table> tables;
        private IEnumerable<View> views;

        string outputContent = "";


        public DbContextScaffolder(IEnumerable<Table> tables, IEnumerable<View> views)
        {
            this.tables = tables;
            this.views = views;
            this.dbContextGenerator = new DbContextGenerator(tables, views);
        }

        internal void Invoke(string targetDirectory, string targetNamespace, string contextName)
        {
            if (!targetDirectory.EndsWith("\\") && !targetDirectory.EndsWith("/"))
                targetDirectory += "\\";

            outputContent = dbContextGenerator.Generate(targetNamespace, contextName);

            GenerateFile(targetDirectory + contextName + ".cs");
        }

        private void GenerateFile(string targetDirectory)
        {
            using (var stream = new StreamWriter(targetDirectory))
            {
                stream.Write(outputContent);
            }
        }
    }
}
