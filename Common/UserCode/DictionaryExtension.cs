namespace LightSwitchApplication
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public static class DictionaryExtension
	{
		private static readonly Encoding enc = Encoding.UTF8;

		public static byte[] Serialize(this Dictionary<string, string> value)
		{
			List<byte> result = new List<byte>();

			foreach (var item in value)
			{
				result.AddRange(EncodeString(item.Key));
				result.AddRange(EncodeString(item.Value));
			}

			return result.ToArray();
		}

		public static bool Deserialize(this Dictionary<string, string> value, byte[] bytes)
		{
			if (bytes == null)
				throw new ArgumentNullException("bytes");

			Dictionary<string, string> cache = new Dictionary<string, string>();
			try
			{
				int position = 0;
				while (position < bytes.Length)
				{
					string key = DecodeString(bytes, ref position);
					string val = DecodeString(bytes, ref position);
					cache.Add(key, val);
				}
			}
			catch
			{
				return false;
			}

			value.Clear();
			foreach (var item in cache)
				value.Add(item.Key, item.Value);
			return true;
		}

		private static byte[] EncodeString(string value)
		{
			byte[] valueBytes = enc.GetBytes(value);
			return BitConverter.GetBytes(valueBytes.Length).Concat(valueBytes).ToArray();
		}
		private static string DecodeString(byte[] array, ref int position)
		{
			int len = BitConverter.ToInt32(array, position);
			string result = enc.GetString(array, position + 4, len);
			position += len + 4;
			return result;
		}
	};
}
