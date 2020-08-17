/*******************************************************************************
 *Author: Kien Truong
 *Program: Falling Apples
 ******************************************************************************/

using System;
using System.Windows.Forms;  //Needed for "Application" on next to last line of Main
public class FallingAppleMain
{  static void Main(string[] args)
   {
    FallingApples fallingApplesApp = new FallingApples();
    Application.Run(fallingApplesApp);
    System.Console.WriteLine("Main method will now shutdown.");
   }//End of Main
}//End of FallingAppleMain
