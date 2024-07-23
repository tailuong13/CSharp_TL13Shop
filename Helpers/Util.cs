using System.Text;

namespace TL13Shop.Helpers
{
	public class Util
	{
		public static string UploadHinh(IFormFile Image, string folder)
		{
			try
			{
				var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadImg", folder, Image.FileName);
				using (var fileStream = new FileStream(fullPath, FileMode.CreateNew))
				{
					Image.CopyTo(fileStream);
				}
				return Image.FileName;
			}
			catch (Exception ex)
			{
				return string.Empty;
			}
		}

		public static string GenerateRandomKey(int length = 5)
		{
			string pattern = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			var sb = new StringBuilder();
			var random = new Random();
			for (int i = 0; i < length; i++)
			{
				sb.Append(pattern[random.Next(0, pattern.Length)]);
			}

			return sb.ToString();
		}
	}
}