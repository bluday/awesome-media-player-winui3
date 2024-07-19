using Microsoft.UI.Windowing;

using Windows.Graphics;

namespace BluDay.AwesomeMediaPlayer.Controls;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    private readonly AppWindow _appWindow;

    private readonly DisplayArea _displayArea;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        _appWindow = AppWindow;

        _displayArea = DisplayArea.GetFromWindowId(_appWindow.Id, DisplayAreaFallback.Primary);

        InitializeComponent();
    }

    /// <summary>
    /// Disables the custom title bar control.
    /// </summary>
    public void DisableCustomTitleBar()
    {
        ExtendsContentIntoTitleBar = false;

        SetTitleBar(null);
    }

    /// <summary>
    /// Enables and configures the custom title bar control.
    /// </summary>
    public void EnableCustomTitleBar()
    {
        ExtendsContentIntoTitleBar = true;

        SetTitleBar(TitleBar);
    }

    /// <summary>
    /// Moves the window to the provided x and y coordinates on the screen.
    /// </summary>
    /// <param name="x">
    /// The x coordinate.
    /// </param>
    /// <param name="y">
    /// The y coordinate.
    /// </param>
    public void Move(int x, int y)
    {
        _appWindow.Move(new PointInt32(x, y));
    }

    /// <summary>
    /// Centers the window on the default <see cref="_displayArea"/>.
    /// </summary>
    public void MoveToCenter()
    {
        MoveToCenter(_displayArea);
    }

    /// <summary>
    /// Centers the window on the provided <paramref name="displayArea"/>.
    /// </summary>
    /// <param name="displayArea">
    /// A <see cref="DisplayArea"/> instance of the targeted display.
    /// </param>
    public void MoveToCenter(DisplayArea displayArea)
    {
        int x = (displayArea.WorkArea.Width - _appWindow.Size.Width) / 2;
        int y = (displayArea.WorkArea.Height - _appWindow.Size.Height) / 2;

        Move(x, y);
    }

    /// <summary>
    /// Resizes the window using the provided width and height integer values.
    /// </summary>
    /// <param name="width">
    /// The width of the window.
    /// </param>
    /// <param name="height">
    /// The height of the window.
    /// </param>
    public void Resize(int width, int height)
    {
        _appWindow.Resize(new SizeInt32(width, height));
    }
}