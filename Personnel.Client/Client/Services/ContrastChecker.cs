namespace Personnel.Client.Client.Services
{
    public class ContrastChecker
    {

        private double GetRelativeLuminance(MudColor color)
        {
            double RsRGB = color.R / 255.0;
            double GsRGB = color.G / 255.0;
            double BsRGB = color.B / 255.0;

            double R = RsRGB <= 0.03928 ? RsRGB / 12.92 : Math.Pow((RsRGB + 0.055) / 1.055, 2.4);
            double G = GsRGB <= 0.03928 ? GsRGB / 12.92 : Math.Pow((GsRGB + 0.055) / 1.055, 2.4);
            double B = BsRGB <= 0.03928 ? BsRGB / 12.92 : Math.Pow((BsRGB + 0.055) / 1.055, 2.4);

            return 0.2126 * R + 0.7152 * G + 0.0722 * B;
        }

        private double GetContrastRatio(string hex1, string hex2)
        {
            MudColor color1, color2;

            color1 = hex1;
            color2 = hex2;

            double L1 = GetRelativeLuminance(color1);
            double L2 = GetRelativeLuminance(color2);

            double brightest = Math.Max(L1, L2);
            double darkest = Math.Min(L1, L2);

            return (brightest + 0.05) / (darkest + 0.05);
        }


        public bool CheckContrast(string hex1, string hex2)
        {
            double ratio = GetContrastRatio(hex1, hex2);

            Console.WriteLine($"Contraste entre {hex1} y {hex2}: {ratio:F2}:1");

            if (ratio >= 7.0)
            {
                Console.WriteLine("✅ Cumple AAA (óptimo contraste)");
                return true;
            }
            else if (ratio >= 4.5)
            {
                Console.WriteLine("✅ Cumple AA (texto normal)");
                return true;
            }
            else if (ratio >= 3.0)
            {
                Console.WriteLine("⚠️ Cumple AA solo para texto grande (≥18px bold o 24px normal)");
                return true;
            }
            else
            {
                Console.WriteLine("❌ No cumple accesibilidad mínima");
                return false;
            }
        }
    }
}
