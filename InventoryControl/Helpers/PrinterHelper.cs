//using System.Runtime.InteropServices;
//using System.Text;

//namespace InventoryControl.Helpers;

//using System;
//using System.Runtime.InteropServices;
//using System.Text;

//public class PrinterHelper
//{
//    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
//    public class DOCINFOA
//    {
//        public string pDocName;
//        public string pOutputFile;
//        public string pDataType;
//    }

//    [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA")]
//    public static extern bool OpenPrinter(string szPrinter, out nint hPrinter, nint pd);

//    [DllImport("winspool.Drv", EntryPoint = "ClosePrinter")]
//    public static extern bool ClosePrinter(nint hPrinter);

//    [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA")]
//    public static extern bool StartDocPrinter(nint hPrinter, int level, DOCINFOA di);

//    [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter")]
//    public static extern bool EndDocPrinter(nint hPrinter);

//    [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter")]
//    public static extern bool StartPagePrinter(nint hPrinter);

//    [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter")]
//    public static extern bool EndPagePrinter(nint hPrinter);

//    [DllImport("winspool.Drv", EntryPoint = "WritePrinter")]
//    public static extern bool WritePrinter(
//        nint hPrinter,
//        byte[] pBytes,
//        int dwCount,
//        out int dwWritten);

//    public static bool SendStringToPrinter(string printerName, string data)
//    {
//        nint hPrinter;
//        DOCINFOA di = new DOCINFOA();
//        di.pDocName = "RFID Tag Print";
//        di.pDataType = "RAW";

//        if (!OpenPrinter(printerName, out hPrinter, nint.Zero))
//            return false;

//        StartDocPrinter(hPrinter, 1, di);
//        StartPagePrinter(hPrinter);

//        byte[] bytes = Encoding.ASCII.GetBytes(data);

//        WritePrinter(hPrinter, bytes, bytes.Length, out int written);

//        EndPagePrinter(hPrinter);
//        EndDocPrinter(hPrinter);
//        ClosePrinter(hPrinter);

//        return true;
//    }
//}



using System.Runtime.InteropServices;
using System.Text;

public static class RawPrinterHelper
{
    public static bool SendStringToPrinter(string printerName, string data)
    {
        var bytes = Encoding.ASCII.GetBytes(data);

        IntPtr unmanagedBytes = Marshal.AllocCoTaskMem(bytes.Length);
        Marshal.Copy(bytes, 0, unmanagedBytes, bytes.Length);

        bool success = SendBytesToPrinter(printerName, unmanagedBytes, bytes.Length);

        Marshal.FreeCoTaskMem(unmanagedBytes);
        return success;
    }

    [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA")]
    static extern bool OpenPrinter(string pPrinterName, out IntPtr phPrinter, IntPtr pDefault);

    [DllImport("winspool.Drv")]
    static extern bool ClosePrinter(IntPtr hPrinter);

    [DllImport("winspool.Drv")]
    static extern bool StartDocPrinter(IntPtr hPrinter, int level, [In] DOCINFO di);

    [DllImport("winspool.Drv")]
    static extern bool EndDocPrinter(IntPtr hPrinter);

    [DllImport("winspool.Drv")]
    static extern bool StartPagePrinter(IntPtr hPrinter);

    [DllImport("winspool.Drv")]
    static extern bool EndPagePrinter(IntPtr hPrinter);

    [DllImport("winspool.Drv")]
    static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);

    public static bool SendBytesToPrinter(string printerName, IntPtr pBytes, int dwCount)
    {
        OpenPrinter(printerName, out IntPtr hPrinter, IntPtr.Zero);

        DOCINFO di = new DOCINFO
        {
            pDocName = "SBPL Print",
            pDataType = "RAW"
        };

        StartDocPrinter(hPrinter, 1, di);
        StartPagePrinter(hPrinter);

        WritePrinter(hPrinter, pBytes, dwCount, out _);

        EndPagePrinter(hPrinter);
        EndDocPrinter(hPrinter);
        ClosePrinter(hPrinter);

        return true;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class DOCINFO
    {
        public string pDocName;
        public string pOutputFile;
        public string pDataType;
    }
}