using Microsoft.Xna.Framework;

namespace PestControlEngine.Libs.Helpers
{
    public static class Util
    {
        public static Vector2 CurrentResolution { get; set; } = new Vector2();

        public static string GetEngineNull()
        {
            return $"{GetEnginePrefix()}null";
        }

        public static string GetEnginePrefix()
        {
            return "enginereserved_";
        }

        public static Vector2 GetBaseScaleResolution()
        {
            return new Vector2(1024, 1024);
        }

        public static double ScreenScale()
        {
            return CurrentResolution.Y / GetBaseScaleResolution().Y;
        }

        public static double HorizontalScreenScale()
        {
            return CurrentResolution.X / GetBaseScaleResolution().X;
        }
    }


}
