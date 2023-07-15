using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthorizeTest.Shared;
public class JwtModel
{
    public bool isCompleted { get; set; }
    public bool isCompletedSuccessfully { get; set; }
    public bool isFaulted { get; set; }
    public bool isCanceled { get; set; }
    public string result { get; set; }
}
