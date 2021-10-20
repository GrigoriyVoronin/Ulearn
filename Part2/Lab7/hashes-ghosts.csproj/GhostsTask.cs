using System;
using System.Collections.Generic;
using System.Text;

namespace hashes
{
	public class GhostsTask :
		IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>,
		IMagic
	{
		public void DoMagic()
		{
			cat.Rename("NewBarsik");
			Robot.BatteryCapacity +=1;
			vector.Add(new Vector(1, 1));
			seg.End.Add(new Vector(1, 1));
			cont[0] = 10;
		}

		readonly byte[] cont = new byte[] { 5 };
		readonly Cat cat = new Cat("Barsik", "Britain", new DateTime(2018, 12, 12));
		Document doc;
		readonly Vector vector = new Vector(5, 5);
		readonly Robot robot = new Robot("10",100);
		readonly Segment seg = new Segment(new Vector(1, 1), new Vector(1, 1));

		Cat IFactory<Cat>.Create()
		{
			return cat;
		}

		Vector IFactory<Vector>.Create()
		{
			return vector;
		}

		Segment IFactory<Segment>.Create()
		{
			return seg;
		}

		Robot IFactory<Robot>.Create()
		{
			return robot;
		}

		Document IFactory<Document>.Create()
		{
			doc = new Document("Stat", Encoding.UTF8,cont);
			return doc;
		}
	}
}