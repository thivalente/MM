using System;

namespace MM.WebApi.Helpers
{
    public class MMGeneral
    {
        public static string ObterInnerExceptionMessage(Exception ex)
        {
            if (ex == null)
                return String.Empty;

            if (ex.InnerException == null)
                return ex.Message;

            return ObterInnerExceptionMessage(ex.InnerException);
        }
    }
}
