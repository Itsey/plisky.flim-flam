namespace Plisky.Diagnostics.FlimFlam {

    using System;

    public class Utility {

        /// <summary>
        /// Writes the content of the exception to a temporary file path.
        /// </summary>
        /// <param name="ex">Exception to log</param>
        public static void LogExceptionToTempFile(string context, Exception ex, string loc = "mcl") {
            if (ex != null) {
                string pth = System.IO.Path.GetTempPath();
                string fn = $"MexTrace_Crash_{loc}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.txt";
                fn = System.IO.Path.Combine(pth, fn);
                System.IO.File.WriteAllText(fn, $"Exception in {context}\r\n\r\n{ex.ToString()}");
            }
        }


    }
}