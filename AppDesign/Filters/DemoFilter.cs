using System;

namespace Lab2
{
	public class CropFilter : IFilter
	{
		public ParameterInfo[] GetParameters()
		{
			return new []
			{
				new ParameterInfo { Name="Коэффициент", MaxValue=10, MinValue=0, Increment=0.1, DefaultValue=1 }
				
			};
		}
		
		public override string ToString ()
		{
			return "Осветление/затемнение";
		}
		
		public Photo Process(Photo original, double[] parameters)
		{
			var result=new Photo();
			result.width=original.width;
			result.height=original.height;
			result.Data=new double[result.width,result.height,3];
			
			for (int x=0;x<result.width;x++)
				for (int y=0;y<result.height;y++)
					for (int z=0;z<3;z++)
						result.Data[x,y,z]=original.Data[x,y,z]*parameters[0];
			return result;
		}
	}
}

