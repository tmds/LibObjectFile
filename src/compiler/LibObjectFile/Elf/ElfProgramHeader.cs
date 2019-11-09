﻿namespace LibObjectFile.Elf
{
    public struct ElfProgramHeader
    {
        public ElfSegmentType Type { get; set; }

        public ElfSectionOffset Offset { get; set; }

        public ulong VirtualAddress { get; set; }

        public ulong PhysicalAddress { get; set; }

        public ulong SizeInFile { get; set; } 

        public ulong SizeInMemory { get; set; }

        public ElfSegmentFlags Flags { get; set; }

        public ulong Align { get; set; }

        public override string ToString()
        {
            return $"{nameof(Type)}: {Type}, {nameof(Offset)}: ({Offset}), {nameof(VirtualAddress)}: 0x{VirtualAddress:X16}, {nameof(PhysicalAddress)}: 0x{PhysicalAddress:X16}, {nameof(SizeInFile)}: {SizeInFile}, {nameof(SizeInMemory)}: {SizeInMemory}, {nameof(Flags)}: {Flags}, {nameof(Align)}: {Align}";
        }
    }
}