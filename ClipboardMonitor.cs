using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GuardaFacil;

public class ClipboardMonitor : NativeWindow, IDisposable
{
  private const int WM_CLIPBOARDUPDATE = 0x031D;

  [DllImport("user32.dll", SetLastError = true)]
  private static extern bool AddClipboardFormatListener(IntPtr hwnd);

  [DllImport("user32.dll", SetLastError = true)]
  private static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

  public event EventHandler? ClipboardContentChanged;

  public ClipboardMonitor()
  {
    this.CreateHandle(new CreateParams());
    AddClipboardFormatListener(this.Handle);
  }

  protected override void WndProc(ref Message m)
  {
    if (m.Msg == WM_CLIPBOARDUPDATE)
    {
      ClipboardContentChanged?.Invoke(this, EventArgs.Empty);
    }
    base.WndProc(ref m);
  }

  public void Dispose()
  {
    RemoveClipboardFormatListener(this.Handle);
    this.DestroyHandle();
  }
}