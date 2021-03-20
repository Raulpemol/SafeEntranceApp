using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ZXing;

namespace SafeEntranceApp.Common
{
    class CodeProcessor
    {
        private const string objectIdFormat = "[0-9A-Fa-f]{24}";

        public string ProcessResult(Result result)
        {
            string text = result.Text;

            if (Regex.Match(text, objectIdFormat).Success)
            {
                return text;
            }

            return string.Empty;
        }
    }
}
