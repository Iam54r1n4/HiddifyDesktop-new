﻿using System.Reactive;
using Avalonia.Media;
using ReactiveUI;

namespace Clasharp.Interfaces;

public interface IClashCoreManageViewModel : IViewModelBase
{
    string CurrentVersion { get; }

    bool UseCustomUrl { get; set; }

    string CustomUrl { get; set; }

    ReactiveCommand<Unit, Unit> Download { get; }

    bool IsDownloading { get; }

    Color TintColor { get; }
}