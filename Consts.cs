using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace AdfJsonGenerator
{
    class Consts
    {
        public static string LinkedServiceNameTemplate = @"AzureStorage_{0}";   // AzureStorage_eastusbingads
        public static string LinkedServiceJsonTemplate
        {
            get
            {
                return ReadTemplate("JsonTemplates/LinkedServiceJsonTemplate.json");
            }
        }

        public static string InputTableStorageNameTemplate = @"InputTable_{0}_{1}";  // InputTable_WU_Trace
        public static string InputTableStorageJsonTemplate
        {
            get
            {
                return ReadTemplate("JsonTemplates/InputTableStorageJsonTemplate.json");
            }
        }

        public static string OutputBlobStorageNameTemplate = @"OutputBlob_{0}_{1}";  // OutputBlob_WU_Trace
        public static string OutputBlobStorageJsonTemplate
        {
            get
            {
                return ReadTemplate("JsonTemplates/OutputBlobStorageJsonTemplate.json");
            }
        }

        public static string PipelineNameTemplate = "Table2Blob_{0}_{1}";   // Table2Blob_WU_Trace
        public static string PipelineJsonTemplate
        {
            get
            {
                return ReadTemplate("JsonTemplates/PipelineJsonTemplate.json");
            }
        }

        public static string InputAzureBlobSiNameTemplate = "InputAzureBlob_siwestus_<tableName>";
        public static string InputAzureBlobSiJsonTemplate
        {
            get
            {
                return ReadTemplate("JsonTemplates/InputAzureBlobSiTemplate.json");
            }
        }

        public static string OutputCosmosSiNameTemplate = "OutputCosmos_siwestus_<tableName>";
        public static string OutputCosmosSiJsonTemplate
        {
            get
            {
                return ReadTemplate("JsonTemplates/OutputCosmosSiTemplate.json");
            }
        }

        public static string Azure2CosmosPipelineSiNameTemplate = "Azure2Cosmos_siwest_<tableName>";
        public static string Azure2CosmosPipelineSiJsonTemplate
        {
            get
            {
                return ReadTemplate("JsonTemplates/Azure2CosmosPipelineSiTemplate.json");
            }
        }

        public static List<TableInfo> _tables = null;
        public static List<TableInfo> Tables
        {
            get
            {
                if (_tables == null)
                {
                    _tables = new List<TableInfo>();
                    _tables.Add(new TableInfo("Error", "error", "Error", "error"));
                    _tables.Add(new TableInfo("ApplicationError", "apperror", "Error", "error"));
                    _tables.Add(new TableInfo("UserError", "usererror", "Error", "error"));
                    _tables.Add(new TableInfo("Performance", "perf", "Perf", "perf"));
                    _tables.Add(new TableInfo("Latency", "latency", "Perf", "perf"));
                    _tables.Add(new TableInfo("Trace", "trace", "Trace", "trace"));
                }

                return _tables;
            }
        }

        private static List<StorageAccount> _storageAccounts = null;
        public static List<StorageAccount> StorageAccounts
        {
            get
            {
                if (_storageAccounts == null)
                {
                    _storageAccounts = new List<StorageAccount>();
                    _storageAccounts.Add(new StorageAccount(0, "EU", "eastusbingads", ""));
                    _storageAccounts.Add(new StorageAccount(1, "WU", "westusbingads", ""));
                    _storageAccounts.Add(new StorageAccount(2, "NE", "northeuropebingads", ""));
                    _storageAccounts.Add(new StorageAccount(3, "SEA", "southeastasiabingads", ""));
                    _storageAccounts.Add(new StorageAccount(4, "NCU", "northcentralusbingads", ""));
                }

                return _storageAccounts;
            }
        }

        public static string InputBlobNameTemplate = @"InputAzureBlob_Prod_<tableName>";
        public static string InputBlobJsonTemplate
        {
            get
            {
                return ReadTemplate("JsonTemplates/InputAzureBlobTemplate.json");
            }
        }

        public static string OutputCosmosNameTemplate = @"OutputCosmos_Prod_<tableName>";
        public static string OutputCosmosJsonTemplate
        {
            get
            {
                return ReadTemplate("JsonTemplates/OutputCosmosTemplate.json");
            }
        }

        public static string Azure2CosmosPipelineNameTemplate = @"Azure2Cosmos_Prod_<tableName>";
        public static string Azure2CosmosPipelineJsonTemplate
        {
            get
            {
                return ReadTemplate("JsonTemplates/Azure2CosmosPipelineTemplate.json");
            }
        }

        private static string ReadEmbeddedResource(string name)
        {
            string content;
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(name))
            {
                using (var sr = new StreamReader(stream))
                {
                    content = sr.ReadToEnd();
                }
            }

            return content;
        }

        private static string ReadTemplate(string name)
        {
            string content;
            using (var sr = new StreamReader(name))
            {
                content = sr.ReadToEnd();
            }

            return content;
        }
    }

    public class StorageAccount
    {
        public int Index;
        public string ShortRegion;
        public string AccountName;
        public string Key;

        public StorageAccount(int index, string shortRegion, string accountName, string key)
        {
            Index = index;
            ShortRegion = shortRegion;
            AccountName = accountName;
            Key = key;
        }
    }

    public class TableInfo
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public string CosmosFolderName { get; set; }
        public string Extension { get; set; }

        public TableInfo(string name, string fileName, string cosmosFolderName, string extension)
        {
            Name = name;
            FileName = fileName;
            CosmosFolderName = cosmosFolderName;
            Extension = extension;
        }
    }
}
