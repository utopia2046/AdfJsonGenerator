using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AdfJsonGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            GenerateAzureTable2BlobJsons();
            GenerateAzureBlob2CosmosJsons();
            GenerateSiAzure2CosmosJsons();
        }

        static void GenerateSiAzure2CosmosJsons()
        {
            foreach (var table in Consts.Tables)
            {
                // Generate Input Blob storages json file
                var blobDataSetName = Consts.InputAzureBlobSiNameTemplate.Replace("<tableName>", table.Name);
                var blobDataSetJson = Consts.InputAzureBlobSiJsonTemplate.Replace("<tableName>", table.Name);
                WriteJsonFile(blobDataSetName, blobDataSetJson);

                // Generate Output Cosmos storage json file
                var cosmosDataSetName = Consts.OutputCosmosSiNameTemplate.Replace("<tableName>", table.Name);
                var cosmosDataSetJson = Consts.OutputCosmosSiJsonTemplate.Replace("<tableName>", table.Name);
                WriteJsonFile(cosmosDataSetName, cosmosDataSetJson);

                // Generate Pipeline json file
                var pipelineName = Consts.Azure2CosmosPipelineSiNameTemplate.Replace("<tableName>", table.Name);
                var pipelineJson = Consts.Azure2CosmosPipelineSiJsonTemplate.Replace("<tableName>", table.Name);
                WriteJsonFile(pipelineName, pipelineJson);
            }
        }

        static void GenerateAzureBlob2CosmosJsons()
        {
            foreach (var table in Consts.Tables)
            {
                // Generate Input Blob storages json file
                var blobDataSetName = Consts.InputBlobNameTemplate.Replace("<tableName>", table.Name);
                var blobDataSetJson = Consts.InputBlobJsonTemplate.Replace("<tableName>", table.Name);
                WriteJsonFile(blobDataSetName, blobDataSetJson);

                // Generate Output Cosmos storage json file
                var cosmosDataSetName = Consts.OutputCosmosNameTemplate.Replace("<tableName>", table.Name);
                var cosmosDataSetJson = Consts.OutputCosmosJsonTemplate.Replace("<tableName>", table.Name)
                    .Replace("<cosmosFolder>", table.CosmosFolderName);
                WriteJsonFile(cosmosDataSetName, cosmosDataSetJson);

                // Generate Pipeline json file
                var pipelineName = Consts.Azure2CosmosPipelineNameTemplate.Replace("<tableName>", table.Name);
                var pipelineJson = Consts.Azure2CosmosPipelineJsonTemplate.Replace("<tableName>", table.Name);
                WriteJsonFile(pipelineName, pipelineJson);
            }
        }

        static void GenerateAzureTable2BlobJsons()
        {
            foreach (var account in Consts.StorageAccounts)
            {
                var shortRegion = account.ShortRegion;
                var accountName = account.AccountName;
                var key = account.Key;
                var index = account.Index;

                // Generate linked service
                var linkedServiceName = String.Format(Consts.LinkedServiceNameTemplate, accountName);
                var LinkedServiceJson = Consts.LinkedServiceJsonTemplate.Replace("<accountName>", accountName)
                    .Replace("<description>", accountName)
                    .Replace("<key>", key);
                WriteJsonFile(linkedServiceName, LinkedServiceJson);

                foreach (var table in Consts.Tables)
                {
                    // Generate Input Table storages json file
                    var tableDataSetName = String.Format(Consts.InputTableStorageNameTemplate, shortRegion, table.Name);
                    var tableDataSetJson = Consts.InputTableStorageJsonTemplate.Replace("<regionShort>", shortRegion)
                        .Replace("<tableName>", table.Name)
                        .Replace("<accountName>", accountName);
                    WriteJsonFile(tableDataSetName, tableDataSetJson);

                    // Generate Output Blob storage json file
                    var blobDataSetName = String.Format(Consts.OutputBlobStorageNameTemplate, shortRegion, table.Name);
                    var blobDataSetJson = Consts.OutputBlobStorageJsonTemplate.Replace("<regionShort>", shortRegion)
                        .Replace("<tableName>", table.Name)
                        .Replace("<tableNameShort>", table.FileName)
                        .Replace("<accountName>", accountName)
                        .Replace("<index>", index.ToString())
                        .Replace("<extension>", table.Extension);
                    WriteJsonFile(blobDataSetName, blobDataSetJson);

                    // Generate Pipeline json file
                    var pipelineName = String.Format(Consts.PipelineNameTemplate, shortRegion, table.Name);
                    var pipelineJson = Consts.PipelineJsonTemplate.Replace("<regionShort>", shortRegion)
                        .Replace("<tableName>", table.Name)
                        .Replace("<accountName>", accountName);
                    WriteJsonFile(pipelineName, pipelineJson);
                }
            }
        }

        static void WriteJsonFile(string name, string content)
        {
            using (var sw = new StreamWriter(name + ".json"))
            {
                sw.Write(content);
                Console.WriteLine("JSON file {0}.json written successfully", name);
            }
        }

        static bool IsFileExist(string name)
        {
            if (File.Exists(name + ".json"))
            {
                Console.WriteLine("Target file exist.");
                return true;
            }

            return false;
        }
    }
}
