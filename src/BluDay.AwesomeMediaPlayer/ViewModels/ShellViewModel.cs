﻿namespace BluDay.AwesomeMediaPlayer.ViewModels;

/// <summary>
/// Represents the view model class for the <see cref="Shell"/> control.
/// </summary>
/// <inheritdoc cref="ViewModel"/>
public sealed partial class ShellViewModel : ViewModel
{
    private WindowConfiguration? _defaultConfiguration;

    private AppWindow _appWindow;

    private AppWindowTitleBar _appWindowTitleBar;

    private DisplayArea _displayArea;

    private OverlappedPresenter _overlappedPresenter;

    private UIElement? _titleBarControl;

    private Window _shell;

    private string? _iconPath;

    private ContentAlignment? _alignment;

    /// <summary>
    /// Gets the default configuration instance.
    /// </summary>
    public WindowConfiguration? DefaultConfiguration
    {
        get => _defaultConfiguration;
        set => _defaultConfiguration = value;
    }

    /// <inheritdoc cref="WindowConfiguration.ExtendsContentIntoTitleBar"/>
    public bool ExtendsContentIntoTitleBar
    {
        get => _appWindowTitleBar.ExtendsContentIntoTitleBar;
        set
        {
            _appWindowTitleBar.ExtendsContentIntoTitleBar = value;

            if (value)
            {
                ShowCustomTitleBar();

                return;
            }

            ShowDefaultTitleBar();

            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets a value indicating whether the window is visible.
    /// </summary>
    public bool IsVisible => _appWindow.IsVisible;

    /// <inheritdoc cref="WindowConfiguration.IconPath"/>
    public string? IconPath
    {
        get => _iconPath;
        set
        {
            _appWindow.SetIcon(value);

            _iconPath = value;

            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="WindowConfiguration.Title"/>
    public string? Title
    {
        get => _appWindow.Title;
        set
        {
            _appWindow.Title = value;

            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets the id of the window.
    /// </summary>
    public ulong? Id => _appWindow.Id.Value;

    /// <inheritdoc cref="WindowConfiguration.DefaultAlignment"/>
    public ContentAlignment? Alignment
    {
        get => _alignment;
        set
        {
            if (value is null) return;

            ContentAlignment alignment = value.Value;

            RectInt32 workArea = _displayArea!.WorkArea;

            SizeInt32 windowSize = _appWindow.Size;

            int x = windowSize.GetXFromAlignment(alignment, workArea);
            int y = windowSize.GetYFromAlignment(alignment, workArea);

            Move(x, y);

            _alignment = value;

            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="WindowConfiguration.Size"/>
    public SizeInt32? Size
    {
        get => _appWindow.Size;
        set
        {
            _appWindow.Resize(value!.Value);

            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
    /// </summary>
    public ShellViewModel(WeakReferenceMessenger messenger) : base(messenger)
    {
        _appWindow = null!;

        _appWindowTitleBar = null!;

        _displayArea = null!;

        _overlappedPresenter = null!;

        _shell = null!;

        CurrentChild = new MainViewModel(messenger);
    }

    /// <summary>
    /// Shows the custom <see cref="TitleBar"/> control and hides the default Win32 title bar.
    /// </summary>
    private void ShowCustomTitleBar()
    {
        if (_titleBarControl is null) return;

        _titleBarControl.Visibility = Visibility.Visible;

        _appWindowTitleBar.BackgroundColor
            = _appWindowTitleBar.ButtonBackgroundColor
            = _appWindowTitleBar.ButtonInactiveBackgroundColor
            = Colors.Transparent;

        _shell.SetTitleBar(_titleBarControl);
    }

    /// <summary>
    /// Shows the defualt Win32 title bar and hides the custom <see cref="TitleBar"/> control.
    /// </summary>
    private void ShowDefaultTitleBar()
    {
        if (_titleBarControl is null) return;

        _titleBarControl.Visibility = Visibility.Collapsed;

        _appWindowTitleBar.ResetToDefault();

        _appWindow.SetIcon(_iconPath);
    }

    /// <summary>
    /// Gets the current <see cref="DisplayArea"/> for this shell and updates
    /// <see cref="_displayArea"/> with the new instance.
    /// </summary>
    private void UpdateDisplayArea()
    {
        _displayArea = DisplayArea.GetFromWindowId(_appWindow.Id, DisplayAreaFallback.Nearest);
    }

    /// <summary>
    /// Notifies the UI to update all observable properties.
    /// </summary>
    /// <remarks>
    /// Used primarily when <see cref="SetShell(Shell, UIElement?)"/> gets invoked.
    /// </remarks>
    private void RefreshAllProperties()
    {
        OnPropertyChanged(nameof(Id));
        OnPropertyChanged(nameof(IsVisible));
        OnPropertyChanged(nameof(ExtendsContentIntoTitleBar));
        OnPropertyChanged(nameof(IconPath));
        OnPropertyChanged(nameof(Title));
        OnPropertyChanged(nameof(Alignment));
        OnPropertyChanged(nameof(Size));
    }

    /// <summary>
    /// Attempts to activate the shell and bring it into the foreground.
    /// </summary>
    public void Activate()
    {
        _shell.Activate();
    }

    /// <summary>
    /// Applies default configuration values that was provided at instantiation.
    /// </summary>
    public void ApplyDefaultConfiguration()
    {
        if (_defaultConfiguration is null) return;

        Configure(_defaultConfiguration);
    }

    /// <summary>
    /// Closes the shell instance.
    /// </summary>
    public void Close()
    {
        _shell.Close();
    }

    /// <summary>
    /// Configures the shell using the provided properties.
    /// </summary>
    /// <param name="config">
    /// The configuration instance.
    /// </param>
    public void Configure(WindowConfiguration config)
    {
        Title                      = config.Title;
        ExtendsContentIntoTitleBar = config.ExtendsContentIntoTitleBar;
        IconPath                   = config.IconPath;
        Size                       = config.Size;
        Alignment                  = config.Alignment;
    }

    /// <summary>
    /// Hides the window.
    /// </summary>
    public void Hide()
    {
        _appWindow.Hide();

        OnPropertyChanged(nameof(IsVisible));
    }

    /// <summary>
    /// Moves the shell to the provided x and y coordinates on the screen.
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
    /// Resizes the shell using the provided width and height integer values.
    /// </summary>
    /// <param name="width">
    /// The width of the shell.
    /// </param>
    /// <param name="height">
    /// The height of the shell.
    /// </param>
    public void Resize(int width, int height)
    {
        _appWindow.Resize(new SizeInt32(width, height));
    }

    /// <summary>
    /// Sets the targeted shell.
    /// </summary>
    /// <param name="shell">
    /// The shell instance.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="shell"/> is null.
    /// </exception>
    public void SetShell(Shell shell, UIElement? titleBar)
    {
        ArgumentNullException.ThrowIfNull(shell);

        _shell = shell;
        
        _appWindow = shell.AppWindow;

        _appWindowTitleBar = _appWindow.TitleBar;

        _overlappedPresenter = (OverlappedPresenter)_appWindow.Presenter;

        _titleBarControl = titleBar;

        UpdateDisplayArea();

        RefreshAllProperties();
    }

    /// <summary>
    /// Shows the window.
    /// </summary>
    public void Show()
    {
        _appWindow.Show();

        OnPropertyChanged(nameof(IsVisible));
    }
}