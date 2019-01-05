using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace Stitchmania
{
  /// <summary>
  /// A control which defers the loading of its content.
  /// </summary>
  public class DeferredLoadContentControl : ContentControl
  {
    private ContentPresenter _contentPresenter;

    private FrameworkElement _loadingIndicator;


    public DeferredLoadContentControl()
    {
      this.DefaultStyleKey = typeof(DeferredLoadContentControl);

      if (!DesignerProperties.IsInDesignTool)
      {
        this.Loaded += new RoutedEventHandler(DeferredLoadContentControl_Loaded);
      }
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();

      _contentPresenter = this.GetTemplateChild("contentPresenter") as ContentPresenter;
      _loadingIndicator = this.GetTemplateChild("loadingIndicator") as FrameworkElement;

      if (!DesignerProperties.IsInDesignTool)
      {
        _contentPresenter.Visibility = Visibility.Collapsed;
      }
      else
      {
        // in design-mode show the contents 'grayed out'
        _contentPresenter.Opacity = 0.5;
      }
    }

    private void DeferredLoadContentControl_Loaded(object sender, RoutedEventArgs e)
    {
      // content has loaded, now show our content presenter
      _contentPresenter.Visibility = Visibility.Visible;
      _contentPresenter.LayoutUpdated += new EventHandler(ContentPresenter_LayoutUpdated);
    }

    private void ContentPresenter_LayoutUpdated(object sender, EventArgs e)
    {
      // the content presenter has been rendered, hide the loading indicator
      _contentPresenter.LayoutUpdated -= ContentPresenter_LayoutUpdated;
      _loadingIndicator.Visibility = Visibility.Collapsed;
    }    
  }
}
