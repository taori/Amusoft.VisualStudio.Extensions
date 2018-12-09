using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

[assembly: ComVisible(false)]
[assembly:ThemeInfo(
    ResourceDictionaryLocation.None, //Speicherort der designspezifischen Ressourcenwörterbücher
                             //(wird verwendet, wenn eine Ressource auf der Seite nicht gefunden wird,
                             // oder in den Anwendungsressourcen-Wörterbüchern nicht gefunden werden kann.)
    ResourceDictionaryLocation.SourceAssembly //Speicherort des generischen Ressourcenwörterbuchs
                                      //(wird verwendet, wenn eine Ressource auf der Seite nicht gefunden wird,
                                      // designspezifischen Ressourcenwörterbuch nicht gefunden werden kann.)
)]
[assembly: XmlnsPrefix("http://schemas.localcontrols.com/winfx/2006/xaml/presentation", "framework")]
[assembly: XmlnsDefinition("http://schemas.localcontrols.com/winfx/2006/xaml/presentation", "Company.Desktop.Framework.Controls")]