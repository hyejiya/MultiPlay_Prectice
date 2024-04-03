using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP.GameElements.Interactions
{
    public interface IOneHandGrabbable
    {
        void Grab(IOneHandGrabber grabber);
        void Ungrab();
    }
}
