namespace LightSwitchApplication
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	public class PositionCollection : IEnumerable
	{
		public Position this[int index]
		{
			get
			{
				return data[index];
			}
		}

		private List<Position> data;

		public PositionCollection()
		{
			data = new List<Position>();
		}
		public PositionCollection(IEnumerable<Position> inital) : this()
		{
			data.AddRange(inital);
		}


		public void Add(Position item)
		{
			data.Add(item);
		}

		public void Clear()
		{
			data.Clear();
		}

		public bool Contains(Position item)
		{
			return data.Contains(item);
		}

		public void CopyTo(Position[] array, int arrayIndex)
		{
			data.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get
			{
				return data.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public bool Remove(Position item)
		{
			return data.Remove(item);
		}

		#region IEnumerable Member

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			var tmp = data.ToArray();
			return tmp.GetEnumerator();
		}

		#endregion

		public override string ToString()
		{
			return String.Join(Position.DATASET_SEPERATOR, data.Select(n => n.ToString()));
		}

		public static PositionCollection FromString(string value)
		{
			return new PositionCollection(value.Split(new string[] { Position.DATASET_SEPERATOR }, StringSplitOptions.None).Select(n => Position.FromString(n)));
		}
	};
}
