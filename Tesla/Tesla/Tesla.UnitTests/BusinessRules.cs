﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Tesla.Tests
{
    /// <summary>
    /// Business Rules for the application. If values change, change them here, then run unit tests and see what fails. 
    /// </summary>
    public class BusinessRules
    {
        #region PinView
        public const int PinLength = 4;
        public const char HiddenChar = '•';
        public const string EmptyHiddenPin = "enter pin";
        #endregion
        
    }
}
