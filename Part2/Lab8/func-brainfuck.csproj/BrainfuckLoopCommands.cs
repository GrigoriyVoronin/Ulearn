using System.Collections.Generic;
using System.Linq;

namespace func.brainfuck
{
	public class BrainfuckLoopCommands
	{
		public static void RegisterTo(IVirtualMachine vm)
		{
			var indexOfBrackets = new Stack<int>();
			var startBrackets = new Dictionary<int, int>();
			var endBrackets = new Dictionary<int, int>();
			for(int i =0; i < vm.Instructions.Length;i++)
			{
				if (vm.Instructions[i] == '[')
					indexOfBrackets.Push(i);
				else if (vm.Instructions[i] == ']')
				{
					startBrackets[i] = indexOfBrackets.Peek();
					endBrackets[indexOfBrackets.Pop()] = i;
				}
			}

			vm.RegisterCommand('[', b => {
				if (vm.Memory[vm.MemoryPointer] == 0)
					vm.InstructionPointer = endBrackets[vm.InstructionPointer];
			});
			vm.RegisterCommand(']', b => vm.InstructionPointer = startBrackets[vm.InstructionPointer]-1);
		}
	}
}