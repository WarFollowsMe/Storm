﻿/*
    Copyright 2016 Cody R. (Demmonic)

    Storm is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Storm is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Storm.  If not, see <http://www.gnu.org/licenses/>.
 */
using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm
{
    public static class CecilUtils
    {
        public static string DescriptionOf(MethodDefinition md)
        {
            var sb = new StringBuilder();
            sb.Append('(');

            var set = false;
            foreach (var param in md.Parameters)
            {
                sb.Append(param.ParameterType.Resolve().FullName);
                sb.Append(',');
                set = true;
            }
            if (set) sb.Length -= 1;

            sb.Append(')');
            sb.Append(md.ReturnType.Resolve().FullName);
            return sb.ToString();
        }

        public static TypeReference GetTypeRef(this AssemblyDefinition asm, string type, bool dynamicFallback = false)
        {
            var tds = asm.Modules.Where(m => m.GetType(type) != null).Select(m => m.GetType(type));
            if (tds.Count() == 0)
            {
                return dynamicFallback ? asm.MainModule.Import(ReflectionUtils.DynamicResolve(type)) : null;
            }
            if (tds.Count() > 1)
            {
                throw new TypeCollisionException();
            }
            return tds.First();
        }

        public static TypeDefinition GetTypeDef(this AssemblyDefinition asm, string type)
        {
            var tds = asm.Modules.Where(m => m.GetType(type) != null).Select(m => m.GetType(type));
            if (tds.Count() == 0)
            {
                return null;
            }
            if (tds.Count() > 1)
            {
                throw new TypeCollisionException();
            }
            return tds.First();
        }

        public static FieldDefinition GetField(this AssemblyDefinition asm, string type, string name, string fieldType)
        {
            var tds = asm.Modules.Where(m => m.GetType(type) != null).Select(m => m.GetType(type));
            if (tds.Count() == 0)
            {
                return null;
            }
            if (tds.Count() > 1)
            {
                throw new TypeCollisionException();
            }
            var td = tds.First();
            return td.Fields.FirstOrDefault(f => f.Name.Equals(name) && f.FieldType.Resolve().FullName.Equals(fieldType));
        }

        public static MethodDefinition GetMethod(this AssemblyDefinition asm, string type, string name, string desc)
        {
            var tds = asm.Modules.Where(m => m.GetType(type) != null).Select(m => m.GetType(type));
            if (tds.Count() == 0)
            {
                return null;
            }
            if (tds.Count() != 1)
            {
                throw new TypeCollisionException();
            }
            var td = tds.First();
            return td.Methods.FirstOrDefault(m => m.Name.Equals(name) && DescriptionOf(m).Equals(desc));
        }

        public static TypeReference Import(this AssemblyDefinition asm, Type t)
        {
            return asm.MainModule.Import(t);
        }

        public static TypeReference Import(this AssemblyDefinition asm, TypeReference tr)
        {
            return asm.MainModule.Import(tr);
        }

        public static IEnumerable<MethodDefinition> FindRefences(this AssemblyDefinition asm, FieldDefinition fd, MethodDefinition exclude = null)
        {
            return asm.Modules.
                SelectMany(m => m.Types).
                SelectMany(t => t.Methods).
                Where(m => m.HasBody && m != exclude && m.Body.Instructions.
                    FirstOrDefault(i => {
                    if (i.Operand != null && i.Operand is FieldReference)
                    {
                        return ((FieldReference)i.Operand).Resolve() == fd;
                    }
                    return false;
               }) != null);
        }

        public static bool IsGettingField(Instruction ins)
        {
            return ins.OpCode == OpCodes.Ldfld || ins.OpCode == OpCodes.Ldflda;
        }

        public static bool IsPuttingField(Instruction ins)
        {
            return ins.OpCode == OpCodes.Stfld;
        }
    }

}
