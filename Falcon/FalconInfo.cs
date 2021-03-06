﻿using System;
using System.Drawing;
using Falcon.Properties;
using Grasshopper.Kernel;

namespace Falcon
{
    public class FalconInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "Falcon";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return Resources.logo_small;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "A grasshopper library mainly dealing with 4x4 matrix mathematics and quaternion mathematics. It also provides basic marker detection and pathfinding functions.";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("90a9231b-f938-4ff6-baef-f3be614a52e7");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "Jingcheng Chen";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "mail@chenjingcheng.com";
            }
        }
    }
}
