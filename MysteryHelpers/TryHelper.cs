namespace MysteryHelper
{
    public static class Try
    {
        public delegate void TryAction();
        public static void To(TryAction action)
        {
            try
            {
                action.Invoke();
            }
            catch
            {

            }
        }
    }
}