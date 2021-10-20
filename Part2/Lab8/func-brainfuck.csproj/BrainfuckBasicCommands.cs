using System;

namespace func.brainfuck
{
	public class BrainfuckBasicCommands
	{
		public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
		{
			for (int i = 'a'; i <= 'z'; i++)
			{
				var j = (char)i;
				vm.RegisterCommand(j, b => vm.Memory[vm.MemoryPointer] = (byte)j);
			}
			for (int i = 'A'; i <= 'Z'; i++)
			{
				var j = (char)i;
				vm.RegisterCommand(j, b => vm.Memory[vm.MemoryPointer] = (byte)j);
			}
			for (int i = '0'; i <= '9'; i++)
			{
				var j = (char)i;
				vm.RegisterCommand(j, b => vm.Memory[vm.MemoryPointer] = (byte)j);
			}

			vm.RegisterCommand('.', b => write((char)vm.Memory[vm.MemoryPointer]));
			vm.RegisterCommand('+', b => vm.Memory[vm.MemoryPointer] = (byte)SizeConvert(vm.Memory[vm.MemoryPointer], 256, 1));
			vm.RegisterCommand('-', b => vm.Memory[vm.MemoryPointer] = (byte)SizeConvert(vm.Memory[vm.MemoryPointer], 256, -1));
			vm.RegisterCommand(',', b => vm.Memory[vm.MemoryPointer] = (byte)read());
			vm.RegisterCommand('>', b => vm.MemoryPointer = SizeConvert(vm.MemoryPointer,vm.Memory.Length,1));
			vm.RegisterCommand('<', b => vm.MemoryPointer = SizeConvert(vm.MemoryPointer, vm.Memory.Length, -1));
		}

		private static int SizeConvert(int element, int size, int delta = 0) => (size + element + delta) % size;
	}
}