﻿namespace LibObjectFile.Elf
{
    public struct ElfRelocation
    {
        public ulong Offset { get; set; }

        public ElfRelocationType Type { get; set; }

        public uint SymbolIndex { get; set; }

        public long Addend { get; set; }

        public override string ToString()
        {
            return $"{nameof(Offset)}: 0x{Offset:X16}, {nameof(Type)}: {Type}, {nameof(SymbolIndex)}: {SymbolIndex}, {nameof(Addend)}: 0x{Addend:X16}";
        }
    }
}