﻿using System;
using System.IO;
using System.Text;

namespace EzConsole
{
    /** Todo
     *  - Random colors (rainbow)
     *  - Copy these guys:
     *      - https://github.com/colored-console/colored-console/wiki/Quickstart
     *      - https://github.com/NullReferenceCorp/ConsoleColorizer/wiki/Documentation
     *      - https://github.com/RMCKirby/ColorConsole
     *      - https://github.com/tomakita/Colorful.Console
     *      - https://github.com/replaysMike/AnyConsole
     *      - https://github.com/aybe/TrueColorConsole
     *      - https://github.com/cbanor/ColorConsole/
     */
    public static class EzConsole
    {
        #region Console Color Management
        public static void Write(string text, ConsoleColor foreground, ConsoleColor background)
        {
            ConsoleColor fg = ForegroundColor;
            ConsoleColor bg = BackgroundColor;
            ForegroundColor = foreground;
            BackgroundColor = background;
            Console.Write(text);
            ForegroundColor = fg;
            BackgroundColor = bg;
        }
        public static void Write(string text, ConsoleColor foreground) => Write(text, foreground, BackgroundColor);
        public static void Write(string text) => Write(text, ForegroundColor, BackgroundColor);
        public static void WriteLine(string text, ConsoleColor foreground, ConsoleColor background) => Write(text + Environment.NewLine, foreground, background);
        public static void WriteLine(string text, ConsoleColor foreground) => Write(text + Environment.NewLine, foreground, BackgroundColor);
        public static void WriteLine(string text) => Write(text + Environment.NewLine, ForegroundColor, BackgroundColor);
        public static void WriteLine() => Write(Environment.NewLine, ForegroundColor, BackgroundColor);
        #endregion


        #region Console Input
        public static string ReadInput(string text, object defaultValue = null) => ReadInput<string>(text, defaultValue);
        public static T ReadInput<T>(string text, object defaultValue = null, bool allowEmpty = false)
        {
            Write(text, ConsoleColor.Yellow);
            if (defaultValue != null)
                Write($" ({defaultValue})", ConsoleColor.Cyan);
            Console.Write(": ");
            string r = ReadLine();

            if (r.Length == 0)
            {
                if (allowEmpty) return default;
                else
                {
                    WriteLine($"Value must not be empty", ConsoleColor.Red);
                    return ReadInput<T>(text); // Re-prompt
                }
            }

            try
            {
                return (T)Convert.ChangeType(r, typeof(T));
            }
            catch
            {
                string type = typeof(T).Name;
                if (typeof(T) == typeof(int)) type = "Integer Number";
                if (typeof(T) == typeof(double) || typeof(T) == typeof(decimal) || typeof(T) == typeof(float)) type = "Decimal Number";
                if (typeof(T) == typeof(string)) type = "Text";
                if (typeof(T) == typeof(bool)) type = "Boolean";

                WriteLine($"Value must be of type {type}!", ConsoleColor.Red);
                return ReadInput<T>(text); // Re-prompt
            }
        }
        #endregion


        #region Loading animations
        public static void Loading(string message, int duration, int updatesPerSecond = 2, int dots = 3)
        {
            for (int i = 0; i < duration * updatesPerSecond; i++)
            {
                string suffix = "";
                for (int j = 0; j < i % dots + 1; j++) suffix += '.';
                for (int j = 0; j < dots - i % dots - 1; j++) suffix += ' ';

                Write(message + suffix, ConsoleColor.Gray);
                System.Threading.Thread.Sleep(1000 / updatesPerSecond);
                CursorLeft = 0;
            }
            WriteLine(); // Newline
        }
        private static int counter = 0;
        public static void LoadingStep(string message, int dots = 3)
        {
            string suffix = "";
            for (int j = 0; j < counter % dots + 1; j++) suffix += '.';
            for (int j = 0; j < dots - counter % dots - 1; j++) suffix += ' ';
            Write(message + suffix, ConsoleColor.Gray);
            CursorLeft = 0;

            counter++;
        }
        #endregion

        #region System Console Wrapping

        //
        // Summary:
        //     Gets a value that indicates whether input has been redirected from the standard
        //     input stream.
        //
        // Returns:
        //     true if input is redirected; otherwise, false.
        public static bool IsInputRedirected { get { return Console.IsInputRedirected; } }
        //
        // Summary:
        //     Gets or sets the height of the buffer area.
        //
        // Returns:
        //     The current height, in rows, of the buffer area.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     The value in a set operation is less than or equal to zero. -or- The value in
        //     a set operation is greater than or equal to System.Int16.MaxValue. -or- The value
        //     in a set operation is less than System.Console.WindowTop + System.Console.WindowHeight.
        //
        //   T:System.Security.SecurityException:
        //     The user does not have permission to perform this action.
        //
        //   T:System.IO.IOException:
        //     An I/O error occurred.
        //
        //   T:System.PlatformNotSupportedException:
        //     The set operation is invoked on an operating system other than Windows.
        public static int BufferHeight { get { return Console.BufferHeight; } set { Console.BufferHeight = value; } }
        //
        // Summary:
        //     Gets or sets the width of the buffer area.
        //
        // Returns:
        //     The current width, in columns, of the buffer area.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     The value in a set operation is less than or equal to zero. -or- The value in
        //     a set operation is greater than or equal to System.Int16.MaxValue. -or- The value
        //     in a set operation is less than System.Console.WindowLeft + System.Console.WindowWidth.
        //
        //   T:System.Security.SecurityException:
        //     The user does not have permission to perform this action.
        //
        //   T:System.IO.IOException:
        //     An I/O error occurred.
        //
        //   T:System.PlatformNotSupportedException:
        //     The set operation is invoked on an operating system other than Windows.
        public static int BufferWidth { get { return Console.BufferWidth; } set { Console.BufferWidth = value; } }
        //
        // Summary:
        //     Gets a value indicating whether the CAPS LOCK keyboard toggle is turned on or
        //     turned off.
        //
        // Returns:
        //     true if CAPS LOCK is turned on; false if CAPS LOCK is turned off.
        //
        // Exceptions:
        //   T:System.PlatformNotSupportedException:
        //     The get operation is invoked on an operating system other than Windows.
        public static bool CapsLock { get { return Console.CapsLock; } }
        //
        // Summary:
        //     Gets or sets the column position of the cursor within the buffer area.
        //
        // Returns:
        //     The current position, in columns, of the cursor.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     The value in a set operation is less than zero. -or- The value in a set operation
        //     is greater than or equal to System.Console.BufferWidth.
        //
        //   T:System.Security.SecurityException:
        //     The user does not have permission to perform this action.
        //
        //   T:System.IO.IOException:
        //     An I/O error occurred.
        public static int CursorLeft { get { return Console.CursorLeft; } set { Console.CursorLeft = value; } }
        //
        // Summary:
        //     Gets or sets the height of the cursor within a character cell.
        //
        // Returns:
        //     The size of the cursor expressed as a percentage of the height of a character
        //     cell. The property value ranges from 1 to 100.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     The value specified in a set operation is less than 1 or greater than 100.
        //
        //   T:System.Security.SecurityException:
        //     The user does not have permission to perform this action.
        //
        //   T:System.IO.IOException:
        //     An I/O error occurred.
        //
        //   T:System.PlatformNotSupportedException:
        //     The set operation is invoked on an operating system other than Windows.
        public static int CursorSize { get { return Console.CursorSize; } set { Console.CursorSize = value; } }
        //
        // Summary:
        //     Gets or sets the row position of the cursor within the buffer area.
        //
        // Returns:
        //     The current position, in rows, of the cursor.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     The value in a set operation is less than zero. -or- The value in a set operation
        //     is greater than or equal to System.Console.BufferHeight.
        //
        //   T:System.Security.SecurityException:
        //     The user does not have permission to perform this action.
        //
        //   T:System.IO.IOException:
        //     An I/O error occurred.
        public static int CursorTop { get { return Console.CursorTop; } set { Console.CursorTop = value; } }
        //
        // Summary:
        //     Gets or sets a value indicating whether the cursor is visible.
        //
        // Returns:
        //     true if the cursor is visible; otherwise, false.
        //
        // Exceptions:
        //   T:System.Security.SecurityException:
        //     The user does not have permission to perform this action.
        //
        //   T:System.IO.IOException:
        //     An I/O error occurred.
        //
        //   T:System.PlatformNotSupportedException:
        //     The get operation is invoked on an operating system other than Windows.
        public static bool CursorVisible { get { return Console.CursorVisible; } set { Console.CursorVisible = value; } }
        //
        // Summary:
        //     Gets the standard error output stream.
        //
        // Returns:
        //     A System.IO.TextWriter that represents the standard error output stream.
        public static TextWriter Error { get { return Console.Error; }}
        //
        // Summary:
        //     Gets or sets the foreground color of the console.
        //
        // Returns:
        //     A System.ConsoleColor that specifies the foreground color of the console; that
        //     is, the color of each character that is displayed. The default is gray.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     The color specified in a set operation is not a valid member of System.ConsoleColor.
        //
        //   T:System.Security.SecurityException:
        //     The user does not have permission to perform this action.
        //
        //   T:System.IO.IOException:
        //     An I/O error occurred.
        public static ConsoleColor ForegroundColor { get { return Console.ForegroundColor; } set { Console.ForegroundColor = value; } }
        //
        // Summary:
        //     Gets the standard input stream.
        //
        // Returns:
        //     A System.IO.TextReader that represents the standard input stream.
        public static TextReader In { get { return Console.In; } }
        //
        // Summary:
        //     Gets or sets the encoding the console uses to read input.
        //
        // Returns:
        //     The encoding used to read console input.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The property value in a set operation is null.
        //
        //   T:System.IO.IOException:
        //     An error occurred during the execution of this operation.
        //
        //   T:System.Security.SecurityException:
        //     Your application does not have permission to perform this operation.
        public static Encoding InputEncoding { get { return Console.InputEncoding; } set { Console.InputEncoding = value; } }
        //
        // Summary:
        //     Gets a value that indicates whether the error output stream has been redirected
        //     from the standard error stream.
        //
        // Returns:
        //     true if error output is redirected; otherwise, false.
        public static bool IsErrorRedirected { get { return Console.IsErrorRedirected; } }
        //
        // Summary:
        //     Gets or sets the width of the console window.
        //
        // Returns:
        //     The width of the console window measured in columns.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     The value of the System.Console.WindowWidth property or the value of the System.Console.WindowHeight
        //     property is less than or equal to 0. -or- The value of the System.Console.WindowHeight
        //     property plus the value of the System.Console.WindowTop property is greater than
        //     or equal to System.Int16.MaxValue. -or- The value of the System.Console.WindowWidth
        //     property or the value of the System.Console.WindowHeight property is greater
        //     than the largest possible window width or height for the current screen resolution
        //     and console font.
        //
        //   T:System.IO.IOException:
        //     Error reading or writing information.
        //
        //   T:System.PlatformNotSupportedException:
        //     The set operation is invoked on an operating system other than Windows.
        public static int WindowWidth { get { return Console.WindowWidth; } set { Console.WindowWidth = value; } }
        //
        // Summary:
        //     Gets a value that indicates whether output has been redirected from the standard
        //     output stream.
        //
        // Returns:
        //     true if output is redirected; otherwise, false.
        public static bool IsOutputRedirected { get { return Console.IsOutputRedirected; } }
        //
        // Summary:
        //     Gets a value indicating whether a key press is available in the input stream.
        //
        // Returns:
        //     true if a key press is available; otherwise, false.
        //
        // Exceptions:
        //   T:System.IO.IOException:
        //     An I/O error occurred.
        //
        //   T:System.InvalidOperationException:
        //     Standard input is redirected to a file instead of the keyboard.
        public static bool KeyAvailable { get { return Console.KeyAvailable; } }
        //
        // Summary:
        //     Gets the largest possible number of console window rows, based on the current
        //     font and screen resolution.
        //
        // Returns:
        //     The height of the largest possible console window measured in rows.
        public static int LargestWindowHeight { get { return Console.LargestWindowHeight; } }
        //
        // Summary:
        //     Gets the largest possible number of console window columns, based on the current
        //     font and screen resolution.
        //
        // Returns:
        //     The width of the largest possible console window measured in columns.
        public static int LargestWindowWidth { get { return Console.LargestWindowWidth; } }
        //
        // Summary:
        //     Gets a value indicating whether the NUM LOCK keyboard toggle is turned on or
        //     turned off.
        //
        // Returns:
        //     true if NUM LOCK is turned on; false if NUM LOCK is turned off.
        //
        // Exceptions:
        //   T:System.PlatformNotSupportedException:
        //     The get operation is invoked on an operating system other than Windows.
        public static bool NumberLock { get { return Console.NumberLock; } }
        //
        // Summary:
        //     Gets the standard output stream.
        //
        // Returns:
        //     A System.IO.TextWriter that represents the standard output stream.
        public static TextWriter Out { get { return Console.Out; } }
        //
        // Summary:
        //     Gets or sets the encoding the console uses to write output.
        //
        // Returns:
        //     The encoding used to write console output.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The property value in a set operation is null.
        //
        //   T:System.IO.IOException:
        //     An error occurred during the execution of this operation.
        //
        //   T:System.Security.SecurityException:
        //     Your application does not have permission to perform this operation.
        public static Encoding OutputEncoding { get { return Console.OutputEncoding; } }
        //
        // Summary:
        //     Gets or sets the title to display in the console title bar.
        //
        // Returns:
        //     The string to be displayed in the title bar of the console. The maximum length
        //     of the title string is 24500 characters.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     In a get operation, the retrieved title is longer than 24500 characters.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     In a set operation, the specified title is longer than 24500 characters.
        //
        //   T:System.ArgumentNullException:
        //     In a set operation, the specified title is null.
        //
        //   T:System.IO.IOException:
        //     An I/O error occurred.
        //
        //   T:System.PlatformNotSupportedException:
        //     The get operation is invoked on an operating system other than Windows.
        public static string Title { get { return Console.Title; } set { Console.Title = value; } }
        //
        // Summary:
        //     Gets or sets a value indicating whether the combination of the System.ConsoleModifiers.Control
        //     modifier key and System.ConsoleKey.C console key (Ctrl+C) is treated as ordinary
        //     input or as an interruption that is handled by the operating system.
        //
        // Returns:
        //     true if Ctrl+C is treated as ordinary input; otherwise, false.
        //
        // Exceptions:
        //   T:System.IO.IOException:
        //     Unable to get or set the input mode of the console input buffer.
        public static bool TreatControlCAsInput { get { return Console.TreatControlCAsInput; } set { Console.TreatControlCAsInput = value; } }
        //
        // Summary:
        //     Gets or sets the height of the console window area.
        //
        // Returns:
        //     The height of the console window measured in rows.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     The value of the System.Console.WindowWidth property or the value of the System.Console.WindowHeight
        //     property is less than or equal to 0. -or- The value of the System.Console.WindowHeight
        //     property plus the value of the System.Console.WindowTop property is greater than
        //     or equal to System.Int16.MaxValue. -or- The value of the System.Console.WindowWidth
        //     property or the value of the System.Console.WindowHeight property is greater
        //     than the largest possible window width or height for the current screen resolution
        //     and console font.
        //
        //   T:System.IO.IOException:
        //     Error reading or writing information.
        //
        //   T:System.PlatformNotSupportedException:
        //     The set operation is invoked on an operating system other than Windows.
        public static int WindowHeight { get { return Console.WindowHeight; } set { Console.WindowHeight = value; } }
        //
        // Summary:
        //     Gets or sets the leftmost position of the console window area relative to the
        //     screen buffer.
        //
        // Returns:
        //     The leftmost console window position measured in columns.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     In a set operation, the value to be assigned is less than zero. -or- As a result
        //     of the assignment, System.Console.WindowLeft plus System.Console.WindowWidth
        //     would exceed System.Console.BufferWidth.
        //
        //   T:System.IO.IOException:
        //     Error reading or writing information.
        //
        //   T:System.PlatformNotSupportedException:
        //     The set operation is invoked on an operating system other than Windows.
        public static int WindowLeft { get { return Console.WindowLeft; } set { Console.WindowLeft = value; } }
        //
        // Summary:
        //     Gets or sets the top position of the console window area relative to the screen
        //     buffer.
        //
        // Returns:
        //     The uppermost console window position measured in rows.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     In a set operation, the value to be assigned is less than zero. -or- As a result
        //     of the assignment, System.Console.WindowTop plus System.Console.WindowHeight
        //     would exceed System.Console.BufferHeight.
        //
        //   T:System.IO.IOException:
        //     Error reading or writing information.
        //
        //   T:System.PlatformNotSupportedException:
        //     The set operation is invoked on an operating system other than Windows.
        public static int WindowTop { get { return Console.WindowTop; } set { Console.WindowTop = value; } }
        //
        // Summary:
        //     Gets or sets the background color of the console.
        //
        // Returns:
        //     A value that specifies the background color of the console; that is, the color
        //     that appears behind each character. The default is black.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     The color specified in a set operation is not a valid member of System.ConsoleColor.
        //
        //   T:System.Security.SecurityException:
        //     The user does not have permission to perform this action.
        //
        //   T:System.IO.IOException:
        //     An I/O error occurred.
        public static ConsoleColor BackgroundColor { get; set; }

        //
        // Summary:
        //     Occurs when the System.ConsoleModifiers.Control modifier key (Ctrl) and either
        //     the System.ConsoleKey.C console key (C) or the Break key are pressed simultaneously
        //     (Ctrl+C or Ctrl+Break).
        public static event ConsoleCancelEventHandler CancelKeyPress;

        //
        // Summary:
        //     Plays the sound of a beep through the console speaker.
        //
        // Exceptions:
        //   T:System.Security.HostProtectionException:
        //     This method was executed on a server, such as SQL Server, that does not permit
        //     access to a user interface.
        public static void Beep() => Console.Beep();
        //
        // Summary:
        //     Plays the sound of a beep of a specified frequency and duration through the console
        //     speaker.
        //
        // Parameters:
        //   frequency:
        //     The frequency of the beep, ranging from 37 to 32767 hertz.
        //
        //   duration:
        //     The duration of the beep measured in milliseconds.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     frequency is less than 37 or more than 32767 hertz. -or- duration is less than
        //     or equal to zero.
        //
        //   T:System.Security.HostProtectionException:
        //     This method was executed on a server, such as SQL Server, that does not permit
        //     access to the console.
        //
        //   T:System.PlatformNotSupportedException:
        //     The current operating system is not Windows.
        public static void Beep(int frequency, int duration) => Console.Beep(frequency, duration);
        //
        // Summary:
        //     Clears the console buffer and corresponding console window of display information.
        //
        // Exceptions:
        //   T:System.IO.IOException:
        //     An I/O error occurred.
        public static void Clear() => Console.Clear();
        //
        // Summary:
        //     Copies a specified source area of the screen buffer to a specified destination
        //     area.
        //
        // Parameters:
        //   sourceLeft:
        //     The leftmost column of the source area.
        //
        //   sourceTop:
        //     The topmost row of the source area.
        //
        //   sourceWidth:
        //     The number of columns in the source area.
        //
        //   sourceHeight:
        //     The number of rows in the source area.
        //
        //   targetLeft:
        //     The leftmost column of the destination area.
        //
        //   targetTop:
        //     The topmost row of the destination area.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     One or more of the parameters is less than zero. -or- sourceLeft or targetLeft
        //     is greater than or equal to System.Console.BufferWidth. -or- sourceTop or targetTop
        //     is greater than or equal to System.Console.BufferHeight. -or- sourceTop + sourceHeight
        //     is greater than or equal to System.Console.BufferHeight. -or- sourceLeft + sourceWidth
        //     is greater than or equal to System.Console.BufferWidth.
        //
        //   T:System.Security.SecurityException:
        //     The user does not have permission to perform this action.
        //
        //   T:System.IO.IOException:
        //     An I/O error occurred.
        //
        //   T:System.PlatformNotSupportedException:
        //     The current operating system is not Windows.
        public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop)
            => Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop);
        //
        // Summary:
        //     Copies a specified source area of the screen buffer to a specified destination
        //     area.
        //
        // Parameters:
        //   sourceLeft:
        //     The leftmost column of the source area.
        //
        //   sourceTop:
        //     The topmost row of the source area.
        //
        //   sourceWidth:
        //     The number of columns in the source area.
        //
        //   sourceHeight:
        //     The number of rows in the source area.
        //
        //   targetLeft:
        //     The leftmost column of the destination area.
        //
        //   targetTop:
        //     The topmost row of the destination area.
        //
        //   sourceChar:
        //     The character used to fill the source area.
        //
        //   sourceForeColor:
        //     The foreground color used to fill the source area.
        //
        //   sourceBackColor:
        //     The background color used to fill the source area.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     One or more of the parameters is less than zero. -or- sourceLeft or targetLeft
        //     is greater than or equal to System.Console.BufferWidth. -or- sourceTop or targetTop
        //     is greater than or equal to System.Console.BufferHeight. -or- sourceTop + sourceHeight
        //     is greater than or equal to System.Console.BufferHeight. -or- sourceLeft + sourceWidth
        //     is greater than or equal to System.Console.BufferWidth.
        //
        //   T:System.ArgumentException:
        //     One or both of the color parameters is not a member of the System.ConsoleColor
        //     enumeration.
        //
        //   T:System.Security.SecurityException:
        //     The user does not have permission to perform this action.
        //
        //   T:System.IO.IOException:
        //     An I/O error occurred.
        //
        //   T:System.PlatformNotSupportedException:
        //     The current operating system is not Windows.
        public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
            => Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop, sourceChar, sourceForeColor, sourceBackColor);
        //
        // Summary:
        //     Acquires the standard error stream, which is set to a specified buffer size.
        //
        // Parameters:
        //   bufferSize:
        //     The internal stream buffer size.
        //
        // Returns:
        //     The standard error stream.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     bufferSize is less than or equal to zero.
        public static Stream OpenStandardError(int bufferSize) => Console.OpenStandardError(bufferSize);
        //
        // Summary:
        //     Acquires the standard error stream.
        //
        // Returns:
        //     The standard error stream.
        public static Stream OpenStandardError() => Console.OpenStandardError();
        //
        // Summary:
        //     Acquires the standard input stream, which is set to a specified buffer size.
        //
        // Parameters:
        //   bufferSize:
        //     The internal stream buffer size.
        //
        // Returns:
        //     The standard input stream.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     bufferSize is less than or equal to zero.
        public static Stream OpenStandardInput(int bufferSize) => Console.OpenStandardInput(bufferSize);
        //
        // Summary:
        //     Acquires the standard input stream.
        //
        // Returns:
        //     The standard input stream.
        public static Stream OpenStandardInput() => Console.OpenStandardInput();
        //
        // Summary:
        //     Acquires the standard output stream, which is set to a specified buffer size.
        //
        // Parameters:
        //   bufferSize:
        //     The internal stream buffer size.
        //
        // Returns:
        //     The standard output stream.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     bufferSize is less than or equal to zero.
        public static Stream OpenStandardOutput(int bufferSize) => Console.OpenStandardOutput(bufferSize);
        //
        // Summary:
        //     Acquires the standard output stream.
        //
        // Returns:
        //     The standard output stream.
        public static Stream OpenStandardOutput() => Console.OpenStandardOutput();
        //
        // Summary:
        //     Reads the next character from the standard input stream.
        //
        // Returns:
        //     The next character from the input stream, or negative one (-1) if there are currently
        //     no more characters to be read.
        //
        // Exceptions:
        //   T:System.IO.IOException:
        //     An I/O error occurred.
        public static int Read() => Console.Read();
        //
        // Summary:
        //     Obtains the next character or function key pressed by the user. The pressed key
        //     is optionally displayed in the console window.
        //
        // Parameters:
        //   intercept:
        //     Determines whether to display the pressed key in the console window. true to
        //     not display the pressed key; otherwise, false.
        //
        // Returns:
        //     An object that describes the System.ConsoleKey constant and Unicode character,
        //     if any, that correspond to the pressed console key. The System.ConsoleKeyInfo
        //     object also describes, in a bitwise combination of System.ConsoleModifiers values,
        //     whether one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously
        //     with the console key.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     The System.Console.In property is redirected from some stream other than the
        //     console.
        public static ConsoleKeyInfo ReadKey(bool intercept) => Console.ReadKey(intercept);
        //
        // Summary:
        //     Obtains the next character or function key pressed by the user. The pressed key
        //     is displayed in the console window.
        //
        // Returns:
        //     An object that describes the System.ConsoleKey constant and Unicode character,
        //     if any, that correspond to the pressed console key. The System.ConsoleKeyInfo
        //     object also describes, in a bitwise combination of System.ConsoleModifiers values,
        //     whether one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously
        //     with the console key.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     The System.Console.In property is redirected from some stream other than the
        //     console.
        public static ConsoleKeyInfo ReadKey() => Console.ReadKey();
        //
        // Summary:
        //     Reads the next line of characters from the standard input stream.
        //
        // Returns:
        //     The next line of characters from the input stream, or null if no more lines are
        //     available.
        //
        // Exceptions:
        //   T:System.IO.IOException:
        //     An I/O error occurred.
        //
        //   T:System.OutOfMemoryException:
        //     There is insufficient memory to allocate a buffer for the returned string.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     The number of characters in the next line of characters is greater than System.Int32.MaxValue.
        public static string ReadLine() => Console.ReadLine();
        //
        // Summary:
        //     Sets the foreground and background console colors to their defaults.
        //
        // Exceptions:
        //   T:System.Security.SecurityException:
        //     The user does not have permission to perform this action.
        //
        //   T:System.IO.IOException:
        //     An I/O error occurred.
        public static void ResetColor() => Console.ResetColor();
        //
        // Summary:
        //     Sets the height and width of the screen buffer area to the specified values.
        //
        // Parameters:
        //   width:
        //     The width of the buffer area measured in columns.
        //
        //   height:
        //     The height of the buffer area measured in rows.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     height or width is less than or equal to zero. -or- height or width is greater
        //     than or equal to System.Int16.MaxValue. -or- width is less than System.Console.WindowLeft
        //     + System.Console.WindowWidth. -or- height is less than System.Console.WindowTop
        //     + System.Console.WindowHeight.
        //
        //   T:System.Security.SecurityException:
        //     The user does not have permission to perform this action.
        //
        //   T:System.IO.IOException:
        //     An I/O error occurred.
        //
        //   T:System.PlatformNotSupportedException:
        //     The current operating system is not Windows.
        public static void SetBufferSize(int width, int height) => Console.SetBufferSize(width, height);
        //
        // Summary:
        //     Sets the position of the cursor.
        //
        // Parameters:
        //   left:
        //     The column position of the cursor. Columns are numbered from left to right starting
        //     at 0.
        //
        //   top:
        //     The row position of the cursor. Rows are numbered from top to bottom starting
        //     at 0.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     left or top is less than zero. -or- left is greater than or equal to System.Console.BufferWidth.
        //     -or- top is greater than or equal to System.Console.BufferHeight.
        //
        //   T:System.Security.SecurityException:
        //     The user does not have permission to perform this action.
        //
        //   T:System.IO.IOException:
        //     An I/O error occurred.
        public static void SetCursorPosition(int left, int top) => Console.SetCursorPosition(left, top);
        //
        // Summary:
        //     Sets the System.Console.Error property to the specified System.IO.TextWriter
        //     object.
        //
        // Parameters:
        //   newError:
        //     A stream that is the new standard error output.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     newError is null.
        //
        //   T:System.Security.SecurityException:
        //     The caller does not have the required permission.
        public static void SetError(TextWriter newError) => Console.SetError(newError);
        //
        // Summary:
        //     Sets the System.Console.In property to the specified System.IO.TextReader object.
        //
        // Parameters:
        //   newIn:
        //     A stream that is the new standard input.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     newIn is null.
        //
        //   T:System.Security.SecurityException:
        //     The caller does not have the required permission.
        public static void SetIn(TextReader newIn) => Console.SetIn(newIn);
        //
        // Summary:
        //     Sets the System.Console.Out property to target the System.IO.TextWriter object.
        //
        // Parameters:
        //   newOut:
        //     A text writer to be used as the new standard output.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     newOut is null.
        //
        //   T:System.Security.SecurityException:
        //     The caller does not have the required permission.
        public static void SetOut(TextWriter newOut) => Console.SetOut(newOut);
        //
        // Summary:
        //     Sets the position of the console window relative to the screen buffer.
        //
        // Parameters:
        //   left:
        //     The column position of the upper left corner of the console window.
        //
        //   top:
        //     The row position of the upper left corner of the console window.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     left or top is less than zero. -or- left + System.Console.WindowWidth is greater
        //     than System.Console.BufferWidth. -or- top + System.Console.WindowHeight is greater
        //     than System.Console.BufferHeight.
        //
        //   T:System.Security.SecurityException:
        //     The user does not have permission to perform this action.
        //
        //   T:System.IO.IOException:
        //     An I/O error occurred.
        //
        //   T:System.PlatformNotSupportedException:
        //     The current operating system is not Windows.
        public static void SetWindowPosition(int left, int top) => Console.SetWindowPosition(left, top);
        //
        // Summary:
        //     Sets the height and width of the console window to the specified values.
        //
        // Parameters:
        //   width:
        //     The width of the console window measured in columns.
        //
        //   height:
        //     The height of the console window measured in rows.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     width or height is less than or equal to zero. -or- width plus System.Console.WindowLeft
        //     or height plus System.Console.WindowTop is greater than or equal to System.Int16.MaxValue.
        //     -or- width or height is greater than the largest possible window width or height
        //     for the current screen resolution and console font.
        //
        //   T:System.Security.SecurityException:
        //     The user does not have permission to perform this action.
        //
        //   T:System.IO.IOException:
        //     An I/O error occurred.
        //
        //   T:System.PlatformNotSupportedException:
        //     The current operating system is not Windows.
        public static void SetWindowSize(int width, int height) => Console.SetWindowPosition(width, height);
        
        #endregion
    
    }
}
