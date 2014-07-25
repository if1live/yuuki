namespace Yuuki
{
    public class MathHelper
    {
        public static bool IsPowOfTwo(int val)
        {
            if ((val & (val - 1)) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
