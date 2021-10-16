using SSDTScaffolderLib.Generators;
using SSDTScaffolderLib.Helpers;
using SSDTScaffolderLib.Interface;
using SSDTScaffolderLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SSDTScaffolderLib.Scaffolders
{
    internal class PocoScaffolder : IScaffolder
    {
        private PocoGenerator pocoGenerator;
        private IEnumerable<Table> tables;
        private IEnumerable<View> views;

        public PocoScaffolder(IEnumerable<Table> tables, IEnumerable<View> views)
        {
            this.tables = tables;
            this.views = views;
            pocoGenerator = new PocoGenerator(tables, views);
        }

        internal void Invoke(string targetDirectory, string targetNamespace)
        {
            if (!targetDirectory.EndsWith("\\") && !targetDirectory.EndsWith("/"))
                targetDirectory += "\\";
            IEnumerable<(string, string)> outputString = pocoGenerator.Generate(targetDirectory, targetNamespace);
            foreach (var item in outputString)
            {
                using (var stream = new StreamWriter(targetDirectory + item.Item1 + ".cs"))
                {
                    stream.Write(item.Item2);
                }
            }
        }
    }
}
