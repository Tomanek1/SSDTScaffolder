using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SSDTScaffolderLib.Helpers
{
    static class ScriptHelper
    {
        public static IEnumerable<string> GetAllSqlFiles(string[] paths)
        {
            foreach (var sqlPath in paths)
            {
                if (sqlPath.EndsWith(".sql"))
                {
                    yield return sqlPath;
                }
                else
                {
                    foreach (var file in Directory.GetFiles(sqlPath, "*.sql", SearchOption.AllDirectories))
                    {
                        yield return file;
                    }
                }
            }
        }

        public static IEnumerable<string> GetProjectFiles(string projectPath)
        {
            List<string> binAndObjFiles = new List<string>();
            foreach (var file in Directory.GetFiles(projectPath, "*.sql", SearchOption.AllDirectories))
            {
                if (file.Contains(@"\bin\") || file.Contains(@"\obj\"))
                    binAndObjFiles.Add(file);
            }

            foreach (var file in Directory.GetFiles(projectPath, "*.sql", SearchOption.AllDirectories))
            {
                //EndsWith because GetFiles returns .sqlproj files
                if (binAndObjFiles.Contains(file) || !file.EndsWith(".sql"))
                    continue;

                yield return file;
            }
        }
    }
}
