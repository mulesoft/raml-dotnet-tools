// PkgCmdID.cs
// MUST match PkgCmdID.h

namespace MuleSoft.RAML.Tools
{
    static class PkgCmdIDList
    {
        public const uint cmdRAMLGenerator = 0x100;
        public const uint cmdRAMLGenerator2 = 0x200;
        public const uint cmdUpdateRAMLReference = 0x200;
        public const uint cmdImplementContract = 0x300;
        public const uint cmdAddContract = 0x400;
        public const uint cmdAddContract2 = 0x400;
        public const uint cmdUpdateRAMLContract = 0x500;
        public const uint cmdEnableMetadataOutput = 0x600;
        public const uint cmdExtractRAML = 0x700;
        public const uint cmdDisableMetadataOutput = 0x800;
        public const uint cmdEditRamlProperties = 0x900;
    };
}