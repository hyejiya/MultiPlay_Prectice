using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MP.GameElements.Interactions
{
    public interface IOneHandGrabber
    {
        Transform hand {  get; }
    }
}
