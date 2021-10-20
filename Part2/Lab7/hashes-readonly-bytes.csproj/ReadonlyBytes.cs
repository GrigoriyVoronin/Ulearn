using System;
using System.Collections;
using System.Collections.Generic;

namespace hashes
{
	public class ReadonlyBytes
	{
		readonly byte[] arr;

		readonly int hashCode;

		public int Length
		{
			get
			{
				return arr.Length;
			}
		}

		public override string ToString()
		{
			return $"[{string.Join(", ", arr)}]";
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;
			if (typeof(ReadonlyBytes) == obj.GetType())
			{
				var byteArr = obj as ReadonlyBytes;
				if (byteArr.arr.Length == arr.Length)
				{
					for (int i = 0; i < arr.Length; i++)
						if (byteArr.arr[i] != arr[i])
							return false;
					return true;
				}
				return false;
			}
			return false;
		}

		public IEnumerator<byte> GetEnumerator()
		{
			for (int i = 0; i < arr.Length; i++)
				yield return arr[i];
		}

		public byte this[int index]
		{
			get
			{
				if (index < arr.Length && index >= 0)
					return arr[index];
				else
					throw new IndexOutOfRangeException();
			}
		}

		public override int GetHashCode()
		{
			return hashCode;
		}

		public ReadonlyBytes(params byte[] arr)
		{
			this.arr = arr ?? throw new ArgumentNullException();
			unchecked
			{
				int offsetBasis = (int)2166136261;
				int prime = 16777619;
				hashCode = offsetBasis;
				foreach (var b in this.arr)
					hashCode = (hashCode ^ b) * prime;
			}
		}
	}
}