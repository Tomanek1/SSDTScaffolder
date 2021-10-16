using SSDTScaffolderLib.Helpers;
using SSDTScaffolderLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSDTScaffolderLib.Generators
{
    internal class DbContextGenerator
    {
        private IEnumerable<Table> tables;
        private IEnumerable<View> views;
        BuilderMethodGenerator builder;

        private string contextString = @"
using Microsoft.EntityFrameworkCore;

namespace {0}
{{
	public partial class {1} : DbContext
	{{
		public {1}(): base()
		{{
		}}

		public {1}(DbContextOptions<{1}> options)
			: base(options)
		{{
		}}

{2}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {{
{3}
        }}		
	}}
}}
";

        public DbContextGenerator(IEnumerable<Table> tables, IEnumerable<View> views)
        {
            this.tables = tables;
            this.views = views;
            contextString = contextString.ReplaceFirst(Environment.NewLine, "");
            builder = new BuilderMethodGenerator(tables, views);
        }

        internal string Generate(string targetNamespace, string contextName)
        {
            string datasets = "";

            foreach (Table item in tables)
            {
                datasets += "\t\tpublic virtual DbSet<" + item.Name + "> " + item.Name + " { get; set; }" + Environment.NewLine;
            }

            foreach (View item in views)
            {
                datasets += "\t\tpublic virtual DbSet<" + item.Name + "> " + item.Name + " { get; set; }" + Environment.NewLine;
            }
            string builderOtput = builder.GenerateBuilder();
            return string.Format(contextString, targetNamespace, contextName, datasets, builderOtput);
        }
    }
}
