// Guids.cs
// MUST match guids.h

using System;

namespace MuleSoft.RAML.Tools
{
    static class GuidList
    {
        public const string guidMuleSoft_RAML_ToolsPackagePkgString = "49d3aa6e-2e80-4568-92e9-4bcb3eb2b40d";
        public const string guidMuleSoft_RAML_ReferencesNodeString = "9556bec5-0574-49ed-a2f7-9c427687cc44";
        public const string guidMuleSoft_RAML_FolderNodeString = "b16d5c80-c404-423c-aa1b-ecf5b58f89bd";
        public const string guidMuleSoft_RAML_FileNodeString = "61eff566-9e0e-4197-b9c4-8b0dab685768";
        public const string guidMuleSoft_RAML_ProjectNodeString = "50f8cbf6-aacf-457d-aa89-0bc300a9b7ae";
        public const string guidMuleSoft_RAML_UpdateRAMLContractString = "14c03645-17a8-49ae-b301-6a619f3942d9";
        public const string guidMuleSoft_RAML_EnableMetadataOutputString = "30206c8b-b59e-477f-929e-d47376b779b7";
        public const string guidMuleSoft_RAML_ExtractRAMLString = "a1f4149f-0985-4a2c-88ad-0e747290b3a2";
        public const string guidMuleSoft_RAML_DisableMetadataOutputString = "9345ba25-7345-4be0-81eb-d365fe004dea";
        private const string guidMuleSoft_RAML_ContractFolderNodeString = "a83de21a-fdf5-4bf4-9ec2-88bab3e327ba";
        public const string guidMuleSoft_RAML_EditPropertiesString = "4dd305ea-f3af-496a-89a1-b9a763758477";

        public static readonly Guid guidMuleSoft_RAML_ReferencesNode = new Guid(guidMuleSoft_RAML_ReferencesNodeString);
        public static readonly Guid guidMuleSoft_RAML_FolderNode = new Guid(guidMuleSoft_RAML_FolderNodeString);
        public static readonly Guid guidMuleSoft_RAML_FileNode = new Guid(guidMuleSoft_RAML_FileNodeString);
        public static readonly Guid guidMuleSoft_RAML_ProjectNode = new Guid(guidMuleSoft_RAML_ProjectNodeString);
        public static readonly Guid guidMuleSoft_RAML_CmdUpdateRAMLContract = new Guid(guidMuleSoft_RAML_UpdateRAMLContractString);
        public static readonly Guid guidMuleSoft_RAML_EnableMetadataOutput = new Guid(guidMuleSoft_RAML_EnableMetadataOutputString);
        public static readonly Guid guidMuleSoft_RAML_DisableMetadataOutput = new Guid(guidMuleSoft_RAML_DisableMetadataOutputString);
        public static readonly Guid guidMuleSoft_RAML_ExtractRAML = new Guid(guidMuleSoft_RAML_ExtractRAMLString);        
        public static readonly Guid guidMuleSoft_RAML_ContractFolderNode = new Guid(guidMuleSoft_RAML_ContractFolderNodeString);
        public static readonly Guid guidMuleSoft_RAML_EditProperties = new Guid(guidMuleSoft_RAML_EditPropertiesString);
    };
}