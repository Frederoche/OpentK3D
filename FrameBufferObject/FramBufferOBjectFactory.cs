using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameBufferObject
{
    public class FramBufferOBjectFactory
    {
        public static IFrameBufferObject Create(int width, int height)
        {
            return new FrameBufferObject( width, height);
        }
    }
}
