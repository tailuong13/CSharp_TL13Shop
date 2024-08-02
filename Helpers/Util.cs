using System.Text;

namespace TL13Shop.Helpers
{
	public class Util
	{
		public static string UploadImage(IFormFile Image, string folder)
		{
			try
			{
				var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UserImg", folder);

				if (!Directory.Exists(directoryPath))
				{
					Directory.CreateDirectory(directoryPath);
				}

				var fileName = Path.GetFileName(Image.FileName);
				var fullPath = Path.Combine(directoryPath, fileName);

				using (var fileStream = new FileStream(fullPath, FileMode.Create))
				{
					Image.CopyTo(fileStream);
				}

				return Path.Combine("UserImg", folder, fileName).Replace("\\", "/");
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