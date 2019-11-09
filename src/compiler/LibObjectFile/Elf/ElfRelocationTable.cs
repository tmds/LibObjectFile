﻿using System;
using System.Collections.Generic;

namespace LibObjectFile.Elf
{
    public class ElfRelocationTable : ElfSection
    {
        public ElfRelocationTable()
        {
            Entries = new List<ElfRelocation>();
            Type = ElfSectionType.Relocation;
        }
        
        public List<ElfRelocation> Entries { get; }

        public override unsafe ulong GetSize(ElfFileClass fileClass)
        {
            bool isRela = this.Type == ElfSectionType.RelocationAddends;
            
            switch (fileClass)
            {
                case ElfFileClass.Is32:
                    return (ulong)(Entries.Count * (isRela ? sizeof(RawElf.Elf32_Rela) : sizeof(RawElf.Elf32_Rel)));
                case ElfFileClass.Is64:
                    return (ulong)(Entries.Count * (isRela ? sizeof(RawElf.Elf64_Rela) : sizeof(RawElf.Elf64_Rel)));
                default:
                    throw ThrowHelper.InvalidEnum(fileClass);
            }
        }

        protected override void Write(ElfWriter writer)
        {
            if (Parent.FileClass == ElfFileClass.Is32)
            {
                Write32(writer);
            }
            else
            {
                Write64(writer);
            }
        }

        public override unsafe ulong GetTableEntrySize(ElfFileClass fileClass)
        {
            bool isRela = this.Type == ElfSectionType.RelocationAddends;
            switch (fileClass)
            {
                case ElfFileClass.Is32:
                    return (ulong)(isRela ? sizeof(RawElf.Elf32_Rela) : sizeof(RawElf.Elf32_Rel));
                case ElfFileClass.Is64:
                    return (ulong)(isRela ? sizeof(RawElf.Elf32_Rela) : sizeof(RawElf.Elf32_Rel));
                default:
                    throw ThrowHelper.InvalidEnum(fileClass);
            }
        }

        private void Write32(ElfWriter writer)
        {
            bool isRela = this.Type == ElfSectionType.RelocationAddends;

            if (isRela)
            {
                // Write all entries
                for (int i = 0; i < Entries.Count; i++)
                {
                    var entry = Entries[i];

                    var sym = new RawElf.Elf32_Rel();
                    writer.Encode(out sym.r_offset, (uint)entry.Offset);
                    uint r_info = (entry.SymbolIndex << 8) | (entry.Type.Value & 0xFF);
                    writer.Encode(out sym.r_info, r_info);

                    unsafe
                    {
                        var span = new ReadOnlySpan<byte>(&sym, sizeof(RawElf.Elf32_Rel));
                        writer.Stream.Write(span);
                    }
                }
            }
            else
            {
                // Write all entries
                for (int i = 0; i < Entries.Count; i++)
                {
                    var entry = Entries[i];

                    var sym = new RawElf.Elf32_Rela();
                    writer.Encode(out sym.r_offset, (uint)entry.Offset);
                    uint r_info = (entry.SymbolIndex << 8) | (entry.Type.Value & 0xFF);
                    writer.Encode(out sym.r_info, r_info);
                    writer.Encode(out sym.r_addend, (int)entry.Addend);

                    unsafe
                    {
                        var span = new ReadOnlySpan<byte>(&sym, sizeof(RawElf.Elf32_Rela));
                        writer.Stream.Write(span);
                    }
                }
            }
        }

        private void Write64(ElfWriter writer)
        {
            bool isRela = this.Type == ElfSectionType.RelocationAddends;

            if (isRela)
            {
                // Write all entries
                for (int i = 0; i < Entries.Count; i++)
                {
                    var entry = Entries[i];

                    var sym = new RawElf.Elf64_Rel();
                    writer.Encode(out sym.r_offset, (uint)entry.Offset);
                    ulong r_info = ((ulong)entry.SymbolIndex << 32) | (entry.Type.Value);
                    writer.Encode(out sym.r_info, r_info);

                    unsafe
                    {
                        var span = new ReadOnlySpan<byte>(&sym, sizeof(RawElf.Elf64_Rel));
                        writer.Stream.Write(span);
                    }
                }
            }
            else
            {
                // Write all entries
                for (int i = 0; i < Entries.Count; i++)
                {
                    var entry = Entries[i];

                    var sym = new RawElf.Elf64_Rela();
                    writer.Encode(out sym.r_offset, entry.Offset);
                    ulong r_info = ((ulong)entry.SymbolIndex << 32) | (entry.Type.Value );
                    writer.Encode(out sym.r_info, r_info);
                    writer.Encode(out sym.r_addend, entry.Addend);

                    unsafe
                    {
                        var span = new ReadOnlySpan<byte>(&sym, sizeof(RawElf.Elf64_Rela));
                        writer.Stream.Write(span);
                    }
                }
            }
        }
    }
}