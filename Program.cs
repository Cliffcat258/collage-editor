using System;
using ImageMagick;
namespace program;

public class Program {

	static void Main() { 
		string dir = Directory.GetCurrentDirectory();
		List<MagickImage> images = new List<MagickImage>();
		Console.WriteLine("How many images to convert?");
		int imageamount = Convert.ToInt32(Console.ReadLine());
		int g = 0;
		for(int i = 0; i < imageamount; i++) {
			try {
			images.Add(new MagickImage(dir + "/images/" + i + ".png"));
			} catch(Exception e) {
				Console.WriteLine("Couldn't load image: " + e);
				g++;
			}
		}
		Console.WriteLine("loaded " + (imageamount - g) + " images out of " + imageamount + " :3");
		/*===========================================================================*/
		MagickImage target = new MagickImage();
		for(;true;) {
			try {
				Console.WriteLine("Name of target image: ");
				string path = Console.ReadLine();
				target = new MagickImage(dir + "/" + path);
			} catch(Exception e) {
				Console.WriteLine("Couldn't load image: " + e);
				Console.WriteLine("Please try again");
				continue;
			}
			break;
		}
		/*===========================================================================*/
		Console.WriteLine("Processing images...");
		byte[][] imagesbytes = new byte[images.Count][];
		for(int i = 0; i < images.Count(); i++) {
			imagesbytes[i] = images[i].GetPixels().ToByteArray(PixelMapping.RGB);
			//Console.WriteLine(imagesbytes[i].Length);
		}
		byte[] targetbytes = target.GetPixels().ToByteArray(PixelMapping.RGB);
		byte[] finalbytes = new byte[targetbytes.Length];
		//Console.WriteLine(targetbytes.Length);
		Console.WriteLine("Computing new image...");
		int diff; byte[] pixel = new byte[3];
		int mindiff;
		for(int i = 0; i < (targetbytes.Length / 3) - 1; i++) {
			mindiff = 999;
			for(int i2 = 0; i2 < imagesbytes.Length; i2++) {
				diff = 0;
				diff += (int)MathF.Abs(targetbytes[i * 3 + 0] - imagesbytes[i2][i * 3 + 0]);
				diff += (int)MathF.Abs(targetbytes[i * 3 + 1] - imagesbytes[i2][i * 3 + 1]);
				diff += (int)MathF.Abs(targetbytes[i * 3 + 2] - imagesbytes[i2][i * 3 + 2]);
				if(diff < mindiff) {
					mindiff = diff;
					pixel[0] = imagesbytes[i2][i * 3 + 0]; pixel[1] = imagesbytes[i2][i * 3 + 1]; pixel[2] = imagesbytes[i2][i * 3 + 2];
				}
			}
			finalbytes[i * 3 + 0] = pixel[0]; finalbytes[i * 3 + 1] = pixel[1]; finalbytes[i * 3 + 2] = pixel[2];
		}
		/*===========================================================================*/
		Console.WriteLine("Converting final image to .png");
		uint width = 1024; uint height = 1024;
		var settings = new PixelReadSettings(width, height, StorageType.Char, PixelMapping.RGB);
		MagickImage finalimage = new MagickImage(finalbytes, settings);
		finalimage.Write(dir + "/" + "final.png");
	}
}
