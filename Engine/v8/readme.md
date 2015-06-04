Espresso V8 uses portions of code from ClearScript (http://clearscript.codeplex.com/) Please see site or accompanying file for license.


Notes: 
Build:
If you get an obscure "cmd exited with error code 1", ensure you've run V8Update.cmd 
Tools -> Options -> Projects and Solutions -> Build and Run -> Set Output Verbosity.!

When upgrading from ClearScript:
Update usings to BaristaLabs.Espresso.Engine.V8, BaristaLabs.Espresso.Common
Update header files from C++/CLI project and add/change the namespace decls to BaristaLabs.Espresso.Engine.V8