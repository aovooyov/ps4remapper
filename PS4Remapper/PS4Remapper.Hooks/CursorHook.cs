using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace PS4Remapper.Hooks
{
    public static class CursorHook
    {
        #region SetCursorPosition
        [DllImport("user32.dll")]
        private static extern IntPtr SetCursorPos(int x, int y);

        public static void SetCursorPosition(int x, int y)
        {
            SetCursorPos(x, y);
        }

        public static void SetCursorPosition(Point p)
        {
            SetCursorPos(p.Y, p.Y);
        }
        #endregion

        #region ShowSystemCursor
        [Flags]
        public enum SPIF
        {
            None = 0x00,
            SPIF_UPDATEINIFILE = 0x01,
            SPIF_SENDCHANGE = 0x02,
            SPIF_SENDWININICHANGE = 0x02
        }
        [Flags]
        public enum SPI
        {
            None = 0x00,
            SPIF_UPDATEINIFILE = 0x01,
            SPIF_SENDCHANGE = 0x02,
            SPIF_SENDWININICHANGE = 0x02
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SystemParametersInfo(SPI uiAction, uint uiParam, IntPtr pvParam, SPIF fWinIni);

        // For setting a string parameter
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, String pvParam, SPIF fWinIni);

        [DllImport("user32.dll")]
        private static extern bool SetSystemCursor(IntPtr hcur, uint id);
        enum OCR_SYSTEM_CURSORS : uint
        {
            /// <summary>
            /// Standard arrow and small hourglass
            /// </summary>
            OCR_APPSTARTING = 32650,
            /// <summary>
            /// Standard arrow
            /// </summary>
            OCR_NORMAL = 32512,
            /// <summary>
            /// Crosshair
            /// </summary>
            OCR_CROSS = 32515,
            /// <summary>
            /// Windows 2000/XP: Hand
            /// </summary>
            OCR_HAND = 32649,
            /// <summary>
            /// Arrow and question mark
            /// </summary>
            OCR_HELP = 32651,
            /// <summary>
            /// I-beam
            /// </summary>
            OCR_IBEAM = 32513,
            /// <summary>
            /// Slashed circle
            /// </summary>
            OCR_NO = 32648,
            /// <summary>
            /// Four-pointed arrow pointing north, south, east, and west
            /// </summary>
            OCR_SIZEALL = 32646,
            /// <summary>
            /// Double-pointed arrow pointing northeast and southwest
            /// </summary>
            OCR_SIZENESW = 32643,
            /// <summary>
            /// Double-pointed arrow pointing north and south
            /// </summary>
            OCR_SIZENS = 32645,
            /// <summary>
            /// Double-pointed arrow pointing northwest and southeast
            /// </summary>
            OCR_SIZENWSE = 32642,
            /// <summary>
            /// Double-pointed arrow pointing west and east
            /// </summary>
            OCR_SIZEWE = 32644,
            /// <summary>
            /// Vertical arrow
            /// </summary>
            OCR_UP = 32516,
            /// <summary>
            /// Hourglass
            /// </summary>
            OCR_WAIT = 32514
        }

        [DllImport("user32.dll")]
        private static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);
        [DllImport("user32.dll")]
        private static extern IntPtr LoadCursorFromFile(string lpFileName);

        private static byte[] _transparentCursor = CreateTransparentCursor();
        private static byte[] CreateTransparentCursor()
        {
            var b = new byte[326];

            b[2] = 0x02; b[4] = 0x01; b[6] = 0x20; b[7] = 0x02;
            b[14] = 0x30; b[15] = 0x01; b[18] = 0x16; b[22] = 0x28;
            b[26] = 0x20; b[30] = 0x40; b[34] = 0x01; b[36] = 0x01;
            b[42] = 0x80; b[54] = 0x02;

            for (var i = 198; i < 326; i++)
            {
                b[i] = 0xFF;
            }

            return b;
        }

        public static bool ShowSystemCursor(bool show)
        {
            try
            {
                if (!show)
                {
                    // Load the cursor
                    var cursorFile = Path.GetTempFileName();
                    using (var fs = new FileStream(cursorFile, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(_transparentCursor, 0, _transparentCursor.Length);
                    }

                    // Set the cursor
                    IntPtr cursor = LoadCursorFromFile(cursorFile);
                    SetSystemCursor(cursor, (uint)OCR_SYSTEM_CURSORS.OCR_NORMAL);

                    // Delete temp file
                    File.Delete(cursorFile);
                }
                else
                {
                    // Reset the cursor
                    SystemParametersInfo(0x0057, 0, null, 0);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion


    }
}