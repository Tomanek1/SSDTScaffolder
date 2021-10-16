using System;
using System.Collections.Generic;
using System.Text;

namespace SSDTScaffolderConsole
{
    public class Arguments
    {
        public string Namespace { get; set; } = "Titulky.DAL.EF_Context";
        public string[] SourcePaths { get; set; }
        public string TargetDirectory { get; set; } = @"C:\GitHub\Titulky\Titulky\Titulky.DAL\";
    }
}
