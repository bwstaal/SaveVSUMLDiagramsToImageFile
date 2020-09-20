SaveVSUMLDiagramsToImageFile
============================

An implementation of the MSDN ["How to: Export UML Diagrams to Image Files"](http://msdn.microsoft.com/en-us/library/ff469815.aspx) article for Visual Studio 2013 Ultimate and Visual Studio 2015 Enterprise.

# Build #
To build the source code of this extension yourself, you will have to install both the *Visual Studio SDK* and the *Visual Studio Modeling SDK* for the correct Visual Studio version: 2013 or 2015. You can find these downloads in the Microsoft Download Center. The code for the 2013 version can be found in the "develop" branch. For the 2015 version, it can be found in the "develop_vs2015" branch. The "master" branch contains the code of the latest released version of this extension for VS2015.

# Download #
This extension can be downloaded from the Visual Studio Gallery.

- [2013 Version](http://visualstudiogallery.msdn.microsoft.com/f13d917d-0e67-4f3e-bdb7-d08046553951)
- [2015 Version](https://visualstudiogallery.msdn.microsoft.com/ac24086a-96dd-45ae-ad38-324c3f0233c3)

# No Visual Studio 2017 support #
Unfortunately there will be no support for Visual Studio 2017, because Microsoft decided to remove the UML designers and also the UML extensibility support in the Modeling SDK.
More information can be found [here](https://blogs.msdn.microsoft.com/devops/2016/10/14/uml-designers-have-been-removed-layer-designer-now-supports-live-architectural-analysis/) and [here](https://docs.microsoft.com/en-us/visualstudio/modeling/what-s-new-for-design-in-visual-studio#uml-designers-have-been-removed).

# License #
This extension is released under the [MIT](https://github.com/bwstaal/SaveVSUMLDiagramsToImageFile/blob/develop/LICENSE.txt) license.