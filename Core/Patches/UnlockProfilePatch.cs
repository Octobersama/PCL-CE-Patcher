using System.Linq;
using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;

namespace PCL_CE_Patcher.Core.Patches
{
    public class UnlockProfilePatch : IPatch
    {
        public string Name => "解锁离线/第三方登录 (CreateProfile)";

        public bool Apply(ModuleDefinition module)
        {
            string uniqueString = "M511.488256";
            var method = module.GetAllTypes()
                .SelectMany(t => t.Methods)
                .FirstOrDefault(m => m.CilMethodBody != null &&
                                     m.CilMethodBody.Instructions.Any(i =>
                                         i.OpCode == CilOpCodes.Ldstr &&
                                         i.Operand?.ToString()?.Contains(uniqueString) == true));

            if (method?.CilMethodBody == null) return false;

            var instructions = method.CilMethodBody.Instructions;
            for (int i = 0; i < instructions.Count; i++)
            {
                if (instructions[i].OpCode == CilOpCodes.Call &&
                    instructions[i].Operand is IMethodDescriptor md && md.Name == "Any")
                {
                    if (i + 1 < instructions.Count)
                    {
                        var next = instructions[i + 1];
                        if (next.OpCode == CilOpCodes.Brfalse || next.OpCode == CilOpCodes.Brfalse_S)
                        {
                            next.OpCode = CilOpCodes.Pop;
                            next.Operand = null;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}