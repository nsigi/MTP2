namespace MTP.Classes
{
	public static class ParamsHelper
	{
        public static double T { get; set; } = 200.0;
        public static double l_x { get; set; } = 10.0;
        public static double Alpha { get; set; } = 0.004;
        public static double k { get; set; } = 0.13;
        public static double c { get; set; } = 1.84;
        public static double u_0 { get; set; } = 0.0;
        public static int I_x { get; set; } = 10;
        public static int K_t { get; set; } = 500;

        //Условие устойчивости говорит о том, что:
        // I^2 <= l_x^2 / (2 * beta * T) * K
	}
}
