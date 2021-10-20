using System;

namespace linq_slideviews
{
	public class VisitRecord
	{
		public VisitRecord(int userId, int slideId, DateTime dateTime, SlideType slideType)
		{
			SlideType = slideType;
			UserId = userId;
			SlideId = slideId;
			DateTime = dateTime;
		}

		public int UserId;
		public int SlideId;
		public SlideType SlideType;
		public DateTime DateTime;

		public override string ToString()
		{
			return $"{nameof(UserId)}: {UserId}, {nameof(SlideId)}: {SlideId}, {nameof(SlideType)}: {SlideType}, {nameof(DateTime)}: {DateTime}";
		}

		protected bool Equals(VisitRecord other)
		{
			return UserId == other.UserId && SlideId == other.SlideId && SlideType == other.SlideType && DateTime.Equals(other.DateTime);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((VisitRecord) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = UserId;
				hashCode = (hashCode*397) ^ SlideId;
				hashCode = (hashCode*397) ^ (int) SlideType;
				hashCode = (hashCode*397) ^ DateTime.GetHashCode();
				return hashCode;
			}
		}
	}

}