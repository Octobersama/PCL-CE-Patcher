using System.Linq;
using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;

namespace PCL_CE_Patcher.Core.Patches
{
    public class RemovePrecheckPatch : IPatch
    {
        public string Name => "移除启动前检查 (Precheck)";

        public bool Apply(ModuleDefinition module)
        {
            var method = module.GetAllTypes()
                .SelectMany(t => t.Methods)
                .FirstOrDefault(m => m.Name == "McLaunchPrecheck");

            if (method?.CilMethodBody == null) return false;

            var instructions = method.CilMethodBody.Instructions;
            for (int i = 0; i < instructions.Count; i++)
            {
                if (instructions[i].OpCode == CilOpCodes.Ldstr &&
                    instructions[i].Operand?.ToString()?.Contains("你必须先登录正版账号才能启动游戏！") == true)
                {
                    if (i > 0)
                    {
                        var prev = instructions[i - 1];
                        prev.OpCode = CilOpCodes.Pop;
                        prev.Operand = null;
                        instructions[i].OpCode = CilOpCodes.Ret;
                        instructions[i].Operand = null;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}