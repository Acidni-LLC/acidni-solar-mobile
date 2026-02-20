using System.Globalization;

namespace AcidniSolar.Mobile.Converters;

/// <summary>Inverts a boolean value.</summary>
public class InvertedBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b ? !b : value;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b ? !b : value;
}

/// <summary>Returns true if the string is not null or empty.</summary>
public class StringToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => !string.IsNullOrEmpty(value?.ToString());

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

/// <summary>Converts a 0-100 percentage to a 0.0-1.0 progress value.</summary>
public class PercentToProgressConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is double d ? Math.Clamp(d / 100.0, 0.0, 1.0) : 0.0;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is double d ? d * 100.0 : 0.0;
}

/// <summary>Maps grid connected boolean to color.</summary>
public class BoolToGridColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool connected && connected ? Colors.DodgerBlue : Colors.OrangeRed;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

/// <summary>Maps alert severity to an icon emoji.</summary>
public class SeverityToIconConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var severity = value?.ToString()?.ToLowerInvariant();
        return severity switch
        {
            "critical" => "🔴",
            "warning"  => "🟡",
            "info"     => "🔵",
            _          => "⚪"
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

/// <summary>Maps boolean valid state to green (valid) or red (invalid) color.</summary>
public class BoolToStatusColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool valid && valid ? Colors.LimeGreen : Colors.OrangeRed;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

/// <summary>Highlight temperature unit button based on current selection.</summary>
public class TempUnitToColorConverter : IValueConverter
{
    private static readonly Color ActiveColor = Color.FromArgb("#FF9800");
    private static readonly Color InactiveColor = Color.FromArgb("#333355");

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var current = value?.ToString();
        var target = parameter?.ToString();
        return string.Equals(current, target, StringComparison.OrdinalIgnoreCase)
            ? ActiveColor
            : InactiveColor;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
