using SConsole = System.Console;

namespace Portable_store.Console
{
    internal static class ConsoleHelper
    {
        /// <summary> Ask a question to the user </summary>
        /// <param name="question">The question to ask the user</param>
        /// <returns>The user answer</returns>
        internal static string Ask(string question)
        {
            string? answer = string.Empty;

            while (string.IsNullOrEmpty(answer))
            {
                answer = OptionalAsk(question, false);

                if (string.IsNullOrEmpty(answer))
                {
                    SConsole.Beep();
                    SConsole.SetCursorPosition(0, SConsole.CursorTop - 1);
                    Clear_current_console_line();
                }
            }

            return answer;
        }


        /// <summary> Ask a question to the user </summary>
        /// <param name="question">The question to ask the user</param>
        /// <returns>The user answer</returns>
        internal static int Ask_number(string question, bool user_hint = true)
        {
            question = (user_hint ? "(Number) " : string.Empty) + question;

            while (true)
            {
                if (int.TryParse(Ask(question), out var answer))
                    return answer;
                else
                {
                    SConsole.Beep();
                    SConsole.SetCursorPosition(0, SConsole.CursorTop - 1);
                    Clear_current_console_line();
                }
            }
        }

        /// <summary> Ask an optional question to the user </summary>
        /// <param name="question">The question to ask the user</param>
        /// <returns>The optional user answer</returns>
        internal static string? OptionalAsk(string question, bool user_hint = true)
        {
            SConsole.Write((user_hint ? "(Optional) " : string.Empty) + question + ": ");
            return SConsole.ReadLine();
        }

        // https://stackoverflow.com/a/8946847/11873025
        internal static void Clear_current_console_line()
        {
            int currentLineCursor = SConsole.CursorTop;
            SConsole.SetCursorPosition(0, SConsole.CursorTop);
            SConsole.Write(new string(' ', SConsole.WindowWidth));
            SConsole.SetCursorPosition(0, currentLineCursor);
        }

        internal static void Write(object value)
        {
            if (!Application_options.Quiet)
                SConsole.Write(value);
        }

        internal static void WriteLine(object value)
        {
            if (!Application_options.Quiet)
                SConsole.WriteLine(value);
        }
    }
}
